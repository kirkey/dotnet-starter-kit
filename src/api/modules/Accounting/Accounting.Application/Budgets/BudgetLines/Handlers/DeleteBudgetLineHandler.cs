using Accounting.Application.Budgets.BudgetLines.Commands;

namespace Accounting.Application.Budgets.BudgetLines.Handlers;

public class DeleteBudgetLineHandler(
    IRepository<Budget> repository)
    : IRequestHandler<DeleteBudgetLineCommand>
{
    public async Task Handle(DeleteBudgetLineCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
            throw new BudgetNotFoundException(request.BudgetId);

        budget.RemoveBudgetLine(request.AccountId);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

