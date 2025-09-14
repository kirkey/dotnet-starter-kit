using Accounting.Application.Budgets.BudgetLines.Commands;
using Accounting.Application.Budgets.BudgetLines.Exceptions;

namespace Accounting.Application.Budgets.BudgetLines.Handlers;

public class AddBudgetLineHandler(
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<AddBudgetLineCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(AddBudgetLineCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
            throw new BudgetNotFoundException(request.BudgetId);

        budget.AddBudgetLine(request.AccountId, request.BudgetedAmount, request.Description);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}

