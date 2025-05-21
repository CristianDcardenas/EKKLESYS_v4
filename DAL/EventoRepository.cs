using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITY;

namespace DAL
{
    public class EventoRepository
    {
        private readonly ConnectionManager connectionManager;

        public EventoRepository(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }




        public void Guardar(Evento evento)
        {
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Eventos (nombre_evento, lugar_evento, descripcion_evento, 
                                  fecha_inicio_evento, fecha_fin_evento, capacidad_max_evento, ruta_imagen_evento) 
                                  VALUES (:nombre_evento, :lugar_evento, :descripcion_evento, 
                                  :fecha_inicio_evento, :fecha_fin_evento, :capacidad_max_evento, :ruta_imagen_evento)
                                  RETURNING id_evento INTO :id_evento";

                    command.Parameters.Add(":nombre_evento", OracleDbType.Varchar2).Value = evento.nombre_evento;
                    command.Parameters.Add(":lugar_evento", OracleDbType.Varchar2).Value = evento.lugar_evento;
                    command.Parameters.Add(":descripcion_evento", OracleDbType.Varchar2).Value = evento.descripcion_evento;
                    command.Parameters.Add(":fecha_inicio_evento", OracleDbType.Date).Value = evento.fecha_inicio_evento;
                    command.Parameters.Add(":fecha_fin_evento", OracleDbType.Date).Value = evento.fecha_fin_evento;
                    command.Parameters.Add(":capacidad_max_evento", OracleDbType.Int32).Value = evento.capacidad_max_evento;
                    command.Parameters.Add(":ruta_imagen_evento", OracleDbType.Varchar2).Value =
                        string.IsNullOrEmpty(evento.ruta_imagen_evento) ? DBNull.Value : (object)evento.ruta_imagen_evento;

                    // Parámetro de salida para el ID generado
                    OracleParameter idParam = new OracleParameter(":id_evento", OracleDbType.Int32);
                    idParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(idParam);

                    command.ExecuteNonQuery();

                    // Solución: Manejar correctamente la conversión de OracleDecimal
                    if (idParam.Value is Oracle.ManagedDataAccess.Types.OracleDecimal oracleDecimal)
                    {
                        evento.id_evento = Convert.ToInt32(oracleDecimal.Value);
                    }
                    else if (idParam.Value != DBNull.Value && idParam.Value != null)
                    {
                        evento.id_evento = Convert.ToInt32(idParam.Value);
                    }
                }
            }
        }



        public void Modificar(Evento evento)
        {
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Eventos SET nombre_evento = :nombre_evento, lugar_evento = :lugar_evento, 
                                  descripcion_evento = :descripcion_evento, fecha_inicio_evento = :fecha_inicio_evento, 
                                  fecha_fin_evento = :fecha_fin_evento, capacidad_max_evento = :capacidad_max_evento,
                                  ruta_imagen_evento = :ruta_imagen_evento 
                                  WHERE id_evento = :id_evento";

                    command.Parameters.Add(":id_evento", OracleDbType.Int32).Value = evento.id_evento;
                    command.Parameters.Add(":nombre_evento", OracleDbType.Varchar2).Value = evento.nombre_evento;
                    command.Parameters.Add(":lugar_evento", OracleDbType.Varchar2).Value = evento.lugar_evento;
                    command.Parameters.Add(":descripcion_evento", OracleDbType.Varchar2).Value = evento.descripcion_evento;
                    command.Parameters.Add(":fecha_inicio_evento", OracleDbType.Date).Value = evento.fecha_inicio_evento;
                    command.Parameters.Add(":fecha_fin_evento", OracleDbType.Date).Value = evento.fecha_fin_evento;
                    command.Parameters.Add(":capacidad_max_evento", OracleDbType.Int32).Value = evento.capacidad_max_evento;
                    command.Parameters.Add(":ruta_imagen_evento", OracleDbType.Varchar2).Value =
                        string.IsNullOrEmpty(evento.ruta_imagen_evento) ? DBNull.Value : (object)evento.ruta_imagen_evento;

                    command.ExecuteNonQuery();
                }
            }
        }

       
        public string Eliminar(int idEvento)
        {
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Primero eliminar las inscripciones asociadas (ajusta los nombres de tablas según tu esquema)
                        using (var commandInscripciones = new OracleCommand())
                        {
                            commandInscripciones.Connection = connection;
                            commandInscripciones.Transaction = transaction;
                            commandInscripciones.CommandText = "DELETE FROM ASISTENCIA_EVENTO WHERE id_evento = :id_evento";
                            commandInscripciones.Parameters.Add(":id_evento", OracleDbType.Int32).Value = idEvento;
                            commandInscripciones.ExecuteNonQuery();
                        }

                        // Luego eliminar el evento
                        using (var commandEvento = new OracleCommand())
                        {
                            commandEvento.Connection = connection;
                            commandEvento.Transaction = transaction;
                            commandEvento.CommandText = "DELETE FROM Eventos WHERE id_evento = :id_evento";
                            commandEvento.Parameters.Add(":id_evento", OracleDbType.Int32).Value = idEvento;
                            commandEvento.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return "Evento y sus registros relacionados eliminados correctamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return "Error al eliminar el evento: " + ex.Message;
                    }
                }
            }
        }


        public Evento BuscarPorId(int idEvento)
        {
            Evento evento = null;
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Eventos WHERE id_evento = :id_evento";

                    command.Parameters.Add(":id_evento", OracleDbType.Int32).Value = idEvento;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            evento = MapToEvento(reader);
                        }
                    }
                }
            }
            return evento;
        }

        public List<Evento> ConsultarTodos()
        {
            List<Evento> eventos = new List<Evento>();
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Eventos";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Evento evento = MapToEvento(reader);
                            eventos.Add(evento);
                        }
                    }
                }
            }
            return eventos;
        }

        public List<Evento> ConsultarProximosEventos()
        {
            List<Evento> eventos = new List<Evento>();
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Eventos WHERE fecha_fin_evento >= :fecha_actual";

                    command.Parameters.Add(":fecha_actual", OracleDbType.Date).Value = DateTime.Now;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Evento evento = MapToEvento(reader);
                            eventos.Add(evento);
                        }
                    }
                }
            }
            return eventos;
        }

        private Evento MapToEvento(OracleDataReader reader)
        {
            return new Evento
            {
                id_evento = Convert.ToInt32(reader["id_evento"]),
                nombre_evento = reader["nombre_evento"].ToString(),
                lugar_evento = reader["lugar_evento"].ToString(),
                descripcion_evento = reader["descripcion_evento"].ToString(),
                fecha_inicio_evento = Convert.ToDateTime(reader["fecha_inicio_evento"]),
                fecha_fin_evento = Convert.ToDateTime(reader["fecha_fin_evento"]),
                capacidad_max_evento = Convert.ToInt32(reader["capacidad_max_evento"]),
                ruta_imagen_evento = reader["ruta_imagen_evento"] != DBNull.Value ? reader["ruta_imagen_evento"].ToString() : null
            };
        }

       
    }
}