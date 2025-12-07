namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareAccounts;

/// <summary>
/// ViewModel used by the ShareAccounts page for add/edit operations.
/// Mirrors the shape of the API's CreateShareAccountCommand and UpdateShareAccountCommand.
/// </summary>
public class ShareAccountViewModel
{
    /// <summary>
    /// Primary identifier of the share account.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique account number. Example: "SHA-0001".
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Member who owns this share account.
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Share product for this account.
    /// </summary>
    public Guid ShareProductId { get; set; }

    /// <summary>
    /// Number of shares held.
    /// </summary>
    public int NumberOfShares { get; set; }

    /// <summary>
    /// Total value of shares (NumberOfShares × CurrentPrice).
    /// </summary>
    public decimal TotalShareValue { get; set; }

    /// <summary>
    /// Date shares were purchased.
    /// </summary>
    public DateOnly? PurchaseDate { get; set; }

    /// <summary>
    /// DateTime wrapper for PurchaseDate to work with MudDatePicker.
    /// </summary>
    public DateTime? PurchaseDateDate
    {
        get => PurchaseDate?.ToDateTime(TimeOnly.MinValue);
        set => PurchaseDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Notes or comments about this share account.
    /// </summary>
    public string? Notes { get; set; }
}
