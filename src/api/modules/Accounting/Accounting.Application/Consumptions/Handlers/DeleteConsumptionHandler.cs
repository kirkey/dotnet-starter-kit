using Accounting.Application.Consumptions.Commands;

namespace Accounting.Application.Consumptions.Handlers;

public class DeleteConsumptionHandler(
    IRepository<Consumption> repository)
    : IRequestHandler<DeleteConsumptionCommand>
{
    public async Task Handle(DeleteConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = entity ?? throw new ConsumptionNotFoundException(request.Id);

        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
