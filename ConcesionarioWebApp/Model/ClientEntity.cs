using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("cliente")]
public class ClientEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [StringLength(15)]
    [Column("cedula")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(20)]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    [Column("telefono")]
    public string? Telefono { get; set; }

    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [Column("correo")]
    public string? Correo { get; set; }

    [StringLength(150)]
    [Column("direccion")]
    public string? Direccion { get; set; }

    // Un cliente tiene muchas ventas
    public virtual ICollection<SaleEntity> Ventas { get; set; } = new List<SaleEntity>();
}
