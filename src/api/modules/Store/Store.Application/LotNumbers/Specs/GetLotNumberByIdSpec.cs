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
    }
}
