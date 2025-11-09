using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.RetainedEarnings;

/// <summary>
/// View model for creating retained earnings records.
/// </summary>
public class RetainedEarningsViewModel
{
    /// <summary>
    /// Fiscal year for the retained earnings (e.g., 2025).
    /// </summary>
    [Required(ErrorMessage = "Fiscal year is required")]
    [Range(1900, 2100, ErrorMessage = "Fiscal year must be between 1900 and 2100")]
    public int FiscalYear { get; set; } = DateTime.UtcNow.Year;

    /// <summary>
    /// Opening retained earnings balance.
    /// </summary>
    [Required(ErrorMessage = "Opening balance is required")]
    [Range(typeof(decimal), "-999999999", "999999999", ErrorMessage = "Opening balance must be between -999,999,999 and 999,999,999")]
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Fiscal year start date.
    /// </summary>
    [Required(ErrorMessage = "Fiscal year start date is required")]
    public DateTime? FiscalYearStartDate { get; set; }

    /// <summary>
    /// Fiscal year end date.
    /// </summary>
    [Required(ErrorMessage = "Fiscal year end date is required")]
    public DateTime? FiscalYearEndDate { get; set; }

    /// <summary>
    /// Retained earnings GL account identifier.
    /// </summary>
    public DefaultIdType? RetainedEarningsAccountId { get; set; }

    /// <summary>
    /// Description of the retained earnings record.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
    public string? Notes { get; set; }
}

