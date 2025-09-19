using Accounting.Application.Consumptions.Responses;
using Accounting.Application.Consumptions.Queries;

namespace Accounting.Application.Consumptions.Handlers;

public class GetConsumptionByIdHandler(IReadRepository<Consumption> repository)
    : IRequestHandler<GetConsumptionByIdQuery, ConsumptionResponse>
{
    public async Task<ConsumptionResponse> Handle(GetConsumptionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new ConsumptionNotFoundException(request.Id);

        return new ConsumptionResponse
        {
            Id = entity.Id,
            MeterId = entity.MeterId,
            ReadingDate = entity.ReadingDate,
            CurrentReading = entity.CurrentReading,
            PreviousReading = entity.PreviousReading,
            KWhUsed = entity.KWhUsed,
            BillingPeriod = entity.BillingPeriod,
            ReadingType = entity.ReadingType,
            Multiplier = entity.Multiplier,
            IsValidReading = entity.IsValidReading,
            ReadingSource = entity.ReadingSource,
            Description = entity.Description,
            Notes = entity.Notes
        };
    }
}
