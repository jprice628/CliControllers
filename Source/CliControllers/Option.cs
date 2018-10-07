using System;

namespace CliControllers
{
    // Used when parsing the args array to represent a named, optional parameter.
    public struct Option
    {
        public string name;
        public string value;

        public Option(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            this.name = name;
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(typeof(Option))) return false;

            var other = (Option)obj;
            return other.name.Equals(name) &&
                other.value.Equals(value);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() ^
                value.GetHashCode();
        }

        public override string ToString()
        {
            return $"({name}, {value})";
        }
    }
}
