using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.TrialBalance;

/// <summary>
/// View model for creating and editing trial balance reports.
/// </summary>
public class TrialBalanceViewModel
{
    /// <summary>
    /// Trial balance identifier (null for new reports).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Unique trial balance number (e.g., "TB-2025-01").
    /// </summary>
    [Required(ErrorMessage = "Trial balance number is required")]
    [StringLength(50, ErrorMessage = "Trial balance number cannot exceed 50 characters")]
    public string TrialBalanceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Accounting period identifier.
    /// </summary>
    [Required(ErrorMessage = "Period is required")]
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Period start date.
    /// </summary>
    [Required(ErrorMessage = "Period start date is required")]
    public DateTime? PeriodStartDate { get; set; }

    /// <summary>
    /// Period end date.
    /// </summary>
    [Required(ErrorMessage = "Period end date is required")]
    public DateTime? PeriodEndDate { get; set; }

    /// <summary>
    /// Whether to include accounts with zero balances.
    /// </summary>
    public bool IncludeZeroBalances { get; set; }

    /// <summary>
    /// Whether to auto-generate from general ledger.
    /// </summary>
    public bool AutoGenerate { get; set; } = true;

    /// <summary>
    /// Report description.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
    public string? Notes { get; set; }
}

