using Accounting.Application.ConsumptionData.Commands;

namespace Accounting.Application.ConsumptionData.Handlers;

public class UpdateConsumptionDataHandler(
    [FromKeyedServices("accounting:consumption")] IRepository<Accounting.Domain.ConsumptionData> repository)
    : IRequestHandler<UpdateConsumptionDataCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateConsumptionDataCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new ConsumptionDataNotFoundException(request.Id);

        // Domain Update performs trimming, length enforcement and validation
        entity.Update(request.CurrentReading, request.PreviousReading, request.ReadingType, request.Multiplier, request.ReadingSource, request.Description, request.Notes);

        await repository.UpdateAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
