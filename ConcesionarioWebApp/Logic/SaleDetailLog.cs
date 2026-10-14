using ConcesionarioWebApp.Data;
using Microsoft.EntityFrameworkCore;
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
            using (var db = new DataContext(new DbContextOptions<DataContext>()))
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
            using (var db = new DataContext(new DbContextOptions<DataContext>()))
            {
                return db.SaleDetails.ToList();
            }
        }

        /*
         * Método que actualiza un detalle de venta existente en la base de datos.
         */
        public static void UpdateSaleDetail(SaleDetailEntity objSaleDetail)
        {
            using (var db = new DataContext(new DbContextOptions<DataContext>()))
            {
                db.SaleDetails.Update(objSaleDetail);
                db.SaveChanges();
            }
        }
    }
}