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
        internal Dictionary<long, UserState> _userStates;

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
            if (_userStates.ContainsKey(chatId) && _userStates[chatId].AwaitingPhoneNumber &&
                _userStates[chatId].StateType == UserStateType.Curso)
            {
                // El usuario está en proceso de inscripción y estamos esperando su número de teléfono
                await ProcessPhoneNumberInput(chatId, text, _userStates[chatId].ItemId, ct);
                return;
            }

            // Procesar comandos normales
            if (text.ToLower() == "/cursos")
            {
                await MostrarCursos(chatId);
            }
            else if (text.ToLower() == "/start" || text.ToLower() == "/ayuda")
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "Hola 👋 Bienvenido al bot de cursos y eventos.\n\n" +
                    "Usa el comando /cursos para ver los cursos disponibles.\n" +
                    "Usa el comando /eventos para ver los eventos próximos.",
                    cancellationToken: ct
                );
            }
            else if (text.ToLower() != "/eventos") // Evitar manejar el comando de eventos
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "No entiendo ese comando. Usa /ayuda para ver los comandos disponibles.",
                    cancellationToken: ct
                );
            }
        }

        public async Task MostrarCursos(long chatId)
        {
            var cursos = _cursoService.ConsultarDTO()
                .Where(c => c.fecha_inicio_curso <= DateTime.Now && c.fecha_fin_curso >= DateTime.Now)
                .ToList();

            if (cursos.Count == 0)
            {
                // Crear botón para volver al menú principal
                var menuButtons = new List<InlineKeyboardButton[]>();
                menuButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver al Menú Principal", "menu_principal")
                });

                var menuMarkup = new InlineKeyboardMarkup(menuButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "No hay cursos disponibles en este momento.",
                    replyMarkup: menuMarkup
                );
                return;
            }

            // Crear una matriz de botones inline, con 1 curso por fila
            var inlineKeyboard = new List<InlineKeyboardButton[]>();

            foreach (var curso in cursos)
            {
                inlineKeyboard.Add(new[] {
                    InlineKeyboardButton.WithCallbackData(curso.nombre_curso, "vercurso|" + curso.id_curso)
                });
            }

            // Agregar botón para volver al menú principal
            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🔙 Volver al Menú Principal", "menu_principal")
            });

            var replyMarkup = new InlineKeyboardMarkup(inlineKeyboard);

            await _bot.SendTextMessageAsync(
                chatId,
                "📚 *Cursos disponibles*\n\nSelecciona un curso para ver más información:",
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup
            );
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

                        // Crear botones inline
                        var detailButtons = new List<InlineKeyboardButton[]>();

                        // Botón de inscripción
                        detailButtons.Add(new[] {
                            InlineKeyboardButton.WithCallbackData("🟢 Inscribirse", "inscribirse|" + curso.id_curso)
                        });

                        // Botón para volver a la lista de cursos
                        detailButtons.Add(new[] {
                            InlineKeyboardButton.WithCallbackData("🔙 Volver a cursos", "volvercursos")
                        });

                        // Botón para volver al menú principal
                        detailButtons.Add(new[] {
                            InlineKeyboardButton.WithCallbackData("🏠 Menú Principal", "menu_principal")
                        });

                        var detailMarkup = new InlineKeyboardMarkup(detailButtons);

                        await _bot.SendTextMessageAsync(
                            chatId,
                            mensaje,
                            parseMode: ParseMode.Markdown,
                            replyMarkup: detailMarkup
                        );
                    }
                }
            }
            else if (action == "volvercursos")
            {
                await MostrarCursos(chatId);
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
            _userStates[chatId] = new UserState
            {
                ItemId = cursoId,
                AwaitingPhoneNumber = true,
                StateType = UserStateType.Curso
            };

            // Crear botones inline para cancelar
            var cancelButtons = new List<InlineKeyboardButton[]>();
            cancelButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❌ Cancelar", "vercurso|" + cursoId)
            });

            var cancelMarkup = new InlineKeyboardMarkup(cancelButtons);

            // Solicitar número de teléfono
            var mensaje = "Para inscribirte en el curso *" + curso.nombre_curso + "*, por favor ingresa tu número de teléfono.\n\n" +
                          "Ejemplo: 3001234567";

            await _bot.SendTextMessageAsync(
                chatId,
                mensaje,
                parseMode: ParseMode.Markdown,
                replyMarkup: cancelMarkup
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
                // Crear botones inline para volver a intentar o cancelar
                var errorButtons = new List<InlineKeyboardButton[]>();
                errorButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔄 Intentar nuevamente", "inscribirse|" + cursoId)
                });
                errorButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("❌ Cancelar", "vercurso|" + cursoId)
                });

                var errorMarkup = new InlineKeyboardMarkup(errorButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ El número de teléfono ingresado no es válido. Por favor, intenta nuevamente.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: errorMarkup,
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

                // Crear botones inline para volver
                var notFoundButtons = new List<InlineKeyboardButton[]>();
                notFoundButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver", "vercurso|" + cursoId)
                });

                var notFoundMarkup = new InlineKeyboardMarkup(notFoundButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    mensajeRegistro,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: notFoundMarkup,
                    cancellationToken: ct
                );
                return;
            }

            // Verificar si ya está inscrito en el curso
            if (_inscripcionRepository.ExisteInscripcion(usuario.id_usuario, cursoId))
            {
                // Crear botones inline para volver
                var existsButtons = new List<InlineKeyboardButton[]>();
                existsButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver", "vercurso|" + cursoId)
                });

                var existsMarkup = new InlineKeyboardMarkup(existsButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ Ya estás inscrito en este curso con el número " + phoneNumber + ".",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: existsMarkup,
                    cancellationToken: ct
                );
                return;
            }

            // Verificar si hay cupo disponible (verificación adicional por seguridad)
            var curso = _cursoService.BuscarPorId(cursoId);
            if (curso.NumeroInscritos >= curso.capacidad_max_curso)
            {
                // Crear botones inline para volver
                var noSpaceButtons = new List<InlineKeyboardButton[]>();
                noSpaceButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver", "volvercursos")
                });

                var noSpaceMarkup = new InlineKeyboardMarkup(noSpaceButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Lo sentimos, este curso ya no tiene cupos disponibles.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: noSpaceMarkup,
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

                // Crear botones inline para volver
                var successButtons = new List<InlineKeyboardButton[]>();
                successButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver a cursos", "volvercursos")
                });
                successButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🏠 Menú Principal", "menu_principal")
                });

                var successMarkup = new InlineKeyboardMarkup(successButtons);

                // Obtener nombre y apellido del usuario (ajustado según la estructura de la clase Usuario)
                string nombreCompleto = usuario.nombre;
                if (usuario.apellido_paterno != null)
                {
                    nombreCompleto += " " + usuario.apellido_paterno;
                }

                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ ¡Te has inscrito exitosamente al curso *" + curso.nombre_curso + "*!\n\n" +
                    "Nombre: " + nombreCompleto + "\n" +
                    "Teléfono: " + usuario.telefono + "\n\n" +
                    "Recibirás notificaciones sobre este curso.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: successMarkup,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                // Crear botones inline para volver a intentar o cancelar
                var failButtons = new List<InlineKeyboardButton[]>();
                failButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔄 Intentar nuevamente", "inscribirse|" + cursoId)
                });
                failButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("❌ Cancelar", "volvercursos")
                });

                var failMarkup = new InlineKeyboardMarkup(failButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Ha ocurrido un error al procesar tu inscripción. Por favor, intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: failMarkup,
                    cancellationToken: ct
                );

                // Registrar el error para depuración
                Console.WriteLine("Error al inscribir usuario: " + ex.Message);
            }
        }

        // Método público para verificar si el usuario está en estado de espera
        public bool IsAwaitingPhoneNumber(long chatId)
        {
            return _userStates.ContainsKey(chatId) &&
                   _userStates[chatId].AwaitingPhoneNumber &&
                   _userStates[chatId].StateType == UserStateType.Curso;
        }
    }
}
