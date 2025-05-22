namespace BLL
{
    // Clase común para mantener el estado de la conversación con el usuario
    public class UserState
    {
        public int ItemId { get; set; } // ID genérico (puede ser curso o evento)
        public bool AwaitingPhoneNumber { get; set; }
        public UserStateType StateType { get; set; }
    }

    public enum UserStateType
    {
        Curso,
        Evento
    }
}
