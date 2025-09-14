namespace Accounting.Application.Budgets.Create;

public sealed class CreateBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<CreateBudgetRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = Budget.Create(
            request.Name,
            request.PeriodId,
            request.FiscalYear,
            request.BudgetType,
            request.Description,
            request.Notes);

        await repository.AddAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}
