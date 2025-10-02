namespace Accounting.Application.BankReconciliations.Create.v1;

/// <summary>
/// Command to create a new bank reconciliation.
/// </summary>
public class CreateBankReconciliationCommand : BaseRequest, IRequest<DefaultIdType>
{
    public DefaultIdType BankAccountId { get; set; }
    public DateTime ReconciliationDate { get; set; }
    public decimal StatementBalance { get; set; }
    public decimal BookBalance { get; set; }
    public string? StatementNumber { get; set; }
}
