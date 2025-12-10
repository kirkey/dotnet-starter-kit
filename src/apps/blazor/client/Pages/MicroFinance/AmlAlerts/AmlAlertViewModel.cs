namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AmlAlerts;

public class AmlAlertViewModel
{
    public DefaultIdType Id { get; set; }
    public string? AlertCode { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? TransactionId { get; set; }
    public string? AlertType { get; set; }
    public string? Severity { get; set; }
    public string? Status { get; set; }
    public string? TriggerRule { get; set; }
    public string? Description { get; set; }
    public decimal? TransactionAmount { get; set; }
    public DateTime AlertedAt { get; set; } = DateTime.Now;
    public DateTime? InvestigationStartedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DefaultIdType? AssignedToId { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? SarReference { get; set; }
    public DateTime? SarFiledDate { get; set; }
    public bool RequiresReporting { get; set; }
}
