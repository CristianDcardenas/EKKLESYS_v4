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
    public class EventoBotController
    {
        private readonly ITelegramBotClient _bot;
        private readonly EventoService _eventoService;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly AsistenciaEventoRepository _asistenciaRepository;

        // Diccionario para mantener el estado de la conversación con cada usuario
        internal Dictionary<long, UserState> _userStates;

        public EventoBotController(ITelegramBotClient bot, EventoService eventoService,
            UsuarioRepository usuarioRepository, AsistenciaEventoRepository asistenciaRepository)
        {
            _bot = bot;
            _eventoService = eventoService;
            _usuarioRepository = usuarioRepository;
            _asistenciaRepository = asistenciaRepository;
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

            // Verificar si el usuario está en proceso de registro de asistencia
            if (_userStates.ContainsKey(chatId) && _userStates[chatId].AwaitingPhoneNumber &&
                _userStates[chatId].StateType == UserStateType.Evento)
            {
                // El usuario está en proceso de registro y estamos esperando su número de teléfono
                await ProcessPhoneNumberInput(chatId, text, _userStates[chatId].ItemId, ct);
                return;
            }

            // Procesar comandos normales
            if (text.ToLower() == "/eventos")
            {
                await MostrarEventos(chatId);
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
        }

        public async Task MostrarEventos(long chatId)
        {
            var eventos = _eventoService.ConsultarProximosEventosDTO()
            .Where(e => e.fecha_inicio_evento >= DateTime.Now && e.fecha_fin_evento >= DateTime.Now)
            .ToList();

            if (eventos.Count == 0)
            {
                // Crear botón para volver al menú principal
                var menuButtons = new List<InlineKeyboardButton[]>();
                menuButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver al Menú Principal", "menu_principal")
                });

                var menuMarkup = new InlineKeyboardMarkup(menuButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "No hay eventos próximos disponibles en este momento.",
                    replyMarkup: menuMarkup
                );
                return;
            }

            // Crear una matriz de botones inline, con 1 evento por fila
            var inlineKeyboard = new List<InlineKeyboardButton[]>();

            foreach (var evento in eventos)
            {
                // Agregar fecha al nombre del evento para mejor contexto
                string eventoText = evento.nombre_evento + " - " + evento.fecha_inicio_evento.ToString("dd/MM/yyyy");

                inlineKeyboard.Add(new[] {
                    InlineKeyboardButton.WithCallbackData(eventoText, "ver evento|" + evento.id_evento)
                });
            }

            // Agregar botón para volver al menú principal
            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🏠 Menú Principal", "menu_principal")
            });

            var replyMarkup = new InlineKeyboardMarkup(inlineKeyboard);

            await _bot.SendTextMessageAsync(
                chatId,
                "📅 *Eventos próximos*\n\nSelecciona un evento para ver más información:",
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

            if (action == "ver evento" && id != null)
            {
                int eventoId;
                if (int.TryParse(id, out eventoId))
                {
                    var evento = _eventoService.BuscarPorId(eventoId);

                    if (evento != null)
                    {
                        var mensaje = "📅 *" + evento.nombre_evento + "*\n" +
                                      "📍 Lugar: " + evento.lugar_evento + "\n" +
                                      "📝 " + evento.descripcion_evento + "\n" +
                                      "⏱️ Del " + evento.fecha_inicio_evento.ToString("dd/MM/yyyy HH:mm") + " al " + evento.fecha_fin_evento.ToString("dd/MM/yyyy HH:mm") + "\n" +
                                      "👥 Asistentes: " + evento.NumeroAsistentes + "/" + evento.capacidad_max_evento;

                        // Crear botones inline
                        var detailButtons = new List<InlineKeyboardButton[]>();

                        // Botón de asistencia
                        detailButtons.Add(new[] {
                            InlineKeyboardButton.WithCallbackData("🟢 Asistir", "asistir|" + evento.id_evento)
                        });

                        // Botón para volver a la lista de eventos
                        detailButtons.Add(new[] {
                            InlineKeyboardButton.WithCallbackData("🔙 Volver a eventos", "volvereventos")
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
            else if (action == "volvereventos")
            {
                await MostrarEventos(chatId);
            }
            else if (action == "asistir" && id != null)
            {
                int eventoId;
                if (int.TryParse(id, out eventoId))
                {
                    // Iniciar proceso de registro de asistencia solicitando número de teléfono
                    await SolicitarNumeroTelefono(chatId, eventoId);
                }
            }

            await _bot.AnswerCallbackQueryAsync(query.Id);
        }

        private async Task SolicitarNumeroTelefono(long chatId, int eventoId)
        {
            // Verificar si hay cupo disponible
            var evento = _eventoService.BuscarPorId(eventoId);
            if (evento.NumeroAsistentes >= evento.capacidad_max_evento)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Lo sentimos, este evento ya no tiene cupos disponibles.",
                    parseMode: ParseMode.Markdown
                );
                return;
            }

            // Guardar el estado del usuario (esperando número de teléfono para asistencia)
            _userStates[chatId] = new UserState
            {
                ItemId = eventoId,
                AwaitingPhoneNumber = true,
                StateType = UserStateType.Evento
            };

            // Crear botones inline para cancelar
            var cancelButtons = new List<InlineKeyboardButton[]>();
            cancelButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❌ Cancelar", "ver evento|" + eventoId)
            });

            var cancelMarkup = new InlineKeyboardMarkup(cancelButtons);

            // Solicitar número de teléfono
            var mensaje = "Para registrar tu asistencia al evento *" + evento.nombre_evento + "*, por favor ingresa tu número de teléfono.\n\n" +
                          "Ejemplo: 3001234567";

            await _bot.SendTextMessageAsync(
                chatId,
                mensaje,
                parseMode: ParseMode.Markdown,
                replyMarkup: cancelMarkup
            );
        }

        private async Task ProcessPhoneNumberInput(long chatId, string phoneNumber, int eventoId, CancellationToken ct)
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
                    InlineKeyboardButton.WithCallbackData("🔄 Intentar nuevamente", "asistir|" + eventoId)
                });
                errorButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("❌ Cancelar", "ver evento|" + eventoId)
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
                                     "Por favor, regístrate primero en nuestra plataforma antes de asistir a un evento.";

                // Crear botones inline para volver
                var notFoundButtons = new List<InlineKeyboardButton[]>();
                notFoundButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver", "ver evento|" + eventoId)
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

            // Verificar si ya está registrado en el evento
            if (_asistenciaRepository.ExisteAsistencia(usuario.id_usuario, eventoId))
            {
                // Crear botones inline para volver
                var existsButtons = new List<InlineKeyboardButton[]>();
                existsButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver", "ver evento|" + eventoId)
                });

                var existsMarkup = new InlineKeyboardMarkup(existsButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ Ya estás registrado para asistir a este evento con el número " + phoneNumber + ".",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: existsMarkup,
                    cancellationToken: ct
                );
                return;
            }

            // Verificar si hay cupo disponible (verificación adicional por seguridad)
            var evento = _eventoService.BuscarPorId(eventoId);
            if (evento.NumeroAsistentes >= evento.capacidad_max_evento)
            {
                // Crear botones inline para volver
                var noSpaceButtons = new List<InlineKeyboardButton[]>();
                noSpaceButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver", "volvereventos")
                });

                var noSpaceMarkup = new InlineKeyboardMarkup(noSpaceButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Lo sentimos, este evento ya no tiene cupos disponibles.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: noSpaceMarkup,
                    cancellationToken: ct
                );
                return;
            }

            // Registrar asistencia
            var asistencia = new Asistencia_evento();
            asistencia.id_usuario = usuario.id_usuario;
            asistencia.id_evento = eventoId;
            asistencia.fecha_asistencia_evento = DateTime.Now;

            try
            {
                _asistenciaRepository.Guardar(asistencia);

                // Crear botones inline para volver
                var successButtons = new List<InlineKeyboardButton[]>();
                successButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔙 Volver a eventos", "volvereventos")
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
                    "✅ ¡Has registrado tu asistencia al evento *" + evento.nombre_evento + "* exitosamente!\n\n" +
                    "Nombre: " + nombreCompleto + "\n" +
                    "Teléfono: " + usuario.telefono + "\n" +
                    "Fecha: " + evento.fecha_inicio_evento.ToString("dd/MM/yyyy HH:mm") + "\n" +
                    "Lugar: " + evento.lugar_evento + "\n\n" +
                    "Recibirás notificaciones sobre este evento.",
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
                    InlineKeyboardButton.WithCallbackData("🔄 Intentar nuevamente", "asistir|" + eventoId)
                });
                failButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("❌ Cancelar", "volvereventos")
                });

                var failMarkup = new InlineKeyboardMarkup(failButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Ha ocurrido un error al registrar tu asistencia. Por favor, intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: failMarkup,
                    cancellationToken: ct
                );

                // Registrar el error para depuración
                Console.WriteLine("Error al registrar asistencia: " + ex.Message);
            }
        }

        // Método público para verificar si el usuario está en estado de espera
        public bool IsAwaitingPhoneNumber(long chatId)
        {
            return _userStates.ContainsKey(chatId) &&
                   _userStates[chatId].AwaitingPhoneNumber &&
                   _userStates[chatId].StateType == UserStateType.Evento;
        }
    }
}
