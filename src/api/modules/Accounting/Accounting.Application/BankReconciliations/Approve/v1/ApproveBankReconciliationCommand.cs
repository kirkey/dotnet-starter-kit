namespace Accounting.Application.BankReconciliations.Approve.v1;

public sealed class ApproveBankReconciliationCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
