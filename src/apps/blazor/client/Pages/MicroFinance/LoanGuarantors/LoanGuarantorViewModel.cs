using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanGuarantors;

public class LoanGuarantorViewModel
{
    public DefaultIdType Id { get; set; }

    [Required]
    public DefaultIdType LoanId { get; set; }

    [Required]
    public DefaultIdType GuarantorMemberId { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public decimal GuaranteedAmount { get; set; }

    public string? Relationship { get; set; }

    public DateOnly? GuaranteeDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Notes { get; set; }

    // Read-only properties
    public string? Status { get; set; }
    public string? GuarantorName { get; set; }
    public string? MemberNumber { get; set; }
    public string? LoanNumber { get; set; }
    public decimal? LoanAmount { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
}
