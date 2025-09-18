using Accounting.Application.Consumptions.Commands;

namespace Accounting.Application.Consumptions.Handlers;

public class DeleteConsumptionDataHandler(
    IRepository<ConsumptionData> repository)
    : IRequestHandler<DeleteConsumptionDataCommand>
{
    public async Task Handle(DeleteConsumptionDataCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new ConsumptionDataNotFoundException(request.Id);

        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
