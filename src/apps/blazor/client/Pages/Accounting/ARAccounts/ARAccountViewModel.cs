using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.ArAccounts;

/// <summary>
/// View model for creating and editing AR accounts.
/// </summary>
public class ArAccountViewModel
{
    /// <summary>
    /// Account identifier.
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Unique account number for the AR account.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [StringLength(50, ErrorMessage = "Account number cannot exceed 50 characters")]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account name or description.
    /// </summary>
    [Required(ErrorMessage = "Account name is required")]
    [StringLength(200, ErrorMessage = "Account name cannot exceed 200 characters")]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Associated customer ID (optional).
    /// </summary>
    public DefaultIdType? CustomerId { get; set; }

    /// <summary>
    /// General ledger account for AR control.
    /// </summary>
    public DefaultIdType? GeneralLedgerAccountId { get; set; }

    /// <summary>
    /// Accounting period ID.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Account description.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
    public string? Notes { get; set; }
}

