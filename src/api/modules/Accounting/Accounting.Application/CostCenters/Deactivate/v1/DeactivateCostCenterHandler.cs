namespace Accounting.Application.CostCenters.Deactivate.v1;

/// <summary>
/// Handler for deactivating a cost center.
/// </summary>
public sealed class DeactivateCostCenterHandler(
    [FromKeyedServices("accounting:costCenters")] IRepository<CostCenter> repository,
    ILogger<DeactivateCostCenterHandler> logger)
    : IRequestHandler<DeactivateCostCenterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeactivateCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deactivating cost center {Id}", request.Id);

        var costCenter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null)
            throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.Deactivate();
        
        await repository.UpdateAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cost center {Code} deactivated successfully", costCenter.Code);
        return costCenter.Id;
    }
}
