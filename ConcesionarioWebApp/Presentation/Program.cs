using Microsoft.AspNetCore.Components;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// La app corre en el servidor (Blazor Server) para que la capa Logic pueda conectarse a MySQL
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri) });

var app = builder.Build();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
