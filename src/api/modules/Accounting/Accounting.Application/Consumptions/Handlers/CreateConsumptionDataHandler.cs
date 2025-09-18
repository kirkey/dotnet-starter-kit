using Accounting.Application.Consumptions.Commands;
using Accounting.Application.Consumptions.Queries;

namespace Accounting.Application.Consumptions.Handlers;

public class CreateConsumptionDataHandler(
    [FromKeyedServices("accounting:consumption") ] IRepository<ConsumptionData> repository)
    : IRequestHandler<CreateConsumptionDataCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateConsumptionDataCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var meterId = request.MeterId;
        var readingDate = request.ReadingDate;

        // Prevent duplicate consumption record for same meter/date
        var existing = await repository.FirstOrDefaultAsync(new ConsumptionDataByMeterDateSpec(meterId, readingDate), cancellationToken);
        if (existing != null)
            throw new ConsumptionDataAlreadyExistsException(meterId, readingDate);

        var entity = ConsumptionData.Create(
            meterId,
            readingDate,
            request.CurrentReading,
            request.PreviousReading,
            request.BillingPeriod,
            request.ReadingType,
            request.Multiplier,
            request.ReadingSource,
            request.Description,
            request.Notes);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

