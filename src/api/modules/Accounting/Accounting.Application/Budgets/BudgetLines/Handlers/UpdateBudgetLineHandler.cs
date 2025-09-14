using Accounting.Application.Budgets.BudgetLines.Commands;

namespace Accounting.Application.Budgets.BudgetLines.Handlers;

public class UpdateBudgetLineHandler(
    IRepository<Budget> repository)
    : IRequestHandler<UpdateBudgetLineCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateBudgetLineCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
            throw new BudgetNotFoundException(request.BudgetId);

        budget.UpdateBudgetLine(request.AccountId, request.BudgetedAmount, request.Description);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}

