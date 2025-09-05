using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;
using FSH.Starter.WebApi.Warehouse.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Features.Create.v1;

public sealed record CreateWarehouseRequest(
    string Name,
    string Code,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    string? Description = null);

public sealed record CreateWarehouseResponse(DefaultIdType Id);

public sealed class CreateWarehouseHandler(IRepository<Domain.Warehouse> repository)
{
    public async Task<CreateWarehouseResponse> Handle(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        // Check if warehouse with the same code already exists
        var existingWarehouse = await repository.FirstOrDefaultAsync(
            w => w.Code == request.Code, cancellationToken);

        if (existingWarehouse is not null)
        {
            throw new WarehouseAlreadyExistsException(request.Code);
        }

        var address = new Address(
            request.Street,
            request.City,
            request.State,
            request.PostalCode,
            request.Country);

        var warehouse = Domain.Warehouse.Create(
            request.Name,
            request.Code,
            address,
            request.Description);

        await repository.AddAsync(warehouse, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateWarehouseResponse(warehouse.Id);
    }
}
