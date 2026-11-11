using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace Logic;

public static class DependencyInjection
{
    // Versión de MySQL fija: evita que la app abra una conexión al arrancar solo para detectarla (AutoDetect)
    private static readonly MySqlServerVersion ServerVersion = new(new Version(8, 0, 34));

    /// <summary>
    /// Registra el acceso a datos y todas las clases de lógica de negocio.
    /// La capa de presentación solo conoce este método, no los detalles de EF Core ni de MySQL.
    /// </summary>
    public static IServiceCollection AddConcesionarioLogic(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<DataContext>(options =>
            options.UseMySql(connectionString, ServerVersion,
                mySql => mySql.EnableRetryOnFailure(maxRetryCount: 3)));

        services.AddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

        services.AddScoped<ClientLog>();
        services.AddScoped<CreditLog>();
        services.AddScoped<EmployeeLog>();
        services.AddScoped<HeadquartersLog>();
        services.AddScoped<MaintenanceLog>();
        services.AddScoped<PayLog>();
        services.AddScoped<QuoteLog>();
        services.AddScoped<SaleDetailLog>();
        services.AddScoped<SaleLog>();
        services.AddScoped<TestDriveLog>();
        services.AddScoped<UserLog>();
        services.AddScoped<VehicleLog>();
        services.AddScoped<WarrantyLog>();

        return services;
    }
}
