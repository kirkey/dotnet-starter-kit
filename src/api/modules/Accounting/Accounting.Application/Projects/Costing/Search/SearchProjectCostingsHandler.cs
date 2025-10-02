using Accounting.Application.Projects.Costing.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Projects.Costing.Search;

public sealed class SearchProjectCostingsHandler(
    [FromKeyedServices("accounting:projectcosting")] IReadRepository<ProjectCostEntry> repository)
    : IRequestHandler<SearchProjectCostingsQuery, PagedList<ProjectCostingResponse>>
{
    public async Task<PagedList<ProjectCostingResponse>> Handle(SearchProjectCostingsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProjectCostingsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ProjectCostingResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
