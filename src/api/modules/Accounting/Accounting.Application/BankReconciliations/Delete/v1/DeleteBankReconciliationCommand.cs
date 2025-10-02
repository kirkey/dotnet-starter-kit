namespace Accounting.Application.BankReconciliations.Delete.v1;

public sealed record DeleteBankReconciliationCommand(DefaultIdType Id) : IRequest;
