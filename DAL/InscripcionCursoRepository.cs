using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITY;

namespace DAL
{
    public class InscripcionCursoRepository
    {
        private readonly ConnectionManager connectionManager;
        private readonly UsuarioRepository usuarioRepository;

        public InscripcionCursoRepository(ConnectionManager connectionManager, UsuarioRepository usuarioRepository)
        {
            this.connectionManager = connectionManager;
            this.usuarioRepository = usuarioRepository;
        }

        public void Guardar(Inscripcion_curso inscripcion)
        {
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Inscripcion_curso (id_usuario, id_curso, FECHA_INSCRIPCION_CURSO) 
                                          VALUES (:id_usuario, :id_curso, :FECHA_INSCRIPCION_CURSO)
                                          RETURNING ID_INSCRIPCION_CURSO INTO :ID_INSCRIPCION_CURSO";

                    command.Parameters.Add(":id_usuario", OracleDbType.Int32).Value = inscripcion.id_usuario;
                    command.Parameters.Add(":id_curso", OracleDbType.Int32).Value = inscripcion.id_curso;
                    command.Parameters.Add(":FECHA_INSCRIPCION_CURSO", OracleDbType.Date).Value = inscripcion.fecha_inscripcion_curso;

                    // Parámetro de salida para el ID generado
                    // Corregido el nombre del parámetro (había un 'i' extra)
                    OracleParameter idParam = new OracleParameter(":ID_INSCRIPCION_CURSO", OracleDbType.Int32);
                    idParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(idParam);

                    command.ExecuteNonQuery();

                    // Manejo seguro de la conversión de OracleDecimal
                    if (idParam.Value is OracleDecimal oracleDecimal)
                    {
                        inscripcion.id_inscripcion = Convert.ToInt32(oracleDecimal.Value);
                    }
                    else if (idParam.Value != null && idParam.Value != DBNull.Value)
                    {
                        inscripcion.id_inscripcion = Convert.ToInt32(idParam.Value);
                    }
                }
            }
        }

        public void Eliminar(int idUsuario, int idCurso)
        {
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Inscripcion_curso WHERE id_usuario = :id_usuario AND id_curso = :id_curso";

                    command.Parameters.Add(":id_usuario", OracleDbType.Int32).Value = idUsuario;
                    command.Parameters.Add(":id_curso", OracleDbType.Int32).Value = idCurso;

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ExisteInscripcion(int idUsuario, int idCurso)
        {
            bool existe = false;
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT COUNT(*) FROM Inscripcion_curso WHERE id_usuario = :id_usuario AND id_curso = :id_curso";

                    command.Parameters.Add(":id_usuario", OracleDbType.Int32).Value = idUsuario;
                    command.Parameters.Add(":id_curso", OracleDbType.Int32).Value = idCurso;

                    // Manejo seguro de la conversión de OracleDecimal
                    var result = command.ExecuteScalar();
                    int count = 0;

                    if (result is OracleDecimal oracleDecimal)
                    {
                        count = Convert.ToInt32(oracleDecimal.Value);
                    }
                    else if (result != null && result != DBNull.Value)
                    {
                        count = Convert.ToInt32(result);
                    }

                    existe = count > 0;
                }
            }
            return existe;
        }

        public List<Usuario> ConsultarEstudiantesPorCurso(int idCurso)
        {
            List<Usuario> estudiantes = new List<Usuario>();
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id_usuario FROM Inscripcion_curso WHERE id_curso = :id_curso";

                    command.Parameters.Add(":id_curso", OracleDbType.Int32).Value = idCurso;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Manejo seguro de la conversión de OracleDecimal
                            int idUsuario;
                            var idUsuarioValue = reader["id_usuario"];

                            if (idUsuarioValue is OracleDecimal oracleDecimal)
                            {
                                idUsuario = Convert.ToInt32(oracleDecimal.Value);
                            }
                            else
                            {
                                idUsuario = Convert.ToInt32(idUsuarioValue);
                            }

                            Usuario usuario = usuarioRepository.BuscarPorId(idUsuario);
                            if (usuario != null)
                            {
                                estudiantes.Add(usuario);
                            }
                        }
                    }
                }
            }
            return estudiantes;
        }

        public List<Curso> ConsultarCursosPorUsuario(int idUsuario, CursoRepository cursoRepository)
        {
            List<Curso> cursos = new List<Curso>();
            using (var connection = connectionManager.GetConnection())
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id_curso FROM Inscripcion_curso WHERE id_usuario = :id_usuario";

                    command.Parameters.Add(":id_usuario", OracleDbType.Int32).Value = idUsuario;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Manejo seguro de la conversión de OracleDecimal
                            int idCurso;
                            var idCursoValue = reader["id_curso"];

                            if (idCursoValue is OracleDecimal oracleDecimal)
                            {
                                idCurso = Convert.ToInt32(oracleDecimal.Value);
                            }
                            else
                            {
                                idCurso = Convert.ToInt32(idCursoValue);
                            }

                            Curso curso = cursoRepository.BuscarPorId(idCurso);
                            if (curso != null)
                            {
                                cursos.Add(curso);
                            }
                        }
                    }
                }
            }
            return cursos;
        }
    }
}


