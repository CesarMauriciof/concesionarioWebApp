using Data;
using Model;

namespace Logic
{
    public class HeadquartersLog
    {
        /*
         * Método que permite crear una nueva sede en la base de datos.
         */
        public static void CreateHeadquarters(HeadquartersEntity objHeadquarters)
        {
            using (var db = new DataContext())
            {
                db.Headquarters.Add(objHeadquarters);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todas las sedes existentes en la base de datos.
         */
        public static List<HeadquartersEntity> HeadquartersList()
        {
            using (var db = new DataContext())
            {
                return db.Headquarters.ToList();
            }
        }

        /*
         * Método que actualiza una sede existente en la base de datos.
         */
        public static void UpdateHeadquarters(HeadquartersEntity objHeadquarters)
        {
            using (var db = new DataContext())
            {
                db.Headquarters.Update(objHeadquarters);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca una sede por su ID. Devuelve null si no existe.
         */
        public static HeadquartersEntity? GetHeadquartersById(int id)
        {
            using (var db = new DataContext())
            {
                return db.Headquarters.Find(id);
            }
        }

        /*
         * Método que elimina una sede por su ID.
         */
        public static void DeleteHeadquarters(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.Headquarters.Find(id);
                if (entity != null)
                {
                    db.Headquarters.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
