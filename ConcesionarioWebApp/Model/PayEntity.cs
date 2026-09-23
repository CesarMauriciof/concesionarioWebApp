using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model;

[Table("pago")]
public class PayEntity
{
    // Data annotations Llave primaria
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_pago { get; set; }

    // Llave foránea a Venta
    [Required]
    public int id_venta { get; set; }
    
    [ForeignKey("id_venta")]
    public virtual SaleEntity? Venta { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal monto { get; set; }

    [Required]
    public DateTime fecha_pago { get; set; }

    // Data annotations tamaño del atributo en la bd
    [Required]
    [StringLength(100)]
    public string metodo_pago { get; set; } = string.Empty;
}