using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliControllers
{
    [Alias("/? ?")]
    public class HelpController
    {
        public void Invoke([Argument(null)]string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName)) PrintGenericHelp();
            else PrintCommandHelp(commandName);
        }

        private void PrintGenericHelp()
        {
            Console.WriteLine();
            Console.WriteLine($"usage: {Application.ApplicationName} <command> [<arguments>] [<optional parameters>]");
            Console.WriteLine();
            Console.WriteLine("The following commands are supported by this application:");
            Console.WriteLine();
            foreach (var controller in Application.Controllers)
            {
                Console.WriteLine("{0,-20}{1}", controller.ControllerName, controller.Description);
            }
            Console.WriteLine();
            Console.WriteLine($"See '{Application.ApplicationName} help <command>' for details on a specific command.");
        }

        private void PrintCommandHelp(string commandName)
        {
            var controller = Application.FindController(commandName);
            Console.WriteLine();
            Console.WriteLine(string.Join(", ", controller.NameAndAliases));
            Console.WriteLine();
            Console.WriteLine("Description: " + controller.Description);
            Console.WriteLine();

            var usage = new StringBuilder($"usage: {Application.ApplicationName} {controller.ControllerName}");
            foreach(var parameter in controller.Parameters)
            {
                usage.Append(' ').Append(ToParameterUsageString(parameter));
            }
            Console.WriteLine(usage.ToString());
            Console.WriteLine();

            Console.WriteLine("Parameters:");
            Console.WriteLine();
            foreach (var parameter in controller.Parameters)
            {
                Console.WriteLine(string.Join(", ", parameter.GetNameAndAliases()));
                Console.WriteLine("    " + DescriptionAttribute.GetDescription(parameter));
                Console.WriteLine("    Type: " + parameter.ParameterType);
                PrintParamterDefaultValue(parameter);
                Console.WriteLine();
            }
        }

        private string ToParameterUsageString(ParameterInfo parameter)
        {
            if (parameter.IsArgument()) return ToArgumentUsageString(parameter);
            else return ToOptionUsageString(parameter);
        }

        private string ToArgumentUsageString(ParameterInfo argument)
        {
            if (ArgumentAttribute.TryGetDefaultValue(argument, out string defaultValue))
            {
                return $"[<{Name.ToArgumentName(argument.Name)}>]";
            }
            else
            {
                return $"<{Name.ToArgumentName(argument.Name)}>";
            }
        }

        private string ToOptionUsageString(ParameterInfo option)
        {
            return $"[{Name.ToOptionName(option.Name)} <value>]";
        }

        private void PrintParamterDefaultValue(ParameterInfo parameter)
        {
            if (parameter.IsArgument()) PrintArgumentDefaultValue(parameter);
            else PrintOptionDefaultValue(parameter);
        }

        private void PrintArgumentDefaultValue(ParameterInfo argument)
        {
            if (ArgumentAttribute.TryGetDefaultValue(argument, out string defaultValue))
            {
                Console.WriteLine($"    Defaults to '{defaultValue}'");
            }
        }

        private void PrintOptionDefaultValue(ParameterInfo option)
        {
            Console.WriteLine($"    Defaults to '{OptionAttribute.GetDefaultValue(option)}'");
        }
    }
}
