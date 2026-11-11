using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace Presentation.Auth;

/// <summary>
/// Configuración de la autenticación (cookie de sesión) y del proveedor OAuth 2.0 (Google).
///
/// Cómo funcionan los dos esquemas de cookie:
///   1. Google autentica al usuario y deja sus datos en la cookie TEMPORAL (ExternalScheme).
///   2. El endpoint /auth/callback lee esa cookie y le pregunta a AuthLog (capa Logic) si el usuario
///      está registrado y activo.
///   3. Solo si AuthLog lo aprueba se crea la cookie DEFINITIVA (esquema por defecto) y se borra
///      la temporal. Así, alguien con cuenta de Google pero sin usuario en el sistema NO entra.
///
/// El ClientId y el ClientSecret de Google se leen de la configuración (user-secrets en desarrollo,
/// variables de entorno en producción). NUNCA se escriben en el código ni en appsettings.json.
/// </summary>
public static class AuthenticationSetup
{
    public const string LoginPath = "/login";
    public const string AccessDeniedPath = "/acceso-denegado";

    /// <summary>Esquema de la cookie temporal donde Google deja el resultado del login.</summary>
    public const string ExternalScheme = "External";

    /// <summary>Claim con el indicador de correo verificado que entrega Google.</summary>
    public const string EmailVerifiedClaim = "urn:google:email_verified";

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

        // Proveedor OAuth 2.0: Google
        var clientId = builder.Configuration["Authentication:Google:ClientId"];
        var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
        {
            authentication.AddGoogle(GoogleDefaults.AuthenticationScheme, "Google", options =>
            {
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;

                // Google deja el resultado en la cookie temporal; la definitiva la crea /auth/callback
                options.SignInScheme = ExternalScheme;

                // Scopes "openid", "email" y "profile" ya vienen por defecto.
                // CallbackPath por defecto: /signin-google (es la URI registrada en Google Cloud).

                // Guardar si Google verificó el correo (campo "email_verified" del endpoint
                // userinfo v3 que usa .NET 9): solo se aceptan correos verificados
                options.ClaimActions.MapJsonKey(EmailVerifiedClaim, "email_verified");
            });
        }
        else
        {
            // Sin las claves la app igual arranca (para que el equipo pueda trabajar en otras tareas),
            // pero el botón de Google no aparece en el login. Se avisa en la consola cómo configurarlas.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "AVISO: Google no está configurado, el inicio de sesión con Google está desactivado.\n" +
                "Configúrelo con (en la carpeta ConcesionarioWebApp):\n" +
                "  dotnet user-secrets set \"Authentication:Google:ClientId\" \"<client-id>\" --project Presentation\n" +
                "  dotnet user-secrets set \"Authentication:Google:ClientSecret\" \"<client-secret>\" --project Presentation");
            Console.ResetColor();
        }

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        return builder;
    }
}