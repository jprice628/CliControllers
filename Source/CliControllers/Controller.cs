using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CliControllers
{
    public class Controller
    {
        public Type ControllerType { get; private set; }

        public string ControllerName
        {
            get { return Name.ToControllerName(ControllerType.Name); }
        }

        public IEnumerable<string> Aliases
        {
            get
            {
                return AliasAttribute
                    .GetAliases(ControllerType)
                    .Select(Name.ToControllerAlias)
                    .ToArray();
            }
        }

        public IEnumerable<string> NameAndAliases
        {
            get
            {
                var list = new List<string> { ControllerName };
                list.AddRange(Aliases);
                return list;
            }
        }

        public string Description
        {
            get { return DescriptionAttribute.GetDescription(ControllerType); }
        }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return ControllerType.GetMethod("Invoke").GetParameters(); }
        }

        //---------------------------------------------------------------------
        // Creating a Controller
        //---------------------------------------------------------------------

        public static Controller Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!Name.IsControllerName(type.Name)) throw new ArgumentException($"'{type.Name}' is not a controller name.");
            if (type.GetConstructor(Array.Empty<Type>()) == null) throw new ArgumentException($"'{type.Name}' does not have a default constructor.");

            ValidateInvokeMethod(type);

            return new Controller(type);
        }

        private static void ValidateInvokeMethod(Type controllerType)
        {
            var invokeMethods = controllerType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.Name.Equals("Invoke"))
                .ToArray();

            if (invokeMethods == null || invokeMethods.Length == 0)
            {
                throw new ArgumentException($"'{controllerType.Name}' does not have an invoke method.");
            }
            else if (invokeMethods.Length > 1)
            {
                throw new ArgumentException($"'{controllerType.Name}' has multiple invoke methods.");
            }
            else if (!invokeMethods[0].ReturnType.Equals(typeof(void)))
            {
                throw new ArgumentException($"The invoke method of '{controllerType.Name}' does not return void.");
            }

            ValidateInvokeMethodParameters(controllerType, invokeMethods[0].GetParameters());
        }

        private static void ValidateInvokeMethodParameters(Type controllerType, ParameterInfo[] parameters)
        {
            var queue = new Queue<ParameterInfo>(parameters);

            while (queue.Count > 0 && queue.Peek().IsArgument())
            {
                var argument = queue.Dequeue();
                ValidateParameterAttribute(argument, controllerType);
                ValidateParameterType(argument, controllerType);
                ValidateParameterDefaultValue(argument, controllerType);
            }

            var validateOptionNameAndAliases = ValidateOptionNameAndAliases();
            while (queue.Count > 0)
            {
                var option = queue.Dequeue();
                ValidateParameterAttribute(option, controllerType);
                EnsureOptionalParameter(option, controllerType);
                validateOptionNameAndAliases(option, controllerType);
                ValidateParameterType(option, controllerType);
                ValidateParameterDefaultValue(option, controllerType);
            }
        }

        private static void ValidateParameterAttribute(ParameterInfo parameter, Type controllerType)
        {
            if (parameter.GetCustomAttribute<ArgumentAttribute>() != null &&
                parameter.GetCustomAttribute<OptionAttribute>() != null)
            {
                throw new ArgumentException($"The parameter '{parameter.Name}' on the invoke method for '{controllerType.Name}' is marked as both an argument and an optional parameter.");
            }
        }

        private static void ValidateParameterType(ParameterInfo parameter, Type controllerType)
        {
            if (!ParameterValueParser.IsAllowedType(parameter.ParameterType))
            {
                throw new ArgumentException($"The parameter '{parameter.Name}' on the invoke method for '{controllerType.Name}' is not an accepted type.");
            }
        }

        private static void ValidateParameterDefaultValue(ParameterInfo parameter, Type controllerType)
        {
            if (parameter.IsArgument()) ValidateArgumentDefaultValue(parameter, controllerType);
            else ValidateOptionDefaultValue(parameter, controllerType);
        }

        private static void ValidateArgumentDefaultValue(ParameterInfo parameter, Type controllerType)
        {
            if (
                ArgumentAttribute.TryGetDefaultValue(parameter, out string defaultValue) &&
                !ParameterValueParser.CanParse(defaultValue, parameter.ParameterType)
                )
            {
                throw new ArgumentException($"The default value for '{parameter.Name}' on the invoke method for '{controllerType.Name}' cannot be parsed to the parameter's type.");
            }
        }

        private static void ValidateOptionDefaultValue(ParameterInfo parameter, Type controllerType)
        {
            var defaultValue = OptionAttribute.GetDefaultValue(parameter);
            if (!ParameterValueParser.CanParse(defaultValue, parameter.ParameterType))
            {
                throw new ArgumentException($"The default value for '{parameter.Name}' on the invoke method for '{controllerType.Name}' cannot be parsed to the parameter's type.");
            }
        }

        private static void EnsureOptionalParameter(ParameterInfo parameter, Type controllerType)
        {
            if (parameter.IsArgument())
            {
                throw new ArgumentException($"The invoke method for '{controllerType.Name}' has arguments following its optional parameters.");
            }
        }

        private static Action<ParameterInfo, Type> ValidateOptionNameAndAliases()
        {
            var set = new HashSet<string>();
            Action<ParameterInfo, Type> action = (ParameterInfo parameter, Type controllerType) =>
            {
                foreach (var item in GetOptionNameAndAliases(parameter))
                {
                    if (!set.Add(item))
                    {
                        throw new ArgumentException($"The parameter name or alias '{item}' is specified more than once.");
                    }
                }
            };
            return action;
        }

        private Controller(Type type)
        {
            ControllerType = type;
        }

        //---------------------------------------------------------------------
        // Invoking a Controller
        //---------------------------------------------------------------------

        public void Invoke(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var parms = MatchArgumentsToParameters(command)
                .Concat(MatchOptionsToParameters(command))
                .ToArray();
            var controller = Activator.CreateInstance(ControllerType);
            ControllerType
                .GetMethod("Invoke")
                .Invoke(controller, parms.ToArray());
            if (controller is IDisposable)
            {
                ((IDisposable)controller).Dispose();
            }
        }

        private IList<object> MatchArgumentsToParameters(Command command)
        {
            var invokeParameters = new Queue<ParameterInfo>(Parameters);
            var commandArguments = new Queue<string>(command.Arguments);
            var result = new List<object>();
            while (invokeParameters.Count > 0 && invokeParameters.Peek().IsArgument())
            {
                var invokeParameter = invokeParameters.Dequeue();
                if (commandArguments.Count > 0)
                {
                    result.Add(ParameterValueParser.Parse(commandArguments.Dequeue(), invokeParameter.ParameterType));
                }
                else if (ArgumentAttribute.TryGetDefaultValue(invokeParameter, out string defaultValue))
                {
                    result.Add(ParameterValueParser.Parse(defaultValue, invokeParameter.ParameterType));
                }
                else
                {
                    throw new InvalidOperationException($"A value must be provided for argument '{Name.ToArgumentName(invokeParameter.Name)}'");
                }
            }

            if (commandArguments.Count > 0)
            {
                throw new InvalidOperationException("Too many arguments.");
            }

            return result;
        }

        private IList<object> MatchOptionsToParameters(Command command)
        {
            var result = new List<object>();
            var invokeParameters = GetOptionalParameterQueue();
            var commandOptions = OptionBag.Fill(command.Options);

            while (invokeParameters.Count > 0 && invokeParameters.Peek().IsOption())
            {
                var invokeParameter = invokeParameters.Dequeue();
                var commandOptionValue = commandOptions.GetAndRemove(GetOptionNameAndAliases(invokeParameter));
                if (commandOptionValue != null)
                {
                    result.Add(ParameterValueParser.Parse(commandOptionValue, invokeParameter.ParameterType));
                }
                else
                {
                    result.Add(ParameterValueParser.Parse(
                        invokeParameter.GetCustomAttribute<OptionAttribute>().DefaultValue,
                        invokeParameter.ParameterType
                        ));
                }
            }

            if (commandOptions.Count > 0)
            {
                throw new InvalidOperationException("Too many options.");
            }

            return result;
        }

        private Queue<ParameterInfo> GetOptionalParameterQueue()
        {
            var parmQueue = new Queue<ParameterInfo>(Parameters);
            while (parmQueue.Count > 0 && parmQueue.Peek().IsArgument())
            {
                parmQueue.Dequeue();
            }
            return parmQueue;
        }

        private static IEnumerable<string> GetOptionNameAndAliases(ParameterInfo parm)
        {
            var list = new List<string>
            {
                Name.ToOptionName(parm.Name)
            };

            var aliasAttribute = parm.GetCustomAttribute<AliasAttribute>();
            if (aliasAttribute != null)
            {
                list.AddRange(aliasAttribute.Aliases.Select(Name.ToOptionName));
            }

            return list;
        }
    }
}
