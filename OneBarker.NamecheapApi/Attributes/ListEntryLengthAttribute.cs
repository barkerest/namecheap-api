using System.ComponentModel.DataAnnotations;

namespace OneBarker.NamecheapApi.Attributes;

public class ListEntryLengthAttribute : ValidationAttribute
{
    /// <summary>
    /// The maximum length for list entries.
    /// </summary>
    public int MaximumLength { get; }

    /// <summary>
    /// The minimum length for list entries.
    /// </summary>
    public int MinimumLength { get; } = 0;

    /// <summary>
    /// Set the maximum length for entries in this list.
    /// </summary>
    /// <param name="maximumLength"></param>
    /// <exception cref="ArgumentException"></exception>
    public ListEntryLengthAttribute(int maximumLength) 
        : base(maximumLength == 1 ? "The {0} list requires entries to be no more than 1 character in length." : "The {0} list requires entries to be no more than {1} characters in length.")
    {
        if (maximumLength < 1) throw new ArgumentException("Maximum length must be at least 1.");
        MaximumLength = maximumLength;
    }

    /// <summary>
    /// Set the minimum and maximum length for entries in this list.
    /// </summary>
    /// <param name="minimumLength"></param>
    /// <param name="maximumLength"></param>
    /// <exception cref="ArgumentException"></exception>
    public ListEntryLengthAttribute(int minimumLength, int maximumLength)
        : base(minimumLength == maximumLength
               ? maximumLength == 1 
                 ? "The {0} list requires entries to be 1 character in length." : "The {0} list requires entries to be {1} characters in length."
               : "The {0} list requires entries to be between {2} and {1} characters in length.")
    {
        if (minimumLength < 1) throw new ArgumentException("Minimum length must be at least 1 when specified.");
        if (maximumLength < minimumLength) throw new ArgumentException("Maximum length must be greater than or equal to minimum length.");
        MinimumLength = minimumLength;
        MaximumLength = maximumLength;
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is not IEnumerable<string?> list) return true; // validated by RequiredAttribute.
        
        foreach (var entry in list)
        {
            if (entry is null ||
                entry.Length < MinimumLength ||
                entry.Length > MaximumLength) return false;
            
            if (MinimumLength > 0 &&
                string.IsNullOrWhiteSpace(entry)) return false;
            
        }

        return true;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, MaximumLength, MinimumLength);
    }
}
