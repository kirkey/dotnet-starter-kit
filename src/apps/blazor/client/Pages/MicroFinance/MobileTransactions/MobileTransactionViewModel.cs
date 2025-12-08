namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileTransactions;

public class MobileTransactionViewModel
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public string? TransactionReference { get; set; }
    public string? TransactionType { get; set; }
    public string? Status { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal NetAmount { get; set; }
    public string? SourcePhone { get; set; }
    public string? DestinationPhone { get; set; }
    public Guid? RecipientWalletId { get; set; }
    public Guid? LinkedLoanId { get; set; }
    public Guid? LinkedSavingsAccountId { get; set; }
    public string? ProviderReference { get; set; }
    public DateTime InitiatedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public string? FailureReason { get; set; }
}
