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
    public class CursoBotController : BaseBotController
    {
        private readonly CursoService _cursoService;
        private readonly InscripcionCursoRepository _inscripcionRepository;
        private readonly AuthenticationService _authService;

        public CursoBotController(ITelegramBotClient bot, CursoService cursoService,
            UsuarioRepository usuarioRepository, InscripcionCursoRepository inscripcionRepository,
            Dictionary<long, UserState> userStates)
            : base(bot, usuarioRepository, userStates)
        {
            _cursoService = cursoService;
            _inscripcionRepository = inscripcionRepository;
            _authService = new AuthenticationService(bot, usuarioRepository, userStates);
        }

        protected override UserStateType GetStateType() => UserStateType.Curso;
        protected override string GetRetryAction() => "inscribirse";
        protected override string GetCancelAction() => "vercurso";

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
            if (text.ToLower() == "/cursos")
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
                case "inscribirse":
                    await ProcesarInscripcion(chatId, usuario, itemId, ct);
                    break;
                case "cancelar_inscripcion":
                    await ProcesarCancelacionInscripcion(chatId, usuario, itemId, ct);
                    break;
                case "consultar_inscripciones":
                    await MostrarInscripcionesDelUsuario(chatId, usuario);
                    break;
            }
        }

        public override async Task MostrarItems(long chatId)
        {
            var cursos = _cursoService.ConsultarDTO()
                .Where(c => c.fecha_inicio_curso >= DateTime.Now && c.fecha_fin_curso >= DateTime.Now)
                .ToList();

            if (cursos.Count == 0)
            {
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

            var inlineKeyboard = new List<InlineKeyboardButton[]>();

            foreach (var curso in cursos)
            {
                inlineKeyboard.Add(new[] {
                    InlineKeyboardButton.WithCallbackData(curso.nombre_curso, "vercurso|" + curso.id_curso)
                });
            }

            // Agregar opciones adicionales
            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📋 Mis Inscripciones", "mis_inscripciones_cursos")
            });

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🔙 Volver al Menú Principal", "menu_principal")
            });

            var replyMarkup = new InlineKeyboardMarkup(inlineKeyboard);

            await _bot.SendTextMessageAsync(
                chatId,
                "📚 *Cursos disponibles*\n\nSelecciona una opción:",
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
                case "vercurso":
                    if (id != null && int.TryParse(id, out int cursoId))
                        await MostrarDetalleCurso(chatId, cursoId);
                    break;

                case "volvercursos":
                    await MostrarItems(chatId);
                    break;

                case "inscribirse":
                    if (id != null && int.TryParse(id, out int inscripcionId))
                        await _authService.IniciarAutenticacion(chatId, "inscribirse", inscripcionId);
                    break;

                case "cancelar_inscripcion":
                    if (id != null && int.TryParse(id, out int cancelId))
                        await _authService.IniciarAutenticacion(chatId, "cancelar_inscripcion", cancelId);
                    break;

                case "mis_inscripciones_cursos":
                    await _authService.IniciarAutenticacion(chatId, "consultar_inscripciones", 0);
                    break;
            }

            await _bot.AnswerCallbackQueryAsync(query.Id);
        }

        private async Task MostrarDetalleCurso(long chatId, int cursoId)
        {
            var curso = _cursoService.BuscarPorId(cursoId);
            if (curso == null) return;

            var mensaje = "📖 *" + curso.nombre_curso + "*\n" +
                          "📝 " + curso.descripcion_curso + "\n" +
                          "📅 Del " + curso.fecha_inicio_curso.ToString("dd/MM/yyyy") + " al " + curso.fecha_fin_curso.ToString("dd/MM/yyyy") + "\n" +
                          "👥 Cupo: " + curso.NumeroInscritos + "/" + curso.capacidad_max_curso;

            var detailButtons = new List<InlineKeyboardButton[]>();

            // Botón de inscripción
            detailButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🟢 Inscribirse", "inscribirse|" + curso.id_curso)
            });

            // Botón para cancelar inscripción
            detailButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❌ Cancelar Inscripción", "cancelar_inscripcion|" + curso.id_curso)
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

        protected override async Task MostrarInscripcionesDelUsuario(long chatId, Usuario usuario)
        {
            var cursos = _inscripcionRepository.ConsultarCursosPorUsuario(usuario.id_usuario, new CursoRepository(new ConnectionManager()));

            if (cursos.Count == 0)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "📚 No tienes inscripciones en cursos actualmente.",
                    parseMode: ParseMode.Markdown
                );
                return;
            }

            var mensaje = "📚 *Tus Cursos Inscritos*\n\n";
            foreach (var curso in cursos)
            {
                mensaje += $"📖 {curso.nombre_curso}\n";
                mensaje += $"📅 {curso.fecha_inicio_curso:dd/MM/yyyy} - {curso.fecha_fin_curso:dd/MM/yyyy}\n\n";
            }

            var backButtons = new List<InlineKeyboardButton[]>();
            backButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🔙 Volver a Cursos", "volvercursos")
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

        private async Task ProcesarCancelacionInscripcion(long chatId, Usuario usuario, int cursoId, CancellationToken ct)
        {
            if (!_inscripcionRepository.ExisteInscripcion(usuario.id_usuario, cursoId))
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ No tienes una inscripción activa en este curso.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

            try
            {
                _inscripcionRepository.Eliminar(usuario.id_usuario, cursoId);

                var curso = _cursoService.BuscarPorId(cursoId);
                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ Tu inscripción al curso *" + curso.nombre_curso + "* ha sido cancelada exitosamente.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Error al cancelar la inscripción. Intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                Console.WriteLine("Error al cancelar inscripción: " + ex.Message);
            }
        }

        private async Task ProcesarInscripcion(long chatId, Usuario usuario, int cursoId, CancellationToken ct)
        {
            if (_inscripcionRepository.ExisteInscripcion(usuario.id_usuario, cursoId))
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "ℹ️ Ya estás inscrito en este curso.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                return;
            }

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

            var inscripcion = new Inscripcion_curso
            {
                id_usuario = usuario.id_usuario,
                id_curso = cursoId,
                fecha_inscripcion_curso = DateTime.Now
            };

            try
            {
                _inscripcionRepository.Guardar(inscripcion);

                string nombreCompleto = usuario.nombre;
                if (!string.IsNullOrEmpty(usuario.apellido_paterno))
                {
                    nombreCompleto += " " + usuario.apellido_paterno;
                }

                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ ¡Te has inscrito exitosamente al curso *" + curso.nombre_curso + "*!\n\n" +
                    "Nombre: " + nombreCompleto + "\n" +
                    "Teléfono: " + usuario.telefono + "\n" +
                    "Correo: " + usuario.email,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Error al procesar la inscripción. Intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
                Console.WriteLine("Error al inscribir usuario: " + ex.Message);
            }
        }
    }
}
