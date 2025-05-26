using BLL.Interfaces;
using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BLL.Controllers
{
    public abstract class BaseBotController : IBotController
    {
        protected readonly ITelegramBotClient _bot;
        protected readonly UsuarioRepository _usuarioRepository;
        protected readonly Dictionary<long, UserState> _userStates;

        protected BaseBotController(ITelegramBotClient bot, UsuarioRepository usuarioRepository, Dictionary<long, UserState> userStates)
        {
            _bot = bot;
            _usuarioRepository = usuarioRepository;
            _userStates = userStates;
        }

        public abstract Task HandleMessageAsync(Message message, CancellationToken ct);
        public abstract Task HandleCallbackAsync(CallbackQuery query);
        public abstract Task MostrarItems(long chatId);
        protected abstract UserStateType GetStateType();

        public virtual bool IsAwaitingPhoneNumber(long chatId)
        {
            return _userStates.ContainsKey(chatId) &&
                   _userStates[chatId].AwaitingPhoneNumber &&
                   _userStates[chatId].StateType == GetStateType();
        }

        protected async Task MostrarInscripcionesUsuario(long chatId, string telefono)
        {
            var usuario = _usuarioRepository.BuscarPorTelefono(telefono);
            if (usuario == null)
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ No se encontró ningún usuario con ese número de teléfono.",
                    parseMode: ParseMode.Markdown
                );
                return;
            }

            await MostrarInscripcionesDelUsuario(chatId, usuario);
        }

        protected abstract Task MostrarInscripcionesDelUsuario(long chatId, Usuario usuario);

        protected virtual async Task<bool> ValidarNumeroTelefono(long chatId, string phoneNumber, int itemId)
        {
            phoneNumber = phoneNumber.Trim();
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 7)
            {
                var errorButtons = new List<InlineKeyboardButton[]>();
                errorButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🔄 Intentar nuevamente", GetRetryAction() + "|" + itemId)
                });
                errorButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("❌ Cancelar", GetCancelAction() + "|" + itemId)
                });

                var errorMarkup = new InlineKeyboardMarkup(errorButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ El número de teléfono ingresado no es válido. Por favor, intenta nuevamente.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: errorMarkup
                );
                return false;
            }
            return true;
        }

        protected abstract string GetRetryAction();
        protected abstract string GetCancelAction();
    }
}
