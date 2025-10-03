using FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;
using Store.Domain.Exceptions.SerialNumber;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Delete.v1;

public class DeleteSerialNumberHandler(
    [FromKeyedServices("store:serialnumbers")] IRepository<SerialNumber> repository,
    [FromKeyedServices("store:serialnumbers")] IReadRepository<SerialNumber> readRepository)
    : IRequestHandler<DeleteSerialNumberCommand, DeleteSerialNumberResponse>
{
    public async Task<DeleteSerialNumberResponse> Handle(DeleteSerialNumberCommand request, CancellationToken cancellationToken)
    {
        var serialNumber = await readRepository.FirstOrDefaultAsync(
            new GetSerialNumberByIdSpec(request.Id),
            cancellationToken);

        if (serialNumber is null)
        {
            throw new SerialNumberNotFoundException(request.Id);
        }

        await repository.DeleteAsync(serialNumber, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new DeleteSerialNumberResponse(serialNumber.Id, serialNumber.SerialNumberValue);
    }
}
