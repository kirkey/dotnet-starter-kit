using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Get.v1;

/// <summary>
/// Handler for retrieving a consumption record by ID.
/// </summary>
public sealed class GetConsumptionHandler(
    [FromKeyedServices("accounting:consumptions")] IReadRepository<Consumption> repository)
    : IRequestHandler<GetConsumptionRequest, ConsumptionResponse>
{
    public async Task<ConsumptionResponse> Handle(GetConsumptionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetConsumptionByIdSpec(request.Id);
        var consumption = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (consumption is null)
            throw new NotFoundException($"Consumption record with ID {request.Id} was not found.");

        return consumption;
    }
}

