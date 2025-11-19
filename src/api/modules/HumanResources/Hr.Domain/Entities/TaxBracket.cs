namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents tax brackets for tax calculation.
/// Used to define tax rates for different income ranges and filing statuses.
/// </summary>
public class TaxBracket : AuditableEntity, IAggregateRoot
{
    private TaxBracket() { }

    private TaxBracket(
        DefaultIdType id,
        string taxType,
        int year,
        decimal minIncome,
        decimal maxIncome,
        decimal rate)
    {
        Id = id;
        TaxType = taxType;
        Year = year;
        MinIncome = minIncome;
        MaxIncome = maxIncome;
        Rate = rate;
    }

    /// <summary>
    /// Gets the tax type (Federal, State, FICA, etc).
    /// </summary>
    public string TaxType { get; private set; } = default!;

    /// <summary>
    /// Gets the year for this tax bracket.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Gets the minimum income for this bracket.
    /// </summary>
    public decimal MinIncome { get; private set; }

    /// <summary>
    /// Gets the maximum income for this bracket.
    /// </summary>
    public decimal MaxIncome { get; private set; }

    /// <summary>
    /// Gets the tax rate (0.0 to 1.0 representing 0% to 100%).
    /// </summary>
    public decimal Rate { get; private set; }

    /// <summary>
    /// Gets the filing status (Single, Married, Head of Household, etc).
    /// </summary>
    public string? FilingStatus { get; private set; }

    /// <summary>
    /// Gets the description of the tax bracket.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new tax bracket.
    /// </summary>
    public static TaxBracket Create(
        string taxType,
        int year,
        decimal minIncome,
        decimal maxIncome,
        decimal rate)
    {
        if (maxIncome <= minIncome)
            throw new ArgumentException("Max income must be greater than min income.", nameof(maxIncome));

        if (rate is < 0 or > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1.", nameof(rate));

        var bracket = new TaxBracket(
            DefaultIdType.NewGuid(),
            taxType,
            year,
            minIncome,
            maxIncome,
            rate);

        return bracket;
    }

    /// <summary>
    /// Updates the tax bracket information.
    /// </summary>
    public TaxBracket Update(string? filingStatus = null, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(filingStatus))
            FilingStatus = filingStatus;

        if (description != null)
            Description = description;

        return this;
    }
}

