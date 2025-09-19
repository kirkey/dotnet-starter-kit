using Accounting.Application.Projects.Responses;


namespace Accounting.Application.Projects.Search;

public sealed class SearchProjectsHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository)
    : IRequestHandler<SearchProjectsRequest, PagedList<ProjectResponse>>
{
    public async Task<PagedList<ProjectResponse>> Handle(SearchProjectsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProjectsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = list.Select(p => p.Adapt<ProjectResponse>()).ToList();

        return new PagedList<ProjectResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
