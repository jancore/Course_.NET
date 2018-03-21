using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace EjemploConsulta
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> WriteLine = Console.WriteLine;
            var path = @"C:\Users\AntonioJavier\source\repos\EjemploConsulta\Bourne.json";
            var commands = JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText(path));
            Print.Printer(commands, WriteLine);

            Console.ReadLine();
        }
    }

    public class Command
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Sql { get; set; }
        public List<Param> Params { get; set; }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public static class Print
    {
        public static void Printer(List<Command> commands, Action<string> writeLine)
        {
            var commandsprojection = commands.Select(c => new { id = c.Id, desc = c.Description });
            foreach (var command in commandsprojection)
            {
                writeLine(string.Format("{0}- {1}", command.id, command.desc));
            }
        }
    }
}
