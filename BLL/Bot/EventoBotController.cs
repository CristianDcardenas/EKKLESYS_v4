using BLL.Controllers;
using BLL.Services;
using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BLL
{
    public class EventoBotController : BaseBotController
    {
        private readonly EventoService _eventoService;
        private readonly AsistenciaEventoRepository _asistenciaRepository;
        private readonly AuthenticationService _authService;

        public EventoBotController(ITelegramBotClient bot, EventoService eventoService,
            UsuarioRepository usuarioRepository, AsistenciaEventoRepository asistenciaRepository,
            Dictionary<long, UserState> userStates)
            : base(bot, usuarioRepository, userStates)
        {
            _eventoService = eventoService;
            _asistenciaRepository = asistenciaRepository;
            _authService = new AuthenticationService(bot, usuarioRepository, userStates);
        }

        protected override UserStateType GetStateType() => UserStateType.Evento;
        protected override string GetRetryAction() => "registrarse";
        protected override string GetCancelAction() => "verevento";

        public override async Task HandleMessageAsync(Message message, CancellationToken ct)
        {
            var chatId = message.Chat.Id;
            var text = message.Text?.Trim();

            if (string.IsNullOrEmpty(text)) return;

            // Verificar si está en proceso de autenticación
            if (_authService.EstaEnProcesoAutenticacion(chatId))
            {
                var authExitoso = await _authService.ProcesarDatosAutenticacion(chatId, text);
                if (authExitoso && _userStates.ContainsKey(chatId) &&
                    _userStates[chatId].AuthenticationStep == AuthenticationStep.Completed)
                {
                    var usuario = await _authService.ValidarCredenciales(chatId);
                    if (usuario != null)
                    {
                        await ProcesarAccionPostAutenticacion(chatId, usuario, ct);
                    }
                }
                return;
            }

            // Procesar comandos normales
            if (text.ToLower() == "/eventos")
            {
                await MostrarItems(chatId);
            }
        }

        private async Task ProcesarAccionPostAutenticacion(long chatId, Usuario usuario, CancellationToken ct)
        {
            if (!_userStates.ContainsKey(chatId)) return;

            var state = _userStates[chatId];
            var accion = state.OriginalAction;
            var itemId = state.ItemId;

            // Limpiar estado de autenticación
            _userStates.Remove(chatId);

            switch (accion)
            {
                case "registrarse":
                    await ProcesarRegistro(chatId, usuario, itemId, ct);
                    break;
                case "cancelar_registro":
                    await ProcesarCancelacionRegistro(chatId, usuario, itemId, ct);
                    break;
                case "consultar_registros":
                    await MostrarInscripcionesDelUsuario(chatId, usuario);
                    break;
            }
        }

        public override async Task MostrarItems(long chatId)
        {
            var eventos = _eventoService.ConsultarProximosEventosDTO()
                .Where(e => e.fecha_inicio_evento >= DateTime.Now && e.fecha_fin_evento >= DateTime.Now)
                .ToList();

            if (eventos.Count == 0)
            {
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

            var inlineKeyboard = new List<InlineKeyboardButton[]>();

            foreach (var evento in eventos)
            {
                string eventoText = evento.nombre_evento + " - " + evento.fecha_inicio_evento.ToString("dd/MM/yyyy");
                inlineKeyboard.Add(new[] {
                    InlineKeyboardButton.WithCallbackData(eventoText, "verevento|" + evento.id_evento)
                });
            }

            // Agregar opciones adicionales
            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📋 Mis Registros", "mis_inscripciones_eventos")
            });

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🔙 Volver al Menú Principal", "menu_principal")
            });

            var replyMarkup = new InlineKeyboardMarkup(inlineKeyboard);

            await _bot.SendTextMessageAsync(
                chatId,
                "📅 *Eventos próximos*\n\nSelecciona una opción:",
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup
            );
        }

        public override async Task HandleCallbackAsync(CallbackQuery query)
        {
            var chatId = query.Message.Chat.Id;
            var data = query.Data.Split('|');
            var action = data[0];
            var id = data.Length > 1 ? data[1] : null;

            switch (action)
            {
                case "verevento":
                    if (id != null && int.TryParse(id, out int eventoId))
                        await MostrarDetalleEvento(chatId, eventoId);
                    break;

                case "volvereventos":
                    await MostrarItems(chatId);
                    break;

                case "registrarse":
                    if (id != null && int.TryParse(id, out int registrarseId))
                        await _authService.IniciarAutenticacion(chatId, "registrarse", registrarseId);
                    break;

                case "cancelar_registro":
                    if (id != null && int.TryParse(id, out int cancelId))
                        await _authService.IniciarAutenticacion(chatId, "cancelar_registro", cancelId);
                    break;

                case "mis_inscripciones_eventos":
                    await _authService.IniciarAutenticacion(chatId, "consultar_registros", -1);
                    break;
            }

            await _bot.AnswerCallbackQueryAsync(query.Id);
        }

        private async Task MostrarDetalleEvento(long chatId, int eventoId)
        {
            var evento = _eventoService.BuscarPorId(eventoId);
            if (evento == null) return;

            var mensaje = "📅 *" + evento.nombre_evento + "*\n" +
                          "📍 Lugar: " + evento.lugar_evento + "\n" +
                          "📝 " + evento.descripcion_evento + "\n" +
                          "⏱️ Del " + evento.fecha_inicio_evento.ToString("dd/MM/yyyy HH:mm") + " al " + evento.fecha_fin_evento.ToString("dd/MM/yyyy HH:mm") + "\n" +
                          "👥 Registrados: " + evento.NumeroAsistentes + "/" + evento.capacidad_max_evento;

            var detailButtons = new List<InlineKeyboardButton[]>();

            // Botón de registro
            detailButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🟢 Registrarse", "registrarse|" + evento.id_evento)
            });

            // Botón para cancelar registro
            detailButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❌ Cancelar Registro", "cancelar_registro|" + evento.id_evento)
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

        protected override async Task MostrarInscripcionesDelUsuario(long chatId, Usuario usuario)
        {
            var eventos = _asistenciaRepository.ConsultarEventosPorUsuario(usuario.id_usuario, new EventoRepository(new ConnectionManager()));

            if (eventos.Count == 0)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "📅 No te has registrado a ningún evento actualmente.",
                    parseMode: ParseMode.Markdown
                );
                return;
            }

            var mensaje = "📅 *Tus Eventos Registrados*\n\n";
            foreach (var evento in eventos)
            {
                mensaje += $"📍 {evento.nombre_evento}\n";
                mensaje += $"📅 {evento.fecha_inicio_evento:dd/MM/yyyy HH:mm}\n";
                mensaje += $"🏢 {evento.lugar_evento}\n\n";
            }

            var backButtons = new List<InlineKeyboardButton[]>();
            backButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🔙 Volver a Eventos", "volvereventos")
            });
            backButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🏠 Menú Principal", "menu_principal")
            });

            var backMarkup = new InlineKeyboardMarkup(backButtons);

            await _bot.SendTextMessageAsync(
                chatId,
                mensaje,
                parseMode: ParseMode.Markdown,
                replyMarkup: backMarkup
            );
        }

        private async Task ProcesarCancelacionRegistro(long chatId, Usuario usuario, int eventoId, CancellationToken ct)
        {
            if (!_asistenciaRepository.ExisteAsistencia(usuario.id_usuario, eventoId))
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ No te has registrado en este evento.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            try
            {
                _asistenciaRepository.Eliminar(usuario.id_usuario, eventoId);

                var evento = _eventoService.BuscarPorId(eventoId);
                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ Tu registro al evento *" + evento.nombre_evento + "* ha sido cancelado exitosamente.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Error al cancelar el registro. Intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                Console.WriteLine("Error al cancelar registro: " + ex.Message);
            }
        }

        private async Task ProcesarRegistro(long chatId, Usuario usuario, int eventoId, CancellationToken ct)
        {
            if (_asistenciaRepository.ExisteAsistencia(usuario.id_usuario, eventoId))
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ Ya estás registrado en este evento.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            var evento = _eventoService.BuscarPorId(eventoId);
            if (evento.NumeroAsistentes >= evento.capacidad_max_evento)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Lo sentimos, este evento ya no tiene cupos disponibles.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            var asistencia = new Asistencia_evento
            {
                id_usuario = usuario.id_usuario,
                id_evento = eventoId,
                fecha_asistencia_evento = DateTime.Now
            };

            try
            {
                _asistenciaRepository.Guardar(asistencia);

                string nombreCompleto = usuario.nombre;
                if (!string.IsNullOrEmpty(usuario.apellido_paterno))
                {
                    nombreCompleto += " " + usuario.apellido_paterno;
                }

                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ ¡Te has registrado al evento *" + evento.nombre_evento + "* exitosamente!\n\n" +
                    "Nombre: " + nombreCompleto + "\n" +
                    "Teléfono: " + usuario.telefono + "\n" +
                    "Correo: " + usuario.email + "\n" +
                    "Fecha: " + evento.fecha_inicio_evento.ToString("dd/MM/yyyy HH:mm") + "\n" +
                    "Lugar: " + evento.lugar_evento,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Error al procesar el registro. Intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                Console.WriteLine("Error al registrar en evento: " + ex.Message);
            }
        }
    }
}
