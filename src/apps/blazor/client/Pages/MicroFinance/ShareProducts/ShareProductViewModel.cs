namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareProducts;

/// <summary>
/// ViewModel used by the ShareProducts page for add/edit operations.
/// Mirrors the shape of the API's CreateShareProductCommand and UpdateShareProductCommand.
/// </summary>
public class ShareProductViewModel
{
    /// <summary>
    /// Primary identifier of the share product.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique product code. Example: "SHARE-001".
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Product name. Required.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the share product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Nominal (face) value per share.
    /// </summary>
    public decimal NominalValue { get; set; }

    /// <summary>
    /// Current market price per share.
    /// </summary>
    public decimal CurrentPrice { get; set; }

    /// <summary>
    /// Minimum shares required for membership.
    /// </summary>
    public int MinSharesForMembership { get; set; }

    /// <summary>
    /// Maximum shares allowed per member.
    /// </summary>
    public int? MaxSharesPerMember { get; set; }

    /// <summary>
    /// Whether share transfers between members are allowed.
    /// </summary>
    public bool AllowTransfer { get; set; }

    /// <summary>
    /// Whether shares can be redeemed (bought back by MFI).
    /// </summary>
    public bool AllowRedemption { get; set; }

    /// <summary>
    /// Whether this share product pays dividends.
    /// </summary>
    public bool PaysDividends { get; set; }

    /// <summary>
    /// Dividend frequency: "Annually", "Semi-Annually", "Quarterly".
    /// </summary>
    public string? DividendFrequency { get; set; }

    /// <summary>
    /// Currency code. Default: "PHP".
    /// </summary>
    public string CurrencyCode { get; set; } = "PHP";
}
