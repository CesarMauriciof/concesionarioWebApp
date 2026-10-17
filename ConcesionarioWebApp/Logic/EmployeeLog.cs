using Data;
using Model;

namespace Logic
{
    public class EmployeeLog
    {
        /*
         * Método que permite crear un nuevo empleado en la base de datos.
         */
        public static void CreateEmployee(EmployeeEntity objEmployee)
        {
            using (var db = new DataContext())
            {
                db.Employees.Add(objEmployee);
                db.SaveChanges();
            }
        }

        /*
         * Método que devuelve una lista de todos los empleados existentes en la base de datos.
         */
        public static List<EmployeeEntity> EmployeeList()
        {
            using (var db = new DataContext())
            {
                return db.Employees.ToList();
            }
        }

        /*
         * Método que actualiza un empleado existente en la base de datos.
         */
        public static void UpdateEmployee(EmployeeEntity objEmployee)
        {
            using (var db = new DataContext())
            {
                db.Employees.Update(objEmployee);
                db.SaveChanges();
            }
        }

        /*
         * Método que busca un empleado por su ID. Devuelve null si no existe.
         */
        public static EmployeeEntity? GetEmployeeById(int id)
        {
            using (var db = new DataContext())
            {
                return db.Employees.Find(id);
            }
        }

        /*
         * Método que elimina un empleado por su ID.
         */
        public static void DeleteEmployee(int id)
        {
            using (var db = new DataContext())
            {
                var entity = db.Employees.Find(id);
                if (entity != null)
                {
                    db.Employees.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
