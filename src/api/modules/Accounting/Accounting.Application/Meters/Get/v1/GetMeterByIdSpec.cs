using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Get.v1;

/// <summary>
/// Specification to get a meter by ID with projection to response.
/// </summary>
public sealed class GetMeterByIdSpec : Specification<Meter, MeterResponse>, ISingleResultSpecification<Meter>
{
    public GetMeterByIdSpec(DefaultIdType id)
    {
        Query.Where(m => m.Id == id);
    }
}

