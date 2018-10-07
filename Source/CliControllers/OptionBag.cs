using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CliControllers
{
    public class OptionBag : IEnumerable<Option>
    {
        private Dictionary<string, string> options;

        public int Count
        {
            get { return options.Count; }
        }

        private  OptionBag()
        {
            options = new Dictionary<string, string>();
        }

        public static OptionBag Fill(IEnumerable<Option> options)
        {
            var optionBag = new OptionBag();

            foreach(var option in options)
            {
                if (optionBag.options.ContainsKey(option.name))
                {
                    throw new InvalidOperationException($"Option '{option.name}' has been specified more than once.");
                }

                optionBag.options.Add(option.name, option.value);
            }

            return optionBag;
        }

        public string GetAndRemove(IEnumerable<string> optionNameAndAliases)
        {
            foreach(var item in optionNameAndAliases)
            {
                if (options.TryGetValue(item, out string value))
                {
                    options.Remove(item);
                    return value;
                }
            }

            return null;
        }

        public IEnumerator<Option> GetEnumerator()
        {
            return options.Select(x => new Option(x.Key, x.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return options.Select(x => new Option(x.Key, x.Value)).GetEnumerator();
        }
    }
}
