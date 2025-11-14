namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Response object for Tax (TaxBracket) details.
/// </summary>
public sealed record TaxResponse
{
    /// <summary>
    /// Gets the unique identifier of the tax bracket.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the tax type (Federal, State, FICA, etc).
    /// </summary>
    public string TaxType { get; init; } = default!;

    /// <summary>
    /// Gets the year for this tax bracket.
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// Gets the minimum income for this bracket.
    /// </summary>
    public decimal MinIncome { get; init; }

    /// <summary>
    /// Gets the maximum income for this bracket.
    /// </summary>
    public decimal MaxIncome { get; init; }

    /// <summary>
    /// Gets the tax rate (0.0 to 1.0).
    /// </summary>
    public decimal Rate { get; init; }

    /// <summary>
    /// Gets the filing status (Single, Married, etc).
    /// </summary>
    public string? FilingStatus { get; init; }

    /// <summary>
    /// Gets the description of the tax bracket.
    /// </summary>
    public string? Description { get; init; }
}
