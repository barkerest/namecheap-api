using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OneBarker.NamecheapApi.Commands.Params;

namespace OneBarker.NamecheapApi.Utility;

internal class CommandValidator
{
    // list of property names defined in IApiConfig and IApiCommand.
    private static readonly string[] ExplicitConfigPropertyNames =
        typeof(IApiConfig)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => x.Name)
            .Concat(
                typeof(IApiCommand)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(x => x.Name)
            )
            .ToArray();

    private static readonly Type CommandParamType = typeof(ICommandParam);
    
    /// <summary>
    /// The command type this validator is for.
    /// </summary>
    public Type CommandType { get; }

    private readonly (PropertyInfo Property, ValidationAttribute[] Validations)[] _propertiesToCheck;
    
    private CommandValidator(Type commandType)
    {
        CommandType = commandType;
        
        _propertiesToCheck = CommandType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(x => x.CanRead && !ExplicitConfigPropertyNames.Contains(x.Name))
                                    .Select(x => (Property: x, Validations: x.GetCustomAttributes<ValidationAttribute>().ToArray()))
                                    .Where(x => x.Validations.Any() || CommandParamType.IsAssignableFrom(x.Property.PropertyType))
                                    .ToArray();
    }

    /// <summary>
    /// Validate the command using this validator.
    /// </summary>
    /// <param name="command">The command being validated.</param>
    /// <param name="errors">The error list to add validation errors to.</param>
    /// <remarks>
    /// The command must be of the same type this validator was created for.
    /// </remarks>
    public void Validate(IApiCommand command, List<string> errors)
        => ValidateObject(command, errors, "");

    private void ValidateObject(object data, ICollection<string> errors, string prefix)
    {
        if (data.GetType() != CommandType)
            throw new ArgumentException("Command being checked does not match type of validator.");

        foreach (var prop in _propertiesToCheck)
        {
            var value = prop.Property.GetValue(data);
            var name  = string.IsNullOrWhiteSpace(prefix) ? prop.Property.Name : $"{prefix}.{prop.Property.Name}";
            
            foreach (var attrib in prop.Validations)
            {
                if (!attrib.IsValid(value))
                {
                    errors.Add(attrib.FormatErrorMessage(name));
                }
            }

            if (value is ICommandParam commandParam)
            {
                FindOrCreate(commandParam).ValidateObject(commandParam, errors, name);
            }
        }
    }
    
    private static readonly Dictionary<Type, CommandValidator> Validators = new();

    /// <summary>
    /// Find or create the validator for the command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static CommandValidator FindOrCreate(IApiCommand command)
    {
        var t = command.GetType();
        if (Validators.ContainsKey(t)) return Validators[t];
        return Validators[t] = new CommandValidator(t);
    }
    
    private static CommandValidator FindOrCreate(ICommandParam commandParam)
    {
        var t = commandParam.GetType();
        if (Validators.ContainsKey(t)) return Validators[t];
        return Validators[t] = new CommandValidator(t);
    }
}
