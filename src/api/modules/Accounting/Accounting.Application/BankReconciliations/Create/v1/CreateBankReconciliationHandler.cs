namespace Accounting.Application.BankReconciliations.Create.v1;

public sealed class CreateBankReconciliationHandler(
    ILogger<CreateBankReconciliationHandler> logger,
    [FromKeyedServices("accounting:bank-reconciliations")] IRepository<BankReconciliation> repository)
    : IRequestHandler<CreateBankReconciliationCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateBankReconciliationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var reconciliation = BankReconciliation.Create(
            bankAccountId: command.BankAccountId,
            reconciliationDate: command.ReconciliationDate,
            statementBalance: command.StatementBalance,
            bookBalance: command.BookBalance,
            statementNumber: command.StatementNumber,
            description: command.Description,
            notes: command.Notes);

        await repository.AddAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bank reconciliation created {ReconciliationId}", reconciliation.Id);

        return reconciliation.Id;
    }
}
