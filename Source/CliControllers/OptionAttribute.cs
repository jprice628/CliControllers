using System;
using System.Reflection;

namespace CliControllers
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class OptionAttribute : Attribute
    {
        public string DefaultValue { get; private set; }

        public OptionAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public static string GetDefaultValue(ParameterInfo parm)
        {
            if (parm == null) throw new ArgumentNullException(nameof(parm));

            var optionAttribute = parm.GetCustomAttribute<OptionAttribute>();

            if (optionAttribute == null)
            {
                throw new InvalidOperationException($"'{parm.Name}' is not an optional parameter.");
            }
            else
            {
                return optionAttribute.DefaultValue;
            }
        }
    }
}
