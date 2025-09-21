using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Application.Budgets.BudgetDetails.Handlers;

public class AddBudgetDetailHandler(
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<AddBudgetDetailCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(AddBudgetDetailCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
            throw new BudgetNotFoundException(request.BudgetId);

        budget.AddBudgetDetail(request.AccountId, request.BudgetedAmount, request.Description);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}
