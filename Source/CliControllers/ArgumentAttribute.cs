using System;
using System.Reflection;

namespace CliControllers
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class ArgumentAttribute : Attribute
    {
        public string DefaultValue { get; private set; }

        public bool HasDefaultValue { get; private set; }

        public ArgumentAttribute()
        {
            HasDefaultValue = false;
        }

        public ArgumentAttribute(string defaultValue)
        {
            HasDefaultValue = true;
            DefaultValue = defaultValue;
        }

        public static bool TryGetDefaultValue(ParameterInfo parm, out string defaultValue)
        {
            if (parm == null) throw new ArgumentNullException(nameof(parm));

            var argumentAttribute = parm.GetCustomAttribute<ArgumentAttribute>();
            if (argumentAttribute != null && argumentAttribute.HasDefaultValue)
            {
                defaultValue = argumentAttribute.DefaultValue;
                return true;
            }
            else
            {
                defaultValue = null;
                return false;
            }
        }
    }
}
