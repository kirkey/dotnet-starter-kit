using FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

/// <summary>
/// Handler for searching sales imports with filtering and pagination.
/// </summary>
public class SearchSalesImportsHandler(IReadRepository<SalesImport> repository)
    : IRequestHandler<SearchSalesImportsRequest, PagedList<SalesImportResponse>>
{
    public async Task<PagedList<SalesImportResponse>> Handle(SearchSalesImportsRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchSalesImportsSpec(request);
        
        var imports = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var responses = imports.Adapt<List<SalesImportResponse>>();

        return new PagedList<SalesImportResponse>(
            responses,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}

