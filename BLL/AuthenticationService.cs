using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BLL.Services
{
    public class AuthenticationService
    {
        private readonly ITelegramBotClient _bot;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly Dictionary<long, UserState> _userStates;

        public AuthenticationService(ITelegramBotClient bot, UsuarioRepository usuarioRepository, Dictionary<long, UserState> userStates)
        {
            _bot = bot;
            _usuarioRepository = usuarioRepository;
            _userStates = userStates;
        }

        public async Task IniciarAutenticacion(long chatId, string accionOriginal, int itemId = 0)
        {
            _userStates[chatId] = new UserState
            {
                StateType = UserStateType.Autenticacion,
                AuthenticationStep = AuthenticationStep.AwaitingPhone,
                OriginalAction = accionOriginal,
                ItemId = itemId
            };

            var cancelButtons = new List<InlineKeyboardButton[]>();
            cancelButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❌ Cancelar", "menu_principal")
            });

            var cancelMarkup = new InlineKeyboardMarkup(cancelButtons);

            await _bot.SendTextMessageAsync(
                chatId,
                "🔐 *Autenticación Requerida*\n\n" +
                "Para continuar, necesito verificar tu identidad.\n\n" +
                "Por favor, ingresa tu *número de teléfono*:\n" +
                "Ejemplo: 3001234567",
                parseMode: ParseMode.Markdown,
                replyMarkup: cancelMarkup
            );
        }

        public async Task<bool> ProcesarDatosAutenticacion(long chatId, string datos)
        {
            if (!_userStates.ContainsKey(chatId) || _userStates[chatId].StateType != UserStateType.Autenticacion)
                return false;

            var state = _userStates[chatId];
            datos = datos.Trim();

            switch (state.AuthenticationStep)
            {
                case AuthenticationStep.AwaitingPhone:
                    if (string.IsNullOrEmpty(datos) || datos.Length < 7)
                    {
                        await _bot.SendTextMessageAsync(chatId, "❌ El número de teléfono debe tener al menos 7 dígitos. Intenta nuevamente:");
                        return false;
                    }

                    var usuario = _usuarioRepository.BuscarPorTelefono(datos);
                    if (usuario == null)
                    {
                        await _bot.SendTextMessageAsync(
                            chatId,
                            "❌ No se encontró ningún usuario con ese número de teléfono.\n" +
                            "Verifica el número o regístrate primero.",
                            parseMode: ParseMode.Markdown
                        );
                        return false;
                    }

                    state.AuthPhone = datos;
                    state.AuthenticationStep = AuthenticationStep.AwaitingEmail;

                    await _bot.SendTextMessageAsync(
                        chatId,
                        "📱 Teléfono verificado: *" + datos + "*\n\n" +
                        "Ahora ingresa tu *correo electrónico*:",
                        parseMode: ParseMode.Markdown
                    );
                    return true;

                case AuthenticationStep.AwaitingEmail:
                    if (string.IsNullOrEmpty(datos) || !datos.Contains("@"))
                    {
                        await _bot.SendTextMessageAsync(chatId, "❌ El correo electrónico no es válido. Intenta nuevamente:");
                        return false;
                    }

                    state.AuthEmail = datos;
                    state.AuthenticationStep = AuthenticationStep.Completed;
                    return true;
            }

            return false;
        }

        public async Task<Usuario> ValidarCredenciales(long chatId)
        {
            if (!_userStates.ContainsKey(chatId) ||
                _userStates[chatId].StateType != UserStateType.Autenticacion ||
                _userStates[chatId].AuthenticationStep != AuthenticationStep.Completed)
                return null;

            var state = _userStates[chatId];
            var usuario = _usuarioRepository.BuscarPorTelefono(state.AuthPhone);

            if (usuario == null || usuario.email.ToLower() != state.AuthEmail.ToLower())
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ *Credenciales incorrectas*\n\n" +
                    "El teléfono o el correo electrónico no coinciden.\n" +
                    "Por favor, intenta nuevamente.",
                    parseMode: ParseMode.Markdown
                );

                // Reiniciar el proceso de autenticación
                state.AuthenticationStep = AuthenticationStep.AwaitingPhone;
                state.AuthPhone = null;
                state.AuthEmail = null;

                await IniciarAutenticacion(chatId, state.OriginalAction, state.ItemId);
                return null;
            }

            return usuario;
        }

        public bool EstaEnProcesoAutenticacion(long chatId)
        {
            return _userStates.ContainsKey(chatId) &&
                   _userStates[chatId].StateType == UserStateType.Autenticacion;
        }
    }
}
