// =====================================================================================
//  PEGAR AQUÍ - T2 (Mauricio)
//  Borra TODO el contenido de este archivo y pega el código que te llegó en tu mensaje.
//  Lo que hay abajo es solo para que el proyecto compile mientras tanto.
// =====================================================================================
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Auth;

public static class AuthenticationSetup
{
    public const string LoginPath = "/login";
    public const string AccessDeniedPath = "/acceso-denegado";
    public const string ExternalScheme = "External";
    public const string EmailVerifiedClaim = "urn:google:email_verified";

    public static WebApplicationBuilder AddConcesionarioAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();
        return builder;
    }
}
