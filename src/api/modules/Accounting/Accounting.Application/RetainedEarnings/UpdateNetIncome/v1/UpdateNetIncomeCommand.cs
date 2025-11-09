namespace Accounting.Application.RetainedEarnings.UpdateNetIncome.v1;

/// <summary>
/// Command to update net income for retained earnings.
/// </summary>
public sealed record UpdateNetIncomeCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The ID of the retained earnings record to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// The net income amount to set.
    /// </summary>
    public decimal NetIncome { get; init; }
}
