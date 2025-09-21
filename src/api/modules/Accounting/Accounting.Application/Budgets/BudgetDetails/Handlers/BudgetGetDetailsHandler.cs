using Accounting.Application.Budgets.BudgetDetails.Commands;
using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.BudgetDetails.Handlers;

/// <summary>
/// Handles retrieval of non-paginated budget details for a budget.
/// </summary>
public sealed class BudgetGetDetailsHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> budgetRepository)
    : IRequestHandler<BudgetGetDetailsCommand, List<BudgetDetailResponse>>
{
    public async Task<List<BudgetDetailResponse>> Handle(BudgetGetDetailsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await budgetRepository.GetByIdAsync(request.BudgetId, cancellationToken).ConfigureAwait(false);
        if (budget == null) throw new BudgetNotFoundException(request.BudgetId);

        // Map Budget.BudgetDetails to response DTOs
        var responses = budget.BudgetDetails
            .Select(bl => new BudgetDetailResponse(
                bl.Id,
                bl.BudgetId,
                bl.AccountId,
                bl.BudgetedAmount,
                bl.ActualAmount,
                bl.Description))
            .ToList();

        return responses;
    }
}
