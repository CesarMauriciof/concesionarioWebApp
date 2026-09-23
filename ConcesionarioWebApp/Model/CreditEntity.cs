using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model;

// 4. Entidad Crédito
[Table("credito")]
public class CreditEntity
{
    // Data annotations Llave primaria
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_credito { get; set; }

    // Llave foránea a Pago
    [Required]
    public int id_pago { get; set; }

    [ForeignKey("id_pago")]
    public virtual PayEntity? Pago { get; set; }

    // Data annotations tamaño del atributo en la bd
    [Required]
    [StringLength(45)]
    public string entidad_bancaria { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal monto_aprobado { get; set; }

    [Required]
    public int numero_cuotas { get; set; }
}