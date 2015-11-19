using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hinshlabs.CommandLineParser;
using $rootnamespace$.Commands;

namespace $rootnamespace$
{
    class ProgramDemo
    {
        public static void Main(string[] args)
        {
			DefaultCommandAction commandAction = DefaultCommandAction.Exit;
            if (Debugger.IsAttached)
            {
                commandAction = DefaultCommandAction.Loop;
                Console.WriteLine("..running in loop mode...");
            }
            // Create root command collection
            CommandCollection root = new CommandCollection("root", null);
            // Add "demo1" fixed command
            root.AddCommand(new Demo1Command(root));
            // Create sub command colleciton
            CommandCollection innerlist = new CommandCollection("subcmd", root);
            // Add "demo2" delegate comamnd
            innerlist.AddCommand(new DelegateCommand<Demo3CommandLine>(root, "Demo2", OnDemo2Run, "demo 2", "no additional information", "demo 2 command", "This command shows how to delegate the run method using the delegate command"));
            // Add "demo3" fixed command
            innerlist.AddCommand(new Demo3Command(root));
            // Add sub command collcection to root
            root.AddCommand(innerlist);
            // Start run
            root.Run();

        }

        private static int OnDemo2Run()
        {
            Console.WriteLine("Running demo 2");
            return 0;
        }
    }
}
