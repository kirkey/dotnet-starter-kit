namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentTransactions;

public class InvestmentTransactionViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType InvestmentAccountId { get; set; }
    public DefaultIdType ProductId { get; set; }
    public string? TransactionReference { get; set; }
    public string? TransactionType { get; set; }
    public string? Status { get; set; }
    public decimal Amount { get; set; }
    public decimal? Units { get; set; }
    public decimal? NavAtTransaction { get; set; }
    public decimal? EntryLoadAmount { get; set; }
    public decimal? ExitLoadAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal? GainLoss { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.Now;
    public DateTime? ProcessedAt { get; set; }
    public DateTime? AllotmentDate { get; set; }
    public string? PaymentMode { get; set; }
    public string? PaymentReference { get; set; }
    public string? Notes { get; set; }
    public string? FailureReason { get; set; }
}
