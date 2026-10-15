using ConcesionarioWebApp.Data;
using Model;

namespace Logic;

public class CreditLog
{
    /*
     * Método que permite crear una nueva venta en la base de datos.
     */
    public static void CreateCredit(CreditEntity objCredit)
    {
        using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
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
        using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
        {
            return db.Credits.ToList();
        }
    }

    /*
     * Método que actualiza una venta existente en la base de datos.
     */
    public static void UpdateCredit(CreditEntity objCredit)
    {
        using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
        {
            db.Credits.Update(objCredit);
            db.SaveChanges();
        }
    }
}