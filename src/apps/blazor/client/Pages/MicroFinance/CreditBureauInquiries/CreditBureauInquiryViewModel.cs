namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditBureauInquiries;

public class CreditBureauInquiryViewModel
{
    public DefaultIdType MemberId { get; set; }
    public string? InquiryNumber { get; set; }
    public string? BureauName { get; set; }
    public string? Purpose { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public string? RequestedBy { get; set; }
    public DefaultIdType? RequestedByUserId { get; set; }
    public decimal? InquiryCost { get; set; }
}
