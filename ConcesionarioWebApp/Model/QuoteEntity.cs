using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

/// <summary>
/// Cotización: oferta de precio que un asesor hace a un cliente por un vehículo antes de la venta.
/// </summary>
[Table("cotizacion")]
public class QuoteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cotizacion")]
    public int IdCotizacion { get; set; }

    [Column("fecha", TypeName = "date")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Column("fecha_vencimiento", TypeName = "date")]
    public DateTime FechaVencimiento { get; set; } = DateTime.Today.AddDays(15);

    [Range(typeof(decimal), "0.01", "9999999999.99", ParseLimitsInInvariantCulture = true, ErrorMessage = "El precio ofrecido debe ser mayor que cero.")]
    [Column("precio_ofrecido", TypeName = "decimal(12,2)")]
    public decimal PrecioOfrecido { get; set; }

    [Column("estado")]
    public QuoteStatus Estado { get; set; } = QuoteStatus.Pendiente;

    [StringLength(250)]
    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [ForeignKey(nameof(IdCliente))]
    public virtual ClientEntity? Cliente { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo.")]
    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [ForeignKey(nameof(IdVehiculo))]
    public virtual VehicleEntity? Vehiculo { get; set; }

    // Asesor que elaboró la cotización (opcional: si el empleado se elimina, la cotización se conserva)
    [Column("id_empleado")]
    public int? IdEmpleado { get; set; }

    [ForeignKey(nameof(IdEmpleado))]
    public virtual EmployeeEntity? Empleado { get; set; }
}
