using Store.Domain.Exceptions.LotNumber;

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;

/// <summary>
/// Handler for updating a lot number.
/// </summary>
public sealed class UpdateLotNumberHandler(
    [FromKeyedServices("store:lotnumbers")] IRepository<LotNumber> repository,
    [FromKeyedServices("store:lotnumbers")] IReadRepository<LotNumber> readRepository)
    : IRequestHandler<UpdateLotNumberCommand, UpdateLotNumberResponse>
{
    public async Task<UpdateLotNumberResponse> Handle(UpdateLotNumberCommand request, CancellationToken cancellationToken)
    {
        var lotNumber = await readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (lotNumber is null)
        {
            throw new LotNumberNotFoundException(request.Id);
        }

        // Update status if provided
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            lotNumber.UpdateStatus(request.Status);
        }

        await repository.UpdateAsync(lotNumber, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateLotNumberResponse(lotNumber.Id);
    }
}
