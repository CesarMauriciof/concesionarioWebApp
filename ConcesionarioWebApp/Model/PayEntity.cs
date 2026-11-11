using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("pago")]
public class PayEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_pago")]
    public int IdPago { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una venta.")]
    [Column("id_venta")]
    public int IdVenta { get; set; }

    // Un pago pertenece a una venta
    [ForeignKey(nameof(IdVenta))]
    public virtual SaleEntity? Venta { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999.99", ParseLimitsInInvariantCulture = true, ErrorMessage = "El monto debe ser mayor que cero.")]
    [Column("monto", TypeName = "decimal(12,2)")]
    public decimal Monto { get; set; }

    [Column("fecha_pago", TypeName = "datetime")]
    public DateTime FechaPago { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un método de pago.")]
    [StringLength(100)]
    [Column("metodo_pago")]
    public string MetodoPago { get; set; } = string.Empty;

    // Un pago puede tener asociados uno o más créditos
    public virtual ICollection<CreditEntity> Creditos { get; set; } = new List<CreditEntity>();
}
