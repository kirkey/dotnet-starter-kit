namespace FSH.Starter.Blazor.Client.Pages.Accounting.Vendors;

/// <summary>
/// ViewModel used by the Vendors page for add/edit operations.
/// Mirrors the shape of the API's VendorUpdateCommand so Mapster/Adapt can map between them.
/// </summary>
public class VendorViewModel
{
    /// <summary>
    /// Primary identifier of the vendor.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique code identifying the vendor (e.g., "VEND001", "ACME-CORP").
    /// </summary>
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// The name of the vendor or supplier company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Mailing or physical address for correspondence and shipments.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Billing address used for invoices and financial correspondence.
    /// </summary>
    public string? BillingAddress { get; set; }

    /// <summary>
    /// Primary contact person at the vendor organization.
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Vendor email address for communications.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Payment terms negotiated with the vendor (e.g., "Net 30", "Net 60", "COD", "2/10 Net 30").
    /// </summary>
    public string? Terms { get; set; }

    /// <summary>
    /// Default expense account code for purchases from this vendor.
    /// Used for automated journal entry creation.
    /// </summary>
    public string? ExpenseAccountCode { get; set; }

    /// <summary>
    /// Expense account name for reference and display purposes.
    /// </summary>
    public string? ExpenseAccountName { get; set; }

    /// <summary>
    /// Tax identification number for 1099 reporting and compliance.
    /// </summary>
    public string? Tin { get; set; }

    /// <summary>
    /// Primary phone number for vendor contact.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Detailed description of the vendor's business or services provided.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes or comments about the vendor (e.g., special handling, preferred contact times).
    /// </summary>
    public string? Notes { get; set; }
}

