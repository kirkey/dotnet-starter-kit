namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Search.v1;

using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching pay components.
/// </summary>
public sealed class SearchPayComponentsHandler(
    [FromKeyedServices("hr:paycomponents")] IReadRepository<PayComponent> repository)
    : IRequestHandler<SearchPayComponentsRequest, PagedList<PayComponentDto>>
{
    public async Task<PagedList<PayComponentDto>> Handle(
        SearchPayComponentsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPayComponentsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(c => new PayComponentDto(
            c.Id,
            c.ComponentName,
            c.ComponentType,
            c.IsActive,
            c.Description)).ToList();

        return new PagedList<PayComponentDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

