namespace Accounting.Application.PrepaidExpenses.Update.v1;

/// <summary>
/// Command to update a prepaid expense.
/// </summary>
public sealed record UpdatePrepaidExpenseCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// Prepaid expense identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// End date.
    /// </summary>
    public DateTime? EndDate { get; init; }
    
    /// <summary>
    /// Cost center identifier.
    /// </summary>
    public DefaultIdType? CostCenterId { get; init; }
    
    /// <summary>
    /// Notes.
    /// </summary>
    public string? Notes { get; init; }
}
