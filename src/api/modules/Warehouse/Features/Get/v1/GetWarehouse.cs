using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Features.Get.v1;

public sealed record GetWarehouseRequest(DefaultIdType Id);

public sealed record GetWarehouseResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    string? Description,
    bool IsActive,
    decimal MaxWeight,
    decimal MaxVolume,
    string WeightUnit,
    string VolumeUnit,
    DateTime CreatedOn,
    DateTime? LastModifiedOn);

public sealed class GetWarehouseHandler(IReadRepository<Domain.Warehouse> repository)
{
    public async Task<GetWarehouseResponse> Handle(GetWarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouse = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (warehouse is null)
        {
            throw new WarehouseNotFoundException(request.Id);
        }

        return new GetWarehouseResponse(
            warehouse.Id,
            warehouse.Name,
            warehouse.Code,
            warehouse.Address.Street,
            warehouse.Address.City,
            warehouse.Address.State,
            warehouse.Address.PostalCode,
            warehouse.Address.Country,
            warehouse.Description,
            warehouse.IsActive,
            warehouse.Capacity.MaxWeight,
            warehouse.Capacity.MaxVolume,
            warehouse.Capacity.WeightUnit,
            warehouse.Capacity.VolumeUnit,
            warehouse.CreatedOn,
            warehouse.LastModifiedOn);
    }
}
