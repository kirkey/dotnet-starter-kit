using FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;
using Store.Domain.Exceptions.SerialNumber;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

public class UpdateSerialNumberHandler(
    [FromKeyedServices("store:serialnumbers")] IRepository<SerialNumber> repository,
    [FromKeyedServices("store:serialnumbers")] IReadRepository<SerialNumber> readRepository)
    : IRequestHandler<UpdateSerialNumberCommand, UpdateSerialNumberResponse>
{
    public async Task<UpdateSerialNumberResponse> Handle(UpdateSerialNumberCommand request, CancellationToken cancellationToken)
    {
        var serialNumber = await readRepository.FirstOrDefaultAsync(
            new GetSerialNumberByIdSpec(request.Id),
            cancellationToken);

        if (serialNumber is null)
        {
            throw new SerialNumberNotFoundException(request.Id);
        }

        // Update status
        serialNumber.UpdateStatus(request.Status);

        // Update location if provided
        serialNumber.UpdateLocation(
            request.WarehouseId,
            request.WarehouseLocationId,
            request.BinId);

        // Update lot number if provided
        if (request.LotNumberId.HasValue)
        {
            // Using reflection to update private setter since no domain method exists
            var lotNumberIdProperty = typeof(SerialNumber).GetProperty(nameof(SerialNumber.LotNumberId));
            lotNumberIdProperty?.SetValue(serialNumber, request.LotNumberId);
        }

        // Update manufacture date if provided
        if (request.ManufactureDate.HasValue)
        {
            var manufactureDateProperty = typeof(SerialNumber).GetProperty(nameof(SerialNumber.ManufactureDate));
            manufactureDateProperty?.SetValue(serialNumber, request.ManufactureDate);
        }

        // Update warranty expiration date if provided
        if (request.WarrantyExpirationDate.HasValue)
        {
            var warrantyExpirationDateProperty = typeof(SerialNumber).GetProperty(nameof(SerialNumber.WarrantyExpirationDate));
            warrantyExpirationDateProperty?.SetValue(serialNumber, request.WarrantyExpirationDate);
        }

        // Update external reference if provided
        if (!string.IsNullOrWhiteSpace(request.ExternalReference))
        {
            var externalReferenceProperty = typeof(SerialNumber).GetProperty(nameof(SerialNumber.ExternalReference));
            externalReferenceProperty?.SetValue(serialNumber, request.ExternalReference);
        }

        // Add notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            serialNumber.AddNotes(request.Notes);
        }

        await repository.UpdateAsync(serialNumber, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var updatedSerialNumber = await readRepository.FirstOrDefaultAsync(
            new GetSerialNumberByIdSpec(request.Id),
            cancellationToken);

        return new UpdateSerialNumberResponse(updatedSerialNumber!.Adapt<SerialNumberResponse>());
    }
}
