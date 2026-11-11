using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

/// <summary>
/// Contexto de EF Core. La cadena de conexión NO se define aquí: se inyecta desde la configuración
/// (appsettings / user-secrets) al registrar el contexto con <c>AddDbContextFactory</c>.
/// </summary>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();
    public DbSet<HeadquartersEntity> Headquarters => Set<HeadquartersEntity>();
    public DbSet<MaintenanceEntity> Maintenances => Set<MaintenanceEntity>();
    public DbSet<SaleDetailEntity> SaleDetails => Set<SaleDetailEntity>();
    public DbSet<SaleEntity> Sales => Set<SaleEntity>();
    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();
    public DbSet<WarrantyEntity> Warranties => Set<WarrantyEntity>();
    public DbSet<PayEntity> Pays => Set<PayEntity>();
    public DbSet<CreditEntity> Credits => Set<CreditEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasCharSet("utf8mb4").UseCollation("utf8mb4_0900_ai_ci");

        // Convención de nombres: uq_<tabla>_<columna>, ix_<tabla>_<columna>, fk_<tabla>_<tabla_referenciada>

        modelBuilder.Entity<ClientEntity>(e =>
        {
            e.HasIndex(c => c.Cedula).IsUnique().HasDatabaseName("uq_cliente_cedula");
        });

        modelBuilder.Entity<HeadquartersEntity>();

        modelBuilder.Entity<EmployeeEntity>(e =>
        {
            e.HasIndex(x => x.Cedula).IsUnique().HasDatabaseName("uq_empleado_cedula");
            e.HasIndex(x => x.IdSede).HasDatabaseName("ix_empleado_id_sede");

            // [Required] del modelo es solo para el formulario: en la BD la FK es opcional
            e.Property(x => x.IdSede).IsRequired(false);
            e.HasOne(x => x.Sede).WithMany(s => s.Empleados).HasForeignKey(x => x.IdSede)
                .OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_empleado_sede");
        });

        modelBuilder.Entity<VehicleEntity>(e =>
        {
            e.HasIndex(x => x.Vin).IsUnique().HasDatabaseName("uq_vehiculo_vin");
            e.HasIndex(x => x.IdSede).HasDatabaseName("ix_vehiculo_id_sede");

            e.Property(x => x.IdSede).IsRequired(false);
            e.HasOne(x => x.Sede).WithMany(s => s.Vehiculos).HasForeignKey(x => x.IdSede)
                .OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_vehiculo_sede");
        });

        modelBuilder.Entity<SaleEntity>(e =>
        {
            e.HasIndex(x => x.IdCliente).HasDatabaseName("ix_venta_id_cliente");
            e.HasIndex(x => x.IdEmpleado).HasDatabaseName("ix_venta_id_empleado");

            e.Property(x => x.IdCliente).IsRequired(false);
            e.Property(x => x.IdEmpleado).IsRequired(false);

            e.HasOne(x => x.Cliente).WithMany(c => c.Ventas).HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_venta_cliente");
            e.HasOne(x => x.Empleado).WithMany(emp => emp.Ventas).HasForeignKey(x => x.IdEmpleado)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_venta_empleado");
        });

        modelBuilder.Entity<SaleDetailEntity>(e =>
        {
            e.HasIndex(x => x.IdVenta).HasDatabaseName("ix_detalle_venta_id_venta");
            e.HasIndex(x => x.IdVehiculo).HasDatabaseName("ix_detalle_venta_id_vehiculo");

            // Al eliminar una venta se eliminan sus detalles
            e.HasOne(x => x.Venta).WithMany(v => v.DetallesVenta).HasForeignKey(x => x.IdVenta)
                .OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_detalle_venta_venta");
            e.HasOne(x => x.Vehiculo).WithMany(v => v.DetallesVenta).HasForeignKey(x => x.IdVehiculo)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_detalle_venta_vehiculo");
        });

        modelBuilder.Entity<PayEntity>(e =>
        {
            e.HasIndex(x => x.IdVenta).HasDatabaseName("ix_pago_id_venta");
            e.HasOne(x => x.Venta).WithMany(v => v.Pagos).HasForeignKey(x => x.IdVenta)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_pago_venta");
        });

        modelBuilder.Entity<CreditEntity>(e =>
        {
            e.HasIndex(x => x.IdPago).HasDatabaseName("ix_credito_id_pago");
            e.HasOne(x => x.Pago).WithMany(p => p.Creditos).HasForeignKey(x => x.IdPago)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_credito_pago");
        });

        modelBuilder.Entity<WarrantyEntity>(e =>
        {
            e.HasIndex(x => x.IdDetalle).HasDatabaseName("ix_garantia_id_detalle");
            e.HasOne(x => x.DetalleVenta).WithMany(d => d.Garantias).HasForeignKey(x => x.IdDetalle)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_garantia_detalle_venta");
        });

        modelBuilder.Entity<MaintenanceEntity>(e =>
        {
            e.HasIndex(x => x.IdGarantia).HasDatabaseName("ix_mantenimiento_id_garantia");
            e.HasOne(x => x.Garantia).WithMany(g => g.Mantenimientos).HasForeignKey(x => x.IdGarantia)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mantenimiento_garantia");
        });

        modelBuilder.Entity<UserEntity>(e =>
        {
            e.HasIndex(x => x.NombreUsuario).IsUnique().HasDatabaseName("uq_usuario_nombre_usuario");
            e.HasIndex(x => x.Correo).IsUnique().HasDatabaseName("uq_usuario_correo");
            // Un empleado tiene como máximo un usuario (MySQL permite varios NULL en un índice único)
            e.HasIndex(x => x.IdEmpleado).IsUnique().HasDatabaseName("uq_usuario_id_empleado");

            e.Property(x => x.ContrasenaHash).IsRequired();
            // El rol se guarda como texto ("Administrador", "Vendedor"...) para que la BD sea legible
            e.Property(x => x.Rol).HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Si se elimina el empleado, el usuario se conserva sin vínculo
            e.HasOne(x => x.Empleado).WithMany().HasForeignKey(x => x.IdEmpleado)
                .OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_usuario_empleado");
        });
    }
}
