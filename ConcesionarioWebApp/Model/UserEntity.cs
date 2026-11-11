using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

/// <summary>
/// Usuario que inicia sesión en el sistema. La contraseña NUNCA se guarda en texto plano:
/// solo se almacena su hash (PBKDF2 con sal, generado por PasswordHasher de ASP.NET Core Identity).
/// </summary>
[Table("usuario")]
public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 50 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo admite letras, números, punto, guion y guion bajo.")]
    [Column("nombre_usuario")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [Column("correo")]
    public string Correo { get; set; } = string.Empty;

    // Lo asigna la capa Logic al crear o cambiar la contraseña; no se valida en formularios
    [StringLength(255)]
    [Column("contrasena_hash")]
    public string ContrasenaHash { get; set; } = string.Empty;

    [Column("rol")]
    public UserRole Rol { get; set; } = UserRole.Vendedor;

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_creacion", TypeName = "datetime")]
    public DateTime FechaCreacion { get; set; }

    // Fecha y hora del último inicio de sesión exitoso (null si nunca ha entrado)
    [Column("ultimo_acceso", TypeName = "datetime")]
    public DateTime? UltimoAcceso { get; set; }

    // Cuentas externas (Google...) vinculadas a este usuario
    public virtual ICollection<ExternalLoginEntity> LoginsExternos { get; set; } = new List<ExternalLoginEntity>();

    // Opcional: vincula el usuario con un empleado del concesionario
    [Column("id_empleado")]
    public int? IdEmpleado { get; set; }

    [ForeignKey(nameof(IdEmpleado))]
    public virtual EmployeeEntity? Empleado { get; set; }
}
