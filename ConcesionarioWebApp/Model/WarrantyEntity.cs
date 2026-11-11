using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("garantia")]
public class WarrantyEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_garantia")]
    public int IdGarantia { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un detalle de venta.")]
    [Column("id_detalle")]
    public int IdDetalle { get; set; }

    [ForeignKey(nameof(IdDetalle))]
    public virtual SaleDetailEntity? DetalleVenta { get; set; }

    [Column("fecha_inicio", TypeName = "date")]
    public DateTime FechaInicio { get; set; }

    [Required(ErrorMessage = "Las condiciones son obligatorias.")]
    [StringLength(200)]
    [Column("condiciones")]
    public string Condiciones { get; set; } = string.Empty;

    // Una garantía puede tener varios mantenimientos
    public virtual ICollection<MaintenanceEntity> Mantenimientos { get; set; } = new List<MaintenanceEntity>();
}
