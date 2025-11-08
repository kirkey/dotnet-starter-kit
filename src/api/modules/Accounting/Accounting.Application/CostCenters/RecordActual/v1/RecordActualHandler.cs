namespace Accounting.Application.CostCenters.RecordActual.v1;

public sealed class RecordActualHandler(IRepository<CostCenter> repository, ILogger<RecordActualHandler> logger)
    : IRequestHandler<RecordActualCommand, DefaultIdType>
{
    private readonly IRepository<CostCenter> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordActualHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordActualCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording actual amount for cost center {Id}: {Amount}", request.Id, request.Amount);

        var costCenter = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (costCenter == null) throw new NotFoundException($"Cost center with ID {request.Id} not found");

        costCenter.RecordActual(request.Amount);
        await _repository.UpdateAsync(costCenter, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Actual recorded for cost center {Code}: Total={ActualAmount}", 
            costCenter.Code, costCenter.ActualAmount);
        return costCenter.Id;
    }
}

