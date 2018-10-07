using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace CliControllers
{
    public static class ParameterInfoExtensions
    {
        public static bool IsArgument(this ParameterInfo parm)
        {
            return parm.GetCustomAttribute<OptionAttribute>() == null;
        }

        public static bool IsOption(this ParameterInfo parm)
        {
            return parm.GetCustomAttribute<OptionAttribute>() != null;
        }

        public static IEnumerable<string> GetNameAndAliases(this ParameterInfo parm)
        {
            if (parm.IsArgument()) return GetArgumentNameAndAliases(parm);
            else return GetOptionNameAndAliases(parm);
        }

        private static IEnumerable<string> GetArgumentNameAndAliases(ParameterInfo argument)
        {
            return new[] { Name.ToArgumentName(argument.Name) };
        }

        private static IEnumerable<string> GetOptionNameAndAliases(ParameterInfo option)
        {
            var list = new List<string> { Name.ToOptionName(option.Name) };
            list.AddRange(AliasAttribute.GetAliases(option).Select(Name.ToOptionName));
            return list;
        }
    }
}
