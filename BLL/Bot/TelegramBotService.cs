using BLL.Interfaces;
using BLL.Services;
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
        private IRegistrationService _registrationService;
        private AuthenticationService _authenticationService;
        private readonly CursoService _cursoService;
        private readonly EventoService _eventoService;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly InscripcionCursoRepository _inscripcionRepository;
        private readonly AsistenciaEventoRepository _asistenciaRepository;
        private readonly TelegramBotClient _botClient;
        private CancellationTokenSource _cts;

        // Estado compartido entre controladores
        private readonly Dictionary<long, UserState> _sharedUserStates;

        public TelegramBotService()
        {
            _botClient = new TelegramBotClient("7829823993:AAG7rxd3rqkJV1CpBdNSJZHLYD0yfC0Bwo4");

            var connectionManager = new ConnectionManager();
            _cursoService = new CursoService();
            _eventoService = new EventoService();
            _usuarioRepository = new UsuarioRepository(connectionManager);
            _inscripcionRepository = new InscripcionCursoRepository(connectionManager, _usuarioRepository);
            _asistenciaRepository = new AsistenciaEventoRepository(connectionManager, _usuarioRepository);

            // Estado compartido
            _sharedUserStates = new Dictionary<long, UserState>();

            // Inicializar servicios
            _registrationService = new RegistrationService(_botClient, _usuarioRepository, _sharedUserStates);
            _authenticationService = new AuthenticationService(_botClient, _usuarioRepository, _sharedUserStates);

            // Inicializar controladores con estado compartido
            _cursoController = new CursoBotController(_botClient, _cursoService, _usuarioRepository, _inscripcionRepository, _sharedUserStates);
            _eventoController = new EventoBotController(_botClient, _eventoService, _usuarioRepository, _asistenciaRepository, _sharedUserStates);
        }

        public async Task StartBotAsync()
        {
            _cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] { }
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

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

                // Verificar si está en proceso de registro
                if (_registrationService.EstaEnProcesoRegistro(chatId))
                {
                    var registroExitoso = await _registrationService.ProcesarDatosRegistro(chatId, update.Message.Text);
                    if (registroExitoso && _sharedUserStates.ContainsKey(chatId) &&
                        _sharedUserStates[chatId].RegistrationStep == RegistrationStep.Completed)
                    {
                        await _registrationService.CompletarRegistro(chatId);
                    }
                    return;
                }

                // Verificar si está en proceso de autenticación y delegar al controlador apropiado
                if (_authenticationService.EstaEnProcesoAutenticacion(chatId))
                {
                    var state = _sharedUserStates[chatId];
                    switch (state.OriginalAction)
                    {
                        case "inscribirse":
                        case "cancelar_inscripcion":
                        case "consultar_inscripciones":
                            await _cursoController.HandleMessageAsync(update.Message, cancellationToken);
                            break;

                        case "registrarse":
                        case "cancelar_registro":
                        case "consultar_registros":
                            await _eventoController.HandleMessageAsync(update.Message, cancellationToken);
                            break;

                        default:
                            // Si no reconoce la acción, enviar al menú principal
                            await ShowMainMenu(chatId, cancellationToken);
                            break;
                    }
                    return;
                }

                // Verificar si hay algún estado activo para este usuario
                if (_sharedUserStates.ContainsKey(chatId))
                {
                    var userState = _sharedUserStates[chatId];

                    // Manejar estados relacionados con cursos
                    if (userState.StateType == UserStateType.Curso)
                    {
                        await _cursoController.HandleMessageAsync(update.Message, cancellationToken);
                        return;
                    }

                    // Manejar estados relacionados con eventos
                    if (userState.StateType == UserStateType.Evento)
                    {
                        await _eventoController.HandleMessageAsync(update.Message, cancellationToken);
                        return;
                    }
                }

                // Mostrar menú principal para cualquier mensaje sin estado
                await ShowMainMenu(chatId, cancellationToken);
            }
            else if (update.CallbackQuery != null)
            {
                var action = update.CallbackQuery.Data.Split('|')[0];
                var chatId = update.CallbackQuery.Message.Chat.Id;

                // Manejar acciones del menú principal
                switch (action)
                {
                    case "menu_cursos":
                        await _cursoController.MostrarItems(chatId);
                        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        break;

                    case "menu_eventos":
                        await _eventoController.MostrarItems(chatId);
                        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        break;

                    case "menu_registro":
                        await _registrationService.IniciarRegistro(chatId);
                        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        break;

                    case "iniciar_registro":
                        await _registrationService.IniciarRegistro(chatId);
                        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        break;

                    case "menu_ayuda":
                        await MostrarAyuda(chatId);
                        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        break;

                    case "menu_principal":
                        await ShowMainMenu(chatId, cancellationToken);
                        await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        break;

                    // Manejar acciones específicas de eventos
                    case "verevento":
                    case "registrarse":
                    case "volvereventos":
                    case "cancelar_registro":
                    case "mis_inscripciones_eventos":
                        await _eventoController.HandleCallbackAsync(update.CallbackQuery);
                        break;

                    // Manejar acciones específicas de cursos
                    case "vercurso":
                    case "inscribirse":
                    case "volvercursos":
                    case "cancelar_inscripcion":
                    case "mis_inscripciones_cursos":
                        await _cursoController.HandleCallbackAsync(update.CallbackQuery);
                        break;

                    // Cualquier otra acción va a cursos por defecto
                    default:
                        await _cursoController.HandleCallbackAsync(update.CallbackQuery);
                        break;
                }
            }
        }

        private async Task ShowMainMenu(long chatId, CancellationToken cancellationToken)
        {
            var inlineKeyboard = new List<InlineKeyboardButton[]>();

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📚 Ver Cursos", "menu_cursos")
            });

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📅 Ver Eventos", "menu_eventos")
            });

            inlineKeyboard.Add(new[] {
                InlineKeyboardButton.WithCallbackData("📝 Registrarse", "menu_registro")
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
                          "📅 *Ver Eventos*: Descubre los próximos eventos y regístrate.\n\n" +
                          "📝 *Registrarse*: Crea tu cuenta con nombre, apellido, teléfono y correo electrónico.\n\n" +
                          "📋 *Consultar Inscripciones*: Ve tus cursos y eventos registrados.\n\n" +
                          "❌ *Cancelar Inscripciones*: Cancela tu participación en cursos o eventos.\n\n" +
                          "🔐 *Seguridad*: Para acceder a las funciones necesitarás tu teléfono y correo electrónico.";

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

            if (exception is ApiRequestException apiRequestException)
            {
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
