using Data;
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
            using (var db = new DataContext())
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
            using (var db = new DataContext())
            {
                return db.Maintenances.ToList();
            }
        }

        /*
         * Método que actualiza un mantenimiento existente en la base de datos.
         */
        public static void UpdateMaintenance(MaintenanceEntity objMaintenance)
        {
            using (var db = new DataContext())
            {
                db.Maintenances.Update(objMaintenance);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca un mantenimiento por su ID. Devuelve null si no existe.
         */
        public static MaintenanceEntity? GetMaintenanceById(int id)
        {
            using (var db = new DataContext())
            {
                return db.Maintenances.Find(id);
            }
        }

        /*
         * Método que elimina un mantenimiento por su ID.
         */
        public static void DeleteMaintenance(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.Maintenances.Find(id);
                if (entity != null)
                {
                    db.Maintenances.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
