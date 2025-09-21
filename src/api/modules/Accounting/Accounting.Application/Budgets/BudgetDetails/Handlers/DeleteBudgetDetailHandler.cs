using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Application.Budgets.BudgetDetails.Handlers;

public class DeleteBudgetDetailHandler(
    IRepository<Budget> repository)
    : IRequestHandler<DeleteBudgetDetailCommand>
{
    public async Task Handle(DeleteBudgetDetailCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
            throw new BudgetNotFoundException(request.BudgetId);

        budget.RemoveBudgetDetail(request.AccountId);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

