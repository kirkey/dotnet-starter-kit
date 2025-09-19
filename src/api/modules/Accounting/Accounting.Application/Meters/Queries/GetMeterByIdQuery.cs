using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Queries;

public class GetMeterByIdQuery(DefaultIdType id) : IRequest<MeterResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
