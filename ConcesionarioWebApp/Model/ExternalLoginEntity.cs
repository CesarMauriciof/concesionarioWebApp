using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

/// <summary>
/// Vincula una cuenta externa (Google u otro proveedor OAuth 2.0) con un usuario del sistema.
/// Un usuario puede tener varias cuentas externas; una cuenta externa pertenece a un solo usuario.
///
/// ESTRUCTURA LISTA. La configuración en DataContext y la migración las hace la tarea T3 (Keiner):
/// ver docs/sprint5/TAREAS_SPRINT5.md. Mientras no se registre en DataContext, EF Core la ignora.
/// </summary>
[Table("usuario_login_externo")]
public class ExternalLoginEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_login")]
    public int IdLogin { get; set; }

    /// <summary>Nombre del proveedor: "Google", "Microsoft", "GitHub"...</summary>
    [Required]
    [StringLength(30)]
    [Column("proveedor")]
    public string Proveedor { get; set; } = string.Empty;

    /// <summary>ID del usuario en el proveedor (claim "sub"). Junto con Proveedor debe ser único.</summary>
    [Required]
    [StringLength(200)]
    [Column("clave_proveedor")]
    public string ClaveProveedor { get; set; } = string.Empty;

    /// <summary>Correo que reportó el proveedor al vincular la cuenta (solo informativo).</summary>
    [StringLength(100)]
    [Column("correo_proveedor")]
    public string? CorreoProveedor { get; set; }

    [Column("fecha_vinculacion", TypeName = "datetime")]
    public DateTime FechaVinculacion { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public virtual UserEntity? Usuario { get; set; }
}
