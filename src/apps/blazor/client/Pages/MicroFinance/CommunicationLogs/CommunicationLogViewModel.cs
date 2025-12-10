namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CommunicationLogs;

public class CommunicationLogViewModel
{
    public string? Channel { get; set; }
    public string? Recipient { get; set; }
    public string? Body { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? TemplateId { get; set; }
    public string? Subject { get; set; }
    public DefaultIdType? SentByUserId { get; set; }
}
