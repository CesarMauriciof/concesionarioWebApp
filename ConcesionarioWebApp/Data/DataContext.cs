using ConcesionarioWebApp.Model;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data
{
    public class DataContext : DbContext
    {
        // Constructor usado por la capa Logic: la conexión se configura en OnConfiguring
        public DataContext()
        {
        }

        // Constructor usado en tiempo de diseño (DataContextFactory / migraciones)
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<HeadquartersEntity> Headquarters { get; set; }
        public DbSet<MaintenanceEntity> Maintenances { get; set; }
        public DbSet<SaleDetailEntity> SaleDetails { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<WarrantyEntity> Warranties { get; set; }
        public DbSet<PayEntity> Pays { get; set; }
        public DbSet<CreditEntity> Credits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user=root;password=r00t123;port=3306;database=concesionario",
                    new MySqlServerVersion(new Version(8, 0, 34)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // venta.id_cliente es la FK de la relación Cliente -> Ventas (evita una columna sombra inexistente)
            modelBuilder.Entity<ClientEntity>()
                .HasMany(c => c.venta)
                .WithOne()
                .HasForeignKey(s => s.id_cliente);

            // Estas FK admiten NULL en MySQL (ON DELETE SET NULL / datos existentes). [Required] se mantiene
            // en el modelo solo para validar formularios; aquí se le indica a EF que la columna es opcional.
            modelBuilder.Entity<EmployeeEntity>().Property(e => e.IdSede).IsRequired(false);
            modelBuilder.Entity<VehicleEntity>().Property(v => v.IdSede).IsRequired(false);
            modelBuilder.Entity<SaleEntity>().Property(s => s.id_cliente).IsRequired(false);
            modelBuilder.Entity<SaleEntity>().Property(s => s.id_empleado).IsRequired(false);

            modelBuilder.Entity<ClientEntity>().HasData(
                new ClientEntity
                {
                    Id_Cliente = 001, Cedula = "001", Nombre = "Yeison Anaya", Telefono = "310546891",
                    Correo = "Yeison@gmail.com", Direccion = "carrera 5 #24-10"
                });
        }
    }
}