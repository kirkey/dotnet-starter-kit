namespace Accounting.Application.Budgets.Update;

/// <summary>
/// Command to update a Budget aggregate.
/// </summary>
public sealed record UpdateBudgetCommand : IRequest<UpdateBudgetResponse>
{
    /// <summary>
    /// Budget identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Period identifier.
    /// </summary>
    public DefaultIdType PeriodId { get; init; }
    
    /// <summary>
    /// Fiscal year.
    /// </summary>
    public int FiscalYear { get; init; }
    
    /// <summary>
    /// Budget name.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Budget type.
    /// </summary>
    public string? BudgetType { get; init; }
    
    /// <summary>
    /// Status.
    /// </summary>
    public string? Status { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Notes.
    /// </summary>
    public string? Notes { get; init; }
}
