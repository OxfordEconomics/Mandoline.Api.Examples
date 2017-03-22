namespace Client.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Replify;

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Entry point fo REPL command line interface
            var repl = new ClearScriptRepl();
            repl.AddHostType("Console", typeof(Console));
            repl.StartReplLoop();
        }
    }
}
