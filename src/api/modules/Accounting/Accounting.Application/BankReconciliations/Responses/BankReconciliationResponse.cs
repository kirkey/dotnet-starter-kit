namespace Accounting.Application.BankReconciliations.Responses;

public class BankReconciliationResponse
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType BankAccountId { get; set; }
    public DateTime ReconciliationDate { get; set; }
    public decimal StatementBalance { get; set; }
    public decimal BookBalance { get; set; }
    public decimal AdjustedBalance { get; set; }
    public decimal OutstandingChecksTotal { get; set; }
    public decimal DepositsInTransitTotal { get; set; }
    public decimal BankErrors { get; set; }
    public decimal BookErrors { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsReconciled { get; set; }
    public DateTime? ReconciledDate { get; set; }
    public string? ReconciledBy { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? StatementNumber { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
