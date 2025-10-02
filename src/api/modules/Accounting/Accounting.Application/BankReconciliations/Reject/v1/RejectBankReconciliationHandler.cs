namespace Accounting.Application.BankReconciliations.Reject.v1;

public sealed class RejectBankReconciliationHandler(
    ILogger<RejectBankReconciliationHandler> logger,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<RejectBankReconciliationCommand>
{
    public async Task Handle(RejectBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        reconciliation.Reject(command.RejectedBy, command.Reason);

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation rejected {ReconciliationId}", command.Id);
    }
}
