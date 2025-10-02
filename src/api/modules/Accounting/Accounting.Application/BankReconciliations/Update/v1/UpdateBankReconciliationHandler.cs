using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Update.v1;

public sealed class UpdateBankReconciliationHandler(
    ILogger<UpdateBankReconciliationHandler> logger,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<UpdateBankReconciliationCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        reconciliation.UpdateReconciliationItems(
            command.OutstandingChecksTotal,
            command.DepositsInTransitTotal,
            command.BankErrors,
            command.BookErrors);

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation updated {ReconciliationId}", reconciliation.Id);

        return reconciliation.Id;
    }
}
