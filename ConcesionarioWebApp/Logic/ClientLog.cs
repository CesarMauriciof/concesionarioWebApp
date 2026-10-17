using ConcesionarioWebApp.Model;
using Data;

namespace Logic
{
    public class ClientLog
    {
        /*
         * Método que permite crear un nuevo cliente en la base de datos.
         */
        public static void CreateClient(ClientEntity objClient)
        {
            using (var db = new DataContext())
            {
                db.Clients.Add(objClient);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todos los clientes existentes en la base de datos.
         */
        public static List<ClientEntity> ClientList()
        {
            using (var db = new DataContext())
            {
                return db.Clients.ToList();
            }
        }

        /*
         * Método que actualiza un cliente existente en la base de datos.
         */
        public static void UpdateClient(ClientEntity objClient)
        {
            using (var db = new DataContext())
            {
                db.Clients.Update(objClient);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca un cliente por su ID. Devuelve null si no existe.
         */
        public static ClientEntity? GetClientById(int id)
        {
            using (var db = new DataContext())
            {
                return db.Clients.Find(id);
            }
        }

        /*
         * Método que elimina un cliente por su ID.
         */
        public static void DeleteClient(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.Clients.Find(id);
                if (entity != null)
                {
                    db.Clients.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
