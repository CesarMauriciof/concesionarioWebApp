using Data;
using Model;

namespace Logic;

public class PayLog
{
    /*
     * Método que permite crear una nueva venta en la base de datos.
     */
    public static void CreatePay(PayEntity objPay)
    {
        using (var db = new DataContext())
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
        using (var db = new DataContext())
        {
            return db.Pays.ToList();
        }
    }

    /*
     * Método que actualiza una venta existente en la base de datos.
     */
    public static void UpdatePay(PayEntity objPay)
    {
        using (var db = new DataContext())
        {
            db.Pays.Update(objPay);
            db.SaveChanges();
        }
    }

    /*
     * Método que busca un pago por su ID. Devuelve null si no existe.
     */
    public static PayEntity? GetPayById(int id)
    {
        using (var db = new DataContext())
        {
            return db.Pays.Find(id);
        }
    }

    /*
     * Método que elimina un pago por su ID.
     */
    public static void DeletePay(int id)
    {
        using (var db = new DataContext())
        {
            var entity = db.Pays.Find(id);
            if (entity != null)
            {
                db.Pays.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
