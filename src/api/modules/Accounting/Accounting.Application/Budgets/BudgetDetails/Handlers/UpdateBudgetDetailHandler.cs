using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Application.Budgets.BudgetDetails.Handlers;

public class UpdateBudgetDetailHandler(
    IRepository<Budget> repository)
    : IRequestHandler<UpdateBudgetDetailCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateBudgetDetailCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
            throw new BudgetNotFoundException(request.BudgetId);

        budget.UpdateBudgetDetail(request.AccountId, request.BudgetedAmount, request.Description);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}
