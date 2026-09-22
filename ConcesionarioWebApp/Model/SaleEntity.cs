using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class SaleEntity
{
    // Data annotations Llave primaria
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_venta { get; set; }

    [Required]
    public DateTime fecha { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal total_pagado { get; set; }

    // Llaves foráneas a Cliente y Empleado
    [Required]
    public int id_cliente { get; set; }

    [Required]
    public int id_empleado { get; set; }
}