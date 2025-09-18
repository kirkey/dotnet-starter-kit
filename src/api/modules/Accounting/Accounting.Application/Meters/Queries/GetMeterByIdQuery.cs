using Accounting.Application.Meters.Dtos;

namespace Accounting.Application.Meters.Queries;

public class GetMeterByIdQuery : IRequest<MeterDto>
{
    public DefaultIdType Id { get; set; }
}