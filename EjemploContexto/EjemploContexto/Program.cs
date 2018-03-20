using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploContexto
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> WriteLine = Console.WriteLine;
            var printer = new Print();
            var dataBase = new DataBase();
            var context = new Context(dataBase, WriteLine, printer);

            var pizza = new Pizza() { Name = "Carbonara" };
            var perro = new Perro() { Raza = "Carlino" };
            var gato = new Gato() { /*Odio = "Mucho"*/ };
            
                context.Save(pizza);
                context.Save(perro);
                context.Save(gato);
            
            Console.ReadLine();
        }
    }

    public class EntityBase
    {
        public Guid ID { get; set; }
    }

    public class Perro : EntityBase
    {
        public string Raza { get; set; }
    }

    public class Gato : EntityBase
    {
        public string Odio { get; set; }
    }

    public class Pizza : EntityBase
    {
        public string Name { get; set; }
    }

    public class Context
    {
        readonly IDataBase _dataBase;
        readonly Action<string> _writeLine;
        readonly IPrint _printer;
        public Context(IDataBase dataBase, Action<string> writeLine, IPrint printer)
        {
            _writeLine = writeLine;
            _dataBase = dataBase;
            _printer = printer;
        }

        private Guid GetID()
        {
            return Guid.NewGuid();
        }

        public void Save<T>(T entity) where T : EntityBase
        {
            entity.ID = GetID();
            var sql = _printer.Printer(_dataBase.GetSql(entity));
            _writeLine(sql);
        }
    }

    public interface IDataBase
    {
        List<List<string>> GetSql(object o);
    }

    public class DataBase : IDataBase
    {
        public List<List<string>> GetSql(object o)
        {
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
                    catch (NullReferenceException)
                    {
                        values.Add(" ");
                    }
                }
                var nlist = new List<string>() { t.Name };

                return new List<List<string>> { nlist, names, values };       
        }
    }

    public interface IPrint
    {
        string Printer(List<List<string>> list);
    }

    public class Print : IPrint
    {
        public string Printer(List<List<string>> list)
        {
            var name = list[0];
            var names = list[1];
            var values = list[2];

            StringBuilder str = new StringBuilder();
            str.Append("Insert into ");
            str.Append(name[0]);
            str.Append(", Properties ");
            str.Append("(" + String.Join(", ", names) + ")");
            str.Append(", Values ");
            str.Append("(" + String.Join(", ", values) + ")");

            return str.ToString();
        }
    }

}
