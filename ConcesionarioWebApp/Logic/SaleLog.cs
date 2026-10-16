using Data;
using Model;

namespace Logic
{
    public class SaleLog
    {
        /*
         * Método que permite crear una nueva venta en la base de datos.
         */
        public static void CreateSale(SaleEntity objSale)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Sales.Add(objSale);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todas las ventas existentes en la base de datos.
         */
        public static List<SaleEntity> SaleList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Sales.ToList();
            }
        }

        /*
         * Método que actualiza una venta existente en la base de datos.
         */
        public static void UpdateSale(SaleEntity objSale)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Sales.Update(objSale);
                db.SaveChanges();
            }
        }
    }
}