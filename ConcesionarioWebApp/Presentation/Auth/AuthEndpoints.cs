// =====================================================================================
//  PEGAR AQUÍ - T4 (Yeison)
//  Borra TODO el contenido de este archivo y pega el código que te llegó en tu mensaje.
//  Lo que hay abajo es solo para que el proyecto compile mientras tanto.
// =====================================================================================
namespace Presentation.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapLogoutEndpoint();
        return app;
    }
}
