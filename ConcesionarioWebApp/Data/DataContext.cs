using ConcesionarioWebApp.Model;
using Microsoft.EntityFrameworkCore;
using Model;

namespace ConcesionarioWebApp.Data
{
    public class DataContext : DbContext
    {
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
        public DbSet<PayEntity> Pay { get; set; }
        public DbSet<CreditEntity> Credits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}