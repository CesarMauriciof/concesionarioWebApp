using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("sede")]
    public class HeadquartersEntity
    {
        [Key]
        [Column("id_sede")]
        public int IdSede { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre_sede")]
        public string NombreSede { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;
    }
}