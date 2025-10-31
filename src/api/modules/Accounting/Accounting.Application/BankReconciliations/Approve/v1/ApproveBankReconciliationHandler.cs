namespace Accounting.Application.BankReconciliations.Approve.v1;

public sealed class ApproveBankReconciliationHandler(
    ILogger<ApproveBankReconciliationHandler> logger,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<ApproveBankReconciliationCommand>
{
    public async Task Handle(ApproveBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        reconciliation.Approve(command.ApprovedBy);

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation approved {ReconciliationId}", command.Id);
    }
}
