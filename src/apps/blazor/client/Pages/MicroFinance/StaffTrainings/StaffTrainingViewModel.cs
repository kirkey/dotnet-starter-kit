namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.StaffTrainings;

/// <summary>
/// ViewModel for Staff Training add/edit operations.
/// </summary>
public class StaffTrainingViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType StaffId { get; set; }
    public string? TrainingCode { get; set; }
    public string? TrainingName { get; set; }
    public string? Description { get; set; }
    public string? TrainingType { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? Provider { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DurationHours { get; set; }
    public decimal? Score { get; set; }
    public decimal? PassingScore { get; set; }
    public bool CertificateIssued { get; set; }
    public decimal? TrainingCost { get; set; }
    public bool IsMandatory { get; set; }
    public string? Notes { get; set; }
}
