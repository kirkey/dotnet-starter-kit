using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Get.v1;

/// <summary>
/// Specification to get a consumption record by ID with projection to response.
/// </summary>
public sealed class GetConsumptionByIdSpec : Specification<Consumption, ConsumptionResponse>, ISingleResultSpecification<Consumption>
{
    public GetConsumptionByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

