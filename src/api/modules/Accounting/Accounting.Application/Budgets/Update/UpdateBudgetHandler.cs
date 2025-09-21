namespace Accounting.Application.Budgets.Update;

using Exceptions;
using Queries;

public sealed class UpdateBudgetHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> accountingPeriodRepository,
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<UpdateBudgetCommand, UpdateBudgetResponse>
{
    public async Task<UpdateBudgetResponse> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
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
        
        var period = await accountingPeriodRepository.GetByIdAsync(request.PeriodId, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.PeriodId);

        budget.Update(request.PeriodId, period.Name, request.FiscalYear, name, request.BudgetType?.Trim(), request.Status?.Trim(), request.Description, request.Notes);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateBudgetResponse(budget.Id);
    }
}
