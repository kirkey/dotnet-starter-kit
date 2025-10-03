using FSH.Starter.WebApi.Store.Application.Bins.Specs;
using Store.Domain.Exceptions.Bin;

namespace FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

public sealed class GetBinHandler(
    [FromKeyedServices("store:bins")] IReadRepository<Bin> repository)
    : IRequestHandler<GetBinRequest, BinResponse>
{
    public async Task<BinResponse> Handle(GetBinRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bin = await repository.FirstOrDefaultAsync(
            new GetBinByIdSpec(request.Id), 
            cancellationToken).ConfigureAwait(false);

        if (bin is null)
            throw new BinNotFoundException(request.Id);

        return new BinResponse
        {
            Id = bin.Id,
            Name = bin.Name,
            Code = bin.Code,
            WarehouseLocationId = bin.WarehouseLocationId,
            WarehouseLocationName = bin.WarehouseLocation?.Name,
            Capacity = bin.Capacity,
            UsedCapacity = bin.CurrentUtilization,
            CapacityUnit = "Units",
            IsActive = bin.IsActive,
            Notes = bin.Description,
            Aisle = bin.WarehouseLocation?.Aisle,
            Section = bin.WarehouseLocation?.Section,
            Shelf = bin.WarehouseLocation?.Shelf,
            LocationType = bin.BinType
        };
    }
}
