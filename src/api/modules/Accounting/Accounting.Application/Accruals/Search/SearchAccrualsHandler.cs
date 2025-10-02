using Accounting.Application.Accruals.Responses;
using Accounting.Application.Accruals.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.Accruals.Search;

/// <summary>
/// Handles search queries for accruals, applying filters and pagination.
/// </summary>
public sealed class SearchAccrualsHandler(
    [FromKeyedServices("accounting:accruals")] IReadRepository<Accrual> repository)
    : IRequestHandler<SearchAccrualsQuery, PagedList<AccrualResponse>>
{
    /// <summary>
    /// Processes the search query, validates input, and returns a paged list of accrual responses.
    /// </summary>
    /// <param name="request">The search query containing filter and pagination parameters.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paged list of accrual responses matching the query.</returns>
    /// <exception cref="ValidationException">Thrown if query validation fails.</exception>
    public async Task<PagedList<AccrualResponse>> Handle(SearchAccrualsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        request.Validate();

        var spec = new SearchAccrualsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AccrualResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
