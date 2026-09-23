using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model;

namespace ConcesionarioWebApp.Model;

public class VehicleEntity
{
    [Key]
    public int IdVehiculo { get; set; }

    [Required]
    [StringLength(17)]
    public string Vin { get; set; }

    [Required]
    [StringLength(50)]
    public string Marca { get; set; }

    [Required]
    [StringLength(50)]
    public string Modelo { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal Precio { get; set; }
    
    public int IdSede { get; set; }
    
    public ICollection<SaleDetailEntity> Detalle_Venta{get; set;}
}