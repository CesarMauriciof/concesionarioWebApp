using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model;

[Table("cliente")]
public class ClientEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cliente")]
    public int Id_Cliente { get; set; }

    [Required]
    [StringLength(15)]
    [Column("cedula")]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(20)]
    [Column("telefono")]
    public string? Telefono { get; set; }

    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [Column("correo")]
    public string? Correo { get; set; }

    [StringLength(150)]
    [Column("direccion")]
    public string? Direccion { get; set; }

    // Relación Uno a Muchos: Un cliente tiene muchas ventas (FK venta.id_cliente, configurada en DataContext)
    public ICollection<SaleEntity> venta { get; set; } = new List<SaleEntity>();

}
