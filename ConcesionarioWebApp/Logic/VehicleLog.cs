using ConcesionarioWebApp.Model;
using Data;

namespace Logic
{
    public class VehicleLog
    {
        public static void CreateVehicle(VehicleEntity objVehicle)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Vehicles.Add(objVehicle);
                db.SaveChanges();
            }
        }

        public static List<VehicleEntity> VehicleList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Vehicles.ToList();
            }
        }

        // ➕ Método nuevo para buscar por ID (necesario para la edición)
        public static VehicleEntity? GetVehicleById(int id)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Vehicles.Find(id);
            }
        }

        public static void UpdateVehicle(VehicleEntity objVehicle)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Vehicles.Update(objVehicle);
                db.SaveChanges();
            }
        }

        // ➕ Método nuevo para eliminar un vehículo
        public static void DeleteVehicle(int id)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                var vehicle = db.Vehicles.Find(id);
                if (vehicle != null)
                {
                    db.Vehicles.Remove(vehicle);
                    db.SaveChanges();
                }
            }
        }
    }
}