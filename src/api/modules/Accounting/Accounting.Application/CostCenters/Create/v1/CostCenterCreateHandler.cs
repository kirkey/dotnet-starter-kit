using Accounting.Application.CostCenters.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.CostCenters.Create.v1;

/// <summary>
/// Handler for creating a new cost center.
/// </summary>
public sealed class CostCenterCreateHandler(
    ILogger<CostCenterCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<CostCenter> repository)
    : IRequestHandler<CostCenterCreateCommand, CostCenterCreateResponse>
{
    public async Task<CostCenterCreateResponse> Handle(CostCenterCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate code
        var existingByCode = await repository.FirstOrDefaultAsync(
            new CostCenterByCodeSpec(request.Code), cancellationToken);
        if (existingByCode != null)
        {
            throw new CostCenterCodeAlreadyExistsException(request.Code);
        }

        // Parse the cost center type enum
        if (!Enum.TryParse<CostCenterType>(request.CostCenterType, out var costCenterType))
        {
            throw new InvalidCostCenterTypeException(request.CostCenterType);
        }

        var costCenter = CostCenter.Create(
            code: request.Code,
            name: request.Name,
            costCenterType: costCenterType,
            parentCostCenterId: request.ParentCostCenterId,
            managerId: request.ManagerId,
            managerName: request.ManagerName,
            budgetAmount: request.BudgetAmount,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(costCenter, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cost center created {CostCenterId} - {Code}", costCenter.Id, costCenter.Code);
        return new CostCenterCreateResponse(costCenter.Id);
    }
}

