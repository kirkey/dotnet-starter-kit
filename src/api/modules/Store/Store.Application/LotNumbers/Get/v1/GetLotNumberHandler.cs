using Store.Domain.Exceptions.LotNumber;

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;

/// <summary>
/// Handler for getting a lot number by ID.
/// </summary>
public sealed class GetLotNumberHandler(
    [FromKeyedServices("store:lotnumbers")] IReadRepository<LotNumber> repository)
    : IRequestHandler<GetLotNumberCommand, LotNumberResponse>
{
    public async Task<LotNumberResponse> Handle(GetLotNumberCommand request, CancellationToken cancellationToken)
    {
        var lotNumber = await repository.FirstOrDefaultAsync(
            new Specs.GetLotNumberByIdSpec(request.Id),
            cancellationToken);

        if (lotNumber is null)
        {
            throw new LotNumberNotFoundException(request.Id);
        }

        return lotNumber;
    }
}
