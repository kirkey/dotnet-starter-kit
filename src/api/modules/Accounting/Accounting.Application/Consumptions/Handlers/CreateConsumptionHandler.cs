using Accounting.Application.Consumptions.Commands;
using Accounting.Application.Consumptions.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.Consumptions.Handlers;

public class CreateConsumptionHandler(
    [FromKeyedServices("accounting:consumption") ] IRepository<Consumption> repository)
    : IRequestHandler<CreateConsumptionCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var meterId = request.MeterId;
        var readingDate = request.ReadingDate;

        // Prevent duplicate consumption record for same meter/date
        var existing = await repository.FirstOrDefaultAsync(new ConsumptionByMeterDateSpec(meterId, readingDate), cancellationToken);
        if (existing != null)
            throw new ConsumptionAlreadyExistsException(meterId, readingDate);

        var entity = Consumption.Create(
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

