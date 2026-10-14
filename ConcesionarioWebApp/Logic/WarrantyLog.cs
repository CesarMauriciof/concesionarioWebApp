using ConcesionarioWebApp.Data;
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
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
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
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Warranties.ToList();
            }
        }

        /*
         * Método que actualiza una garantía existente en la base de datos.
         */
        public static void UpdateWarranty(WarrantyEntity objWarranty)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Warranties.Update(objWarranty);
                db.SaveChanges();
            }
        }
    }
}