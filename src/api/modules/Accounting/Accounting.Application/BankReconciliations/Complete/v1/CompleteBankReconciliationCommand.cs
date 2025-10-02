namespace Accounting.Application.BankReconciliations.Complete.v1;

public sealed class CompleteBankReconciliationCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public string ReconciledBy { get; set; } = string.Empty;
}
