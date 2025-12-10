namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditBureauReports;

public class CreditBureauReportViewModel
{
    public DefaultIdType MemberId { get; set; }
    public string? ReportNumber { get; set; }
    public string? BureauName { get; set; }
    public DateTime ReportDate { get; set; } = DateTime.Today;
    public DefaultIdType? InquiryId { get; set; }
    public int? CreditScore { get; set; }
    public int? ScoreMin { get; set; }
    public int? ScoreMax { get; set; }
    public string? ScoreModel { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
