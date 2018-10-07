using System;
using System.Collections.Generic;

namespace CliControllers
{
    public static class ParameterValueParser
    {
        private delegate bool TryParseDelegate(string value, out object parsedValue);

        private static readonly Dictionary<Type, TryParseDelegate> ParameterTypes = new Dictionary<Type, TryParseDelegate>()
        {
            { typeof(string), TryParseString },
            { typeof(short), TryParseShort },
            { typeof(int), TryParseInt },
            { typeof(long), TryParseLong },
            { typeof(float), TryParseFloat },
            { typeof(decimal), TryParseDecimal },
            { typeof(double), TryParseDouble },
            { typeof(bool), TryParseBool },
            { typeof(DateTime), TryParseDateTime }
        };

        public static bool IsAllowedType(Type type)
        {
            return ParameterTypes.ContainsKey(type);
        }

        public static bool CanParse(string value, Type parseToType)
        {
            if (parseToType == null) throw new ArgumentNullException(nameof(parseToType));
            if (!IsAllowedType(parseToType)) throw new ArgumentOutOfRangeException($"'{parseToType.Name}' is not an allowed type of invoke method parameter.");

            return ParameterTypes[parseToType](value, out object parsedValue);
        }

        public static object Parse(string value, Type parseToType)
        {
            if (parseToType == null) throw new ArgumentNullException(nameof(parseToType));
            if (!IsAllowedType(parseToType)) throw new ArgumentOutOfRangeException($"'{parseToType.Name}' is not an allowed type of invoke method parameter.");

            if (ParameterTypes[parseToType](value, out object parsedValue))
            {
                return parsedValue;
            }
            else
            {
                throw new InvalidOperationException($"Unable to parse value '{value}' to type {parseToType.Name}");
            }
        }

        private static bool TryParseString(string value, out object parsedValue)
        {
            parsedValue = value;
            return true;
        }

        private static bool TryParseShort(string value, out object parsedValue)
        {
            var result = short.TryParse(value, out short tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseInt(string value, out object parsedValue)
        {
            var result = int.TryParse(value, out int tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseLong(string value, out object parsedValue)
        {
            var result = long.TryParse(value, out long tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseFloat(string value, out object parsedValue)
        {
            var result = float.TryParse(value, out float tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseDecimal(string value, out object parsedValue)
        {
            var result = decimal.TryParse(value, out decimal tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseDouble(string value, out object parsedValue)
        {
            var result = double.TryParse(value, out double tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseBool(string value, out object parsedValue)
        {
            var result = bool.TryParse(value, out bool tempValue);
            parsedValue = tempValue;
            return result;
        }

        private static bool TryParseDateTime(string value, out object parsedValue)
        {
            if (value.Equals("now", StringComparison.InvariantCultureIgnoreCase))
            {
                parsedValue = DateTime.Now;
                return true;
            }
            else if (value.Equals("today", StringComparison.InvariantCultureIgnoreCase))
            {
                parsedValue = DateTime.Today;
                return true;
            }
            else
            {
                var result = DateTime.TryParse(value, out DateTime tempValue);
                parsedValue = tempValue;
                return result;
            }
        }
    }
}
