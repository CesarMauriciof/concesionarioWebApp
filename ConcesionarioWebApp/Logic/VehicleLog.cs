using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de vehículos.</summary>
public class VehicleLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<VehicleEntity>(dbFactory)
{
    /// <summary>Devuelve todos los vehículos.</summary>
    public Task<List<VehicleEntity>> VehicleListAsync() => ListAsync(q => q.Include(v => v.Sede).OrderBy(v => v.Marca).ThenBy(v => v.Modelo));

    /// <summary>Busca un vehículo por su ID. Devuelve null si no existe.</summary>
    public Task<VehicleEntity?> GetVehicleByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un vehículo.</summary>
    public Task CreateVehicleAsync(VehicleEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza un vehículo.</summary>
    public Task UpdateVehicleAsync(VehicleEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina un vehículo por su ID.</summary>
    public Task DeleteVehicleAsync(int id) => DeleteAsync(id);

    protected override void Normalize(VehicleEntity entity)
    {
        entity.Vin = entity.Vin.Trim().ToUpperInvariant();
        entity.Marca = entity.Marca.Trim();
        entity.Modelo = entity.Modelo.Trim();
    }
}
