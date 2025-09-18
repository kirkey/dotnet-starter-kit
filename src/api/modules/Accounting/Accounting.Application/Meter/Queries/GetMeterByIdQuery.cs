using Accounting.Application.Meter.Dtos;

namespace Accounting.Application.Meter.Queries
{
    public class GetMeterByIdQuery : IRequest<MeterDto>
    {
        public DefaultIdType Id { get; set; }
    }
}

