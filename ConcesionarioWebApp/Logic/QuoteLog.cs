using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de cotizaciones.</summary>
public class QuoteLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<QuoteEntity>(dbFactory)
{
    /// <summary>Devuelve todas las cotizaciones con cliente, vehículo y asesor.</summary>
    public Task<List<QuoteEntity>> QuoteListAsync() =>
        ListAsync(q => q.Include(x => x.Cliente).Include(x => x.Vehiculo).Include(x => x.Empleado)
            .OrderByDescending(x => x.Fecha).ThenByDescending(x => x.IdCotizacion));

    /// <summary>Busca una cotización por su ID. Devuelve null si no existe.</summary>
    public Task<QuoteEntity?> GetQuoteByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra una cotización. No se puede cotizar un vehículo que ya fue vendido.</summary>
    public async Task CreateQuoteAsync(QuoteEntity entity)
    {
        ValidateDates(entity);
        await using (var db = await DbFactory.CreateDbContextAsync())
        {
            if (await db.SaleDetails.AnyAsync(d => d.IdVehiculo == entity.IdVehiculo))
            {
                throw new BusinessException("No se puede cotizar el vehículo seleccionado porque ya fue vendido.");
            }
        }

        await CreateAsync(entity);
    }

    /// <summary>Actualiza una cotización.</summary>
    public async Task UpdateQuoteAsync(QuoteEntity entity)
    {
        ValidateDates(entity);
        await UpdateAsync(entity);
    }

    /// <summary>Elimina una cotización por su ID.</summary>
    public Task DeleteQuoteAsync(int id) => DeleteAsync(id);

    protected override void Normalize(QuoteEntity entity)
    {
        entity.Observaciones = string.IsNullOrWhiteSpace(entity.Observaciones) ? null : entity.Observaciones.Trim();
        if (entity.IdEmpleado <= 0)
        {
            entity.IdEmpleado = null;
        }
    }

    private static void ValidateDates(QuoteEntity entity)
    {
        if (entity.FechaVencimiento.Date < entity.Fecha.Date)
        {
            throw new BusinessException("La fecha de vencimiento no puede ser anterior a la fecha de la cotización.");
        }
    }
}
