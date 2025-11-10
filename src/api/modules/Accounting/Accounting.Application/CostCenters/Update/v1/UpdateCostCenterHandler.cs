namespace Accounting.Application.CostCenters.Update.v1;

/// <summary>
/// Handler for updating a cost center.
/// </summary>
public sealed class UpdateCostCenterHandler(
    [FromKeyedServices("accounting:costCenters")] IRepository<CostCenter> repository,
    ILogger<UpdateCostCenterHandler> logger)
    : IRequestHandler<UpdateCostCenterCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating cost center {Id}", request.Id);

        var costCenter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null)
            throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.Update(request.Name, request.ManagerId, request.ManagerName, request.Location, 
            request.EndDate, request.Description, request.Notes);
            
        await repository.UpdateAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cost center {Code} updated successfully", costCenter.Code);
        return costCenter.Id;
    }
}

