namespace Accounting.Application.CostCenters.UpdateBudget.v1;

/// <summary>
/// Handler for updating a cost center's budget amount.
/// </summary>
public sealed class UpdateCostCenterBudgetHandler(IRepository<CostCenter> repository, ILogger<UpdateCostCenterBudgetHandler> logger)
    : IRequestHandler<UpdateCostCenterBudgetCommand, DefaultIdType>
{
    private readonly IRepository<CostCenter> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateCostCenterBudgetHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateCostCenterBudgetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating budget for cost center {Id}: {Amount}", request.Id, request.BudgetAmount);

        var costCenter = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null) throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.UpdateBudget(request.BudgetAmount);
        await _repository.UpdateAsync(costCenter, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Budget updated for cost center {Code}", costCenter.Code);
        return costCenter.Id;
    }
}

