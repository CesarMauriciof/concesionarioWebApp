using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de garantías.</summary>
public class WarrantyLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<WarrantyEntity>(dbFactory)
{
    /// <summary>Devuelve todos los garantías.</summary>
    public Task<List<WarrantyEntity>> WarrantyListAsync() => ListAsync(q => q.OrderByDescending(w => w.FechaInicio));

    /// <summary>Busca una garantía por su ID. Devuelve null si no existe.</summary>
    public Task<WarrantyEntity?> GetWarrantyByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra una garantía.</summary>
    public Task CreateWarrantyAsync(WarrantyEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza una garantía.</summary>
    public Task UpdateWarrantyAsync(WarrantyEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina una garantía por su ID.</summary>
    public Task DeleteWarrantyAsync(int id) => DeleteAsync(id);

    protected override void Normalize(WarrantyEntity entity)
    {
        entity.Condiciones = entity.Condiciones.Trim();
    }
}
