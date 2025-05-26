using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using ENTITY;

namespace BLL
{
    public class EmailNotificationService
    {
        private readonly UsuarioRepository usuarioRepository;
        private readonly InscripcionCursoRepository inscripcionRepository;
        private readonly AsistenciaEventoRepository asistenciaRepository;
        private readonly EmailService emailService;

        public EmailNotificationService()
        {
            var connectionManager = new ConnectionManager();
            usuarioRepository = new UsuarioRepository(connectionManager);
            inscripcionRepository = new InscripcionCursoRepository(connectionManager, usuarioRepository);
            asistenciaRepository = new AsistenciaEventoRepository(connectionManager, usuarioRepository);
            emailService = new EmailService();
        }

        public string NotificarCreacionCurso(Curso curso)
        {
            try
            {
                Console.WriteLine($"=== INICIANDO NOTIFICACIÓN DE NUEVO CURSO ===");
                Console.WriteLine($"Curso: {curso.nombre_curso}");
                Console.WriteLine($"Fecha inicio: {curso.fecha_inicio_curso}");

                // Obtener todos los usuarios con email válido
                var usuarios = usuarioRepository.ConsultarTodos();
                Console.WriteLine($"Total usuarios en BD: {usuarios.Count}");

                var usuariosConEmail = usuarios.Where(u => !string.IsNullOrEmpty(u.email)).ToList();
                Console.WriteLine($"Usuarios con email: {usuariosConEmail.Count}");

                if (usuariosConEmail.Count == 0)
                {
                    string mensaje = "No hay usuarios con email válido para notificar";
                    Console.WriteLine($"ADVERTENCIA: {mensaje}");
                    return mensaje;
                }

                int exitosos = 0;
                int fallidos = 0;

                foreach (var usuario in usuariosConEmail)
                {
                    try
                    {
                        Console.WriteLine($"Enviando correo a: {usuario.email} ({usuario.nombre})");

                        EnviarCorreoNuevoCurso(
                            usuario.email,
                            usuario.nombre,
                            curso.nombre_curso,
                            curso.fecha_inicio_curso,
                            curso.descripcion_curso
                        );

                        exitosos++;
                        Console.WriteLine($"✓ Correo enviado exitosamente a {usuario.email}");
                    }
                    catch (Exception ex)
                    {
                        fallidos++;
                        Console.WriteLine($"✗ Error enviando a {usuario.email}: {ex.Message}");
                    }
                }

                string resultado = $"Notificaciones enviadas: {exitosos} exitosos, {fallidos} fallidos";
                Console.WriteLine($"=== RESULTADO: {resultado} ===");
                return resultado;
            }
            catch (Exception ex)
            {
                string error = $"Error general en NotificarCreacionCurso: {ex.Message}";
                Console.WriteLine($"ERROR: {error}");
                return error;
            }
        }

        public string NotificarCreacionEvento(Evento evento)
        {
            try
            {
                Console.WriteLine($"=== INICIANDO NOTIFICACIÓN DE NUEVO EVENTO ===");
                Console.WriteLine($"Evento: {evento.nombre_evento}");
                Console.WriteLine($"Fecha inicio: {evento.fecha_inicio_evento}");

                // Obtener todos los usuarios con email válido
                var usuarios = usuarioRepository.ConsultarTodos();
                Console.WriteLine($"Total usuarios en BD: {usuarios.Count}");

                var usuariosConEmail = usuarios.Where(u => !string.IsNullOrEmpty(u.email)).ToList();
                Console.WriteLine($"Usuarios con email: {usuariosConEmail.Count}");

                if (usuariosConEmail.Count == 0)
                {
                    string mensaje = "No hay usuarios con email válido para notificar";
                    Console.WriteLine($"ADVERTENCIA: {mensaje}");
                    return mensaje;
                }

                int exitosos = 0;
                int fallidos = 0;

                foreach (var usuario in usuariosConEmail)
                {
                    try
                    {
                        Console.WriteLine($"Enviando correo a: {usuario.email} ({usuario.nombre})");

                        EnviarCorreoNuevoEvento(
                            usuario.email,
                            usuario.nombre,
                            evento.nombre_evento,
                            evento.fecha_inicio_evento,
                            evento.lugar_evento,
                            evento.descripcion_evento
                        );

                        exitosos++;
                        Console.WriteLine($"✓ Correo enviado exitosamente a {usuario.email}");
                    }
                    catch (Exception ex)
                    {
                        fallidos++;
                        Console.WriteLine($"✗ Error enviando a {usuario.email}: {ex.Message}");
                    }
                }

                string resultado = $"Notificaciones enviadas: {exitosos} exitosos, {fallidos} fallidos";
                Console.WriteLine($"=== RESULTADO: {resultado} ===");
                return resultado;
            }
            catch (Exception ex)
            {
                string error = $"Error general en NotificarCreacionEvento: {ex.Message}";
                Console.WriteLine($"ERROR: {error}");
                return error;
            }
        }

        public string EjecutarRecordatoriosDiarios()
        {
            try
            {
                Console.WriteLine("=== EJECUTANDO RECORDATORIOS DIARIOS ===");
                DateTime manana = DateTime.Today.AddDays(1);

                int totalEnviados = 0;

                // Recordatorios de cursos
                var cursosManana = ObtenerCursosParaManana(manana);
                foreach (var curso in cursosManana)
                {
                    totalEnviados += EnviarRecordatoriosCurso(curso);
                }

                // Recordatorios de eventos
                var eventosManana = ObtenerEventosParaManana(manana);
                foreach (var evento in eventosManana)
                {
                    totalEnviados += EnviarRecordatoriosEvento(evento);
                }

                string resultado = $"Total recordatorios enviados: {totalEnviados}";
                Console.WriteLine($"=== {resultado} ===");
                return resultado;
            }
            catch (Exception ex)
            {
                string error = $"Error en recordatorios diarios: {ex.Message}";
                Console.WriteLine($"ERROR: {error}");
                return error;
            }
        }

        public string TestearEnvioCorreo()
        {
            try
            {
                Console.WriteLine("=== TESTING ENVÍO DE CORREO ===");

                // Usar el correo de prueba que ya sabes que funciona
                emailService.EnviarCorreoRegistroEvento(
                    "liceh270324@gmail.com",
                    "Usuario de Prueba",
                    "Evento de Prueba",
                    DateTime.Today
                );

                Console.WriteLine("✓ Correo de prueba enviado exitosamente");
                return "Correo de prueba enviado exitosamente";
            }
            catch (Exception ex)
            {
                string error = $"Error en test de correo: {ex.Message}";
                Console.WriteLine($"ERROR: {error}");
                return error;
            }
        }

        private List<Curso> ObtenerCursosParaManana(DateTime fecha)
        {
            var cursoService = new CursoService();
            var todosCursos = cursoService.Consultar();
            return todosCursos.Where(c => c.fecha_inicio_curso.Date == fecha.Date).ToList();
        }

        private List<Evento> ObtenerEventosParaManana(DateTime fecha)
        {
            var eventoService = new EventoService();
            var todosEventos = eventoService.Consultar();
            return todosEventos.Where(e => e.fecha_inicio_evento.Date == fecha.Date).ToList();
        }

        private int EnviarRecordatoriosCurso(Curso curso)
        {
            try
            {
                var inscritos = inscripcionRepository.ConsultarEstudiantesPorCurso(curso.id_curso);
                int enviados = 0;

                foreach (var usuario in inscritos.Where(u => !string.IsNullOrEmpty(u.email)))
                {
                    try
                    {
                        EnviarRecordatorioCurso(usuario.email, usuario.nombre, curso.nombre_curso, curso.fecha_inicio_curso);
                        enviados++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error enviando recordatorio de curso a {usuario.email}: {ex.Message}");
                    }
                }

                return enviados;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EnviarRecordatoriosCurso: {ex.Message}");
                return 0;
            }
        }

        private int EnviarRecordatoriosEvento(Evento evento)
        {
            try
            {
                var asistentes = asistenciaRepository.ConsultarAsistentesPorEvento(evento.id_evento);
                int enviados = 0;

                foreach (var usuario in asistentes.Where(u => !string.IsNullOrEmpty(u.email)))
                {
                    try
                    {
                        EnviarRecordatorioEvento(usuario.email, usuario.nombre, evento.nombre_evento, evento.fecha_inicio_evento, evento.lugar_evento);
                        enviados++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error enviando recordatorio de evento a {usuario.email}: {ex.Message}");
                    }
                }

                return enviados;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EnviarRecordatoriosEvento: {ex.Message}");
                return 0;
            }
        }

        // Métodos de envío de correo específicos
        private void EnviarCorreoNuevoCurso(string email, string nombre, string nombreCurso, DateTime fechaInicio, string descripcion)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ width: 80%; margin: 20px auto; background-color: white; padding: 20px; border: 1px solid #ddd; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                    .header {{ color: #2E8B57; padding-bottom: 20px; border-bottom: 2px solid #2E8B57; }}
                    .content {{ padding: 20px 0; }}
                    .footer {{ padding-top: 20px; border-top: 2px solid #2E8B57; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#2E8B57;'>¡Nuevo Curso Disponible!</h1>
                    </div>
                    <div class='content'>
                        <p style='font-size:1.2em;'>Hola {nombre},</p>
                        <p>Te informamos que hay un nuevo curso disponible:</p>
                        <h2 style='color:#333;'>{nombreCurso}</h2>
                        <p><strong>Fecha de inicio:</strong> {fechaInicio:dd/MM/yyyy}</p>
                        <p><strong>Descripción:</strong> {descripcion}</p>
                        <p>¡No te pierdas esta oportunidad de aprender!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#2E8B57;'>Equipo de Cursos - EKKLESYS</p>
                    </div>
                </div>
            </body>
            </html>";

            emailService.EnviarCorreoHTML(email, "Nuevo Curso Disponible", html);
        }

        private void EnviarCorreoNuevoEvento(string email, string nombre, string nombreEvento, DateTime fechaInicio, string lugar, string descripcion)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ width: 80%; margin: 20px auto; background-color: white; padding: 20px; border: 1px solid #ddd; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                    .header {{ color: #FF6347; padding-bottom: 20px; border-bottom: 2px solid #FF6347; }}
                    .content {{ padding: 20px 0; }}
                    .footer {{ padding-top: 20px; border-top: 2px solid #FF6347; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#FF6347;'>¡Nuevo Evento Disponible!</h1>
                    </div>
                    <div class='content'>
                        <p style='font-size:1.2em;'>Hola {nombre},</p>
                        <p>Te informamos que hay un nuevo evento disponible:</p>
                        <h2 style='color:#333;'>{nombreEvento}</h2>
                        <p><strong>Fecha:</strong> {fechaInicio:dd/MM/yyyy HH:mm}</p>
                        <p><strong>Lugar:</strong> {lugar}</p>
                        <p><strong>Descripción:</strong> {descripcion}</p>
                        <p>¡Te esperamos!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#FF6347;'>Equipo de Eventos - EKKLESYS</p>
                    </div>
                </div>
            </body>
            </html>";

            emailService.EnviarCorreoHTML(email, "Nuevo Evento Disponible", html);
        }

        private void EnviarRecordatorioCurso(string email, string nombre, string nombreCurso, DateTime fechaInicio)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ width: 80%; margin: 20px auto; background-color: white; padding: 20px; border: 1px solid #ddd; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                    .header {{ color: #4169E1; padding-bottom: 20px; border-bottom: 2px solid #4169E1; }}
                    .content {{ padding: 20px 0; }}
                    .footer {{ padding-top: 20px; border-top: 2px solid #4169E1; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#4169E1;'>Recordatorio de Curso</h1>
                    </div>
                    <div class='content'>
                        <p style='font-size:1.2em;'>Hola {nombre},</p>
                        <p>Te recordamos que mañana inicia el curso:</p>
                        <h2 style='color:#333;'>{nombreCurso}</h2>
                        <p><strong>Fecha:</strong> {fechaInicio:dd/MM/yyyy HH:mm}</p>
                        <p>¡No olvides asistir!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#4169E1;'>Equipo de Cursos - EKKLESYS</p>
                    </div>
                </div>
            </body>
            </html>";

            emailService.EnviarCorreoHTML(email, "Recordatorio: Curso mañana", html);
        }

        private void EnviarRecordatorioEvento(string email, string nombre, string nombreEvento, DateTime fechaInicio, string lugar)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ width: 80%; margin: 20px auto; background-color: white; padding: 20px; border: 1px solid #ddd; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                    .header {{ color: #DC143C; padding-bottom: 20px; border-bottom: 2px solid #DC143C; }}
                    .content {{ padding: 20px 0; }}
                    .footer {{ padding-top: 20px; border-top: 2px solid #DC143C; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#DC143C;'>Recordatorio de Evento</h1>
                    </div>
                    <div class='content'>
                        <p style='font-size:1.2em;'>Hola {nombre},</p>
                        <p>Te recordamos que mañana es el evento:</p>
                        <h2 style='color:#333;'>{nombreEvento}</h2>
                        <p><strong>Fecha:</strong> {fechaInicio:dd/MM/yyyy HH:mm}</p>
                        <p><strong>Lugar:</strong> {lugar}</p>
                        <p>¡Te esperamos!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#DC143C;'>Equipo de Eventos - EKKLESYS</p>
                    </div>
                </div>
            </body>
            </html>";

            emailService.EnviarCorreoHTML(email, "Recordatorio: Evento mañana", html);
        }
    }
}