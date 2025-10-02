using Accounting.Domain.Entities;

namespace Accounting.Application.BankReconciliations.Start.v1;

public sealed class StartBankReconciliationHandler(
    ILogger<StartBankReconciliationHandler> logger,
    IRepository<BankReconciliation> repository)
    : IRequestHandler<StartBankReconciliationCommand>
{
    public async Task Handle(StartBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (reconciliation == null)
            throw new BankReconciliationNotFoundException(command.Id);

        reconciliation.StartReconciliation();

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation started {ReconciliationId}", command.Id);
    }
}
