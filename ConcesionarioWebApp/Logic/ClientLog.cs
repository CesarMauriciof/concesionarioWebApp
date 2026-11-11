using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de clientes.</summary>
public class ClientLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<ClientEntity>(dbFactory)
{
    /// <summary>Devuelve todos los clientes.</summary>
    public Task<List<ClientEntity>> ClientListAsync() => ListAsync(q => q.OrderBy(c => c.Nombre));

    /// <summary>Busca un cliente por su ID. Devuelve null si no existe.</summary>
    public Task<ClientEntity?> GetClientByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un cliente.</summary>
    public Task CreateClientAsync(ClientEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza un cliente.</summary>
    public Task UpdateClientAsync(ClientEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina un cliente por su ID.</summary>
    public Task DeleteClientAsync(int id) => DeleteAsync(id);

    protected override void Normalize(ClientEntity entity)
    {
        entity.Cedula = entity.Cedula.Trim();
        entity.Nombre = entity.Nombre.Trim();
        entity.Telefono = entity.Telefono?.Trim();
        entity.Correo = entity.Correo?.Trim().ToLowerInvariant();
        entity.Direccion = entity.Direccion?.Trim();
    }
}
