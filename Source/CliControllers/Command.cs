using System;
using System.Collections.Generic;
using System.Linq;

namespace CliControllers
{
    public class Command
    {
        private Dictionary<string, string> options;

        public string Name { get; private set; }

        public IEnumerable<string> Arguments { get; private set; }

        public IEnumerable<Option> Options
        {
            get { return options.Select(x => new Option(x.Key, x.Value)); }
        }

        public static Command Parse(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            if (args.Length == 0) return new Command("help", Enumerable.Empty<string>(), new Dictionary<string, string>());

            var queue = new Queue<string>(args);
            var name = ParseName(queue);
            var arguments = ParseArguments(queue);
            var options = ParseOptions(queue);
            return new Command(name, arguments, options);
        }

        private static string ParseName(Queue<string> queue)
        {
            var rawName = queue.Dequeue();
            var name = rawName.Trim().ToLower();

            if (IsLiteral(name))
            {
                return name;
            }
            else
            {
                throw new InvalidOperationException($"'{rawName}' is not a valid command name.");
            }
        }

        private static IEnumerable<string> ParseArguments(Queue<string> queue)
        {
            var arguments = new List<string>();

            while (queue.Count > 0 && IsLiteral(queue.Peek()))
            {
                arguments.Add(queue.Dequeue());
            }

            return arguments;
        }

        private static Dictionary<string, string> ParseOptions(Queue<string> queue)
        {
            var result = new Dictionary<string, string>();

            while (queue.Count > 0)
            {
                var key = queue.Dequeue().ToLower();

                if (IsLiteral(key))
                {
                    throw new InvalidOperationException($"Found an argument '{key}' where option was expected.");
                }
                else if (result.ContainsKey(key))
                {
                    throw new InvalidOperationException($"The option '{key}' has already been specified.");
                }
                else if (queue.Count > 0 && IsLiteral(queue.Peek()))
                {
                    result.Add(key, queue.Dequeue());
                }
                else
                {
                    result.Add(key, "true");
                }
            }

            return result;
        }

        private static bool IsLiteral(string value)
        {
            // Tokens are either literals or they are CommandOption keys, 
            // which start with a dash.
            return !value.StartsWith("-");
        }

        private Command(string name, IEnumerable<string> arguments, Dictionary<string, string> options)
        {
            Name = name;
            Arguments = arguments;
            this.options = options;
        }
    }
}
