using Telegram.Bot.Types;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBotController
    {
        Task HandleMessageAsync(Message message, CancellationToken ct);
        Task HandleCallbackAsync(CallbackQuery query);
        bool IsAwaitingPhoneNumber(long chatId);
        Task MostrarItems(long chatId);
    }
}
