using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Details.Create;

/// <summary>
/// Handles creation of a new <see cref="BudgetDetail"/> and updates the parent <see cref="Budget"/> totals.
/// </summary>
/// <remarks>
/// Validates that:
/// - The budget exists and is modifiable (not Approved/Active).
/// - No duplicate detail exists for the same (BudgetId, AccountId).
/// On success, recalculates totals and persists changes.
/// </remarks>
public sealed class CreateBudgetDetailHandler(
    IRepository<Budget> budgetRepo,
    IRepository<BudgetDetail> detailRepo,
    IReadRepository<BudgetDetail> detailReadRepo)
    : IRequestHandler<CreateBudgetDetailCommand, DefaultIdType>
{
    /// <summary>
    /// Creates the detail and updates the budget totals.
    /// </summary>
    public async Task<DefaultIdType> Handle(CreateBudgetDetailCommand request, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Ensure budget exists and is modifiable
        var budget = await budgetRepo.GetByIdAsync(request.BudgetId, ct);
        if (budget is null) throw new BudgetNotFoundException(request.BudgetId);
        if (budget.Status is "Approved" or "Active")
            throw new BudgetCannotBeModifiedException(budget.Id);

        // Prevent duplicate (BudgetId, AccountId)
        var exists = await detailReadRepo.AnyAsync(
            new Specs.BudgetDetailByBudgetAndAccountSpec(request.BudgetId, request.AccountId), ct);
        if (exists)
            throw new BudgetDetailAlreadyExistsException(request.BudgetId, request.AccountId);

        var entity = BudgetDetail.Create(request.BudgetId, request.AccountId, request.BudgetedAmount, request.Description);
        await detailRepo.AddAsync(entity, ct);

        // Recalculate totals and update budget
        var details = await detailReadRepo.ListAsync(new Specs.BudgetDetailsByBudgetIdSpec(request.BudgetId), ct);
        var totalBudgeted = details.Sum(d => d.BudgetedAmount);
        var totalActual = details.Sum(d => d.ActualAmount);
        budget.SetTotals(totalBudgeted, totalActual);

        await budgetRepo.SaveChangesAsync(ct);
        return entity.Id;
    }
}
