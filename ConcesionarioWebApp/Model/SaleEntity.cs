using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("venta")]
public class SaleEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_venta")]
    public int id_venta { get; set; }

    [Required]
    [Column("fecha", TypeName = "date")]
    public DateTime fecha { get; set; }

    [Required]
    [Column("total_pagado", TypeName = "decimal(12,2)")]
    public decimal total_pagado { get; set; }

    // Nullable porque la columna admite NULL en la BD (existen ventas sin empleado)
    [Required(ErrorMessage = "Debe seleccionar un cliente.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
    [Column("id_cliente")]
    public int? id_cliente { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un empleado.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado.")]
    [Column("id_empleado")]
    public int? id_empleado { get; set; }

    // Propiedad de navegación: Una venta tiene muchos detalles
    public virtual ICollection<SaleDetailEntity> DetallesVenta { get; set; } = new List<SaleDetailEntity>();
}