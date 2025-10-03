namespace FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;

/// <summary>
/// Handler for searching stock levels with filtering, sorting, and pagination support.
/// </summary>
/// <remarks>
/// Processes SearchStockLevelsCommand to retrieve a paginated list of stock levels
/// based on search criteria such as item ID, warehouse ID, quantity ranges, and location filters.
/// Uses SearchStockLevelsSpec for building the query specification and response mapping.
/// </remarks>
public sealed class SearchStockLevelsHandler(
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> repository)
    : IRequestHandler<SearchStockLevelsCommand, PagedList<StockLevelResponse>>
{
    /// <summary>
    /// Handles the search request for stock levels.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of stock level responses.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<StockLevelResponse>> Handle(SearchStockLevelsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.SearchStockLevelsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<StockLevelResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
