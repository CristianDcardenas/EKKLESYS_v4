using DAL;
using ENTITY;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    public class CursoBotController
    {
        private readonly ITelegramBotClient _bot;
        private readonly CursoService _cursoService;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly InscripcionCursoRepository _inscripcionRepository;

        // Diccionario para mantener el estado de la conversación con cada usuario
        private Dictionary<long, UserState> _userStates;

        public CursoBotController(ITelegramBotClient bot, CursoService cursoService,
            UsuarioRepository usuarioRepository, InscripcionCursoRepository inscripcionRepository)
        {
            _bot = bot;
            _cursoService = cursoService;
            _usuarioRepository = usuarioRepository;
            _inscripcionRepository = inscripcionRepository;
            _userStates = new Dictionary<long, UserState>();
        }

        public async Task HandleMessageAsync(Message message, CancellationToken ct)
        {
            var chatId = message.Chat.Id;
            var text = message.Text;
            if (text != null)
            {
                text = text.Trim();
            }
            else
            {
                // Si el mensaje no tiene texto, ignorarlo
                return;
            }

            // Verificar si el usuario está en proceso de inscripción
            if (_userStates.ContainsKey(chatId) && _userStates[chatId].AwaitingPhoneNumber)
            {
                // El usuario está en proceso de inscripción y estamos esperando su número de teléfono
                await ProcessPhoneNumberInput(chatId, text, _userStates[chatId].CourseId, ct);
                return;
            }

            // Procesar comandos normales
            if (text.ToLower() == "/cursos")
            {
                var cursos = _cursoService.ConsultarDTO();

                if (cursos.Count == 0)
                {
                    await _bot.SendTextMessageAsync(chatId, "No hay cursos disponibles.", cancellationToken: ct);
                    return;
                }

                var menu = cursos.Select(c =>
                    new[] { InlineKeyboardButton.WithCallbackData(c.nombre_curso, "vercurso|" + c.id_curso) }
                ).ToArray();

                var replyMarkup = new InlineKeyboardMarkup(menu);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "📚 Cursos disponibles:\n\nSelecciona uno para más información.",
                    replyMarkup: replyMarkup,
                    cancellationToken: ct
                );
            }
            else if (text.ToLower() == "/start" || text.ToLower() == "/ayuda")
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "Hola 👋 Bienvenido al bot de cursos.\n\n" +
                    "Usa el comando /cursos para ver los cursos disponibles.",
                    cancellationToken: ct
                );
            }
            else
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "No entiendo ese comando. Usa /ayuda para ver los comandos disponibles.",
                    cancellationToken: ct
                );
            }
        }

        public async Task HandleCallbackAsync(CallbackQuery query)
        {
            var chatId = query.Message.Chat.Id;
            var data = query.Data.Split('|');
            var action = data[0];
            var id = data.Length > 1 ? data[1] : null;

            if (action == "vercurso" && id != null)
            {
                int cursoId;
                if (int.TryParse(id, out cursoId))
                {
                    var curso = _cursoService.BuscarPorId(cursoId);

                    if (curso != null)
                    {
                        var mensaje = "📖 *" + curso.nombre_curso + "*\n" +
                                      "📝 " + curso.descripcion_curso + "\n" +
                                      "📅 Del " + curso.fecha_inicio_curso.ToString("dd/MM/yyyy") + " al " + curso.fecha_fin_curso.ToString("dd/MM/yyyy") + "\n" +
                                      "👥 Cupo: " + curso.NumeroInscritos + "/" + curso.capacidad_max_curso;

                        var botones = new InlineKeyboardMarkup(new[]
                        {
                            InlineKeyboardButton.WithCallbackData("🟢 Inscribirse", "inscribirse|" + curso.id_curso)
                        });

                        await _bot.SendTextMessageAsync(
                            chatId,
                            mensaje,
                            parseMode: ParseMode.Markdown,
                            replyMarkup: botones
                        );
                    }
                }
            }
            else if (action == "inscribirse" && id != null)
            {
                int cursoId;
                if (int.TryParse(id, out cursoId))
                {
                    // Iniciar proceso de inscripción solicitando número de teléfono
                    await SolicitarNumeroTelefono(chatId, cursoId);
                }
            }

            await _bot.AnswerCallbackQueryAsync(query.Id);
        }

        private async Task SolicitarNumeroTelefono(long chatId, int cursoId)
        {
            // Verificar si hay cupo disponible
            var curso = _cursoService.BuscarPorId(cursoId);
            if (curso.NumeroInscritos >= curso.capacidad_max_curso)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Lo sentimos, este curso ya no tiene cupos disponibles.",
                    parseMode: ParseMode.Markdown
                );
                return;
            }

            // Guardar el estado del usuario (esperando número de teléfono para inscripción)
            _userStates[chatId] = new UserState { CourseId = cursoId, AwaitingPhoneNumber = true };

            // Solicitar número de teléfono
            var mensaje = "Para inscribirte en el curso *" + curso.nombre_curso + "*, por favor ingresa tu número de teléfono.\n\n" +
                          "Ejemplo: 3001234567";

            await _bot.SendTextMessageAsync(
                chatId,
                mensaje,
                parseMode: ParseMode.Markdown
            );
        }

        private async Task ProcessPhoneNumberInput(long chatId, string phoneNumber, int cursoId, CancellationToken ct)
        {
            // Limpiar el estado del usuario
            _userStates.Remove(chatId);

            // Validar formato del número de teléfono (implementación básica)
            phoneNumber = phoneNumber.Trim();
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 7)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ El número de teléfono ingresado no es válido. Por favor, intenta nuevamente con el comando /cursos.",
                    cancellationToken: ct
                );
                return;
            }

            // Buscar si el usuario existe en la base de datos
            var usuario = _usuarioRepository.BuscarPorTelefono(phoneNumber);

            if (usuario == null)
            {
                // El usuario no existe, enviar mensaje para que se registre
                var mensajeRegistro = "⚠️ No encontramos ningún usuario registrado con el número " + phoneNumber + ".\n\n" +
                                     "Por favor, regístrate primero en nuestra plataforma antes de inscribirte a un curso.";

                await _bot.SendTextMessageAsync(
                    chatId,
                    mensajeRegistro,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            // Verificar si ya está inscrito en el curso
            if (_inscripcionRepository.ExisteInscripcion(usuario.id_usuario, cursoId))
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ Ya estás inscrito en este curso con el número " + phoneNumber + ".",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            // Verificar si hay cupo disponible (verificación adicional por seguridad)
            var curso = _cursoService.BuscarPorId(cursoId);
            if (curso.NumeroInscritos >= curso.capacidad_max_curso)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Lo sentimos, este curso ya no tiene cupos disponibles.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            // Realizar la inscripción
            var inscripcion = new Inscripcion_curso();
            inscripcion.id_usuario = usuario.id_usuario;
            inscripcion.id_curso = cursoId;
            inscripcion.fecha_inscripcion_curso = DateTime.Now;

            try
            {
                _inscripcionRepository.Guardar(inscripcion);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ ¡Te has inscrito exitosamente al curso *" + curso.nombre_curso + "*!\n\n" +
                    "Nombre: " + usuario.nombre + " " + usuario.apellido_paterno + "\n" +
                    "Teléfono: " + usuario.telefono + "\n\n" +
                    "Recibirás notificaciones sobre este curso.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Ha ocurrido un error al procesar tu inscripción. Por favor, intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );

                // Registrar el error para depuración
                Console.WriteLine("Error al inscribir usuario: " + ex.Message);
            }
        }
    }

    // Clase para mantener el estado de la conversación con el usuario
    public class UserState
    {
        public int CourseId { get; set; }
        public bool AwaitingPhoneNumber { get; set; }
    }
}
