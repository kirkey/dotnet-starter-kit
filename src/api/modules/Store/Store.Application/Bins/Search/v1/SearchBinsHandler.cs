using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Specs;

namespace FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

public sealed class SearchBinsHandler(
    [FromKeyedServices("store:bins")] IReadRepository<Bin> repository)
    : IRequestHandler<SearchBinsCommand, PagedList<BinResponse>>
{
    public async Task<PagedList<BinResponse>> Handle(SearchBinsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBinsSpec(request);

        var bins = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var binResponses = bins.Select(b => new BinResponse
        {
            Id = b.Id,
            Name = b.Name,
            Code = b.Code,
            WarehouseLocationId = b.WarehouseLocationId,
            WarehouseLocationName = b.WarehouseLocation?.Name,
            Capacity = b.Capacity,
            UsedCapacity = b.CurrentUtilization,
            CapacityUnit = "Units",
            IsActive = b.IsActive,
            Notes = b.Description,
            Aisle = b.WarehouseLocation?.Aisle,
            Section = b.WarehouseLocation?.Section,
            Shelf = b.WarehouseLocation?.Shelf,
            LocationType = b.BinType
        }).ToList();

        return new PagedList<BinResponse>(binResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
