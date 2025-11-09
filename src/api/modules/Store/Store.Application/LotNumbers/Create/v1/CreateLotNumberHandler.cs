using FSH.Starter.WebApi.Store.Application.LotNumbers.Specs;
using Store.Domain.Exceptions.LotNumber;

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Create.v1;

/// <summary>
/// Handler for creating a lot number.
/// </summary>
public sealed class CreateLotNumberHandler(
    [FromKeyedServices("store:lotnumbers")] IRepository<LotNumber> repository,
    [FromKeyedServices("store:lotnumbers")] IReadRepository<LotNumber> readRepository)
    : IRequestHandler<CreateLotNumberCommand, CreateLotNumberResponse>
{
    public async Task<CreateLotNumberResponse> Handle(CreateLotNumberCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate lot number for this item
        var existing = await readRepository.FirstOrDefaultAsync(
            new LotNumberByCodeAndItemSpec(request.LotCode, request.ItemId),
            cancellationToken);

        if (existing is not null)
        {
            throw new DuplicateLotNumberException(request.LotCode, request.ItemId);
        }

        var lotNumber = LotNumber.Create(
            request.LotCode,
            request.ItemId,
            request.QuantityReceived,
            request.SupplierId,
            request.ManufactureDate,
            request.ExpirationDate,
            request.ReceiptDate,
            request.QualityNotes);

        await repository.AddAsync(lotNumber, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateLotNumberResponse(lotNumber.Id);
    }
}
