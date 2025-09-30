namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payees;

/// <summary>
/// ViewModel used by the Payees page for add/edit operations.
/// Mirrors the shape of the API's PayeeUpdateCommand so Mapster/Adapt can map between them.
/// </summary>
public class PayeeViewModel
{
    /// <summary>
    /// Primary identifier of the payee.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique code identifying the payee (e.g., "VEND001", "UTIL-ELEC").
    /// </summary>
    public string PayeeCode { get; set; } = string.Empty;

    /// <summary>
    /// The name of the payee or vendor company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Mailing or physical address for correspondence and check printing.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Default expense account code for automated journal entries.
    /// </summary>
    public string? ExpenseAccountCode { get; set; }

    /// <summary>
    /// Expense account name for reference.
    /// </summary>
    public string? ExpenseAccountName { get; set; }

    /// <summary>
    /// Tax identification number for 1099 reporting and compliance.
    /// </summary>
    public string? Tin { get; set; }

    /// <summary>
    /// Detailed description of the payee's business or services.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes or comments about the payee.
    /// </summary>
    public string? Notes { get; set; }
}
