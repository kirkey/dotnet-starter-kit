namespace Accounting.Application.CostCenters.Activate.v1;

public sealed class ActivateCostCenterHandler(
    IRepository<CostCenter> repository,
    ILogger<ActivateCostCenterHandler> logger)
    : IRequestHandler<ActivateCostCenterCommand, DefaultIdType>
{
    private readonly IRepository<CostCenter> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ActivateCostCenterHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ActivateCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Activating cost center {Id}", request.Id);

        var costCenter = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null) throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.Activate();
        await _repository.UpdateAsync(costCenter, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cost center {Code} activated successfully", costCenter.Code);
        return costCenter.Id;
    }
}

