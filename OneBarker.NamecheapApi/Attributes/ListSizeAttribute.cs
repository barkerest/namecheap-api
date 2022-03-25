using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace OneBarker.NamecheapApi.Attributes;

public class ListSizeAttribute : ValidationAttribute
{
    /// <summary>
    /// The maximum accepted size for the list.
    /// </summary>
    public int MaximumSize { get; }
    
    /// <summary>
    /// The minimum accepted size for the list.
    /// </summary>
    public int MinimumSize { get; }
    
    /// <summary>
    /// Set the maximum size for the list.
    /// </summary>
    /// <param name="maximumSize"></param>
    /// <exception cref="ArgumentException"></exception>
    public ListSizeAttribute(int maximumSize) 
        : base(maximumSize == 1 ? "The {0} list must have no more than 1 entry." :"The {0} list must have no more than {1} entries.")
    {
        if (maximumSize < 1) throw new ArgumentException("Maximum size must be a positive integer.");
        MaximumSize = maximumSize;
    }

    /// <summary>
    /// Set the size range for the list.
    /// </summary>
    /// <param name="minimumSize"></param>
    /// <param name="maximumSize"></param>
    /// <exception cref="ArgumentException"></exception>
    public ListSizeAttribute(int minimumSize, int maximumSize)
        :base(
            minimumSize == maximumSize ?
                    maximumSize == 1 ? "The {0} list must have exactly 1 entry." : "The {0} list must have exactly {1} entries."
                    : "The {0} list must have between {2} and {1} entries."
        )
    {
        if (minimumSize < 1) throw new ArgumentException("Minimum size must be a positive integer.");
        if (maximumSize < minimumSize) throw new ArgumentException("Maximum size must be greater than or equal to minimum size.");
        MinimumSize = minimumSize;
        MaximumSize = maximumSize;
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is not IEnumerable e) return true;    // validated by RequiredAttribute
        
        var count = e.Cast<object>().Count();

        return count >= MinimumSize && count <= MaximumSize;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
        => string.Format(ErrorMessageString, name, MaximumSize, MinimumSize);
}
