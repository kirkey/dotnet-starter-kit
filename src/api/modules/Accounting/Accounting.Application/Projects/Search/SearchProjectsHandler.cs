using Accounting.Application.Projects.Dtos;
using Accounting.Domain;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Projects.Search;

public sealed class SearchProjectsHandler(
    [FromKeyedServices("accounting:projects")] IReadRepository<Project> repository)
    : IRequestHandler<SearchProjectsRequest, PagedList<ProjectDto>>
{
    public async Task<PagedList<ProjectDto>> Handle(SearchProjectsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProjectsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ProjectDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


