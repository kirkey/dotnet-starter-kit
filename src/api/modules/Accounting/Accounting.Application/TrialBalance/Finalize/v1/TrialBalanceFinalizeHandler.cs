using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Handler for finalizing a trial balance report.
/// Validates balance and accounting equation before finalizing.
/// </summary>
public sealed class TrialBalanceFinalizeHandler(
    [FromKeyedServices("accounting:trial-balance")] IRepository<Domain.Entities.TrialBalance> repository,
    ICurrentUser currentUser,
    ILogger<TrialBalanceFinalizeHandler> logger)
    : IRequestHandler<TrialBalanceFinalizeCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(TrialBalanceFinalizeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Finalizing trial balance with ID {TrialBalanceId}", request.Id);

        var trialBalance = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (trialBalance == null)
        {
            logger.LogWarning("Trial balance with ID {TrialBalanceId} not found", request.Id);
            throw new NotFoundException($"Trial balance with ID {request.Id} not found");
        }

        var finalizerName = currentUser.GetUserName() ?? "Unknown";

        // Use the entity's Finalize method (includes validation)
        trialBalance.Finalize(finalizerName);

        await repository.UpdateAsync(trialBalance, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Trial balance {TrialBalanceNumber} finalized successfully by {FinalizedBy}",
            trialBalance.TrialBalanceNumber, finalizerName);

        return trialBalance.Id;
    }
}

