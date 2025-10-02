namespace Accounting.Application.BankReconciliations.Complete.v1;

public sealed class CompleteBankReconciliationHandler(
    ILogger<CompleteBankReconciliationHandler> logger,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<CompleteBankReconciliationCommand>
{
    public async Task Handle(CompleteBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        reconciliation.Complete(command.ReconciledBy);

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation completed {ReconciliationId}", command.Id);
    }
}
