using ConcesionarioWebApp.Data;
using ConcesionarioWebApp.Model;

namespace Logic
{
    public class VehicleLog
    {
        /*
         * Método que permite crear un nuevo vehículo en la base de datos.
         */
        public static void CreateVehicle(VehicleEntity objVehicle)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Vehicles.Add(objVehicle);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todos los vehículos existentes en la base de datos.
         */
        public static List<VehicleEntity> VehicleList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Vehicles.ToList();
            }
        }

        /*
         * Método que actualiza un vehículo existente en la base de datos.
         */
        public static void UpdateVehicle(VehicleEntity objVehicle)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Vehicles.Update(objVehicle);
                db.SaveChanges();
            }
        }
    }
}