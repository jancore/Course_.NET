using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaExamen
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> drawer = Console.Write;
            var printer = new Print();
            var dataBase = new DataBase();
            var context = new Context(dataBase, printer);

            var canvas = Canvas.Instance;
            var heart = new Heart();
            var ray = new Ray();
            var star = new Star();
            List<string> list = new List<string>();

            heart.BorderColor = "Pink";
            heart.FontColor = "Red";
            ray.BorderColor = "Black";
            ray.FontColor = "Yellow";
            star.BorderColor = "Orange";
            star.FontColor = "Orange";

            list.Add(context.Draw(heart));
            list.Add(context.Draw(ray));
            list.Add(context.Draw(star));
            canvas.Figures = list;

            foreach(var figure in canvas.Figures)
            {
                drawer(figure);
            }
            Console.ReadLine();
        }
    }

    public class Canvas //Lienzo con patrón Singleton
    {
        private static Canvas instance = null;
        protected Canvas() { }

        public static Canvas Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Canvas();
                    return instance;
                }

                else
                    return null;
            }
        }

        public List<string> Figures; //Figuras que hay en el lienzo.
    }

    public interface IShape
    {
        string Name { get; set; }
        string FontColor { get; set; }
        string BorderColor { get; set; }
    }

    public class Heart : IShape
    {
        public string Name { get; set; }
        public string FontColor { get; set; }
        public string BorderColor { get; set; }

        public Heart()
        {
            Name = "Heart";
        }
    }

    public class Ray : IShape
    {
        public string Name { get; set; }
        public string FontColor { get; set; }
        public string BorderColor { get; set; }

        public Ray()
        {
            Name = "Ray";
        }
    }

    public class Star : IShape
    {
        public string Name { get; set; }
        public string FontColor { get; set; }
        public string BorderColor { get; set; }

        public Star()
        {
            Name = "Star";
        }
    }

    public class Context
    {
        readonly IDataBase _dataBase;
        readonly IPrint _printer;
        public Context(IDataBase dataBase, IPrint printer)
        {
            _dataBase = dataBase;
            _printer = printer;
        }

        public string Draw<T>(T entity) where T : IShape
        {
            var sql = _printer.Printer(_dataBase.GetSql(entity));
            return sql;
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
            str.Append("Figure ");
            str.Append(name[0]);
            str.Append(", Properties ");
            str.Append("(" + String.Join(", ", names) + ")");
            str.Append(", Values ");
            str.Append("(" + String.Join(", ", values) + ")");
            str.Append("\n");

            return str.ToString();
        }
    }
}
