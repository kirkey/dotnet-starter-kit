using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;

namespace FSH.Starter.WebApi.Warehouse.Features.GetList.v1;

public sealed record GetWarehouseListRequest(
    string? SearchTerm = null,
    bool? IsActive = null,
    int PageNumber = 1,
    int PageSize = 10);

public sealed record WarehouseDto(
    DefaultIdType Id,
    string Name,
    string Code,
    string Address,
    string? Description,
    bool IsActive,
    DateTime CreatedOn);

public sealed class GetWarehouseListHandler(IReadRepository<Domain.Warehouse> repository)
{
    public async Task<PagedList<WarehouseDto>> Handle(GetWarehouseListRequest request, CancellationToken cancellationToken)
    {
        var spec = new WarehouseListSpec(request);
        var warehouses = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var warehouseDtos = warehouses.Select(w => new WarehouseDto(
            w.Id,
            w.Name,
            w.Code,
            w.Address.ToString(),
            w.Description,
            w.IsActive,
            w.CreatedOn)).ToList();

        return new PagedList<WarehouseDto>(warehouseDtos, totalCount, request.PageNumber, request.PageSize);
    }
}

public sealed class WarehouseListSpec : Specification<Domain.Warehouse>
{
    public WarehouseListSpec(GetWarehouseListRequest request)
    {
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            Query.Where(w => w.Name.Contains(request.SearchTerm) || 
                           w.Code.Contains(request.SearchTerm) ||
                           (w.Description != null && w.Description.Contains(request.SearchTerm)));
        }

        if (request.IsActive.HasValue)
        {
            Query.Where(w => w.IsActive == request.IsActive.Value);
        }

        Query.OrderBy(w => w.Name);
        Query.Skip((request.PageNumber - 1) * request.PageSize);
        Query.Take(request.PageSize);
    }
}
