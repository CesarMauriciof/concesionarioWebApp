using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Auth;

/// <summary>
/// Configuración de autenticación y del proveedor OAuth 2.0.
///
/// RESPONSABLE: Mauricio - Tarea T2 (ver docs/sprint5/TAREAS_SPRINT5.md).
///
/// Ya está lista la base: cookie de sesión, cookie externa temporal y autorización.
/// Falta registrar Google. Program.cs ya llama a este método: NO hace falta tocar Program.cs.
///
/// Cómo funcionan los dos esquemas de cookie:
///   1. Google autentica al usuario y deja sus datos en la cookie TEMPORAL (ExternalScheme).
///   2. El endpoint /auth/callback (T4) lee esa cookie y le pregunta a AuthLog si el usuario
///      está registrado y activo.
///   3. Solo si AuthLog lo aprueba se crea la cookie DEFINITIVA (esquema por defecto) y se borra
///      la temporal. Así, alguien con cuenta de Google pero sin usuario en el sistema NO entra.
/// </summary>
public static class AuthenticationSetup
{
    public const string LoginPath = "/login";
    public const string AccessDeniedPath = "/acceso-denegado";

    /// <summary>Esquema de la cookie temporal donde Google deja el resultado del login.</summary>
    public const string ExternalScheme = "External";

    public static WebApplicationBuilder AddConcesionarioAuthentication(this WebApplicationBuilder builder)
    {
        var authentication = builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            // Cookie definitiva de la sesión
            .AddCookie(options =>
            {
                options.LoginPath = LoginPath;
                options.AccessDeniedPath = AccessDeniedPath;
                options.Cookie.Name = "Concesionario.Auth";
                options.Cookie.HttpOnly = true;                 // JavaScript no puede leer la cookie
                options.Cookie.SameSite = SameSiteMode.Lax;     // necesario para volver desde Google
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromHours(8); // jornada laboral
                options.SlidingExpiration = true;
            })
            // Cookie temporal (5 minutos) solo para el regreso desde el proveedor externo
            .AddCookie(ExternalScheme, options =>
            {
                options.Cookie.Name = "Concesionario.External";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

        // TODO [T2 - Mauricio]: registrar el proveedor Google.
        //  1. Crear las credenciales OAuth en Google Cloud Console (guía en TAREAS_SPRINT5.md, T2).
        //  2. Leer ClientId y ClientSecret de la configuración (user-secrets), NUNCA escribirlos aquí:
        //       builder.Configuration["Authentication:Google:ClientId"]
        //       builder.Configuration["Authentication:Google:ClientSecret"]
        //     Si faltan, lanzar InvalidOperationException explicando cómo configurarlos
        //     (igual que se hace con la cadena de conexión en Program.cs).
        //  3. authentication.AddGoogle(options => { ... }) con:
        //       - ClientId y ClientSecret leídos en el punto 2
        //       - options.SignInScheme = ExternalScheme;   (¡importante! ver explicación arriba)
        //       - CallbackPath: dejar el de defecto "/signin-google" (es el que se registra en Google)
        //       - Scopes: openid, email y profile ya vienen por defecto
        //  Documentación: https://learn.microsoft.com/aspnet/core/security/authentication/social/google-logins
        _ = authentication;

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        return builder;
    }
}
