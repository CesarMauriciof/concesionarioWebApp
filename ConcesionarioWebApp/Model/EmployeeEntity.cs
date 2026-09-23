using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("empleado")]
    public class EmployeeEntity
    {
        [Key]
        [Column("id_empleado")]
        public int IdEmpleado { get; set; }

        [Required]
        [MaxLength(15)]
        [Column("cedula")]
        public string Cedula { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required]
        [Column("salario", TypeName = "decimal(10,2)")]
        public decimal Salario { get; set; }

        [Required]
        [Column("id_sede")]
        public int IdSede { get; set; }

        // Propiedad de navegación: Un empleado pertenece a una sede
        [ForeignKey(nameof(IdSede))]
        public virtual HeadquartersEntity? Sede { get; set; }
    }
}