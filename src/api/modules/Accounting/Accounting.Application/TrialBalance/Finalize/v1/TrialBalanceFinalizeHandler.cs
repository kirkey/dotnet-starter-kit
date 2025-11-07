namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Handler for finalizing a trial balance report.
/// Validates balance and accounting equation before finalizing.
/// </summary>
public sealed class TrialBalanceFinalizeHandler(
    IRepository<Domain.Entities.TrialBalance> repository,
    ILogger<TrialBalanceFinalizeHandler> logger)
    : IRequestHandler<TrialBalanceFinalizeCommand, DefaultIdType>
{
    private readonly IRepository<Domain.Entities.TrialBalance> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<TrialBalanceFinalizeHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(TrialBalanceFinalizeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Finalizing trial balance with ID {TrialBalanceId}", request.Id);

        var trialBalance = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (trialBalance == null)
        {
            _logger.LogWarning("Trial balance with ID {TrialBalanceId} not found", request.Id);
            throw new NotFoundException($"Trial balance with ID {request.Id} not found");
        }

        // Use the entity's Finalize method (includes validation)
        trialBalance.Finalize(request.FinalizedBy);

        await _repository.UpdateAsync(trialBalance, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Trial balance {TrialBalanceNumber} finalized successfully by {FinalizedBy}",
            trialBalance.TrialBalanceNumber, request.FinalizedBy);

        return trialBalance.Id;
    }
}

