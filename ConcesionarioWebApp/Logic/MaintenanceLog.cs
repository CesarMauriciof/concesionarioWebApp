using ConcesionarioWebApp.Data;
using Model;

namespace Logic
{
    public class MaintenanceLog
    {
        /*
         * Método que permite crear un nuevo mantenimiento en la base de datos.
         */
        public static void CreateMaintenance(MaintenanceEntity objMaintenance)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Maintenances.Add(objMaintenance);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todos los mantenimientos existentes en la base de datos.
         */
        public static List<MaintenanceEntity> MaintenanceList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Maintenances.ToList();
            }
        }

        /*
         * Método que actualiza un mantenimiento existente en la base de datos.
         */
        public static void UpdateMaintenance(MaintenanceEntity objMaintenance)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Maintenances.Update(objMaintenance);
                db.SaveChanges();
            }
        }
    }
}

    
