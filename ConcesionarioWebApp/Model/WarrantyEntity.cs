
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

// 1. Entidad Garantia
[Table("garantia")]
public class WarrantyEntity
{
    // Data annotations Llave primaria
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_garantia { get; set; }

    // Llave foránea a DetalleVenta
    [Required]
    public int id_detalle { get; set; }

    [ForeignKey("id_detalle")]
    public virtual SaleDetailEntity? DetalleVenta { get; set; }

    [Required]
    public DateTime fecha_inicio { get; set; }

    // Data annotations tamaño del atributo en la bd
    [Required]
    [StringLength(200)]
    public string condiciones { get; set; } = string.Empty;
    
    // Relación
    public ICollection<MaintenanceEntity> Mantenimientos { get; set; }
}
