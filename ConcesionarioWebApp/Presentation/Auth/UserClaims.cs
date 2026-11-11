using System.Security.Claims;
using Logic.Auth;

namespace Presentation.Auth;

/// <summary>
/// Convierte el usuario autenticado en los claims que se guardan en la cookie de sesión.
///
/// RESPONSABLE: Yeison - Tarea T4 (ver docs/sprint5/TAREAS_SPRINT5.md).
/// </summary>
public static class UserClaims
{
    public static ClaimsPrincipal CreatePrincipal(AuthenticatedUser user)
    {
        // TODO [T4 - Yeison]: crear la lista de claims y devolver el ClaimsPrincipal:
        //   ClaimTypes.NameIdentifier = user.IdUsuario
        //   ClaimTypes.Name           = user.NombreUsuario
        //   ClaimTypes.Email          = user.Correo
        //   ClaimTypes.Role           = user.Rol.ToString()   <- lo usa [Authorize(Roles = "Administrador")] (T7)
        // var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // return new ClaimsPrincipal(identity);
        //
        // NO guardar datos sensibles en los claims (la cookie viaja en cada petición).
        _ = user;
        throw new NotImplementedException("T4: UserClaims.CreatePrincipal pendiente de implementar.");
    }
}
