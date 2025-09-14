using Accounting.Application.ConsumptionData.Queries;
using Accounting.Application.ConsumptionData.Dtos;

namespace Accounting.Application.ConsumptionData.Handlers;

public class GetConsumptionDataByIdHandler(IReadRepository<Accounting.Domain.ConsumptionData> repository)
    : IRequestHandler<GetConsumptionDataByIdQuery, ConsumptionDataDto>
{
    public async Task<ConsumptionDataDto> Handle(GetConsumptionDataByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new ConsumptionDataNotFoundException(request.Id);

        return new ConsumptionDataDto
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

