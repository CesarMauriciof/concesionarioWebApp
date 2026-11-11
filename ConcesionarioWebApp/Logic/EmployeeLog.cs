using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de empleados.</summary>
public class EmployeeLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<EmployeeEntity>(dbFactory)
{
    /// <summary>Devuelve todos los empleados.</summary>
    public Task<List<EmployeeEntity>> EmployeeListAsync() => ListAsync(q => q.Include(e => e.Sede).OrderBy(e => e.Nombre));

    /// <summary>Busca un empleado por su ID. Devuelve null si no existe.</summary>
    public Task<EmployeeEntity?> GetEmployeeByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Registra un empleado.</summary>
    public Task CreateEmployeeAsync(EmployeeEntity entity) => CreateAsync(entity);

    /// <summary>Actualiza un empleado.</summary>
    public Task UpdateEmployeeAsync(EmployeeEntity entity) => UpdateAsync(entity);

    /// <summary>Elimina un empleado por su ID.</summary>
    public Task DeleteEmployeeAsync(int id) => DeleteAsync(id);

    protected override void Normalize(EmployeeEntity entity)
    {
        entity.Cedula = entity.Cedula.Trim();
        entity.Nombre = entity.Nombre.Trim();
        entity.Cargo = entity.Cargo.Trim();
    }
}
