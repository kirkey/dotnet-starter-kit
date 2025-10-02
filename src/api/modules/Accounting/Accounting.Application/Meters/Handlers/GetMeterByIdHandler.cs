using Accounting.Application.Meters.Queries;
using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Handlers;

public class GetMeterByIdHandler(IReadRepository<Meter> repository)
    : IRequestHandler<GetMeterByIdQuery, MeterResponse>
{
    public async Task<MeterResponse> Handle(GetMeterByIdQuery request, CancellationToken cancellationToken)
    {
        var meter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (meter == null) throw new MeterByIdNotFoundException(request.Id);

        return meter.Adapt<MeterResponse>();
    }
}
