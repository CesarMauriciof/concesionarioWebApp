using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("vehiculo")]
public class VehicleEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [Required(ErrorMessage = "El VIN es obligatorio.")]
    [StringLength(17, ErrorMessage = "El VIN no puede superar 17 caracteres.")]
    [Column("vin")]
    public string Vin { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es obligatoria.")]
    [StringLength(50)]
    [Column("marca")]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "El modelo es obligatorio.")]
    [StringLength(50)]
    [Column("modelo")]
    public string Modelo { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "9999999999.99", ErrorMessage = "El precio debe ser mayor que cero.")]
    [Column("precio", TypeName = "decimal(12,2)")]
    public decimal Precio { get; set; }

    // La columna admite NULL en la BD (ON DELETE SET NULL); [Required] solo valida el formulario.
    [Required(ErrorMessage = "Debe seleccionar una sede.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sede.")]
    [Column("id_sede")]
    public int? IdSede { get; set; }

    [ForeignKey(nameof(IdSede))]
    public virtual HeadquartersEntity? Sede { get; set; }

    public virtual ICollection<SaleDetailEntity> DetallesVenta { get; set; } = new List<SaleDetailEntity>();
}
