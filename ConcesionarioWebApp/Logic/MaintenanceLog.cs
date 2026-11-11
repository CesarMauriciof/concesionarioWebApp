using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de mantenimientos.</summary>
public class MaintenanceLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<MaintenanceEntity>(dbFactory)
{
    /// <summary>Devuelve todos los mantenimientos.</summary>
    public Task<List<MaintenanceEntity>> MaintenanceListAsync() => ListAsync(q => q.OrderByDescending(m => m.FechaProgramada));

    /// <summary>Busca un mantenimiento por su ID. Devuelve null si no existe.</summary>
    public Task<MaintenanceEntity?> GetMaintenanceByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un mantenimiento.</summary>
    public Task CreateMaintenanceAsync(MaintenanceEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza un mantenimiento.</summary>
    public Task UpdateMaintenanceAsync(MaintenanceEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina un mantenimiento por su ID.</summary>
    public Task DeleteMaintenanceAsync(int id) => DeleteAsync(id);

    protected override void Normalize(MaintenanceEntity entity)
    {
        entity.ServiciosRealizados = entity.ServiciosRealizados.Trim();
    }
}
