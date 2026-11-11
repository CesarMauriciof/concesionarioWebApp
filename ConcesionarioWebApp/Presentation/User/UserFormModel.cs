using System.ComponentModel.DataAnnotations;
using Logic;
using Model;

namespace Presentation.User;

/// <summary>
/// Modelo del formulario de usuario. Se usa en lugar de <see cref="UserEntity"/> para no exponer
/// el hash de la contraseña en la interfaz y para manejar los campos "contraseña" y "confirmar".
/// </summary>
public class UserFormModel : IValidatableObject
{
    public int IdUsuario { get; set; }

    /// <summary>true al crear: la contraseña es obligatoria. false al editar: es opcional.</summary>
    public bool EsNuevo { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 50 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "El nombre de usuario solo admite letras, números, punto, guion y guion bajo.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Correo { get; set; } = string.Empty;

    public UserRole Rol { get; set; } = UserRole.Vendedor;

    public bool Activo { get; set; } = true;

    public int? IdEmpleado { get; set; }

    [DataType(DataType.Password)]
    public string? Contrasena { get; set; }

    [DataType(DataType.Password)]
    public string? ConfirmarContrasena { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Contrasena))
        {
            if (EsNuevo)
            {
                yield return new ValidationResult("La contraseña es obligatoria.", [nameof(Contrasena)]);
            }

            yield break;
        }

        if (Contrasena.Length < UserLog.MinPasswordLength)
        {
            yield return new ValidationResult(
                $"La contraseña debe tener al menos {UserLog.MinPasswordLength} caracteres.", [nameof(Contrasena)]);
        }

        if (!Contrasena.Any(char.IsLetter) || !Contrasena.Any(char.IsDigit))
        {
            yield return new ValidationResult(
                "La contraseña debe contener al menos una letra y un número.", [nameof(Contrasena)]);
        }

        if (Contrasena != ConfirmarContrasena)
        {
            yield return new ValidationResult("Las contraseñas no coinciden.", [nameof(ConfirmarContrasena)]);
        }
    }

    public static UserFormModel FromEntity(UserEntity entity) => new()
    {
        IdUsuario = entity.IdUsuario,
        NombreUsuario = entity.NombreUsuario,
        Correo = entity.Correo,
        Rol = entity.Rol,
        Activo = entity.Activo,
        IdEmpleado = entity.IdEmpleado
    };

    public UserEntity ToEntity() => new()
    {
        IdUsuario = IdUsuario,
        NombreUsuario = NombreUsuario,
        Correo = Correo,
        Rol = Rol,
        Activo = Activo,
        IdEmpleado = IdEmpleado
    };
}
