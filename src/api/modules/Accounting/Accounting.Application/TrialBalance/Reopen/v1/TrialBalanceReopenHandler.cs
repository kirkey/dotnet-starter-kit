namespace Accounting.Application.TrialBalance.Reopen.v1;

/// <summary>
/// Handler for reopening a finalized trial balance report.
/// </summary>
public sealed class TrialBalanceReopenHandler(
    IRepository<Domain.Entities.TrialBalance> repository,
    ILogger<TrialBalanceReopenHandler> logger)
    : IRequestHandler<TrialBalanceReopenCommand, DefaultIdType>
{
    private readonly IRepository<Domain.Entities.TrialBalance> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<TrialBalanceReopenHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(TrialBalanceReopenCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Reopening trial balance with ID {TrialBalanceId} - Reason: {Reason}",
            request.Id, request.Reason);

        var trialBalance = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (trialBalance == null)
        {
            _logger.LogWarning("Trial balance with ID {TrialBalanceId} not found", request.Id);
            throw new NotFoundException($"Trial balance with ID {request.Id} not found");
        }

        // Use the entity's Reopen method (includes validation)
        trialBalance.Reopen(request.Reason);

        await _repository.UpdateAsync(trialBalance, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Trial balance {TrialBalanceNumber} reopened successfully",
            trialBalance.TrialBalanceNumber);

        return trialBalance.Id;
    }
}
