using FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

/// <summary>
/// Handler for searching sales imports with filtering and pagination.
/// </summary>
public sealed class SearchSalesImportsHandler(
    ILogger<SearchSalesImportsHandler> logger,
    [FromKeyedServices("store:sales-imports")] IReadRepository<SalesImport> repository)
    : IRequestHandler<SearchSalesImportsRequest, PagedList<SalesImportResponse>>
{
    public async Task<PagedList<SalesImportResponse>> Handle(SearchSalesImportsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSalesImportsSpec(request);
        
        var imports = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Search complete: retrieved {Count} sales imports (Page {PageNumber}, Size {PageSize})", 
            totalCount, request.PageNumber, request.PageSize);

        return new PagedList<SalesImportResponse>(imports, request.PageNumber, request.PageSize, totalCount);
    }
}

