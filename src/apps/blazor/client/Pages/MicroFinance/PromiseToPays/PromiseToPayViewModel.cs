namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.PromiseToPays;

public class PromiseToPayViewModel
{
    public Guid Id { get; set; }
    public Guid CollectionCaseId { get; set; }
    public Guid LoanId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime PromiseDate { get; set; } = DateTime.Today;
    public DateTime PromisedPaymentDate { get; set; } = DateTime.Today.AddDays(7);
    public decimal PromisedAmount { get; set; }
    public decimal ActualAmountPaid { get; set; }
    public DateTime? ActualPaymentDate { get; set; }
    public string? Status { get; set; }
    public int RescheduleCount { get; set; }
    public string? BreachReason { get; set; }
    public string? PaymentMethod { get; set; }
}
