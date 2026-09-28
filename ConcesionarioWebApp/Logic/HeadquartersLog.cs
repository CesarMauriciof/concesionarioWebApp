using ConcesionarioWebApp.Data;
using Model;

namespace Logic
{
    public class HeadquartersLog
    {
        /*
         * Método que permite crear una nueva sede en la base de datos.
         */
        public void CreateHeadquarters(HeadquartersEntity objHeadquarters)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Headquarters.Add(objHeadquarters);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todas las sedes existentes en la base de datos.
         */
        public List<HeadquartersEntity> HeadquartersList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Headquarters.ToList();
            }
        }

        /*
         * Método que actualiza una sede existente en la base de datos.
         */
        public void UpdateHeadquarters(HeadquartersEntity objHeadquarters)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Headquarters.Update(objHeadquarters);
                db.SaveChanges();
            }
        }
    }
}