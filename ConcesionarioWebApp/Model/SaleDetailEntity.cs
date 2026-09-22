using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class SaleDetailEntity
{
    // Data annotations Llave primaria
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_detalle { get; set; }

    // Llave foránea a Venta
    [Required]
    public int id_venta { get; set; }

    [ForeignKey("id_venta")]
    public virtual SaleEntity? Venta { get; set; }

    // Llave foránea a Vehículo
    [Required]
    public int id_vehiculo { get; set; }
}