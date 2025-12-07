using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AmlAlerts;

public class AmlAlertViewModel
{
    public DefaultIdType Id { get; set; }

    [Required]
    public string AlertCode { get; set; } = string.Empty;

    [Required]
    public string AlertType { get; set; } = string.Empty;

    [Required]
    public string Severity { get; set; } = "Medium";

    [Required]
    public string TriggerRule { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public DefaultIdType? MemberId { get; set; }

    public DefaultIdType? TransactionId { get; set; }

    public decimal? TransactionAmount { get; set; }

    // Read-only properties
    public string? Status { get; set; }
    public string? MemberName { get; set; }
    public string? MemberNumber { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Resolution { get; set; }
    public bool? IsSarFiled { get; set; }
    public string? SarReferenceNumber { get; set; }
}
