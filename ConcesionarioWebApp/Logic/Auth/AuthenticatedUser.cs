using Model;

namespace Logic.Auth;

/// <summary>
/// Datos del usuario que inició sesión. Es lo que la capa Logic le entrega a Presentation
/// para construir la cookie de sesión (claims). NUNCA incluye el hash de la contraseña.
/// </summary>
/// <param name="IdUsuario">ID en la tabla usuario (claim NameIdentifier).</param>
/// <param name="NombreUsuario">Nombre de usuario (claim Name).</param>
/// <param name="Correo">Correo del usuario (claim Email).</param>
/// <param name="Rol">Rol del usuario (claim Role); se usa en [Authorize(Roles = "...")].</param>
public sealed record AuthenticatedUser(int IdUsuario, string NombreUsuario, string Correo, UserRole Rol);
