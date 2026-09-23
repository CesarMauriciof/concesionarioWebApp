using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("mantenimiento")]
public class MaintenanceEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_mantenimiento { get; set; }

    [Required]
    public int id_garantia { get; set; }

    [ForeignKey("id_garantia")]
    public virtual WarrantyEntity? Garantia { get; set; }

    [Required]
    public int kilometraje { get; set; }

    [Required]
    public int año_mantenimiento { get; set; }

    [Required]
    [StringLength(200)]
    public string servicios_realizados { get; set; } = string.Empty;

    [Required]
    public DateTime fecha_programada { get; set; }
}