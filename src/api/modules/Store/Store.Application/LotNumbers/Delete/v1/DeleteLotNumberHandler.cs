using Store.Domain.Exceptions.LotNumber;

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Delete.v1;

/// <summary>
/// Handler for deleting a lot number.
/// </summary>
public sealed class DeleteLotNumberHandler(
    [FromKeyedServices("store:lotnumbers")] IRepository<LotNumber> repository,
    [FromKeyedServices("store:lotnumbers")] IReadRepository<LotNumber> readRepository)
    : IRequestHandler<DeleteLotNumberCommand>
{
    public async Task Handle(DeleteLotNumberCommand request, CancellationToken cancellationToken)
    {
        var lotNumber = await readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (lotNumber is null)
        {
            throw new LotNumberNotFoundException(request.Id);
        }

        await repository.DeleteAsync(lotNumber, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
