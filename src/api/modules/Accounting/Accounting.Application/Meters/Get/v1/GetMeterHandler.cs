using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Get.v1;

/// <summary>
/// Handler for retrieving a meter by ID.
/// </summary>
public sealed class GetMeterHandler(
    [FromKeyedServices("accounting:meters")] IReadRepository<Meter> repository)
    : IRequestHandler<GetMeterRequest, MeterResponse>
{
    public async Task<MeterResponse> Handle(GetMeterRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetMeterByIdSpec(request.Id);
        var meter = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (meter is null)
            throw new NotFoundException($"Meter with ID {request.Id} was not found.");

        return meter;
    }
}

