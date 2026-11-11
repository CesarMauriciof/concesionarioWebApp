namespace Model;

/// <summary>Estados de una cotización. Se guardan como texto en la columna cotizacion.estado.</summary>
public enum QuoteStatus
{
    Pendiente,
    Aceptada,
    Rechazada,
    Vencida
}
