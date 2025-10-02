using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Close;

/// <summary>
/// Handler for closing a budget.
/// </summary>
public sealed class CloseBudgetHandler(
    ILogger<CloseBudgetHandler> logger,
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<CloseBudgetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CloseBudgetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        
        if (budget == null)
        {
            throw new BudgetNotFoundException(request.BudgetId);
        }

        budget.Close();

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Budget {BudgetId} closed", request.BudgetId);

        return budget.Id;
    }
}
