namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.DebtSettlements;

public class DebtSettlementViewModel
{
    public Guid Id { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid CollectionCaseId { get; set; }
    public Guid LoanId { get; set; }
    public Guid MemberId { get; set; }
    public string? SettlementType { get; set; }
    public string? Status { get; set; }
    public decimal OriginalOutstanding { get; set; }
    public decimal SettlementAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingBalance { get; set; }
    public int? NumberOfInstallments { get; set; }
    public decimal? InstallmentAmount { get; set; }
    public DateTime ProposedDate { get; set; } = DateTime.Today;
    public DateTime? ApprovedDate { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Today.AddMonths(1);
    public DateTime? CompletedDate { get; set; }
    public string? Terms { get; set; }
    public string? Justification { get; set; }
}
