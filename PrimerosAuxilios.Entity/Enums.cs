namespace PrimerosAuxilios.Entity
{
    /// <summary>
    /// Resultado del proceso de registro de usuario
    /// </summary>
    public enum RegistrationResult
    {
        Success,
        UsernameExists,
        EmailExists
    }

    /// <summary>
    /// Resultado del proceso de recuperación de contraseña
    /// </summary>
    public enum PasswordResetResult
    {
        Success,
        EmailNotFound,
        EmailSendFailed
    }
}