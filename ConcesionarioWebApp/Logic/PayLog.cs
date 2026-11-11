using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de pagos.</summary>
public class PayLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<PayEntity>(dbFactory)
{
    /// <summary>Devuelve todos los pagos.</summary>
    public Task<List<PayEntity>> PayListAsync() => ListAsync(q => q.OrderByDescending(p => p.FechaPago));

    /// <summary>Busca un pago por su ID. Devuelve null si no existe.</summary>
    public Task<PayEntity?> GetPayByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un pago.</summary>
    public Task CreatePayAsync(PayEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza un pago.</summary>
    public Task UpdatePayAsync(PayEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina un pago por su ID.</summary>
    public Task DeletePayAsync(int id) => DeleteAsync(id);
}
