using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;

/// <summary>
/// Handler for searching designations.
/// </summary>
public sealed class SearchDesignationsHandler(
    [FromKeyedServices("hr:designations")] IReadRepository<Designation> repository)
    : IRequestHandler<SearchDesignationsRequest, PagedList<DesignationResponse>>
{
    public async Task<PagedList<DesignationResponse>> Handle(SearchDesignationsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDesignationsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<DesignationResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

