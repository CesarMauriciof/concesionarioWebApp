using Data;
using Model;

namespace Logic
{
    public class SaleDetailLog
    {
        /*
         * Método que permite crear un nuevo detalle de venta en la base de datos.
         */
        public static void CreateSaleDetail(SaleDetailEntity objSaleDetail)
        {
            using (var db = new DataContext())
            {
                db.SaleDetails.Add(objSaleDetail);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todos los detalles de venta existentes.
         */
        public static List<SaleDetailEntity> SaleDetailList()
        {
            using (var db = new DataContext())
            {
                return db.SaleDetails.ToList();
            }
        }

        /*
         * Método que actualiza un detalle de venta existente en la base de datos.
         */
        public static void UpdateSaleDetail(SaleDetailEntity objSaleDetail)
        {
            using (var db = new DataContext())
            {
                db.SaleDetails.Update(objSaleDetail);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca un detalle de venta por su ID. Devuelve null si no existe.
         */
        public static SaleDetailEntity? GetSaleDetailById(int id)
        {
            using (var db = new DataContext())
            {
                return db.SaleDetails.Find(id);
            }
        }

        /*
         * Método que elimina un detalle de venta por su ID.
         */
        public static void DeleteSaleDetail(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.SaleDetails.Find(id);
                if (entity != null)
                {
                    db.SaleDetails.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
