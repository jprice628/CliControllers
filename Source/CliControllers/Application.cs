using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CliControllers
{
    public static class Application
    {
        public static IEnumerable<Controller> Controllers { get; private set; } = new Controller[0];

        public static Command Command { get; private set; } = null;

        public static string ApplicationName { get; private set; } = "Unknown";

        public static void Run(string[] args)
        {
            try
            {
                ApplicationName = Assembly.GetCallingAssembly().GetName().Name;

                Controllers =
                    Assembly.GetCallingAssembly().GetTypes()
                    .Concat(Assembly.GetExecutingAssembly().GetTypes())
                    .Where(x => Name.IsControllerName(x.Name))
                    .Select(Controller.Create)
                    .ToArray();
                EnsureControllersDoNotOverlap(Controllers);

                Command = Command.Parse(args);

                FindController(Command.Name)
                    .Invoke(Command);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private static void EnsureControllersDoNotOverlap(IEnumerable<Controller> controllers)
        {
            var set = new HashSet<string>();
            foreach (var item in controllers.SelectMany(x => x.NameAndAliases))
            {
                if (!set.Add(item))
                {
                    throw new InvalidOperationException($"The name or alias '{item}' is specified more than once.");
                }
            }
        }

        public static Controller FindController(string commandName)
        {
            var controller = Controllers
                .Where(x => x.NameAndAliases.Contains(commandName.Trim().ToLower()))
                .SingleOrDefault();
            EnsureController(controller);
            return controller;
        }

        private static void EnsureController(Controller controller)
        {
            if (controller == null)
            {
                throw new InvalidOperationException($"'{Command.Name}' is not a command. See '{ApplicationName} help'.");
            }
        }
    }
}
