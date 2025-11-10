namespace Accounting.Application.CostCenters.UpdateBudget.v1;

/// <summary>
/// Handler for updating a cost center's budget amount.
/// </summary>
public sealed class UpdateCostCenterBudgetHandler(
    [FromKeyedServices("accounting:costCenters")] IRepository<CostCenter> repository,
    ILogger<UpdateCostCenterBudgetHandler> logger)
    : IRequestHandler<UpdateCostCenterBudgetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCostCenterBudgetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        logger.LogInformation("Updating budget for cost center {Id}: {Amount}", request.Id, request.BudgetAmount);

        var costCenter = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null)
            throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.UpdateBudget(request.BudgetAmount);
        
        await repository.UpdateAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Budget updated for cost center {Code}", costCenter.Code);
        return costCenter.Id;
    }
}

