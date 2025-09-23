using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Search.v1;

public sealed class SearchProjectsHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository)
    : IRequestHandler<SearchProjectsCommand, PagedList<ProjectResponse>>
{
    public async Task<PagedList<ProjectResponse>> Handle(SearchProjectsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProjectsSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ProjectResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

