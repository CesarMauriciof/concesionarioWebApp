using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("venta")]
public class SaleEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_venta")]
    public int IdVenta { get; set; }

    [Column("fecha", TypeName = "date")]
    public DateTime Fecha { get; set; }

    [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El total pagado no puede ser negativo.")]
    [Column("total_pagado", TypeName = "decimal(12,2)")]
    public decimal TotalPagado { get; set; }

    // Las columnas admiten NULL en la BD (hay ventas históricas sin empleado);
    // [Required] solo valida el formulario. En DataContext se marcan como opcionales para EF.
    [Required(ErrorMessage = "Debe seleccionar un cliente.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [ForeignKey(nameof(IdCliente))]
    public virtual ClientEntity? Cliente { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un empleado.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado.")]
    [Column("id_empleado")]
    public int? IdEmpleado { get; set; }

    [ForeignKey(nameof(IdEmpleado))]
    public virtual EmployeeEntity? Empleado { get; set; }

    // Una venta tiene muchos detalles y muchos pagos
    public virtual ICollection<SaleDetailEntity> DetallesVenta { get; set; } = new List<SaleDetailEntity>();
    public virtual ICollection<PayEntity> Pagos { get; set; } = new List<PayEntity>();
}
