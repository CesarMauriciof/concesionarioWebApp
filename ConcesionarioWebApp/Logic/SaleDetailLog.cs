using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de detalles de venta.</summary>
public class SaleDetailLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<SaleDetailEntity>(dbFactory)
{
    /// <summary>Devuelve todos los detalles de venta con su vehículo.</summary>
    public Task<List<SaleDetailEntity>> SaleDetailListAsync() =>
        ListAsync(q => q.Include(d => d.Vehiculo).OrderByDescending(d => d.IdDetalle));

    /// <summary>Busca un detalle de venta por su ID. Devuelve null si no existe.</summary>
    public Task<SaleDetailEntity?> GetSaleDetailByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un detalle de venta (un vehículo solo puede venderse una vez).</summary>
    public async Task CreateSaleDetailAsync(SaleDetailEntity entity)
    {
        await EnsureVehicleNotSoldAsync(entity);
        await CreateAsync(entity);
    }

    /// <summary>Actualiza un detalle de venta (un vehículo solo puede venderse una vez).</summary>
    public async Task UpdateSaleDetailAsync(SaleDetailEntity entity)
    {
        await EnsureVehicleNotSoldAsync(entity);
        await UpdateAsync(entity);
    }

    /// <summary>Elimina un detalle de venta por su ID.</summary>
    public Task DeleteSaleDetailAsync(int id) => DeleteAsync(id);

    private async Task EnsureVehicleNotSoldAsync(SaleDetailEntity entity)
    {
        await using var db = await DbFactory.CreateDbContextAsync();
        var otherSaleId = await db.SaleDetails
            .Where(d => d.IdVehiculo == entity.IdVehiculo && d.IdDetalle != entity.IdDetalle)
            .Select(d => (int?)d.IdVenta)
            .FirstOrDefaultAsync();

        if (otherSaleId != null)
        {
            throw new BusinessException($"El vehículo seleccionado ya fue vendido en la venta #{otherSaleId}.");
        }
    }
}
