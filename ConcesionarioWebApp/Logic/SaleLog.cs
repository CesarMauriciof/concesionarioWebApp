using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de ventas.</summary>
public class SaleLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<SaleEntity>(dbFactory)
{
    /// <summary>Devuelve todos los ventas.</summary>
    public Task<List<SaleEntity>> SaleListAsync() => ListAsync(q => q.Include(s => s.Cliente).Include(s => s.Empleado).OrderByDescending(s => s.Fecha));

    /// <summary>Busca una venta por su ID. Devuelve null si no existe.</summary>
    public Task<SaleEntity?> GetSaleByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra una venta.</summary>
    public Task CreateSaleAsync(SaleEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza una venta.</summary>
    public Task UpdateSaleAsync(SaleEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina una venta por su ID.</summary>
    public Task DeleteSaleAsync(int id) => DeleteAsync(id);
}
