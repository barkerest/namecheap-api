

using System.Xml;

namespace OneBarker.NamecheapApi.Utility;

/// <summary>
/// Extension methods for XmlElements.
/// </summary>
public static class XmlElementExtensions
{
    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetContent(this XmlElement element) => element.InnerText.Trim();

    /// <summary>
    /// Gets the content of the child element with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetChildContent(this XmlElement element, string name)
        => element.ChildNodes
                  .OfType<XmlElement>()
                  .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                  ?.InnerText ?? "";
    
    /// <summary>
    /// Gets the specified child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <typeparam name="TChild"></typeparam>
    /// <returns></returns>
    public static XmlElement? GetChild(this XmlElement element, string name)
         => element.ChildNodes
                   .OfType<XmlElement>()
                   .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    
    /// <summary>
    /// Gets the specified child element as the specified type.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <typeparam name="TChild"></typeparam>
    /// <returns></returns>
    public static TChild? GetChildAs<TChild>(this XmlElement element, string name) where TChild : class, IXmlParseable, new()
    {
        var child = element.GetChild(name);
        if (child is null) return null;
        var ret = new TChild();
        ret.LoadFromXmlElement(child);
        return ret;
    }

    /// <summary>
    /// Convert this element into the specified type.
    /// </summary>
    /// <param name="element"></param>
    /// <typeparam name="TDest"></typeparam>
    /// <returns></returns>
    public static TDest To<TDest>(this XmlElement element) where TDest : class, IXmlParseable, new()
    {
        var ret = new TDest();
        ret.LoadFromXmlElement(element);
        return ret;
    }
    
    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TEnum GetAttributeAsEnum<TEnum>(this XmlElement element, string name, TEnum defaultValue = default) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException("TEnum must be an enum type.");
        return Enum.TryParse<TEnum>(element.GetAttribute(name), true, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TEnum GetContentAsEnum<TEnum>(this XmlElement element, TEnum defaultValue = default) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException("TEnum must be an enum type.");
        return Enum.TryParse<TEnum>(element.GetContent(), true, out var result) ? result : defaultValue;
    }   
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TEnum GetChildContentAsEnum<TEnum>(this XmlElement element, string name, TEnum defaultValue = default) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException("TEnum must be an enum type.");
        return Enum.TryParse<TEnum>(element.GetChildContent(name), true, out var result) ? result : defaultValue;
    }   

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetAttributeAsBoolean(this XmlElement element, string name, bool defaultValue = default)
        => bool.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetContentAsBoolean(this XmlElement element, bool defaultValue = default)
        => bool.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetChildContentAsBoolean(this XmlElement element, string name, bool defaultValue = default)
        => bool.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static sbyte GetAttributeAsInt8(this XmlElement element, string name, sbyte defaultValue = default)
        => sbyte.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static sbyte GetContentAsInt8(this XmlElement element, sbyte defaultValue = default)
        => sbyte.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static sbyte GetChildContentAsInt8(this XmlElement element, string name, sbyte defaultValue = default)
        => sbyte.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte GetAttributeAsUInt8(this XmlElement element, string name, byte defaultValue = default)
        => byte.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte GetContentAsUInt8(this XmlElement element, byte defaultValue = default)
        => byte.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static byte GetChildContentAsUInt8(this XmlElement element, string name, byte defaultValue = default)
        => byte.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static short GetAttributeAsInt16(this XmlElement element, string name, short defaultValue = default)
        => short.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static short GetContentAsInt16(this XmlElement element, short defaultValue = default)
        => short.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static short GetChildContentAsInt16(this XmlElement element, string name, short defaultValue = default)
        => short.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ushort GetAttributeAsUInt16(this XmlElement element, string name, ushort defaultValue = default)
        => ushort.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ushort GetContentAsUInt16(this XmlElement element, ushort defaultValue = default)
        => ushort.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ushort GetChildContentAsUInt16(this XmlElement element, string name, ushort defaultValue = default)
        => ushort.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetAttributeAsInt32(this XmlElement element, string name, int defaultValue = default)
        => int.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetContentAsInt32(this XmlElement element, int defaultValue = default)
        => int.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetChildContentAsInt32(this XmlElement element, string name, int defaultValue = default)
        => int.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static uint GetAttributeAsUInt32(this XmlElement element, string name, uint defaultValue = default)
        => uint.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static uint GetContentAsUInt32(this XmlElement element, uint defaultValue = default)
        => uint.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static uint GetChildContentAsUInt32(this XmlElement element, string name, uint defaultValue = default)
        => uint.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long GetAttributeAsInt64(this XmlElement element, string name, long defaultValue = default)
        => long.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long GetContentAsInt64(this XmlElement element, long defaultValue = default)
        => long.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long GetChildContentAsInt64(this XmlElement element, string name, long defaultValue = default)
        => long.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ulong GetAttributeAsUInt64(this XmlElement element, string name, ulong defaultValue = default)
        => ulong.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ulong GetContentAsUInt64(this XmlElement element, ulong defaultValue = default)
        => ulong.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static ulong GetChildContentAsUInt64(this XmlElement element, string name, ulong defaultValue = default)
        => ulong.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetAttributeAsFloat(this XmlElement element, string name, float defaultValue = default)
        => float.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetContentAsFloat(this XmlElement element, float defaultValue = default)
        => float.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetChildContentAsFloat(this XmlElement element, string name, float defaultValue = default)
        => float.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double GetAttributeAsDouble(this XmlElement element, string name, double defaultValue = default)
        => double.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double GetContentAsDouble(this XmlElement element, double defaultValue = default)
        => double.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double GetChildContentAsDouble(this XmlElement element, string name, double defaultValue = default)
        => double.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal GetAttributeAsDecimal(this XmlElement element, string name, decimal defaultValue = default)
        => decimal.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal GetContentAsDecimal(this XmlElement element, decimal defaultValue = default)
        => decimal.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal GetChildContentAsDecimal(this XmlElement element, string name, decimal defaultValue = default)
        => decimal.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime GetAttributeAsDateTime(this XmlElement element, string name, DateTime defaultValue = default)
        => DateTime.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime GetContentAsDateTime(this XmlElement element, DateTime defaultValue = default)
        => DateTime.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime GetChildContentAsDateTime(this XmlElement element, string name, DateTime defaultValue = default)
        => DateTime.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Returns the value of the attribute with the specified name.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Guid GetAttributeAsGuid(this XmlElement element, string name, Guid defaultValue = default)
        => Guid.TryParse(element.GetAttribute(name), out var result) ? result : defaultValue;

    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Guid GetContentAsGuid(this XmlElement element, Guid defaultValue = default)
        => Guid.TryParse(element.GetContent(), out var result) ? result : defaultValue;
    
    /// <summary>
    /// Gets the content of the named child element.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Guid GetChildContentAsGuid(this XmlElement element, string name, Guid defaultValue = default)
        => Guid.TryParse(element.GetChildContent(name), out var result) ? result : defaultValue;

}

