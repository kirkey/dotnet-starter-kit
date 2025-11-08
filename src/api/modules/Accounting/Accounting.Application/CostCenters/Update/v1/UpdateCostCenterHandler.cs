namespace Accounting.Application.CostCenters.Update.v1;

public sealed class UpdateCostCenterHandler(IRepository<CostCenter> repository, ILogger<UpdateCostCenterHandler> logger)
    : IRequestHandler<UpdateCostCenterCommand, DefaultIdType>
{
    private readonly IRepository<CostCenter> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateCostCenterHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateCostCenterCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating cost center {Id}", request.Id);

        var costCenter = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null) throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.Update(request.Name, request.ManagerId, request.ManagerName, request.Location, 
            request.EndDate, request.Description, request.Notes);
        await _repository.UpdateAsync(costCenter, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cost center {Code} updated successfully", costCenter.Code);
        return costCenter.Id;
    }
}

