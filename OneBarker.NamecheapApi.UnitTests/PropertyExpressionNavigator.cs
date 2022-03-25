using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OneBarker.NamecheapApi.UnitTests;

internal class PropertyExpressionNavigator<TParent, TValue>
{
    public Expression<Func<TParent, TValue>> Expression { get; }
    
    public  string       PropertyName        { get; }
    
    private PropertyInfo TopProperty { get; }
    
    private IReadOnlyList<PropertyInfo> Properties  { get; }

    public bool CanWrite => TopProperty.CanWrite;

    public TValue? GetValue(TParent item)
    {
        object parent = item;
        foreach (var prop in Properties)
        {
            parent = prop.GetValue(parent) ?? throw new NullReferenceException($"Failed to get parent value for {PropertyName}.");
        }

        return (TValue?)TopProperty.GetValue(parent);
    }

    public void SetValue(TParent item, TValue? value)
    {
        if (!CanWrite) throw new InvalidOperationException($"Cannot write to {PropertyName}.");
        
        object parent = item;
        foreach (var prop in Properties)
        {
            parent = prop.GetValue(parent) ?? throw new NullReferenceException($"Failed to get parent value for {PropertyName}.");
        }

        TopProperty.SetValue(parent, value);
    }
    
    public PropertyExpressionNavigator(Expression<Func<TParent, TValue>> property)
    {
        Expression = property;
        
        var propEx = property.Body as MemberExpression ?? throw new ArgumentException("Not a member expression.", nameof(property));
        var prop   = propEx.Member as PropertyInfo ?? throw new ArgumentException("Not a property expression.", nameof(property));
        var props  = new List<PropertyInfo>();

        if (!prop.CanRead) throw new ArgumentException("Property is not readable.", nameof(property));
        TopProperty = prop;
        
        var nameBuilder = new StringBuilder(prop.Name);
        while (propEx.Expression is not ParameterExpression)
        {
            propEx = propEx.Expression as MemberExpression ?? throw new ArgumentException("Not a member expression.", nameof(property));
            prop   = propEx.Member as PropertyInfo ?? throw new ArgumentException("Not a property expression.", nameof(property));
            nameBuilder.Insert(0, '.');
            nameBuilder.Insert(0, prop.Name);
            props.Add(prop);
        }

        Properties   = ((IEnumerable<PropertyInfo>)props).Reverse().ToArray();
        PropertyName = nameBuilder.ToString();
    }
    
}
