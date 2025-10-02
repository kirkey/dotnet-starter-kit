using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Details.Delete;

public sealed class DeleteBudgetDetailHandler(
    IRepository<Budget> budgetRepo,
    IRepository<BudgetDetail> detailRepo,
    IReadRepository<BudgetDetail> detailReadRepo)
    : IRequestHandler<DeleteBudgetDetailCommand>
{
    public async Task Handle(DeleteBudgetDetailCommand request, CancellationToken ct)
    {
        var detail = await detailRepo.GetByIdAsync(request.Id, ct) ?? throw new NotFoundException($"budget detail {request.Id} not found");

        var budget = await budgetRepo.GetByIdAsync(detail.BudgetId, ct) ?? throw new BudgetNotFoundException(detail.BudgetId);
        if (budget.Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(budget.Id);

        await detailRepo.DeleteAsync(detail, ct);

        var details = await detailReadRepo.ListAsync(new Specs.BudgetDetailsByBudgetIdSpec(detail.BudgetId), ct);
        var totalBudgeted = details.Sum(d => d.BudgetedAmount);
        var totalActual = details.Sum(d => d.ActualAmount);
        budget.SetTotals(totalBudgeted, totalActual);

        await budgetRepo.SaveChangesAsync(ct);
    }
}

