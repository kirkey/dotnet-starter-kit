using FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;
using Store.Domain.Exceptions.SerialNumber;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

public class GetSerialNumberHandler(
    [FromKeyedServices("store:serialnumbers")] IReadRepository<SerialNumber> repository)
    : IRequestHandler<GetSerialNumberCommand, SerialNumberResponse>
{
    public async Task<SerialNumberResponse> Handle(GetSerialNumberCommand request, CancellationToken cancellationToken)
    {
        var serialNumber = await repository.FirstOrDefaultAsync(
            new GetSerialNumberByIdSpec(request.Id),
            cancellationToken);

        if (serialNumber is null)
        {
            throw new SerialNumberNotFoundException(request.Id);
        }

        return serialNumber.Adapt<SerialNumberResponse>();
    }
}
