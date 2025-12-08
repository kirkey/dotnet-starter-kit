namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditBureauInquiries;

public class CreditBureauInquiryViewModel
{
    public Guid MemberId { get; set; }
    public string? InquiryNumber { get; set; }
    public string? BureauName { get; set; }
    public string? Purpose { get; set; }
    public Guid? LoanId { get; set; }
    public string? RequestedBy { get; set; }
    public Guid? RequestedByUserId { get; set; }
    public decimal? InquiryCost { get; set; }
}
