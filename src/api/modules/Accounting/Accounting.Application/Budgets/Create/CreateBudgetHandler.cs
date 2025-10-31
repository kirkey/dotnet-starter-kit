namespace Accounting.Application.Budgets.Create;

using Exceptions;
using Queries;

public sealed class CreateBudgetHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> accountingPeriodRepository,
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<CreateBudgetCommand, CreateBudgetResponse>
{
    public async Task<CreateBudgetResponse> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var name = request.Name?.Trim() ?? string.Empty;

        // Check duplicate name for period
        var existing = await repository.FirstOrDefaultAsync(new BudgetByNamePeriodSpec(name, request.PeriodId), cancellationToken);
        if (existing != null)
            throw new BudgetAlreadyExistsException(name, request.PeriodId);
        
        var period = await accountingPeriodRepository.GetByIdAsync(request.PeriodId, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.PeriodId);

        var budget = Budget.Create(
            name,
            request.PeriodId,
            period.Name?.Trim() ?? string.Empty,
            request.FiscalYear,
            request.BudgetType?.Trim() ?? string.Empty,
            request.Description,
            request.Notes);

        await repository.AddAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateBudgetResponse(budget.Id);
    }
}
