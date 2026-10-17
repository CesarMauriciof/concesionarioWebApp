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
            using (var db = new DataContext())
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
            using (var db = new DataContext())
            {
                return db.Sales.ToList();
            }
        }

        /*
         * Método que actualiza una venta existente en la base de datos.
         */
        public static void UpdateSale(SaleEntity objSale)
        {
            using (var db = new DataContext())
            {
                db.Sales.Update(objSale);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca una venta por su ID. Devuelve null si no existe.
         */
        public static SaleEntity? GetSaleById(int id)
        {
            using (var db = new DataContext())
            {
                return db.Sales.Find(id);
            }
        }

        /*
         * Método que elimina una venta por su ID.
         */
        public static void DeleteSale(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.Sales.Find(id);
                if (entity != null)
                {
                    db.Sales.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
