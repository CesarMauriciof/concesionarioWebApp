using System.ComponentModel.DataAnnotations;
using Model;

public class ClientEntity
{
    [Key]
    public int Id_Cliente { get; set; }

    [Required]
    [StringLength(15)]
    public string Cedula { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(20)]
    public string Telefono { get; set; }

    [Required]
    [StringLength(100)]
    public string Correo { get; set; }

    [Required]
    [StringLength(150)]
    public string Direccion { get; set; }
    
    public ICollection<SaleEntity>  venta { get; set; }
    
}