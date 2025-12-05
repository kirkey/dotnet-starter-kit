namespace Accounting.Application.TrialBalance.Reopen.v1;

/// <summary>
/// Handler for reopening a finalized trial balance report.
/// </summary>
public sealed class ReopenTrialBalanceHandler(
    [FromKeyedServices("accounting:trial-balance")] IRepository<Domain.Entities.TrialBalance> repository,
    ILogger<ReopenTrialBalanceHandler> logger)
    : IRequestHandler<ReopenTrialBalanceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ReopenTrialBalanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Reopening trial balance with ID {TrialBalanceId} - Reason: {Reason}",
            request.Id, request.Reason);

        var trialBalance = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (trialBalance == null)
        {
            logger.LogWarning("Trial balance with ID {TrialBalanceId} not found", request.Id);
            throw new NotFoundException($"Trial balance with ID {request.Id} not found");
        }

        // Use the entity's Reopen method (includes validation)
        trialBalance.Reopen(request.Reason);

        await repository.UpdateAsync(trialBalance, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Trial balance {TrialBalanceNumber} reopened successfully",
            trialBalance.TrialBalanceNumber);

        return trialBalance.Id;
    }
}
