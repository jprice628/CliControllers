using System;
using System.Linq;

namespace CliControllers
{
    public static class Name
    {
        private const string Controller = "Controller";

        public static bool IsControllerName(string value)
        {
            return value.Length > Controller.Length &&
                value.EndsWith(Controller);
        }

        public static string ToControllerName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (!IsControllerName(value)) throw new ArgumentException("Value is not a controller name.");

            return value
                .Substring(0, value.Length - Controller.Length)
                .ToCliString()
                .TrimLeadingDashes();
        }

        public static string ToControllerAlias(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            return value
                .ToCliString()
                .TrimLeadingDashes();
        }

        public static string ToOptionName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            return value
                .ToCliString()
                .AddLeadingDash();
        }

        private static string AddLeadingDash(this string value)
        {
            if (value.StartsWith("-"))
            {
                return value;
            }
            else
            {
                return "-" + value;
            }
        }

        public static string ToArgumentName(string value)
        {
            return value
                .ToCliString()
                .TrimLeadingDashes();
        }

        private static string ToCliString(this string value)
        {
            return string.Concat(
                value
                .Replace('_', '-')
                .Select(ToLower)
                );
        }

        private static string ToLower(char c)
        {
            if (c >= 65 && c <= 90)
            {
                return "-" + (char)(c + 32);
            }
            else
            {
                return c.ToString();
            }
        }

        private static string TrimLeadingDashes(this string value)
        {
            return value.Trim('-');
        }
    }
}
