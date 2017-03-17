using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Replify;

namespace Client.Repl
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleOutput output = new ConsoleOutput();
            while (Core.AppConstants.API_TOKEN == null)
            {
                Console.WriteLine("No valid API token detected. Please log in.");
                Console.Write("Username: ");
                string user = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();
                Core.User.RunLoginAsync(output, user, pass);
            }
            //Entry point fo REPL command line interface
            var repl = new ClearScriptRepl();
            repl.AddHostType("Console", typeof(Console));
            repl.StartReplLoop();
        }
    }
}
