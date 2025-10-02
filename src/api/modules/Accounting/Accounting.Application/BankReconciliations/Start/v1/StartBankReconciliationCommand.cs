namespace Accounting.Application.BankReconciliations.Start.v1;

public sealed record StartBankReconciliationCommand(DefaultIdType Id) : IRequest;
