using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConcesionarioWebApp.Model;

namespace Model;

[Table("detalle_venta")]
public class SaleDetailEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_detalle")]
    public int id_detalle { get; set; }
    
    [Required]
    [Column("id_venta")]
    public int id_venta { get; set; }

    // Clave foránea explícita apuntando a SaleEntity
    [ForeignKey(nameof(id_venta))]
    public virtual SaleEntity? Venta { get; set; }

    [Required]
    [Column("id_vehiculo")]
    public int id_vehiculo { get; set; }

    [ForeignKey(nameof(id_vehiculo))]
    public virtual VehicleEntity? Vehiculo { get; set; }
}