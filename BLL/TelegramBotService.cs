using DAL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BLL
{
    public class TelegramBotService
    {
        private CursoBotController _cursoController;
        private EventoBotController _eventoController;
        private readonly CursoService _cursoService;
        private readonly EventoService _eventoService;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly InscripcionCursoRepository _inscripcionRepository;
        private readonly AsistenciaEventoRepository _asistenciaRepository;
        private readonly TelegramBotClient _botClient;
        private CancellationTokenSource _cts;

        public TelegramBotService()
        {
            // Usa tu token aquí (considera moverlo a configuración)
            _botClient = new TelegramBotClient("7829823993:AAG7rxd3rqkJV1CpBdNSJZHLYD0yfC0Bwo4");

            // Inicializar servicios y repositorios
            var connectionManager = new ConnectionManager();
            _cursoService = new CursoService();
            _eventoService = new EventoService();
            _usuarioRepository = new UsuarioRepository(connectionManager);
            _inscripcionRepository = new InscripcionCursoRepository(connectionManager, _usuarioRepository);
            _asistenciaRepository = new AsistenciaEventoRepository(connectionManager, _usuarioRepository);

            // Inicializar los controladores con todas las dependencias
            _cursoController = new CursoBotController(_botClient, _cursoService, _usuarioRepository, _inscripcionRepository);
            _eventoController = new EventoBotController(_botClient, _eventoService, _usuarioRepository, _asistenciaRepository);
        }

        public async Task StartBotAsync()
        {
            _cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] { } // Forma compatible con C# 7.3
            };

            // Inicia el bot (versión compatible)
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            // Obtiene información del bot
            var me = await _botClient.GetMeAsync();
            Console.WriteLine("Bot iniciado: @" + me.Username);
        }

        public async Task StopBotAsync()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null && update.Message.Text != null)
            {
                var text = update.Message.Text.Trim().ToLower();
                var chatId = update.Message.Chat.Id;

                // Verificar si el usuario está en proceso de inscripción o asistencia
                if (_cursoController.IsAwaitingPhoneNumber(chatId))
                {
                    await _cursoController.HandleMessageAsync(update.Message, cancellationToken);
                    return;
                }
                else if (_eventoController.IsAwaitingPhoneNumber(chatId))
                {
                    await _eventoController.HandleMessageAsync(update.Message, cancellationToken);
                    return;
                }

                // Si el mensaje es /start o cualquier otro comando, mostrar el menú principal
                await ShowMainMenu(chatId, cancellationToken);
            }
            else if (update.CallbackQuery != null)
            {
                var action = update.CallbackQuery.Data.Split('|')[0];
                var chatId = update.CallbackQuery.Message.Chat.Id;

                // Manejar acciones del menú principal
                if (action == "menu_cursos")
                {
                    await _cursoController.MostrarCursos(chatId);
                    await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                }
                else if (action == "menu_eventos")
                {
                    await _eventoController.MostrarEventos(chatId);
                    await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                }
                else if (action == "menu_ayuda")
                {
                    await MostrarAyuda(chatId);
                    await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                }
                else if (action == "menu_principal")
                {
                    await ShowMainMenu(chatId, cancellationToken);
                    await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                }
                // Manejar acciones específicas de cursos y eventos
                else if (action == "verevento" || action == "asistir" || action == "volvereventos")
                {
                    await _eventoController.HandleCallbackAsync(update.CallbackQuery);
                }
                else
                {
                    await _cursoController.HandleCallbackAsync(update.CallbackQuery);
                }
            }
        }

        private async Task ShowMainMenu(long chatId, CancellationToken cancellationToken)
        {
            // Crear botones para el menú principal
            var inlineKeyboard = new List<InlineKeyboardButton[]>();

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📚 Ver Cursos", "menu_cursos")
            });

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📅 Ver Eventos", "menu_eventos")
            });

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❓ Ayuda", "menu_ayuda")
            });

            var replyMarkup = new InlineKeyboardMarkup(inlineKeyboard);

            await _botClient.SendTextMessageAsync(
                chatId,
                "👋 *Bienvenido al Bot de Cursos y Eventos*\n\n" +
                "Selecciona una opción para continuar:",
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup,
                cancellationToken: cancellationToken
            );
        }

        private async Task MostrarAyuda(long chatId)
        {
            var mensaje = "❓ *Ayuda*\n\n" +
                          "Este bot te permite:\n\n" +
                          "📚 *Ver Cursos*: Explora los cursos disponibles e inscríbete.\n\n" +
                          "📅 *Ver Eventos*: Descubre los próximos eventos y registra tu asistencia.\n\n" +
                          "Para inscribirte a un curso o asistir a un evento, necesitarás proporcionar tu número de teléfono para verificar tu registro en nuestra plataforma.";

            // Agregar botón para volver al menú principal
            var inlineKeyboard = new List<InlineKeyboardButton[]>();
            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("🔙 Volver al Menú Principal", "menu_principal")
            });

            var replyMarkup = new InlineKeyboardMarkup(inlineKeyboard);

            await _botClient.SendTextMessageAsync(
                chatId,
                mensaje,
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup
            );
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage;

            if (exception is ApiRequestException)
            {
                ApiRequestException apiRequestException = (ApiRequestException)exception;
                errorMessage = "Error de la API de Telegram:\n[" + apiRequestException.ErrorCode + "]\n" + apiRequestException.Message;
            }
            else
            {
                errorMessage = exception.ToString();
            }

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}
