using Accounting.Application.Budgets.Exceptions;

namespace Accounting.Application.Budgets.Approve;

/// <summary>
/// Handler for approving a budget.
/// </summary>
public sealed class ApproveBudgetHandler(
    ILogger<ApproveBudgetHandler> logger,
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<ApproveBudgetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveBudgetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        
        if (budget == null)
        {
            throw new BudgetNotFoundException(request.BudgetId);
        }

        if (string.IsNullOrWhiteSpace(request.ApprovedBy))
        {
            throw new ArgumentException("ApprovedBy is required for budget approval.");
        }

        budget.Approve(request.ApprovedBy);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Budget {BudgetId} approved by {ApprovedBy}", 
            request.BudgetId, request.ApprovedBy);

        return budget.Id;
    }
}
