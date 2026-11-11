namespace Presentation.Auth;

/// <summary>
/// Cierre de sesión (Logout).
///
/// RESPONSABLE: Mauricio - Tarea T6 (ver docs/sprint5/TAREAS_SPRINT5.md).
/// El botón "Cerrar sesión" está en Layout/LoginDisplay.razor (también T6).
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

    private static Task<IResult> Logout(HttpContext http)
    {
        // TODO [T6 - Mauricio]:
        //  1. await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //  2. return Results.Redirect("/login?logout=1");   (la página de login muestra "Sesión cerrada")
        _ = http;
        return Task.FromResult(Results.Redirect(AuthenticationSetup.LoginPath));
    }
}
