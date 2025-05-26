using ENTITY;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRegistrationService
    {
        Task<bool> IniciarRegistro(long chatId);
        Task<bool> ProcesarDatosRegistro(long chatId, string datos);
        Task<Usuario> CompletarRegistro(long chatId);
        bool EstaEnProcesoRegistro(long chatId);
    }
}
