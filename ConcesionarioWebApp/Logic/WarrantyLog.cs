using Data;
using Model;

namespace Logic
{
    public class WarrantyLog
    {
        /*
         * Método que permite crear una nueva garantía en la base de datos.
         */
        public static void CreateWarranty(WarrantyEntity objWarranty)
        {
            using (var db = new DataContext())
            {
                db.Warranties.Add(objWarranty);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todas las garantías existentes en la base de datos.
         */
        public static List<WarrantyEntity> WarrantyList()
        {
            using (var db = new DataContext())
            {
                return db.Warranties.ToList();
            }
        }

        /*
         * Método que actualiza una garantía existente en la base de datos.
         */
        public static void UpdateWarranty(WarrantyEntity objWarranty)
        {
            using (var db = new DataContext())
            {
                db.Warranties.Update(objWarranty);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca una garantía por su ID. Devuelve null si no existe.
         */
        public static WarrantyEntity? GetWarrantyById(int id)
        {
            using (var db = new DataContext())
            {
                return db.Warranties.Find(id);
            }
        }

        /*
         * Método que elimina una garantía por su ID.
         */
        public static void DeleteWarranty(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.Warranties.Find(id);
                if (entity != null)
                {
                    db.Warranties.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
