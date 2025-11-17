namespace Accounting.Application.AccountReconciliations.Delete.v1;

/// <summary>
/// Command to delete an account reconciliation.
/// </summary>
public sealed record DeleteAccountReconciliationCommand(DefaultIdType Id) : IRequest;

