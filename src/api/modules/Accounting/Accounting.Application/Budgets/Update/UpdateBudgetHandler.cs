namespace Accounting.Application.Budgets.Update;

using Accounting.Application.Budgets.Exceptions;
using Accounting.Application.Budgets.Queries;

public sealed class UpdateBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<UpdateBudgetRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (budget == null) throw new BudgetNotFoundException(request.Id);

        var name = request.Name?.Trim();

        // If changing name, check duplicate within same period
        if (!string.IsNullOrWhiteSpace(name))
        {
            var existing = await repository.FirstOrDefaultAsync(
                new BudgetByNamePeriodSpec(name, budget.PeriodId, request.Id), cancellationToken);
            if (existing != null && existing.Id != request.Id)
                throw new BudgetAlreadyExistsException(name, budget.PeriodId);
        }

        budget.Update(name, request.BudgetType?.Trim(), request.Status?.Trim(), request.Description, request.Notes);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}
