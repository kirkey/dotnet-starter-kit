using Accounting.Application.ConsumptionData.Commands;

namespace Accounting.Application.ConsumptionData.Handlers;

public class DeleteConsumptionDataHandler(
    IRepository<Accounting.Domain.ConsumptionData> repository)
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
