namespace Accounting.Application.Payees.Import;

/// <summary>
/// Strongly-typed row parsed from the import Excel file for Payees.
/// Represents a single payee entry with all required and optional fields for payee creation.
/// </summary>
public sealed class PayeeImportRow
{
    /// <summary>
    /// Unique code identifying the payee (e.g., "VEND001", "UTIL-ELEC"). Required.
    /// </summary>
    public string? PayeeCode { get; init; }

    /// <summary>
    /// The payee name (e.g., "ABC Office Supply Company"). Required.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Mailing or physical address for the payee.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// Default expense account code to use for payments to this payee (e.g., "5100").
    /// </summary>
    public string? ExpenseAccountCode { get; init; }

    /// <summary>
    /// Default expense account name for readability (e.g., "Office Supplies Expense").
    /// </summary>
    public string? ExpenseAccountName { get; init; }

    /// <summary>
    /// Taxpayer identification number (e.g., "12-3456789" for EIN).
    /// </summary>
    public string? Tin { get; init; }

    /// <summary>
    /// Whether the payee is active and can be used for payments.
    /// </summary>
    public bool? IsActive { get; init; }

    /// <summary>
    /// Detailed description of the payee.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Additional notes or comments about the payee.
    /// </summary>
    public string? Notes { get; init; }
}
