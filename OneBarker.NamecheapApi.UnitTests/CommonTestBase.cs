using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Utility;
using Xunit.Abstractions;
using Xunit.Extensions.Logging;
using Xunit.Sdk;

namespace OneBarker.NamecheapApi.UnitTests;

public abstract class CommonTestBase
{
    protected const string Invalid21CharString  = "A-21-char-string-zz2z";
    protected const string Invalid31CharString  = "A-31-char-string-zz2zzzzzzzzz3z";
    protected const string Invalid33CharString  = "A-33-char-string-zz2zzzzzzzzz3zzz";
    protected const string Invalid41CharString  = "A-41-char-string-zz2zzzzzzzzz3zzzzzzzzz4z";
    protected const string Invalid51CharString  = "A-51-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5z";
    protected const string Invalid61CharString  = "A-61-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6z";
    protected const string Invalid65CharString  = "A-65-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzz";
    protected const string Invalid71CharString  = "A-71-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7z";
    protected const string Invalid81CharString  = "A-81-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8z";
    protected const string Invalid91CharString  = "A-91-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9z";
    protected const string Invalid101CharString = "A-101-char-string-z2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0z";
    protected const string Invalid129CharString = "A-129-char-string-z2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0zzzzzzzzz1zzzzzzzzz2zzzzzzzzz";
    protected const string Invalid256CharString = "A-256-char-string-z2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0zzzzzzzzz1zzzzzzzzz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0zzzzzzzzz1zzzzzzzzz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzz";
    protected const string Valid20CharString    = "A-20-char-string-zz2";
    protected const string Valid30CharString    = "A-30-char-string-zz2zzzzzzzzz3";
    protected const string Valid32CharString    = "A-32-char-string-zz2zzzzzzzzz3zz";
    protected const string Valid40CharString    = "A-40-char-string-zz2zzzzzzzzz3zzzzzzzzz4";
    protected const string Valid50CharString    = "A-50-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5";
    protected const string Valid60CharString    = "A-60-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz60";
    protected const string Valid64CharString    = "A-64-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzz";
    protected const string Valid70CharString    = "A-70-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7";
    protected const string Valid80CharString    = "A-80-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8";
    protected const string Valid90CharString    = "A-90-char-string-zz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9";
    protected const string Valid100CharString   = "A-100-char-string-z2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0";
    protected const string Valid128CharString   = "A-128-char-string-z2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0zzzzzzzzz1zzzzzzzzz2zzzzzzzz";
    protected const string Valid255CharString   = "A-255-char-string-z2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0zzzzzzzzz1zzzzzzzzz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzzzzzz6zzzzzzzzz7zzzzzzzzz8zzzzzzzzz9zzzzzzzzz0zzzzzzzzz1zzzzzzzzz2zzzzzzzzz3zzzzzzzzz4zzzzzzzzz5zzzzz";

    protected class CustomAssertException : XunitException
    {
        public CustomAssertException(string msg)
            : base(msg)
        {
        }
    }

    protected readonly ITestOutputHelper OutputHelper;
    protected readonly IApiConfig        GoodConfig;

    protected CommonTestBase(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        GoodConfig = TestConfig.Config.ApiConfigWithLogging(
            builder => builder.AddProvider(new XunitLoggerProvider(OutputHelper, TestConfig.Config.BasicLoggingFilter))
        );
    }

    protected void TestValidConfig(IApiConfig config)
    {
        var valid = config.IsValid(out var errors);

        if (!valid)
        {
            OutputHelper.WriteLine("Errors found:");
            foreach (var error in errors)
            {
                OutputHelper.WriteLine("  " + error);
            }

            throw new CustomAssertException("Configuration is not valid for testing.");
        }

        OutputHelper.WriteLine("The configuration is valid.");
    }

    protected void TestValidOption<TConfig, TValue>(TConfig testConfig, TValue testValue, string propertyName) where TConfig : IApiConfig
        => TestOption(true, testConfig, testValue, new[] { propertyName }, null, false);

    protected void TestValidOption<TConfig, TValue>(TValue testValue, Func<IApiConfig, TValue, TConfig> setValue, string propertyName) where TConfig : IApiConfig
        => TestOption(true, GoodConfig, testValue, setValue, new[] { propertyName }, null, false);

    protected void TestValidOption<TConfig, TValue>(IApiConfig sourceConfig, TValue testValue, Func<IApiConfig, TValue, TConfig> setValue, string propertyName)
        where TConfig : IApiConfig
        => TestOption(true, sourceConfig, testValue, setValue, new[] { propertyName }, null, false);

    protected void TestInvalidOption<TConfig, TValue>(TConfig testConfig, TValue testValue, string propertyName, string? expectedPartialError = null, bool disallowAdditionalErrors = true) where TConfig : IApiConfig
        => TestOption(false, testConfig, testValue, new[] { propertyName }, expectedPartialError, disallowAdditionalErrors);

    protected void TestInvalidOption<TConfig, TValue>(TConfig testConfig, TValue testValue, string[] propertyNames, string? expectedPartialError = null, bool disallowAdditionalErrors = true) where TConfig : IApiConfig
        => TestOption(false, testConfig, testValue, propertyNames, expectedPartialError, disallowAdditionalErrors);

    protected void TestInvalidOption<TConfig, TValue>(TValue testValue, Func<IApiConfig, TValue, TConfig> setValue, string? expectedPartialError = null, bool disallowAdditionalErrors = true, params string[] affectedProperties)
        where TConfig : IApiConfig
        => TestOption(false, GoodConfig, testValue, setValue, affectedProperties, expectedPartialError, disallowAdditionalErrors);

    protected void TestInvalidOption<TConfig, TValue>(IApiConfig sourceConfig, TValue testValue, Func<IApiConfig, TValue, TConfig> setValue, string? expectedPartialError = null, bool disallowAdditionalErrors = true, params string[] affectedProperties)
        where TConfig : IApiConfig
        => TestOption(false, sourceConfig, testValue, setValue, affectedProperties, expectedPartialError, disallowAdditionalErrors);

    private static readonly Dictionary<Type, Func<IApiConfig, IApiConfig>> Constructors = new();

    protected void TestOption<TConfig, TValue>(
        bool                              expectToBeValid,
        IApiConfig                        sourceConfig,
        TValue                            testValue,
        Func<IApiConfig, TValue, TConfig> setValue,
        string[]                          affectedProperties,
        string?                           expectedPartialError,
        bool                              disallowAdditionalErrors
    )
        where TConfig : IApiConfig
        => TestOption(expectToBeValid, setValue(sourceConfig, testValue), testValue, affectedProperties, expectedPartialError, disallowAdditionalErrors);

    protected void TestOption<TConfig, TValue>(
        bool     expectToBeValid,
        TConfig  testConfig,
        TValue   testValue,
        string[] affectedProperties,
        string?  expectedPartialError,
        bool     disallowAdditionalErrors
    )
        where TConfig : IApiConfig
    {
        var testValueString = testValue switch
        {
            null     => "NULL",
            string s => '"' + (s.Length < 25 ? s : (s[..22] + "...")).Replace("\"", "\"\"") + '"',
            _        => testValue.ToString() ?? ""
        };

        if (affectedProperties.Length < 1) throw new CustomAssertException("At least one affected property is required.");

        var tConfig = typeof(TConfig);
        var valid   = testConfig.IsValid(out var errors);

        if (expectToBeValid)
        {
            if (!valid)
            {
                OutputHelper.WriteLine("Errors found:");
                foreach (var error in errors)
                {
                    OutputHelper.WriteLine("  " + error);
                }

                throw new CustomAssertException($"{tConfig} is not valid after changing {affectedProperties[0]} to {testValueString}.");
            }

            OutputHelper.WriteLine($"{tConfig} is valid after changing {affectedProperties[0]} to {testValueString}.");
        }
        else
        {
            if (valid)
            {
                throw new CustomAssertException($"{tConfig} is valid after changing {affectedProperties[0]} to {testValueString}.");
            }

            OutputHelper.WriteLine(string.Join("\n", errors));

            var expected = affectedProperties.Length == 1 ? "1 error" : $"{affectedProperties.Length} errors";

            if (disallowAdditionalErrors)
            {
                if (errors.Length != affectedProperties.Length)
                {
                    throw new CustomAssertException($"Setting {affectedProperties[0]} to {testValueString} in {tConfig} should have raised {expected}.");
                }
            }
            else
            {
                if (errors.Length < affectedProperties.Length)
                {
                    throw new CustomAssertException($"Setting {affectedProperties[0]} to {testValueString} in {tConfig} should have raised at least {expected}.");
                }
            }

            for (var i = 0; i < affectedProperties.Length; i++)
            {
                var prop  = affectedProperties[i];
                var rex   = new Regex(@"(^|\s)" + Regex.Escape(prop) + @"(\s|$)");
                var error = errors.FirstOrDefault(x => rex.IsMatch(x));

                if (error is null)
                {
                    throw new CustomAssertException($"Setting {affectedProperties[0]} to {testValueString} in {tConfig} did not cause an error referencing the {prop} property.");
                }

                if (i == 0 &&
                    expectedPartialError is not null)
                {
                    if (!error.Contains(expectedPartialError, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new CustomAssertException($"Setting {affectedProperties[0]} to {testValueString} in {tConfig} did not cause an error containing \"{expectedPartialError}\".");
                    }
                }
            }
        }
    }
}

public abstract class CommonTestBase<TCommand> : CommonTestBase where TCommand : IApiConfig
{
    protected CommonTestBase(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected abstract TCommand CreateValidCommand();

    private TCommand SetValue<TValue>(
        Expression<Func<TCommand, TValue>> property,
        TValue                             value,
        out string                         name,
        Action<TCommand>?                  preConfig = null
    )
    {
        var nav = new PropertyExpressionNavigator<TCommand, TValue>(property);
        var cmd = CreateValidCommand();
        preConfig?.Invoke(cmd);
        nav.SetValue(cmd, value);
        name = nav.PropertyName;
        return cmd;
    }

    protected void TestValidOption<TValue>(
        Expression<Func<TCommand, TValue>> property,
        TValue                             value,
        Action<TCommand>?                  preConfig = null
    )
    {
        var cmd = SetValue(property, value, out var propName, preConfig);
        TestOption(true, cmd, value, new[] { propName }, null, false);
    }

    protected void TestInvalidOption<TValue>(
        Expression<Func<TCommand, TValue>> property,
        TValue                             value,
        string?                            expectedPartialError         = null,
        bool                               disallowAdditionalErrors     = true,
        string[]?                          additionalAffectedProperties = null,
        Action<TCommand>?                  preConfig                    = null
    )
    {
        var cmd = SetValue(property, value, out var propName, preConfig);

        if (additionalAffectedProperties is not null &&
            additionalAffectedProperties.Any())
        {
            var dot    = propName.LastIndexOf('.');
            var prefix = dot > 0 ? propName[..(dot + 1)] : "";
            var list   = new[] { propName }.Union(additionalAffectedProperties.Select(x => prefix + x)).ToArray();
            TestOption(false, cmd, value, list, expectedPartialError, disallowAdditionalErrors);
        }
        else
        {
            TestOption(false, cmd, value, new[] { propName }, expectedPartialError, disallowAdditionalErrors);
        }
    }

    public void TestValidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        TValue                                        value,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
        => TestValidOptionsFor(property, null, value, commonProperties);

    public void TestValidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        Action<TCommand>?                             preConfig,
        TValue                                        value,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
    {
        var propNav = new PropertyExpressionNavigator<TCommon, TValue>(property);
        if (!propNav.CanWrite) throw new ArgumentException("Property navigation must be writable.");
        foreach (var commonProp in commonProperties)
        {
            var commonNav = new PropertyExpressionNavigator<TCommand, TCommon?>(commonProp);
            var cmd       = CreateValidCommand();
            preConfig?.Invoke(cmd);
            var commonValue = commonNav.GetValue(cmd) ?? throw new InvalidOperationException($"The value of {commonNav.PropertyName} is null.");
            propNav.SetValue(commonValue, value);

            TestOption(true, cmd, value, new[] { commonNav.PropertyName + '.' + propNav.PropertyName }, null, false);
        }
    }

    public void TestInvalidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        TValue                                        value,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
        => TestInvalidOptionsFor(property, null, value, null, true, null, commonProperties);

    public void TestInvalidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        Action<TCommand>?                             preConfig,
        TValue                                        value,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
        => TestInvalidOptionsFor(property, preConfig, value, null, true, null, commonProperties);

    public void TestInvalidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        TValue                                        value,
        string?                                       expectedPartialError,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
        => TestInvalidOptionsFor(property, null, value, expectedPartialError, true, null, commonProperties);

    public void TestInvalidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        TValue                                        value,
        string[]                                      additionalAffectedProperties,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
        => TestInvalidOptionsFor(property, null, value, null, true, additionalAffectedProperties, commonProperties);

    public void TestInvalidOptionsFor<TCommon, TValue>(
        Expression<Func<TCommon, TValue>>             property,
        Action<TCommand>?                             preConfig,
        TValue                                        value,
        string?                                       expectedPartialError,
        bool                                          disallowAdditionalErrors,
        string[]?                                     additionalAffectedProperties,
        params Expression<Func<TCommand, TCommon?>>[] commonProperties
    )
    {
        var propNav = new PropertyExpressionNavigator<TCommon, TValue>(property);
        if (!propNav.CanWrite) throw new ArgumentException("Property navigation must be writable.");
        foreach (var commonProp in commonProperties)
        {
            var commonNav = new PropertyExpressionNavigator<TCommand, TCommon?>(commonProp);
            var cmd       = CreateValidCommand();
            preConfig?.Invoke(cmd);
            var commonValue = commonNav.GetValue(cmd) ?? throw new InvalidOperationException($"The value of {commonNav.PropertyName} is null.");
            propNav.SetValue(commonValue, value);

            IEnumerable<string> props                           = new[] { propNav.PropertyName };
            if (additionalAffectedProperties is not null) props = props.Union(additionalAffectedProperties);
            props = props.Select(x => commonNav.PropertyName + '.' + x);
            TestOption(false, cmd, value, props.ToArray(), expectedPartialError, disallowAdditionalErrors);
        }
    }
}
