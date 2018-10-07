using System;
using System.Reflection;

namespace CliControllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public DescriptionAttribute(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));

            Description = description;
        }

        public static string GetDescription(Type type)
        {
            return GetDescription(type.GetCustomAttribute<DescriptionAttribute>());
        }

        public static string GetDescription(ParameterInfo parm)
        {
            return GetDescription(parm.GetCustomAttribute<DescriptionAttribute>());            
        }

        private static string GetDescription(DescriptionAttribute descriptionAttribute)
        {
            if (descriptionAttribute == null)
            {
                return "No description available.";
            }
            else
            {
                return descriptionAttribute.Description;
            }
        }
    }
}
