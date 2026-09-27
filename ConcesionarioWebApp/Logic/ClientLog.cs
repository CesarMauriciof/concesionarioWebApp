using ConcesionarioWebApp.Data;
using ConcesionarioWebApp.Model;

namespace Logic
{
    public class ClientLog
    {
        /*
         * Método que permite crear un nuevo cliente en la base de datos.
         */
        public void CreateClient(ClientEntity objClient)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Clients.Add(objClient);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todos los clientes existentes en la base de datos.
         */
        public List<ClientEntity> ClientList()
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Clients.ToList();
            }
        }

        /*
         * Método que actualiza un cliente existente en la base de datos.
         */
        public void UpdateClient(ClientEntity objClient)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Clients.Update(objClient);
                db.SaveChanges();
            }
        }
    }
}