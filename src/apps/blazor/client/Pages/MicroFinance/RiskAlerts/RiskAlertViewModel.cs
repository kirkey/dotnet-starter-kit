namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.RiskAlerts;

public class RiskAlertViewModel
{
    public Guid Id { get; set; }
    public string? AlertNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? RiskCategoryId { get; set; }
    public Guid? RiskIndicatorId { get; set; }
    public string? Severity { get; set; }
    public string? Source { get; set; }
    public string? Status { get; set; }
    public decimal? ThresholdValue { get; set; }
    public decimal? ActualValue { get; set; }
    public decimal? Variance { get; set; }
    public DateTime AlertedAt { get; set; } = DateTime.Now;
    public Guid? AssignedToUserId { get; set; }
    public bool IsEscalated { get; set; }
    public int EscalationLevel { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Resolution { get; set; }
}
