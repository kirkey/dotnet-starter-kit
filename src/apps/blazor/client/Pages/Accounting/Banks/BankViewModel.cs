namespace FSH.Starter.Blazor.Client.Pages.Accounting.Banks;

/// <summary>
/// ViewModel used by the Banks page for add/edit operations.
/// Mirrors the shape of the API's UpdateBankCommand so Mapster/Adapt can map between them.
/// </summary>
public class BankViewModel
{
    /// <summary>
    /// Primary identifier of the bank.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique code identifying the bank (e.g., "BNK001", "CHASE-NYC").
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// The name of the bank or financial institution.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ABA routing number for domestic transfers (9 digits for US banks).
    /// </summary>
    public string? RoutingNumber { get; set; }

    /// <summary>
    /// SWIFT/BIC code for international transfers (8 or 11 characters).
    /// </summary>
    public string? SwiftCode { get; set; }

    /// <summary>
    /// Physical address of the bank branch.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Name of the account officer or primary contact at the bank.
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Bank phone number for inquiries and support.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Bank email address for electronic correspondence.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Bank website URL for online banking and information.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Detailed description of the bank or banking relationship.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes or comments about the bank.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether the bank is active and can be used for bank account operations.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Image URL for the bank logo.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Image file upload command for uploading bank logo.
    /// </summary>
    public FileUploadCommand? Image { get; set; }
}

