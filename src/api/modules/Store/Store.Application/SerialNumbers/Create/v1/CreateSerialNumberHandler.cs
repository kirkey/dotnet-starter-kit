using FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;
using Store.Domain.Exceptions.SerialNumber;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Create.v1;

public class CreateSerialNumberHandler(
    [FromKeyedServices("store:serialnumbers")] IRepository<SerialNumber> repository,
    [FromKeyedServices("store:serialnumbers")] IReadRepository<SerialNumber> readRepository)
    : IRequestHandler<CreateSerialNumberCommand, CreateSerialNumberResponse>
{
    public async Task<CreateSerialNumberResponse> Handle(CreateSerialNumberCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate serial number value
        var existingSerialNumber = await readRepository.FirstOrDefaultAsync(
            new SerialNumberByValueSpec(request.SerialNumberValue),
            cancellationToken);

        if (existingSerialNumber is not null)
        {
            throw new DuplicateSerialNumberException(request.SerialNumberValue);
        }

        var serialNumber = SerialNumber.Create(
            request.SerialNumberValue,
            request.ItemId,
            request.WarehouseId,
            request.WarehouseLocationId,
            request.BinId,
            request.LotNumberId,
            request.ReceiptDate,
            request.ManufactureDate,
            request.WarrantyExpirationDate,
            request.ExternalReference,
            request.Notes);

        await repository.AddAsync(serialNumber, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateSerialNumberResponse(serialNumber.Id, serialNumber.SerialNumberValue);
    }
}
