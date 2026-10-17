using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model;

namespace ConcesionarioWebApp.Model;

[Table("vehiculo")]
public class VehicleEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [Required]
    [StringLength(17)]
    [Column("vin")]
    public string Vin { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Column("marca")]
    public string Marca { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Column("modelo")]
    public string Modelo { get; set; } = string.Empty;

    [Required]
    [Column("precio", TypeName = "decimal(12,2)")]
    public decimal Precio { get; set; }

    // Nullable porque en la BD la FK tiene ON DELETE SET NULL
    [Required(ErrorMessage = "Debe seleccionar una sede.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sede.")]
    [Column("id_sede")]
    public int? IdSede { get; set; }

    public ICollection<SaleDetailEntity> Detalle_Venta { get; set; } = new List<SaleDetailEntity>();
}
