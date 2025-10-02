using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Delete.v1;

public sealed class DeleteBankReconciliationHandler(
    ILogger<DeleteBankReconciliationHandler> logger,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<DeleteBankReconciliationCommand>
{
    public async Task Handle(DeleteBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        if (reconciliation.IsReconciled)
            throw new BankReconciliationCannotBeModifiedException(command.Id);

        await repository.DeleteAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation deleted {ReconciliationId}", command.Id);
    }
}
