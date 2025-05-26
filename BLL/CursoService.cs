using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ENTITY;

namespace BLL
{
    public class CursoService
    {
        private readonly CursoRepository cursoRepository;
        private readonly InscripcionCursoRepository inscripcionCursoRepository;
        private readonly UsuarioRepository usuarioRepository;
        private readonly EmailNotificationService emailNotificationService;
        private readonly string connectionString;

        public CursoService()
        {
            connectionString = "Data Source=localhost;Initial Catalog=IglesiaDB;Integrated Security=True";
            var connectionManager = new ConnectionManager();
            usuarioRepository = new UsuarioRepository(connectionManager);
            cursoRepository = new CursoRepository(connectionManager);
            inscripcionCursoRepository = new InscripcionCursoRepository(connectionManager, usuarioRepository);
            emailNotificationService = new EmailNotificationService(); // Nuevo servicio
        }

        public string Guardar(Curso curso)
        {
            try
            {
                // Validar fechas
                if (curso.fecha_inicio_curso > curso.fecha_fin_curso)
                {
                    return "Error al guardar: La fecha de inicio no puede ser posterior a la fecha de fin";
                }

                cursoRepository.Guardar(curso);

                // *** NUEVA FUNCIONALIDAD: Enviar notificación de nuevo curso ***
                try
                {
                    //emailNotificationService.NotificarCreacionCurso(curso);
                    //Console.WriteLine($"Notificaciones de nuevo curso enviadas para: {curso.nombre_curso}");
                    var emailNotificationService = new EmailNotificationService();
                    string resultado = emailNotificationService.NotificarCreacionCurso(curso);
                    // Opcional: mostrar resultado
                }
                catch (Exception emailEx)
                {
                    Console.WriteLine($"Error al enviar notificaciones de nuevo curso: {emailEx.Message}");
                    // No fallar la operación principal por un error de email
                }

                return $"Curso {curso.nombre_curso} guardado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al guardar: {ex.Message}";
            }
        }

        public string Modificar(Curso curso)
        {
            try
            {
                // Validar que el curso exista
                var cursoExistente = cursoRepository.BuscarPorId(curso.id_curso);
                if (cursoExistente == null)
                {
                    return $"Error al modificar: El curso con ID {curso.id_curso} no existe";
                }

                // Validar fechas
                if (curso.fecha_inicio_curso > curso.fecha_fin_curso)
                {
                    return "Error al modificar: La fecha de inicio no puede ser posterior a la fecha de fin";
                }

                cursoRepository.Modificar(curso);
                return $"Curso {curso.nombre_curso} modificado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al modificar: {ex.Message}";
            }
        }

        public string Eliminar(int idCurso)
        {
            try
            {
                // Validar que el curso exista
                var curso = cursoRepository.BuscarPorId(idCurso);
                if (curso == null)
                {
                    return $"Error al eliminar: El curso con ID {idCurso} no existe";
                }

                cursoRepository.Eliminar(idCurso);
                return $"Curso {curso.nombre_curso} eliminado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar: {ex.Message}";
            }
        }

        public Curso BuscarPorId(int idCurso)
        {
            var curso = cursoRepository.BuscarPorId(idCurso);
            if (curso != null)
            {
                curso.Estudiantes = inscripcionCursoRepository.ConsultarEstudiantesPorCurso(idCurso);
            }
            return curso;
        }

        public List<Curso> Consultar()
        {
            var cursos = cursoRepository.ConsultarTodos();
            foreach (var curso in cursos)
            {
                curso.Estudiantes = inscripcionCursoRepository.ConsultarEstudiantesPorCurso(curso.id_curso);
            }
            return cursos;
        }

        public List<Curso> ConsultarCursosDisponibles()
        {
            var cursos = cursoRepository.ConsultarCursosDisponibles();
            foreach (var curso in cursos)
            {
                curso.Estudiantes = inscripcionCursoRepository.ConsultarEstudiantesPorCurso(curso.id_curso);
            }
            return cursos;
        }

        public List<CursoDTO> ConsultarDTO()
        {
            var cursos = Consultar();
            var cursosDTO = new List<CursoDTO>();

            foreach (var curso in cursos)
            {
                cursosDTO.Add(new CursoDTO
                {
                    id_curso = curso.id_curso,
                    nombre_curso = curso.nombre_curso,
                    descripcion_curso = curso.descripcion_curso,
                    fecha_inicio_curso = curso.fecha_inicio_curso,
                    fecha_fin_curso = curso.fecha_fin_curso,
                    capacidad_max_curso = curso.capacidad_max_curso,
                    NumeroInscritos = curso.NumeroInscritos,
                    ruta_imagen_curso = curso.ruta_imagen_curso
                });
            }

            return cursosDTO;
        }

        public List<CursoDTO> ConsultarCursosDisponiblesDTO()
        {
            var cursos = ConsultarCursosDisponibles();
            var cursosDTO = new List<CursoDTO>();

            foreach (var curso in cursos)
            {
                cursosDTO.Add(new CursoDTO
                {
                    id_curso = curso.id_curso,
                    nombre_curso = curso.nombre_curso,
                    descripcion_curso = curso.descripcion_curso,
                    fecha_inicio_curso = curso.fecha_inicio_curso,
                    fecha_fin_curso = curso.fecha_fin_curso,
                    capacidad_max_curso = curso.capacidad_max_curso,
                    NumeroInscritos = curso.NumeroInscritos
                });
            }

            return cursosDTO;
        }

        public string Inscribirse(int idUsuario, int idCurso)
        {
            try
            {
                // Validar que el usuario exista
                var usuario = usuarioRepository.BuscarPorId(idUsuario);
                if (usuario == null)
                {
                    return $"Error al inscribirse: El usuario con ID {idUsuario} no existe";
                }

                // Validar que el curso exista
                var curso = cursoRepository.BuscarPorId(idCurso);
                if (curso == null)
                {
                    return $"Error al inscribirse: El curso con ID {idCurso} no existe";
                }

                // Validar que no esté ya inscrito
                if (inscripcionCursoRepository.ExisteInscripcion(idUsuario, idCurso))
                {
                    return $"Error al inscribirse: El usuario ya está inscrito en este curso";
                }

                // Validar capacidad
                curso.Estudiantes = inscripcionCursoRepository.ConsultarEstudiantesPorCurso(idCurso);
                if (curso.NumeroInscritos >= curso.capacidad_max_curso)
                {
                    return $"Error al inscribirse: El curso ha alcanzado su capacidad máxima";
                }

                // Inscribir
                var inscripcion = new Inscripcion_curso
                {
                    id_usuario = idUsuario,
                    id_curso = idCurso
                };
                inscripcionCursoRepository.Guardar(inscripcion);

                return $"Inscripción al curso {curso.nombre_curso} realizada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al inscribirse: {ex.Message}";
            }
        }

        public string CancelarInscripcion(int idUsuario, int idCurso)
        {
            try
            {
                // Validar que el usuario exista
                var usuario = usuarioRepository.BuscarPorId(idUsuario);
                if (usuario == null)
                {
                    return $"Error al cancelar inscripción: El usuario con ID {idUsuario} no existe";
                }

                // Validar que el curso exista
                var curso = cursoRepository.BuscarPorId(idCurso);
                if (curso == null)
                {
                    return $"Error al cancelar inscripción: El curso con ID {idCurso} no existe";
                }

                // Validar que esté inscrito
                if (!inscripcionCursoRepository.ExisteInscripcion(idUsuario, idCurso))
                {
                    return $"Error al cancelar inscripción: El usuario no está inscrito en este curso";
                }

                // Cancelar inscripción
                inscripcionCursoRepository.Eliminar(idUsuario, idCurso);

                return $"Inscripción al curso {curso.nombre_curso} cancelada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al cancelar inscripción: {ex.Message}";
            }
        }

        public List<Usuario> ConsultarEstudiantes(int idCurso)
        {
            return inscripcionCursoRepository.ConsultarEstudiantesPorCurso(idCurso);
        }

        public List<Curso> ConsultarCursosPorUsuario(int idUsuario)
        {
            return inscripcionCursoRepository.ConsultarCursosPorUsuario(idUsuario, cursoRepository);
        }
    }
}