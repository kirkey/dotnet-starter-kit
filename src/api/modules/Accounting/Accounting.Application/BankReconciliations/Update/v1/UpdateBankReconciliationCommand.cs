namespace Accounting.Application.BankReconciliations.Update.v1;

/// <summary>
/// Command to update reconciliation items.
/// </summary>
public class UpdateBankReconciliationCommand : BaseRequest, IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public decimal OutstandingChecksTotal { get; set; }
    public decimal DepositsInTransitTotal { get; set; }
    public decimal BankErrors { get; set; }
    public decimal BookErrors { get; set; }
}
