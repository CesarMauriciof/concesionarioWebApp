using System.Security.Claims;
using Logic.Auth;
using Logic.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Auth;

/// <summary>
/// Endpoints HTTP del proceso de inicio de sesión.
///
/// ¿Por qué endpoints y no código dentro de los componentes Blazor?
/// La app corre en modo InteractiveServer (SignalR): desde un componente interactivo NO se pueden
/// escribir cookies. Por eso el login y el logout son peticiones HTTP normales a estas rutas.
/// </summary>
public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").AllowAnonymous();

        group.MapGet("/login/{provider}", LoginExternal);
        group.MapGet("/callback", ExternalCallback);
        group.MapPost("/login-local", LoginLocal);

        // El cierre de sesión está en LogoutEndpoint.cs
        app.MapLogoutEndpoint();

        return app;
    }

    /// <summary>GET /auth/login/google?returnUrl=/sale/list : inicia el flujo OAuth con el proveedor.</summary>
    private static async Task<IResult> LoginExternal(
        string provider, string? returnUrl, IAuthenticationSchemeProvider schemes)
    {
        // Por ahora solo Google. El esquema solo existe si se configuraron ClientId y ClientSecret.
        if (!provider.Equals("google", StringComparison.OrdinalIgnoreCase)
            || await schemes.GetSchemeAsync(GoogleDefaults.AuthenticationScheme) == null)
        {
            return RedirectToLogin("El proveedor de inicio de sesión no está disponible.", returnUrl);
        }

        var properties = new AuthenticationProperties
        {
            // Después de /signin-google, Google vuelve a nuestro callback conservando el returnUrl
            RedirectUri = $"/auth/callback?returnUrl={Uri.EscapeDataString(SafeReturnUrl(returnUrl))}"
        };

        return Results.Challenge(properties, [GoogleDefaults.AuthenticationScheme]);
    }

    /// <summary>GET /auth/callback : Google regresa aquí (después de /signin-google).</summary>
    private static async Task<IResult> ExternalCallback(HttpContext http, AuthLog authLog, string? returnUrl)
    {
        // 1. Leer lo que Google dejó en la cookie temporal
        var result = await http.AuthenticateAsync(AuthenticationSetup.ExternalScheme);
        if (!result.Succeeded || result.Principal == null)
        {
            return RedirectToLogin("No se pudo completar el inicio de sesión con Google. Intente de nuevo.", returnUrl);
        }

        var external = result.Principal;

        // Borrar la cookie temporal pase lo que pase
        await http.SignOutAsync(AuthenticationSetup.ExternalScheme);

        // 2. Solo se aceptan correos verificados por Google
        var emailVerified = external.FindFirstValue(AuthenticationSetup.EmailVerifiedClaim);
        if (!string.Equals(emailVerified, "true", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToLogin("Su correo de Google no está verificado.", returnUrl);
        }

        var data = new ExternalLoginData(
            Proveedor: "Google",
            ClaveProveedor: external.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            Correo: external.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Nombre: external.FindFirstValue(ClaimTypes.Name));

        // 3. La capa Logic decide si puede entrar
        AuthenticatedUser user;
        try
        {
            user = await authLog.LoginWithExternalProviderAsync(data);
        }
        catch (BusinessException ex)
        {
            return RedirectToLogin(ex.Message, returnUrl);
        }

        // 4. Crear la cookie definitiva de la sesión
        await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, UserClaims.CreatePrincipal(user));

        return Results.LocalRedirect(SafeReturnUrl(returnUrl));
    }

    /// <summary>POST /auth/login-local : formulario de usuario y contraseña (respaldo).</summary>
    private static async Task<IResult> LoginLocal(
        HttpContext http,
        AuthLog authLog,
        [FromForm] string? usuario,
        [FromForm] string? contrasena,
        [FromForm] string? returnUrl)
    {
        // El token antiforgery del formulario lo valida ASP.NET automáticamente (UseAntiforgery)
        AuthenticatedUser user;
        try
        {
            user = await authLog.LoginWithPasswordAsync(usuario ?? string.Empty, contrasena ?? string.Empty);
        }
        catch (BusinessException ex)
        {
            return RedirectToLogin(ex.Message, returnUrl);
        }

        await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, UserClaims.CreatePrincipal(user));

        return Results.LocalRedirect(SafeReturnUrl(returnUrl));
    }

    /// <summary>
    /// Evita el ataque "open redirect": solo se aceptan rutas locales ("/algo"),
    /// nunca "//otro-sitio.com" ni "https://otro-sitio.com".
    /// </summary>
    private static string SafeReturnUrl(string? returnUrl) =>
        !string.IsNullOrEmpty(returnUrl)
        && returnUrl.StartsWith('/')
        && !returnUrl.StartsWith("//")
        && !returnUrl.StartsWith("/\\")
        && !returnUrl.StartsWith("/auth/", StringComparison.OrdinalIgnoreCase)
            ? returnUrl
            : "/";

    private static IResult RedirectToLogin(string error, string? returnUrl) =>
        Results.LocalRedirect(
            $"{AuthenticationSetup.LoginPath}?error={Uri.EscapeDataString(error)}" +
            $"&returnUrl={Uri.EscapeDataString(SafeReturnUrl(returnUrl))}");
}