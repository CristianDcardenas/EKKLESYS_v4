using BLL.Interfaces;
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
    public class RegistrationService : IRegistrationService
    {
        private readonly ITelegramBotClient _bot;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly Dictionary<long, UserState> _userStates;

        public RegistrationService(ITelegramBotClient bot, UsuarioRepository usuarioRepository, Dictionary<long, UserState> userStates)
        {
            _bot = bot;
            _usuarioRepository = usuarioRepository;
            _userStates = userStates;
        }

        public async Task<bool> IniciarRegistro(long chatId)
        {
            _userStates[chatId] = new UserState
            {
                StateType = UserStateType.Registro,
                RegistrationStep = RegistrationStep.AwaitingName
            };

            var cancelButtons = new List<InlineKeyboardButton[]>();
            cancelButtons.Add(new[] {
                InlineKeyboardButton.WithCallbackData("❌ Cancelar", "menu_principal")
            });

            var cancelMarkup = new InlineKeyboardMarkup(cancelButtons);

            await _bot.SendTextMessageAsync(
                chatId,
                "📝 *Registro de Usuario*\n\n" +
                "Para registrarte, necesito algunos datos.\n\n" +
                "Por favor, ingresa tu *nombre*:",
                parseMode: ParseMode.Markdown,
                replyMarkup: cancelMarkup
            );

            return true;
        }

        public async Task<bool> ProcesarDatosRegistro(long chatId, string datos)
        {
            if (!_userStates.ContainsKey(chatId) || _userStates[chatId].StateType != UserStateType.Registro)
                return false;

            var state = _userStates[chatId];
            datos = datos.Trim();

            switch (state.RegistrationStep)
            {
                case RegistrationStep.AwaitingName:
                    if (string.IsNullOrEmpty(datos) || datos.Length < 2)
                    {
                        await _bot.SendTextMessageAsync(chatId, "❌ El nombre debe tener al menos 2 caracteres. Intenta nuevamente:");
                        return false;
                    }
                    state.TempName = datos;
                    state.RegistrationStep = RegistrationStep.AwaitingLastName;

                    await _bot.SendTextMessageAsync(
                        chatId,
                        "✅ Nombre registrado: *" + datos + "*\n\n" +
                        "Ahora ingresa tu *apellido paterno*:",
                        parseMode: ParseMode.Markdown
                    );
                    return true;

                case RegistrationStep.AwaitingLastName:
                    if (string.IsNullOrEmpty(datos) || datos.Length < 2)
                    {
                        await _bot.SendTextMessageAsync(chatId, "❌ El apellido debe tener al menos 2 caracteres. Intenta nuevamente:");
                        return false;
                    }
                    state.TempLastName = datos;
                    state.RegistrationStep = RegistrationStep.AwaitingPhone;

                    await _bot.SendTextMessageAsync(
                        chatId,
                        "✅ Apellido registrado: *" + datos + "*\n\n" +
                        "Ahora ingresa tu *número de teléfono*:\n" +
                        "Ejemplo: 3001234567",
                        parseMode: ParseMode.Markdown
                    );
                    return true;

                case RegistrationStep.AwaitingPhone:
                    if (string.IsNullOrEmpty(datos) || datos.Length < 7)
                    {
                        await _bot.SendTextMessageAsync(chatId, "❌ El número de teléfono debe tener al menos 7 dígitos. Intenta nuevamente:");
                        return false;
                    }

                    // Verificar si el teléfono ya existe
                    var usuarioExistente = _usuarioRepository.BuscarPorTelefono(datos);
                    if (usuarioExistente != null)
                    {
                        await _bot.SendTextMessageAsync(
                            chatId,
                            "❌ Ya existe un usuario registrado con este número de teléfono.\n" +
                            "Intenta con otro número:",
                            parseMode: ParseMode.Markdown
                        );
                        return false;
                    }

                    state.TempPhone = datos;
                    state.RegistrationStep = RegistrationStep.AwaitingEmail;

                    await _bot.SendTextMessageAsync(
                        chatId,
                        "✅ Teléfono registrado: *" + datos + "*\n\n" +
                        "Finalmente, ingresa tu *correo electrónico*:\n" +
                        "Ejemplo: usuario@correo.com",
                        parseMode: ParseMode.Markdown
                    );
                    return true;

                case RegistrationStep.AwaitingEmail:
                    if (string.IsNullOrEmpty(datos) || !datos.Contains("@") || !datos.Contains("."))
                    {
                        await _bot.SendTextMessageAsync(chatId, "❌ El correo electrónico no es válido. Intenta nuevamente:");
                        return false;
                    }

                    // Verificar si el email ya existe
                    var usuarioExistenteEmail = _usuarioRepository.BuscarPorEmail(datos);
                    if (usuarioExistenteEmail != null)
                    {
                        await _bot.SendTextMessageAsync(
                            chatId,
                            "❌ Ya existe un usuario registrado con este correo electrónico.\n" +
                            "Intenta con otro correo:",
                            parseMode: ParseMode.Markdown
                        );
                        return false;
                    }

                    state.TempEmail = datos;
                    state.RegistrationStep = RegistrationStep.Completed;
                    return true;
            }

            return false;
        }

        public async Task<Usuario> CompletarRegistro(long chatId)
        {
            if (!_userStates.ContainsKey(chatId) ||
                _userStates[chatId].StateType != UserStateType.Registro ||
                _userStates[chatId].RegistrationStep != RegistrationStep.Completed)
                return null;

            var state = _userStates[chatId];

            try
            {
                var nuevoUsuario = new Usuario
                {
                    nombre = state.TempName,
                    apellido_paterno = state.TempLastName,
                    telefono = state.TempPhone,
                    email = state.TempEmail,
                    clave = "telegram_user", // Clave temporal
                    es_miembro = "N",
                    es_administrador = "N"
                };

                _usuarioRepository.Guardar(nuevoUsuario);

                // Limpiar estado
                _userStates.Remove(chatId);

                var successButtons = new List<InlineKeyboardButton[]>();
                successButtons.Add(new[] {
                    InlineKeyboardButton.WithCallbackData("🏠 Menú Principal", "menu_principal")
                });

                var successMarkup = new InlineKeyboardMarkup(successButtons);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ *¡Registro completado exitosamente!*\n\n" +
                    "📝 Nombre: " + nuevoUsuario.nombre + "\n" +
                    "📝 Apellido: " + nuevoUsuario.apellido_paterno + "\n" +
                    "📞 Teléfono: " + nuevoUsuario.telefono + "\n" +
                    "📧 Correo: " + nuevoUsuario.email + "\n\n" +
                    "Ahora puedes inscribirte a cursos y eventos usando tu teléfono y correo electrónico.",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: successMarkup
                );

                return nuevoUsuario;
            }
            catch (Exception ex)
            {
                _userStates.Remove(chatId);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "❌ Error al completar el registro. Por favor, intenta nuevamente más tarde.",
                    parseMode: ParseMode.Markdown
                );

                Console.WriteLine("Error al registrar usuario: " + ex.Message);
                return null;
            }
        }

        public bool EstaEnProcesoRegistro(long chatId)
        {
            return _userStates.ContainsKey(chatId) &&
                   _userStates[chatId].StateType == UserStateType.Registro;
        }
    }
}
