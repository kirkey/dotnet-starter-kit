using Accounting.Application.Consumptions.Commands;

namespace Accounting.Application.Consumptions.Handlers;

public class UpdateConsumptionHandler(
    [FromKeyedServices("accounting:consumption")] IRepository<Consumption> repository)
    : IRequestHandler<UpdateConsumptionCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = entity ?? throw new ConsumptionNotFoundException(request.Id);

        // Domain Update performs trimming, length enforcement and validation
        entity.Update(request.CurrentReading, request.PreviousReading, request.ReadingType, request.Multiplier, request.ReadingSource, request.Description, request.Notes);

        await repository.UpdateAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
