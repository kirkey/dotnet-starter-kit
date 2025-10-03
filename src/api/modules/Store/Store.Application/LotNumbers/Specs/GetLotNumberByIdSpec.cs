using FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Specs;

/// <summary>
/// Specification to get a lot number by ID with response mapping.
/// </summary>
public sealed class GetLotNumberByIdSpec : Specification<LotNumber, LotNumberResponse>
{
    public GetLotNumberByIdSpec(DefaultIdType id)
    {
        Query.Where(l => l.Id == id);

        Query.Select(l => new LotNumberResponse(
            l.Id,
            l.LotCode,
            l.ItemId,
            l.SupplierId,
            l.ManufactureDate,
            l.ExpirationDate,
            l.ReceiptDate,
            l.QuantityReceived,
            l.QuantityRemaining,
            l.Status,
            l.QualityNotes,
            l.CreatedOn,
            l.CreatedBy));
    }
}
