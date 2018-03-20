using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploSingleton
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = Singleton.Instance;
            var bar = Singleton.Instance;

            foo.Name = "juan";
            bar.Name = "pedro";
            Console.WriteLine(foo.Name);
            Console.ReadLine();
        }
    }

    public class Singleton
    {
        private static Singleton instance = null;
        protected Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                    return instance;
                }
                
                else
                {
                    //Console.WriteLine("Presidente solo hay uno.");
                    return null;
                }

            }
        }

        public string Name { get; set; }
    }
}
