namespace Accounting.Application.AccountingPeriods.Commands.CloseAccountingPeriod.v1;

/// <summary>
/// Command to close an accounting period. Supports options to generate closing journal entries
/// and perform year-end adjustments as part of the close process.
/// </summary>
public class CloseAccountingPeriodCommand : BaseRequest, IRequest<DefaultIdType>
{
    /// <summary>
    /// Identifier of the accounting period to close.
    /// </summary>
    public DefaultIdType AccountingPeriodId { get; set; }

    /// <summary>
    /// Closing date to use for generated entries (must fall inside the period).
    /// </summary>
    public DateTime ClosingDate { get; set; }

    /// <summary>
    /// Optional notes about the closing operation.
    /// </summary>
    public string? ClosingNotes { get; set; }

    /// <summary>
    /// Whether year-end adjustments should be performed as part of the close.
    /// </summary>
    public bool PerformYearEndAdjustments { get; set; }

    /// <summary>
    /// Whether to auto-generate closing journal entries when closing the period.
    /// </summary>
    public bool GenerateClosingEntries { get; set; } = true;
}
