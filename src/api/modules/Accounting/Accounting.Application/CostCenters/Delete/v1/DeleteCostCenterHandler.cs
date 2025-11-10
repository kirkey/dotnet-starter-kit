namespace Accounting.Application.CostCenters.Delete.v1;

/// <summary>
/// Handler for deleting a cost center.
/// Only inactive cost centers with no transactions or children can be deleted.
/// </summary>
public sealed class DeleteCostCenterHandler(
    [FromKeyedServices("accounting:costCenters")] IRepository<CostCenter> repository,
    ILogger<DeleteCostCenterHandler> logger)
    : IRequestHandler<DeleteCostCenterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Deleting cost center {Id}", request.Id);

        var costCenter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null)
            throw new NotFoundException($"Cost center with ID {request.Id} not found");

        if (costCenter.IsActive)
            throw new InvalidOperationException("Cannot delete an active cost center. Deactivate it first.");

        if (costCenter.ActualAmount != 0)
            throw new InvalidOperationException("Cannot delete cost center with recorded transactions.");

        // Check for child cost centers
        var spec = new Specification<CostCenter>();
        spec.Query.Where(c => c.ParentCostCenterId == request.Id);
        var children = await repository.ListAsync(spec, cancellationToken);
        
        if (children.Count > 0)
            throw new InvalidOperationException("Cannot delete cost center with child cost centers.");

        await repository.DeleteAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cost center {Code} deleted successfully", costCenter.Code);
        return request.Id;
    }
}

