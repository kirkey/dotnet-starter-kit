using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.FiscalPeriodClose;

/// <summary>
/// View model for initiating fiscal period close process.
/// </summary>
public class FiscalPeriodCloseViewModel
{
    /// <summary>
    /// Unique close identifier (e.g., "CLOSE-2025-10").
    /// </summary>
    [Required(ErrorMessage = "Close number is required")]
    [StringLength(50, ErrorMessage = "Close number cannot exceed 50 characters")]
    public string CloseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Accounting period identifier.
    /// </summary>
    [Required(ErrorMessage = "Period is required")]
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Type of close (MonthEnd, QuarterEnd, YearEnd).
    /// </summary>
    [Required(ErrorMessage = "Close type is required")]
    public string CloseType { get; set; } = "MonthEnd";

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
    /// Description of the close process.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
    public string? Notes { get; set; }
}

