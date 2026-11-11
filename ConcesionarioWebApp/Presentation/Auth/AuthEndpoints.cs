using Logic.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Auth;

/// <summary>
/// Endpoints HTTP del proceso de inicio de sesión.
///
/// RESPONSABLE: Yeison - Tarea T4 (ver docs/sprint5/TAREAS_SPRINT5.md).
///
/// ¿Por qué endpoints y no código dentro de los componentes Blazor?
/// La app corre en modo InteractiveServer (SignalR): desde un componente interactivo NO se pueden
/// escribir cookies. Por eso el login y el logout son peticiones HTTP normales a estas rutas.
///
/// Program.cs ya llama a MapAuthEndpoints(): NO hace falta tocar Program.cs.
/// Mientras no estén implementados, redirigen a /login con un aviso.
/// </summary>
public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").AllowAnonymous();

        group.MapGet("/login/{provider}", LoginExternal);
        group.MapGet("/callback", ExternalCallback);
        group.MapPost("/login-local", LoginLocal);

        // El cierre de sesión está en LogoutEndpoint.cs (T6 - Mauricio)
        app.MapLogoutEndpoint();

        return app;
    }

    /// <summary>GET /auth/login/google?returnUrl=/sale/list : inicia el flujo OAuth con el proveedor.</summary>
    private static IResult LoginExternal(string provider, string? returnUrl)
    {
        // TODO [T4 - Yeison]:
        //  - Validar que provider sea uno configurado (por ahora solo "google"); si no, redirigir a /login?error=...
        //  - return Results.Challenge(new AuthenticationProperties { RedirectUri = "/auth/callback?returnUrl=..." },
        //                              [GoogleDefaults.AuthenticationScheme]);
        //  - returnUrl debe ser una ruta LOCAL (empieza por "/" y no por "//"); si no, usar "/".
        //    Esto evita el ataque "open redirect".
        _ = (provider, returnUrl);
        return Results.Redirect($"{AuthenticationSetup.LoginPath}?error=pendiente");
    }

    /// <summary>GET /auth/callback : Google regresa aquí (después de /signin-google).</summary>
    private static Task<IResult> ExternalCallback(HttpContext http, AuthLog authLog, string? returnUrl)
    {
        // TODO [T4 - Yeison]:
        //  1. var result = await http.AuthenticateAsync(AuthenticationSetup.ExternalScheme);
        //     Si no tuvo éxito -> /login?error=externo
        //  2. Armar ExternalLoginData con los claims de result.Principal:
        //       Proveedor = "Google", ClaveProveedor = ClaimTypes.NameIdentifier,
        //       Correo = ClaimTypes.Email, Nombre = ClaimTypes.Name
        //  3. try { var user = await authLog.LoginWithExternalProviderAsync(data); }
        //     catch (BusinessException ex) -> /login?error=<mensaje codificado con Uri.EscapeDataString>
        //  4. await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, UserClaims.CreatePrincipal(user));
        //  5. await http.SignOutAsync(AuthenticationSetup.ExternalScheme);   (borra la cookie temporal)
        //  6. Redirigir a returnUrl (validado como ruta local) o a "/".
        _ = (http, authLog, returnUrl);
        return Task.FromResult(Results.Redirect($"{AuthenticationSetup.LoginPath}?error=pendiente"));
    }

    /// <summary>POST /auth/login-local : formulario de usuario y contraseña (respaldo).</summary>
    private static Task<IResult> LoginLocal(
        HttpContext http,
        AuthLog authLog,
        [FromForm] string usuario,
        [FromForm] string contrasena,
        [FromForm] string? returnUrl)
    {
        // TODO [T4 - Yeison]:
        //  1. var user = await authLog.LoginWithPasswordAsync(usuario, contrasena)  (capturar BusinessException)
        //  2. await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, UserClaims.CreatePrincipal(user));
        //  3. Redirigir a returnUrl (ruta local) o a "/". En error: /login?error=...
        //  El token antiforgery lo valida ASP.NET automáticamente (el formulario de T5 debe incluir <AntiforgeryToken />).
        _ = (http, authLog, usuario, contrasena, returnUrl);
        return Task.FromResult(Results.Redirect($"{AuthenticationSetup.LoginPath}?error=pendiente"));
    }
}
