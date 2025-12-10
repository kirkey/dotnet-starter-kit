namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileTransactions;

public class MobileTransactionViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType WalletId { get; set; }
    public string? TransactionReference { get; set; }
    public string? TransactionType { get; set; }
    public string? Status { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal NetAmount { get; set; }
    public string? SourcePhone { get; set; }
    public string? DestinationPhone { get; set; }
    public DefaultIdType? RecipientWalletId { get; set; }
    public DefaultIdType? LinkedLoanId { get; set; }
    public DefaultIdType? LinkedSavingsAccountId { get; set; }
    public string? ProviderReference { get; set; }
    public DateTime InitiatedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public string? FailureReason { get; set; }
}
