using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Details.Update;

public sealed class UpdateBudgetDetailHandler(
    IRepository<Budget> budgetRepo,
    IRepository<BudgetDetail> detailRepo,
    IReadRepository<BudgetDetail> detailReadRepo)
    : IRequestHandler<UpdateBudgetDetailCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateBudgetDetailCommand request, CancellationToken ct)
    {
        var detail = await detailRepo.GetByIdAsync(request.Id, ct) ?? throw new NotFoundException($"budget detail {request.Id} not found");

        var budget = await budgetRepo.GetByIdAsync(detail.BudgetId, ct) ?? throw new BudgetNotFoundException(detail.BudgetId);
        if (budget.Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(budget.Id);

        detail.Update(request.BudgetedAmount, request.Description);

        var details = await detailReadRepo.ListAsync(new Specs.BudgetDetailsByBudgetIdSpec(detail.BudgetId), ct);
        var totalBudgeted = details.Sum(d => d.BudgetedAmount);
        var totalActual = details.Sum(d => d.ActualAmount);
        budget.SetTotals(totalBudgeted, totalActual);

        await budgetRepo.SaveChangesAsync(ct);
        return detail.Id;
    }
}
