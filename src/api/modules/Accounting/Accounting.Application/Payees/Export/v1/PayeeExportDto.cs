namespace Accounting.Application.Payees.Export.v1;

/// <summary>
/// Data Transfer Object for exporting Payees to Excel format.
/// Contains all relevant payee information formatted for spreadsheet export.
/// </summary>
public sealed class PayeeExportDto
{
    /// <summary>
    /// Unique code identifying the payee (e.g., "VEND001", "UTIL-ELEC").
    /// </summary>
    public string PayeeCode { get; set; } = string.Empty;

    /// <summary>
    /// The payee name (e.g., "ABC Office Supply Company").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Mailing or physical address for the payee.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Default expense account code to use for payments to this payee.
    /// </summary>
    public string? ExpenseAccountCode { get; set; }

    /// <summary>
    /// Default expense account name for readability.
    /// </summary>
    public string? ExpenseAccountName { get; set; }

    /// <summary>
    /// Taxpayer identification number.
    /// </summary>
    public string? Tin { get; set; }

    /// <summary>
    /// Whether the payee is active and can be used for payments.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Detailed description of the payee.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes or comments about the payee.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Date when the payee was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Date when the payee was last modified.
    /// </summary>
    public DateTimeOffset LastModifiedOn { get; set; }
}
