namespace Accounting.Application.AccountingPeriods.Update.v1;

/// <summary>
/// Command to update an existing accounting period.
/// Only mutable fields are included; domain invariants are validated by the handler.
/// </summary>
public record UpdateAccountingPeriodCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// Identifier of the accounting period to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Optional new name for the period.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Optional new start date.
    /// </summary>
    public DateTime? StartDate { get; init; }
    
    /// <summary>
    /// Optional new end date.
    /// </summary>
    public DateTime? EndDate { get; init; }
    
    /// <summary>
    /// Flag indicating adjustment period.
    /// </summary>
    public bool IsAdjustmentPeriod { get; init; }
    
    /// <summary>
    /// Optional fiscal year.
    /// </summary>
    public int? FiscalYear { get; init; }
    
    /// <summary>
    /// Optional period type (Monthly, Quarterly, Yearly, Annual).
    /// </summary>
    public string? PeriodType { get; init; }
    
    /// <summary>
    /// Optional long description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Optional administrative notes.
    /// </summary>
    public string? Notes { get; init; }
}
