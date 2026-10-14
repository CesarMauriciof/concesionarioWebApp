using ConcesionarioWebApp.Data;
using Model;

namespace Logic;

public class PayLog
{
    /*
     * Método que permite crear una nueva venta en la base de datos.
     */
    public static void CreatePay(PayEntity objPay)
    {
        using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
        {
            db.Pays.Add(objPay);
            db.SaveChanges();
        }
    }

    /*
     * Método que devuelve una lista de todas las ventas existentes en la base de datos.
     */
    public static List<PayEntity> PayList()
    {
        using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
        {
            return db.Pays.ToList();
        }
    }

    /*
     * Método que actualiza una venta existente en la base de datos.
     */
    public static void UpdatePay(PayEntity objPay)
    {
        using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
        {
            db.Pays.Update(objPay);
            db.SaveChanges();
        }
    }
}

