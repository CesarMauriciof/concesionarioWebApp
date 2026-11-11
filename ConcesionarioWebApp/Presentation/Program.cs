using System.Globalization;
using Logic;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// La cadena de conexión viene de appsettings.json + user-secrets (desarrollo) o variables de entorno (producción).
// La contraseña NUNCA se escribe en el código ni se sube al repositorio. Ver README.md.
var connectionString = builder.Configuration.GetConnectionString("Concesionario");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'ConnectionStrings:Concesionario'. " +
        "Configúrela con: dotnet user-secrets set \"ConnectionStrings:Concesionario\" \"<cadena>\" --project Presentation");
}

builder.Services.AddConcesionarioLogic(connectionString);

// Blazor Server: los componentes se ejecutan en el servidor, por eso pueden usar la capa Logic directamente
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Formato colombiano para fechas y moneda ($ 1.000.000,00)
var culture = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
