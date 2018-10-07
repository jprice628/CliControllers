using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CliControllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class AliasAttribute : Attribute
    {
        public IEnumerable<string> Aliases { get; private set; }

        public AliasAttribute(string aliases)
        {
            if (string.IsNullOrWhiteSpace(aliases)) throw new ArgumentNullException(nameof(aliases));

            Aliases = aliases.Split(' ', '\t')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower().Trim())
                .ToArray();
        }

        public static IEnumerable<string> GetAliases(Type type)
        {
            return GetAliases(type.GetCustomAttribute<AliasAttribute>());
        }

        public static IEnumerable<string> GetAliases(ParameterInfo parm)
        {
            return GetAliases(parm.GetCustomAttribute<AliasAttribute>());
        }

        private static IEnumerable<string> GetAliases(AliasAttribute aliasAttribute)
        {
            if (aliasAttribute == null)
            {
                return Enumerable.Empty<string>();
            }
            else
            {
                return aliasAttribute.Aliases;
            }
        }
    }
}
