using Data;
using Microsoft.EntityFrameworkCore;

namespace Logic.Common;

/// <summary>
/// Operaciones CRUD comunes a todas las entidades.
/// Cada operación crea y libera su propio DataContext a través de <see cref="IDbContextFactory{TContext}"/>,
/// que es el patrón recomendado para Blazor Server (un DbContext no es seguro entre hilos ni debe vivir
/// durante todo el circuito del usuario).
/// </summary>
public abstract class BaseLog<TEntity>(IDbContextFactory<DataContext> dbFactory) where TEntity : class
{
    protected IDbContextFactory<DataContext> DbFactory { get; } = dbFactory;

    /// <summary>Permite a cada Log limpiar/normalizar datos antes de guardar (p. ej. Trim).</summary>
    protected virtual void Normalize(TEntity entity)
    {
    }

    protected async Task<List<TEntity>> ListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query = null)
    {
        await using var db = await DbFactory.CreateDbContextAsync();
        IQueryable<TEntity> source = db.Set<TEntity>().AsNoTracking();
        return await (query?.Invoke(source) ?? source).ToListAsync();
    }

    protected async Task<TEntity?> GetByIdAsync(int id)
    {
        await using var db = await DbFactory.CreateDbContextAsync();
        var entity = await db.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            db.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    protected async Task CreateAsync(TEntity entity)
    {
        Normalize(entity);
        await using var db = await DbFactory.CreateDbContextAsync();
        db.Set<TEntity>().Add(entity);
        await SaveAsync(db);
    }

    protected async Task UpdateAsync(TEntity entity)
    {
        Normalize(entity);
        await using var db = await DbFactory.CreateDbContextAsync();
        db.Set<TEntity>().Update(entity);
        await SaveAsync(db);
    }

    protected async Task DeleteAsync(int id)
    {
        await using var db = await DbFactory.CreateDbContextAsync();
        var entity = await db.Set<TEntity>().FindAsync(id)
                     ?? throw new BusinessException("El registro que intenta eliminar no existe.");

        db.Set<TEntity>().Remove(entity);
        await SaveAsync(db);
    }

    /// <summary>Guarda los cambios convirtiendo los errores de BD en <see cref="BusinessException"/>.</summary>
    protected static async Task SaveAsync(DataContext db)
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw DbErrorTranslator.Translate(ex);
        }
    }
}
