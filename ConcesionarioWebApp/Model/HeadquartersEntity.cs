using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("sede")]
public class HeadquartersEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_sede")]
    public int IdSede { get; set; }

    [Required(ErrorMessage = "El nombre de la sede es obligatorio.")]
    [StringLength(100)]
    [Column("nombre_sede")]
    public string NombreSede { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ciudad es obligatoria.")]
    [StringLength(50)]
    [Column("ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [StringLength(150)]
    [Column("direccion")]
    public string Direccion { get; set; } = string.Empty;

    // Una sede tiene muchos empleados y muchos vehículos
    public virtual ICollection<EmployeeEntity> Empleados { get; set; } = new List<EmployeeEntity>();
    public virtual ICollection<VehicleEntity> Vehiculos { get; set; } = new List<VehicleEntity>();
}
