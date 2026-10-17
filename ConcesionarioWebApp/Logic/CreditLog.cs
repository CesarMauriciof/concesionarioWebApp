using Data;
using Model;

namespace Logic;

public class CreditLog
{
    /*
     * Método que permite crear una nueva venta en la base de datos.
     */
    public static void CreateCredit(CreditEntity objCredit)
    {
        using (var db = new DataContext())
        {
            db.Credits.Add(objCredit);
            db.SaveChanges();
        }
    }

    /*
     * Método que devuelve una lista de todas las ventas existentes en la base de datos.
     */
    public static List<CreditEntity> CreditList()
    {
        using (var db = new DataContext())
        {
            return db.Credits.ToList();
        }
    }

    /*
     * Método que actualiza una venta existente en la base de datos.
     */
    public static void UpdateCredit(CreditEntity objCredit)
    {
        using (var db = new DataContext())
        {
            db.Credits.Update(objCredit);
            db.SaveChanges();
        }
    }

    /*
     * Método que busca un crédito por su ID. Devuelve null si no existe.
     */
    public static CreditEntity? GetCreditById(int id)
    {
        using (var db = new DataContext())
        {
            return db.Credits.Find(id);
        }
    }

    /*
     * Método que elimina un crédito por su ID.
     */
    public static void DeleteCredit(int id)
    {
        using (var db = new DataContext())
        {
            var entity = db.Credits.Find(id);
            if (entity != null)
            {
                db.Credits.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
