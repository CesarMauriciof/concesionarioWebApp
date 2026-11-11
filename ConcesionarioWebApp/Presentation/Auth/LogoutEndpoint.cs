using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Auth;

/// <summary>
/// Cierre de sesión (Logout). El botón "Cerrar sesión" está en Layout/LoginDisplay.razor.
///
/// Es POST (no GET) a propósito: así un enlace o imagen maliciosa no puede cerrar la sesión
/// de alguien, y ASP.NET valida el token antiforgery del formulario automáticamente.
/// </summary>
public static class LogoutEndpoint
{
    public static IEndpointRouteBuilder MapLogoutEndpoint(this IEndpointRouteBuilder app)
    {
        // El cast a Delegate es necesario: sin él, ASP.NET trata Logout como RequestDelegate
        // y descarta el IResult (la redirección nunca se ejecutaría).
        app.MapPost("/auth/logout", (Delegate)Logout).RequireAuthorization();
        return app;
    }

    private static async Task<IResult> Logout(HttpContext http)
    {
        // Borra la cookie de la sesión (y la temporal de Google, por si quedó alguna)
        await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await http.SignOutAsync(AuthenticationSetup.ExternalScheme);

        // Evita que el navegador muestre páginas protegidas desde su caché al presionar "Atrás"
        http.Response.Headers.CacheControl = "no-store";

        return Results.LocalRedirect($"{AuthenticationSetup.LoginPath}?logout=1");
    }
}