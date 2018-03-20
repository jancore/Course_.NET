using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploReflexion
{
    class Program
    {
        static void Main(string[] args)
        {
            var pizza = new Pizza() { Nombre = "Carbonara"/*, Observacion = "Sin nata"*/ };
            Console.WriteLine(Sql.GetSql(pizza));
            Console.WriteLine(Sql.GetSql(null));
            Console.ReadLine();
        }   
    }
    
    public class Pizza
    {
        public string Nombre { get; set; }
        public string Observacion { get; set; }
    }

    public class Context
    {
        readonly IPrint _print;
        public Context(IPrint print)
        {
            _print = print;
        }

        public string Save(string name, List<string> names, List<string> values)
        {
            var sql = _print.Printer(name, names, values);
            return sql;
        }
    }

    public class Sql
    {
        public static string GetSql(object o)
        {
            try
            {
                var print = new Print();
                var context = new Context(print);
                Type t = o.GetType();
                var properties = t.GetProperties();
                var names = new List<string>();
                var values = new List<string>();
                foreach (var property in properties)
                {
                    var name = property.Name;                   
                    names.Add(name);
                    try
                    {
                        var value = property.GetValue(o);
                        values.Add(value.ToString());
                    }
                    catch(NullReferenceException)
                    {
                        values.Add(" ");
                    }
                    
                }

                return context.Save(t.Name, names, values);
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("El objeto es Nulo.");
                return String.Empty;
            }
            
        }
    }

    public interface IPrint
    {
        string Printer(string name, List<string> names, List<string> values);
    }
    public class Print : IPrint
    {
        public string Printer(string name, List<string> names, List<string> values)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Insert into ");
            str.Append(name);
            str.Append(", Properties ");
            str.Append("(" + String.Join(", ", names) + ")");
            str.Append(", Values ");
            str.Append("(" + String.Join(", ", values) + ")");

            return str.ToString();
        }
    }
}
//Solucionar el problema de si una prapiedad vale null y que el código cumpla con el principio de responsabilidad única.
//Es decir, que cada clase haga solo una cosa.
//GetSql debe devolver un string llamado sql que devuelva toda la información de cada clase.