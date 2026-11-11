using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("mantenimiento")]
public class MaintenanceEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_mantenimiento")]
    public int IdMantenimiento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una garantía.")]
    [Column("id_garantia")]
    public int IdGarantia { get; set; }

    // Un mantenimiento pertenece a una garantía
    [ForeignKey(nameof(IdGarantia))]
    public virtual WarrantyEntity? Garantia { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El kilometraje no puede ser negativo.")]
    [Column("kilometraje")]
    public int Kilometraje { get; set; }

    // Antes se llamaba "año_mantenimiento": se evita la "ñ" en identificadores de la BD
    [Range(1900, 2100, ErrorMessage = "El año del mantenimiento no es válido.")]
    [Column("anio_mantenimiento")]
    public int AnioMantenimiento { get; set; }

    [Required(ErrorMessage = "Debe indicar los servicios realizados.")]
    [StringLength(200)]
    [Column("servicios_realizados")]
    public string ServiciosRealizados { get; set; } = string.Empty;

    [Column("fecha_programada", TypeName = "date")]
    public DateTime FechaProgramada { get; set; }
}
