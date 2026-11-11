using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("credito")]
public class CreditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_credito")]
    public int IdCredito { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un pago.")]
    [Column("id_pago")]
    public int IdPago { get; set; }

    // Un crédito está vinculado a un pago
    [ForeignKey(nameof(IdPago))]
    public virtual PayEntity? Pago { get; set; }

    [Required(ErrorMessage = "La entidad bancaria es obligatoria.")]
    [StringLength(45)]
    [Column("entidad_bancaria")]
    public string EntidadBancaria { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "9999999999.99", ParseLimitsInInvariantCulture = true, ErrorMessage = "El monto aprobado debe ser mayor que cero.")]
    [Column("monto_aprobado", TypeName = "decimal(12,2)")]
    public decimal MontoAprobado { get; set; }

    [Range(1, 360, ErrorMessage = "El número de cuotas debe estar entre 1 y 360.")]
    [Column("numero_cuotas")]
    public int NumeroCuotas { get; set; }
}
