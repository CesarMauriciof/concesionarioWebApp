using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("detalle_venta")]
public class SaleDetailEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_detalle")]
    public int IdDetalle { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una venta.")]
    [Column("id_venta")]
    public int IdVenta { get; set; }

    [ForeignKey(nameof(IdVenta))]
    public virtual SaleEntity? Venta { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo.")]
    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [ForeignKey(nameof(IdVehiculo))]
    public virtual VehicleEntity? Vehiculo { get; set; }

    // Un detalle de venta puede tener garantías
    public virtual ICollection<WarrantyEntity> Garantias { get; set; } = new List<WarrantyEntity>();
}
