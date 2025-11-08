namespace Accounting.Application.CostCenters.Deactivate.v1;

public sealed class DeactivateCostCenterHandler(
    IRepository<CostCenter> repository,
    ILogger<DeactivateCostCenterHandler> logger)
    : IRequestHandler<DeactivateCostCenterCommand, DefaultIdType>
{
    private readonly IRepository<CostCenter> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<DeactivateCostCenterHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(DeactivateCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Deactivating cost center {Id}", request.Id);

        var costCenter = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null) throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.Deactivate();
        await _repository.UpdateAsync(costCenter, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cost center {Code} deactivated successfully", costCenter.Code);
        return costCenter.Id;
    }
}
