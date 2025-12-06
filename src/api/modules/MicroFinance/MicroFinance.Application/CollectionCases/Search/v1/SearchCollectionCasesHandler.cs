using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Search.v1;

/// <summary>
/// Handler for searching collection cases with pagination and filtering capabilities.
/// Implements CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public sealed class SearchCollectionCasesHandler(
    [FromKeyedServices("microfinance:collectioncases")] IReadRepository<CollectionCase> repository)
    : IRequestHandler<SearchCollectionCasesCommand, PagedList<CollectionCaseResponse>>
{
    /// <summary>
    /// Handles the search request for collection cases with filtering and pagination.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination settings.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of collection case responses matching the search criteria.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<CollectionCaseResponse>> Handle(SearchCollectionCasesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollectionCasesSpec(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollectionCaseResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
