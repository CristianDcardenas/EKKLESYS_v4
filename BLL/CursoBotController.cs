using DAL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace BLL
{
    public class CursoBotController
    {
        private readonly ITelegramBotClient _bot;
        private readonly CursoService _cursoService;

        public CursoBotController(ITelegramBotClient bot, CursoService cursoService)
        {
            _bot = bot;
            _cursoService = cursoService;
        }

        public async Task HandleMessageAsync(Message message, CancellationToken ct)
        {
            var chatId = message.Chat.Id;
            var text = message.Text?.Trim().ToLower();

            if (text == "/cursos")
            {
                var cursos = _cursoService.ConsultarDTO();

                if (cursos.Count == 0)
                {
                    await _bot.SendTextMessageAsync(chatId, "No hay cursos disponibles.", cancellationToken: ct);
                    return;
                }

                var menu = cursos.Select(c =>
                    new[] { InlineKeyboardButton.WithCallbackData($"{c.nombre_curso}", $"vercurso|{c.id_curso}") }
                ).ToArray();

                var replyMarkup = new InlineKeyboardMarkup(menu);

                await _bot.SendTextMessageAsync(
                    chatId,
                    "📚 Cursos disponibles:\n\nSelecciona uno para más información.",
                    replyMarkup: replyMarkup,
                    cancellationToken: ct
                );
            }
            else
            {
                await _bot.SendTextMessageAsync(
                    chatId,
                    "Hola 👋 Usa el comando /cursos para ver los cursos disponibles.",
                    cancellationToken: ct
                );
            }
        }

        public async Task HandleCallbackAsync(CallbackQuery query)
        {
            var chatId = query.Message.Chat.Id;
            var data = query.Data.Split('|');
            var action = data[0];
            var id = data.Length > 1 ? data[1] : null;

            if (action == "vercurso" && int.TryParse(id, out int cursoId))
            {
                var curso = _cursoService.BuscarPorId(cursoId); // Asegúrate de tener este método

                if (curso != null)
                {
                    var mensaje = $"📖 *{curso.nombre_curso}*\n" +
                                  $"📝 {curso.descripcion_curso}\n" +
                                  $"📅 Del {curso.fecha_inicio_curso:dd/MM/yyyy} al {curso.fecha_fin_curso:dd/MM/yyyy}\n" +
                                  $"👥 Cupo: {curso.NumeroInscritos}/{curso.capacidad_max_curso}";

                    var botones = new InlineKeyboardMarkup(new[]
                    {
                    InlineKeyboardButton.WithCallbackData("🟢 Inscribirse", $"inscribirse|{curso.id_curso}")
                });

                    await _bot.SendTextMessageAsync(
                        chatId,
                        mensaje,
                        parseMode: ParseMode.Markdown,
                        replyMarkup: botones
                    );
                }
            }
            else if (action == "inscribirse" && int.TryParse(id, out int cursoIdInsc))
            {
                // Aquí puedes agregar lógica real para inscribir al usuario en la base de datos
                // Simulación de inscripción exitosa:
                await _bot.SendTextMessageAsync(
                    chatId,
                    "✅ ¡Te has inscrito exitosamente al curso!",
                    cancellationToken: default
                );
            }

            await _bot.AnswerCallbackQueryAsync(query.Id);
        }
    }
}
