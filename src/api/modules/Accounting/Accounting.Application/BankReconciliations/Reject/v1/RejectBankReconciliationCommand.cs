namespace Accounting.Application.BankReconciliations.Reject.v1;

public sealed class RejectBankReconciliationCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public string RejectedBy { get; set; } = string.Empty;
    public string? Reason { get; set; }
}
