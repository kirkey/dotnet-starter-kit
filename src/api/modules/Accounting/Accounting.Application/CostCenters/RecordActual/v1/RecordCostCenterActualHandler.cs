namespace Accounting.Application.CostCenters.RecordActual.v1;

/// <summary>
/// Handler for recording actual spending for a cost center.
/// </summary>
public sealed class RecordCostCenterActualHandler(
    [FromKeyedServices("accounting:costCenters")] IRepository<CostCenter> repository,
    ILogger<RecordCostCenterActualHandler> logger)
    : IRequestHandler<RecordCostCenterActualCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RecordCostCenterActualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Recording actual amount for cost center {Id}: {Amount}", request.Id, request.Amount);

        var costCenter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null)
            throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.RecordActual(request.Amount);
        
        await repository.UpdateAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Actual recorded for cost center {Code}: Total={ActualAmount}", 
            costCenter.Code, costCenter.ActualAmount);
        return costCenter.Id;
    }
}

