namespace Accounting.Application.CostCenters.Activate.v1;

/// <summary>
/// Handler for activating a cost center.
/// </summary>
public sealed class ActivateCostCenterHandler(
    [FromKeyedServices("accounting:costCenters")] IRepository<CostCenter> repository,
    ILogger<ActivateCostCenterHandler> logger)
    : IRequestHandler<ActivateCostCenterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ActivateCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Activating cost center {Id}", request.Id);

        var costCenter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null)
            throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.Activate();
        
        await repository.UpdateAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cost center {Code} activated successfully", costCenter.Code);
        return costCenter.Id;
    }
}

