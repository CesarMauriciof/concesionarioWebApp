using System.Security.Claims;
using Logic.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Auth;

/// <summary>
/// Convierte el usuario autenticado en los claims que se guardan en la cookie de sesión.
/// No se guardan datos sensibles: la cookie viaja (cifrada) en cada petición.
/// </summary>
public static class UserClaims
{
    public static ClaimsPrincipal CreatePrincipal(AuthenticatedUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
            new(ClaimTypes.Name, user.NombreUsuario),
            new(ClaimTypes.Email, user.Correo),
            // Lo usa [Authorize(Roles = "Administrador")] y <AuthorizeView Roles="...">
            new(ClaimTypes.Role, user.Rol.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}