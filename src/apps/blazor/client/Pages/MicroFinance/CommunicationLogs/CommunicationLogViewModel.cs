namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CommunicationLogs;

public class CommunicationLogViewModel
{
    public string? Channel { get; set; }
    public string? Recipient { get; set; }
    public string? Body { get; set; }
    public Guid? MemberId { get; set; }
    public Guid? LoanId { get; set; }
    public Guid? TemplateId { get; set; }
    public string? Subject { get; set; }
    public Guid? SentByUserId { get; set; }
}
