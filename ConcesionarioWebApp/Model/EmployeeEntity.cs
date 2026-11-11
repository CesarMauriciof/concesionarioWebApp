using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("empleado")]
public class EmployeeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_empleado")]
    public int IdEmpleado { get; set; }

    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [StringLength(15)]
    [Column("cedula")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El cargo es obligatorio.")]
    [StringLength(50)]
    [Column("cargo")]
    public string Cargo { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true, ErrorMessage = "El salario no puede ser negativo.")]
    [Column("salario", TypeName = "decimal(10,2)")]
    public decimal Salario { get; set; }

    // La columna admite NULL en la BD (ON DELETE SET NULL); [Required] solo valida el formulario.
    // En DataContext se marca como opcional para EF.
    [Required(ErrorMessage = "Debe seleccionar una sede.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sede.")]
    [Column("id_sede")]
    public int? IdSede { get; set; }

    // Un empleado pertenece a una sede
    [ForeignKey(nameof(IdSede))]
    public virtual HeadquartersEntity? Sede { get; set; }

    // Un empleado registra muchas ventas
    public virtual ICollection<SaleEntity> Ventas { get; set; } = new List<SaleEntity>();
}
