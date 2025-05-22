using DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BLL
{
    public class TelegramBotService
    {
        private CursoBotController _controller;
        private readonly CursoService _cursoService;
        private readonly EventoService _eventoService;
        private readonly TelegramBotClient _botClient;
        private CancellationTokenSource _cts;

        public TelegramBotService()
        {
            // Usa tu token aquí (considera moverlo a configuración)
            _botClient = new TelegramBotClient("7829823993:AAG7rxd3rqkJV1CpBdNSJZHLYD0yfC0Bwo4");
            _cursoService = new CursoService();
            _eventoService = new EventoService();
            _controller = new CursoBotController(_botClient, _cursoService);

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
            Console.WriteLine($"Bot iniciado: @{me.Username}");
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
                await _controller.HandleMessageAsync(update.Message, cancellationToken);
            }
            else if (update.CallbackQuery != null)
            {
                await _controller.HandleCallbackAsync(update.CallbackQuery);
            }
        }



        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage;

            if (exception is ApiRequestException apiRequestException)
            {
                errorMessage = $"Error de la API de Telegram:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}";
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