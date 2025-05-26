using System;

namespace BLL
{
    public class UserState
    {
        public int ItemId { get; set; }
        public bool AwaitingPhoneNumber { get; set; }
        public UserStateType StateType { get; set; }
        public RegistrationStep RegistrationStep { get; set; }
        public AuthenticationStep AuthenticationStep { get; set; }
        public string TempName { get; set; }
        public string TempLastName { get; set; }
        public string TempPhone { get; set; }
        public string TempEmail { get; set; }
        public string AuthPhone { get; set; }
        public string AuthEmail { get; set; }
        public string OriginalAction { get; set; } // Para recordar qué acción quería hacer después de autenticarse
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
