using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITY;
using Oracle.ManagedDataAccess.Types;

namespace DAL
{
    public class UsuarioRepository
    {
        private readonly ConnectionManager connectionManager;

        public UsuarioRepository(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public void Guardar(Usuario usuario)
        {
            

            using (var connection = connectionManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "pkg_usuarios.sp_registrar_usuario";

                        // Parámetros de entrada
                        command.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = usuario.nombre;
                        command.Parameters.Add("p_apellido_paterno", OracleDbType.Varchar2).Value = usuario.apellido_paterno;
                        command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = usuario.email;
                        command.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = usuario.clave;
                        command.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = usuario.telefono ?? (object)DBNull.Value;
                        command.Parameters.Add("p_cedula", OracleDbType.Varchar2).Value = usuario.cedula ?? (object)DBNull.Value;
                        command.Parameters.Add("p_fecha_nacimiento", OracleDbType.Date).Value = usuario.fecha_nacimiento != DateTime.MinValue ? (object)usuario.fecha_nacimiento : DBNull.Value;
                        command.Parameters.Add("p_direccion", OracleDbType.Varchar2).Value = usuario.direccion ?? (object)DBNull.Value;
                        command.Parameters.Add("p_id_comuna", OracleDbType.Int32).Value = usuario.id_comuna > 0 ? (object)usuario.id_comuna : DBNull.Value;
                        command.Parameters.Add("p_telefono_2", OracleDbType.Varchar2).Value = usuario.telefono_2 ?? (object)DBNull.Value;
                        command.Parameters.Add("p_ruta_imagen_usuario", OracleDbType.Varchar2).Value = usuario.ruta_imagen_usuario ?? (object)DBNull.Value; // Nuevo parámetro

                        // Parámetros de salida
                        OracleParameter p_resultado = new OracleParameter("p_resultado", OracleDbType.Int32);
                        p_resultado.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_resultado);

                        OracleParameter p_mensaje = new OracleParameter("p_mensaje", OracleDbType.Varchar2, 500);
                        p_mensaje.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_mensaje);

                        command.ExecuteNonQuery();

                        int resultado;
                        if (p_resultado.Value == DBNull.Value)
                        {
                            resultado = 0;
                        }
                        else
                        {
                            try
                            {
                                resultado = Convert.ToInt32(p_resultado.Value);
                            }
                            catch (InvalidCastException)
                            {
                                // Si hay un error de conversión, intenta con OracleDecimal
                                OracleDecimal oracleDecimal = (OracleDecimal)p_resultado.Value;
                                resultado = oracleDecimal.ToInt32();
                            }
                        }
                        if (resultado <= 0)
                        {
                            throw new Exception($"Error al registrar usuario: {p_mensaje.Value}");
                        }

                        usuario.id_usuario = resultado; // El ID generado se devuelve en p_resultado
                    }
                }
                catch (OracleException ex)
                {
                    Console.WriteLine($"Error de Oracle: {ex.Message}, Código: {ex.Number}");
                    throw;
                }
            }

        }
        public void Modificar(Usuario usuario)
        {
            using (var connection = connectionManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "pkg_usuarios.sp_actualizar_perfil";

                        // Parámetros de entrada
                        command.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = usuario.id_usuario;
                        command.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = usuario.nombre;
                        command.Parameters.Add("p_apellido_paterno", OracleDbType.Varchar2).Value = usuario.apellido_paterno;
                        command.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = usuario.telefono ?? (object)DBNull.Value;
                        command.Parameters.Add("p_direccion", OracleDbType.Varchar2).Value = usuario.direccion ?? (object)DBNull.Value;
                        command.Parameters.Add("p_id_comuna", OracleDbType.Int32).Value = usuario.id_comuna > 0 ? (object)usuario.id_comuna : DBNull.Value;
                        command.Parameters.Add("p_telefono_2", OracleDbType.Varchar2).Value = usuario.telefono_2 ?? (object)DBNull.Value;
                        command.Parameters.Add("p_ruta_imagen_usuario", OracleDbType.Varchar2).Value = usuario.ruta_imagen_usuario ?? (object)DBNull.Value;

                        // Parámetros de salida
                        OracleParameter p_resultado = new OracleParameter("p_resultado", OracleDbType.Int32);
                        p_resultado.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_resultado);

                        OracleParameter p_mensaje = new OracleParameter("p_mensaje", OracleDbType.Varchar2, 500);
                        p_mensaje.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_mensaje);

                        command.ExecuteNonQuery();

                        // Verificar el resultado con manejo adecuado de OracleDecimal
                        int resultado = 0;
                        if (p_resultado.Value != DBNull.Value)
                        {
                            if (p_resultado.Value is OracleDecimal oracleDecimal)
                            {
                                resultado = oracleDecimal.ToInt32();
                            }
                            else
                            {
                                try
                                {
                                    resultado = Convert.ToInt32(p_resultado.Value);
                                }
                                catch (InvalidCastException)
                                {
                                    // Si hay un error de conversión, intentamos con ToString y luego a int
                                    resultado = int.Parse(p_resultado.Value.ToString());
                                }
                            }
                        }

                        if (resultado <= 0)
                        {
                            string mensaje = p_mensaje.Value != DBNull.Value ? p_mensaje.Value.ToString() : "Error desconocido";
                            throw new Exception($"Error al actualizar perfil: {mensaje}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al modificar usuario: {ex.Message}");
                    throw;
                }
            }
        }

        public void CambiarPassword(int idUsuario, string passwordActual, string nuevoPassword)
        {
            using (var connection = connectionManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "pkg_usuarios.sp_cambiar_clave";

                        // Parámetros de entrada
                        command.Parameters.Add("p_id_usuario", OracleDbType.Int32).Value = idUsuario;
                        command.Parameters.Add("p_password_actual", OracleDbType.Varchar2).Value = passwordActual;
                        command.Parameters.Add("p_nuevo_password", OracleDbType.Varchar2).Value = nuevoPassword;

                        // Parámetros de salida
                        OracleParameter p_resultado = new OracleParameter("p_resultado", OracleDbType.Int32);
                        p_resultado.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_resultado);

                        OracleParameter p_mensaje = new OracleParameter("p_mensaje", OracleDbType.Varchar2, 500);
                        p_mensaje.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_mensaje);

                        command.ExecuteNonQuery();

                        // Verificar el resultado con manejo adecuado de OracleDecimal
                        int resultado = 0;
                        if (p_resultado.Value != DBNull.Value)
                        {
                            if (p_resultado.Value is OracleDecimal oracleDecimal)
                            {
                                resultado = oracleDecimal.ToInt32();
                            }
                            else
                            {
                                try
                                {
                                    resultado = Convert.ToInt32(p_resultado.Value);
                                }
                                catch (InvalidCastException)
                                {
                                    resultado = int.Parse(p_resultado.Value.ToString());
                                }
                            }
                        }

                        if (resultado <= 0)
                        {
                            string mensaje = p_mensaje.Value != DBNull.Value ? p_mensaje.Value.ToString() : "Error desconocido";
                            throw new Exception(mensaje);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cambiar contraseña: {ex.Message}");
                    throw;
                }
            }
        }

        public void Eliminar(int idUsuario)
        {
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Usuarios WHERE id_usuario = :id_usuario";

                    command.Parameters.Add("id_usuario", idUsuario);

                    command.ExecuteNonQuery();
                }
            }
        }

        public Usuario BuscarPorId(int idUsuario)
        {
            Usuario usuario = null;
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Usuarios WHERE id_usuario = :id_usuario";
                    command.Parameters.Add("id_usuario", idUsuario);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = MapToUsuario(reader);
                        }
                    }
                }
            }
            return usuario;
        }

        public Usuario BuscarPorEmail(string email)
        {
            Usuario usuario = null;
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Usuarios WHERE email = :email";
                    command.Parameters.Add("email", email);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = MapToUsuario(reader);
                        }
                    }
                }
            }
            return usuario;
        }

        public Usuario Login(string email, string clave)
        {
            Usuario usuario = null;
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Usuarios WHERE email = :email AND clave = :clave";

                    command.Parameters.Add("email", email);
                    command.Parameters.Add("clave", clave);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = MapToUsuario(reader);
                        }
                    }
                }
            }
            return usuario;
        }

        public List<Usuario> ConsultarTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Usuarios";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario usuario = MapToUsuario(reader);
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        private Usuario MapToUsuario(OracleDataReader reader)
        {
            try
            {
                return new Usuario
                {
                    id_usuario = Convert.ToInt32(reader["ID_USUARIO"]),
                    nombre = reader["NOMBRE"].ToString(),
                    apellido_paterno = reader["APELLIDO_PATERNO"].ToString(),
                    fecha_nacimiento = reader["FECHA_NACIMIENTO"] != DBNull.Value ? Convert.ToDateTime(reader["FECHA_NACIMIENTO"]) : DateTime.MinValue,
                    direccion = reader["DIRECCION"] != DBNull.Value ? reader["DIRECCION"].ToString() : null,
                    telefono = reader["TELEFONO"] != DBNull.Value ? reader["TELEFONO"].ToString() : null,
                    telefono_2 = reader["TELEFONO_2"] != DBNull.Value ? reader["TELEFONO_2"].ToString() : null, // Corregido: TELEFONO_2 en lugar de telefono2
                    email = reader["EMAIL"] != DBNull.Value ? reader["EMAIL"].ToString() : null,
                    cedula = reader["CEDULA"] != DBNull.Value ? reader["CEDULA"].ToString() : null,
                    clave = reader["CLAVE"].ToString(),
                    es_miembro = reader["ES_MIEMBRO"].ToString(),
                    es_administrador = reader["ES_ADMINISTRADOR"].ToString(),
                    id_comuna = reader["ID_COMUNA"] != DBNull.Value ? Convert.ToInt32(reader["ID_COMUNA"]) : 0,
                    ruta_imagen_usuario = reader["ruta_imagen_usuario"] != DBNull.Value ? reader["ruta_imagen_usuario"].ToString() : null
                };
            }
            catch (IndexOutOfRangeException ex)
            {
                // Depuración: imprimir los nombres de las columnas disponibles
                Console.WriteLine("Columnas disponibles en el resultado:");
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.WriteLine($"{i}: {reader.GetName(i)}");
                }
                throw new Exception($"Error al mapear usuario: {ex.Message}", ex);
            }
        }

        public void ActualizarEstadoMiembro(int idUsuario, string esMiembro)
        {
            using (var connection = connectionManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "sp_actualizar_estado_miembro";

                        // Parámetros de entrada
                        command.Parameters.Add(new OracleParameter("p_id_usuario", OracleDbType.Int32) { Value = idUsuario });
                        command.Parameters.Add(new OracleParameter("p_es_miembro", OracleDbType.Char) { Value = esMiembro });

                        // Parámetros de salida
                        OracleParameter p_resultado = new OracleParameter("p_resultado", OracleDbType.Int32);
                        p_resultado.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_resultado);

                        OracleParameter p_mensaje = new OracleParameter("p_mensaje", OracleDbType.Varchar2, 500);
                        p_mensaje.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_mensaje);

                        command.ExecuteNonQuery();

                        // Verificar el resultado
                        int resultado = 0;
                        if (p_resultado.Value != DBNull.Value)
                        {
                            if (p_resultado.Value is OracleDecimal oracleDecimal)
                            {
                                resultado = oracleDecimal.ToInt32();
                            }
                            else
                            {
                                resultado = Convert.ToInt32(p_resultado.Value);
                            }
                        }

                        if (resultado <= 0)
                        {
                            string mensaje = p_mensaje.Value != DBNull.Value ? p_mensaje.Value.ToString() : "Error desconocido";
                            throw new Exception(mensaje);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar estado de miembro: {ex.Message}");
                    throw;
                }
            }
        }

        public void ActualizarEstadoAdministrador(int idUsuario, string esAdministrador)
        {
            using (var connection = connectionManager.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "sp_actualizar_estado_admin";

                        // Parámetros de entrada
                        command.Parameters.Add(new OracleParameter("p_id_usuario", OracleDbType.Int32) { Value = idUsuario });
                        command.Parameters.Add(new OracleParameter("p_es_administrador", OracleDbType.Char) { Value = esAdministrador });

                        // Parámetros de salida
                        OracleParameter p_resultado = new OracleParameter("p_resultado", OracleDbType.Int32);
                        p_resultado.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_resultado);

                        OracleParameter p_mensaje = new OracleParameter("p_mensaje", OracleDbType.Varchar2, 500);
                        p_mensaje.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(p_mensaje);

                        command.ExecuteNonQuery();

                        // Verificar el resultado
                        int resultado = 0;
                        if (p_resultado.Value != DBNull.Value)
                        {
                            if (p_resultado.Value is OracleDecimal oracleDecimal)
                            {
                                resultado = oracleDecimal.ToInt32();
                            }
                            else
                            {
                                resultado = Convert.ToInt32(p_resultado.Value);
                            }
                        }

                        if (resultado <= 0)
                        {
                            string mensaje = p_mensaje.Value != DBNull.Value ? p_mensaje.Value.ToString() : "Error desconocido";
                            throw new Exception(mensaje);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar estado de administrador: {ex.Message}");
                    throw;
                }
            }
        }
        public Usuario BuscarPorTelefono(string telefono)
        {
            using (var conn = connectionManager.GetConnection())
            {
                conn.Open();
                var cmd = new OracleCommand("SELECT * FROM usuarios WHERE TELEFONO = :telefono", conn);
                cmd.Parameters.Add(new OracleParameter("telefono", OracleDbType.Varchar2)).Value = telefono;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToUsuario(reader);
                    }
                }
            }
            return null;
        }
    }
}
