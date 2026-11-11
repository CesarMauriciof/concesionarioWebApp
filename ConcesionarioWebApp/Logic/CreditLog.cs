using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de créditos.</summary>
public class CreditLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<CreditEntity>(dbFactory)
{
    /// <summary>Devuelve todos los créditos.</summary>
    public Task<List<CreditEntity>> CreditListAsync() => ListAsync(q => q.OrderByDescending(c => c.IdCredito));

    /// <summary>Busca un crédito por su ID. Devuelve null si no existe.</summary>
    public Task<CreditEntity?> GetCreditByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un crédito.</summary>
    public Task CreateCreditAsync(CreditEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza un crédito.</summary>
    public Task UpdateCreditAsync(CreditEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina un crédito por su ID.</summary>
    public Task DeleteCreditAsync(int id) => DeleteAsync(id);

    protected override void Normalize(CreditEntity entity)
    {
        entity.EntidadBancaria = entity.EntidadBancaria.Trim();
    }
}
