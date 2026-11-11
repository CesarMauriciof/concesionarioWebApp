using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de sedes.</summary>
public class HeadquartersLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<HeadquartersEntity>(dbFactory)
{
    /// <summary>Devuelve todos los sedes.</summary>
    public Task<List<HeadquartersEntity>> HeadquartersListAsync() => ListAsync(q => q.OrderBy(h => h.NombreSede));

    /// <summary>Busca una sede por su ID. Devuelve null si no existe.</summary>
    public Task<HeadquartersEntity?> GetHeadquartersByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra una sede.</summary>
    public Task CreateHeadquartersAsync(HeadquartersEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza una sede.</summary>
    public Task UpdateHeadquartersAsync(HeadquartersEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina una sede por su ID.</summary>
    public Task DeleteHeadquartersAsync(int id) => DeleteAsync(id);

    protected override void Normalize(HeadquartersEntity entity)
    {
        entity.NombreSede = entity.NombreSede.Trim();
        entity.Ciudad = entity.Ciudad.Trim();
        entity.Direccion = entity.Direccion.Trim();
    }
}
