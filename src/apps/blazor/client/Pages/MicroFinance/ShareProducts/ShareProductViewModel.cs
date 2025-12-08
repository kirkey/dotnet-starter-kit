namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareProducts;

/// <summary>
/// ViewModel used by the ShareProducts page for CRUD operations.
/// Mirrors the shape of the API's CreateShareProductCommand so Mapster/Adapt can map between them.
/// </summary>
public class ShareProductViewModel
{
    /// <summary>
    /// Primary identifier of the share product.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique code for the share product.
    /// </summary>
    public string? Code { get; set; } = "COMMON";

    /// <summary>
    /// Name of the share product.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the share product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Nominal (par) value of each share.
    /// </summary>
    public decimal NominalValue { get; set; } = 100m;

    /// <summary>
    /// Current market price of each share.
    /// </summary>
    public decimal CurrentPrice { get; set; } = 100m;

    /// <summary>
    /// Minimum shares required for membership.
    /// </summary>
    public int MinSharesForMembership { get; set; } = 1;

    /// <summary>
    /// Maximum shares allowed per member.
    /// </summary>
    public int? MaxSharesPerMember { get; set; } = 1000;

    /// <summary>
    /// Whether shares can be transferred between members.
    /// </summary>
    public bool AllowTransfer { get; set; }

    /// <summary>
    /// Whether shares can be redeemed (sold back).
    /// </summary>
    public bool AllowRedemption { get; set; } = true;

    /// <summary>
    /// Minimum holding period in months before redemption.
    /// </summary>
    public int? MinHoldingPeriodMonths { get; set; } = 12;

    /// <summary>
    /// Whether this share product pays dividends.
    /// </summary>
    public bool PaysDividends { get; set; } = true;
}
