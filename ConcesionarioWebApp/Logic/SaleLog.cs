using ConcesionarioWebApp.Data;
using Model;

namespace Logic
{
    public class SaleLog
    {
        /*
         * Método que permite crear una nueva venta en la base de datos.
         */
        public void CreateSale(SaleEntity objSale)
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
        public List<SaleEntity> SaleList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Sales.ToList();
            }
        }

        /*
         * Método que actualiza una venta existente en la base de datos.
         */
        public void UpdateSale(SaleEntity objSale)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Sales.Update(objSale);
                db.SaveChanges();
            }
        }
    }
}