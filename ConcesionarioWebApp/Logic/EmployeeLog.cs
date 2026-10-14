using ConcesionarioWebApp.Data;
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
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
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
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                return db.Employees.ToList();
            }
        }

        /*
         * Método que actualiza un empleado existente en la base de datos.
         */
        public static void UpdateEmployee(EmployeeEntity objEmployee)
        {
            using (var db = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                db.Employees.Update(objEmployee);
                db.SaveChanges();
            }
        }
    }
}