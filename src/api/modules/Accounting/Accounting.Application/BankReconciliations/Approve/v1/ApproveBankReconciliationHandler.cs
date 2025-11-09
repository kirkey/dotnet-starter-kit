using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.BankReconciliations.Approve.v1;

public sealed class ApproveBankReconciliationHandler(
    ILogger<ApproveBankReconciliationHandler> logger,
    ICurrentUser currentUser,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<ApproveBankReconciliationCommand>
{
    public async Task Handle(ApproveBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        reconciliation.Approve(approverId, approverName);

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation {ReconciliationId} approved by user {ApproverId}", command.Id, approverId);
    }
}
