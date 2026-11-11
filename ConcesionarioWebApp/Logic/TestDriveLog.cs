using Data;
using Logic.Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Logic;

/// <summary>Lógica de negocio de pruebas de manejo.</summary>
public class TestDriveLog(IDbContextFactory<DataContext> dbFactory) : BaseLog<TestDriveEntity>(dbFactory)
{
    /// <summary>Tiempo que se reserva el vehículo para cada prueba de manejo.</summary>
    public static readonly TimeSpan DuracionPrueba = TimeSpan.FromHours(1);

    /// <summary>Devuelve todas las pruebas de manejo con cliente, vehículo y asesor.</summary>
    public Task<List<TestDriveEntity>> TestDriveListAsync() =>
        ListAsync(q => q.Include(x => x.Cliente).Include(x => x.Vehiculo).Include(x => x.Empleado)
            .OrderByDescending(x => x.FechaHora));

    /// <summary>Busca una prueba de manejo por su ID. Devuelve null si no existe.</summary>
    public Task<TestDriveEntity?> GetTestDriveByIdAsync(int id) => GetByIdAsync(id);

    /// <summary>Agenda una prueba de manejo (no en el pasado, vehículo sin vender y libre a esa hora).</summary>
    public async Task CreateTestDriveAsync(TestDriveEntity entity)
    {
        if (entity.Estado == TestDriveStatus.Programada && entity.FechaHora < DateTime.Now)
        {
            throw new BusinessException("No se puede programar una prueba de manejo en una fecha u hora pasada.");
        }

        await ValidateScheduleAsync(entity);
        await CreateAsync(entity);
    }

    /// <summary>Actualiza una prueba de manejo (p. ej. para marcarla como realizada o cancelada).</summary>
    public async Task UpdateTestDriveAsync(TestDriveEntity entity)
    {
        await ValidateScheduleAsync(entity);
        await UpdateAsync(entity);
    }

    /// <summary>Elimina una prueba de manejo por su ID.</summary>
    public Task DeleteTestDriveAsync(int id) => DeleteAsync(id);

    protected override void Normalize(TestDriveEntity entity)
    {
        entity.Observaciones = string.IsNullOrWhiteSpace(entity.Observaciones) ? null : entity.Observaciones.Trim();
        if (entity.IdEmpleado <= 0)
        {
            entity.IdEmpleado = null;
        }
    }

    private async Task ValidateScheduleAsync(TestDriveEntity entity)
    {
        // Las pruebas canceladas o ya realizadas no bloquean la agenda
        if (entity.Estado != TestDriveStatus.Programada)
        {
            return;
        }

        await using var db = await DbFactory.CreateDbContextAsync();

        if (await db.SaleDetails.AnyAsync(d => d.IdVehiculo == entity.IdVehiculo))
        {
            throw new BusinessException("El vehículo seleccionado ya fue vendido; no se puede agendar una prueba de manejo.");
        }

        var desde = entity.FechaHora - DuracionPrueba;
        var hasta = entity.FechaHora + DuracionPrueba;
        var conflict = await db.TestDrives
            .Where(t => t.IdVehiculo == entity.IdVehiculo
                        && t.IdPrueba != entity.IdPrueba
                        && t.Estado == TestDriveStatus.Programada
                        && t.FechaHora > desde && t.FechaHora < hasta)
            .Select(t => (DateTime?)t.FechaHora)
            .FirstOrDefaultAsync();

        if (conflict != null)
        {
            throw new BusinessException(
                $"El vehículo ya tiene una prueba de manejo programada el {conflict:dd/MM/yyyy} a las {conflict:HH:mm}. " +
                "Elija otra hora (cada prueba reserva el vehículo por 1 hora).");
        }
    }
}
