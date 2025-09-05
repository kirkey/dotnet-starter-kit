using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;
using FSH.Starter.WebApi.Warehouse.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Features.Update.v1;

public sealed record UpdateWarehouseRequest(
    DefaultIdType Id,
    string? Name = null,
    string? Description = null,
    string? Street = null,
    string? City = null,
    string? State = null,
    string? PostalCode = null,
    string? Country = null);

public sealed record UpdateWarehouseResponse(DefaultIdType Id);

public sealed class UpdateWarehouseHandler(IRepository<Domain.Warehouse> repository)
{
    public async Task<UpdateWarehouseResponse> Handle(UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouse = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (warehouse is null)
        {
            throw new WarehouseNotFoundException(request.Id);
        }

        Address? newAddress = null;
        if (!string.IsNullOrEmpty(request.Street) ||
            !string.IsNullOrEmpty(request.City) ||
            !string.IsNullOrEmpty(request.State) ||
            !string.IsNullOrEmpty(request.PostalCode) ||
            !string.IsNullOrEmpty(request.Country))
        {
            newAddress = new Address(
                request.Street ?? warehouse.Address.Street,
                request.City ?? warehouse.Address.City,
                request.State ?? warehouse.Address.State,
                request.PostalCode ?? warehouse.Address.PostalCode,
                request.Country ?? warehouse.Address.Country);
        }

        warehouse.Update(request.Name, request.Description, newAddress);

        await repository.UpdateAsync(warehouse, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateWarehouseResponse(warehouse.Id);
    }
}
