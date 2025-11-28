namespace FSH.Starter.Blazor.Client.Pages.Hr.Taxes;

/// <summary>
/// View model for Tax CRUD operations.
/// Contains all properties needed for create and update operations.
/// </summary>
public class TaxViewModel
{
    /// <summary>
    /// Gets or sets the tax ID.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the tax code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the tax name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the tax type.
    /// </summary>
    public string? TaxType { get; set; } = "VAT";

    /// <summary>
    /// Gets or sets the tax rate (0-1 for percentage).
    /// </summary>
    public decimal Rate { get; set; } = 0.12m;

    /// <summary>
    /// Gets or sets whether this is a compound tax.
    /// </summary>
    public bool IsCompound { get; set; }

    /// <summary>
    /// Gets or sets the jurisdiction.
    /// </summary>
    public string? Jurisdiction { get; set; }

    /// <summary>
    /// Gets or sets the effective date.
    /// </summary>
    public DateTime? EffectiveDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets the expiry date.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets whether the tax is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets the rate as percentage display.
    /// </summary>
    public string RatePercentage => $"{Rate * 100:N2}%";
}
