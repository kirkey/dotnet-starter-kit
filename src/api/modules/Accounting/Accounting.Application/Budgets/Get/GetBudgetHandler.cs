using Accounting.Application.Budgets.Responses;
using Accounting.Application.Budgets.Specs;

namespace Accounting.Application.Budgets.Get;

/// <summary>
/// Handler for getting a budget by ID.
/// Uses database-level projection for optimal performance with caching.
/// </summary>
public sealed class GetBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository,
    ICacheService cache)
    : IRequestHandler<GetBudgetQuery, BudgetResponse>
{
    /// <summary>
    /// Handles the get budget query.
    /// </summary>
    /// <param name="request">The query containing the budget ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The budget response.</returns>
    /// <exception cref="BudgetNotFoundException">Thrown when budget is not found.</exception>
    public async Task<BudgetResponse> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await cache.GetOrSetAsync(
            $"budget:{request.Id}",
            async () =>
            {
                var spec = new GetBudgetSpec(request.Id);
                return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
                    ?? throw new BudgetNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return response!;
    }
}
