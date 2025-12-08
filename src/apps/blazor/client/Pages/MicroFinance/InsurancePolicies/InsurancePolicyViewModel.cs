using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsurancePolicies;

/// <summary>
/// ViewModel for creating/editing insurance policies.
/// </summary>
public class InsurancePolicyViewModel
{
    public DefaultIdType Id { get; set; }

    [Required(ErrorMessage = "Policy number is required")]
    [MaxLength(50)]
    public string? PolicyNumber { get; set; }

    [Required(ErrorMessage = "Customer ID is required")]
    public DefaultIdType CustomerId { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    public DefaultIdType ProductId { get; set; }

    [Required(ErrorMessage = "Coverage amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Coverage amount must be greater than 0")]
    public decimal CoverageAmount { get; set; }

    public decimal PremiumAmount { get; set; }

    public string? PaymentFrequency { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? BeneficiaryName { get; set; }

    public string? BeneficiaryRelationship { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }
}
