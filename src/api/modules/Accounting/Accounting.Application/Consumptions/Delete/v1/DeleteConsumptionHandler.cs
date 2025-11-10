namespace Accounting.Application.Consumptions.Delete.v1;

/// <summary>
/// Handler for deleting a consumption record.
/// </summary>
public sealed class DeleteConsumptionHandler(
    [FromKeyedServices("accounting:consumptions")] IRepository<Consumption> repository,
    ILogger<DeleteConsumptionHandler> logger)
    : IRequestHandler<DeleteConsumptionCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteConsumptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deleting consumption record {Id}", request.Id);

        var consumption = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (consumption == null)
            throw new NotFoundException($"Consumption record with ID {request.Id} not found");

        await repository.DeleteAsync(consumption, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Consumption record {Id} deleted successfully", request.Id);
        return request.Id;
    }
}

