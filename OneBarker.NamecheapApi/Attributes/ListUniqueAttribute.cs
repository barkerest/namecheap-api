using System.ComponentModel.DataAnnotations;

namespace OneBarker.NamecheapApi.Attributes;

public class ListUniqueAttribute : ValidationAttribute
{
    /// <summary>
    /// Indicates whether the list entries should be compared in a case-insensitive manner.
    /// </summary>
    public bool CaseInsensitive { get; set; }

    public ListUniqueAttribute()
        : base("The {0} list requires the entries to be unique.")
    {
        
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is not IEnumerable<string> list) return true; // validated by RequireAttribute

        if (CaseInsensitive) list = list.Select(x => x.ToLower());

        var arr = list.OrderBy(x => x).ToArray();
        
        for (var i = 1; i < arr.Length; i++)
        {
            if (arr[i] == arr[i - 1]) return false;
        }

        return true;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name) => string.Format(ErrorMessageString, name);
}
