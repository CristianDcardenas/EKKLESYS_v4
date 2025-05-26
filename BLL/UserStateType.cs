namespace BLL
{
    public enum UserStateType
    {
        Curso,
        Evento,
        Registro,
        CancelarInscripcion,
        ConsultarInscripciones,
        Autenticacion
    }

    public enum RegistrationStep
    {
        AwaitingName,
        AwaitingLastName,
        AwaitingPhone,
        AwaitingEmail,
        Completed
    }

    public enum AuthenticationStep
    {
        AwaitingPhone,
        AwaitingEmail,
        Completed
    }
}
