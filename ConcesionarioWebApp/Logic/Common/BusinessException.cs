namespace Logic.Common;

/// <summary>
/// Error de negocio con un mensaje apto para mostrarse al usuario final.
/// La excepción técnica original (si existe) queda en <see cref="Exception.InnerException"/> para el log.
/// </summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
