using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceProducts;

/// <summary>
/// ViewModel for creating/editing insurance products.
/// </summary>
public class InsuranceProductViewModel
{
    public DefaultIdType Id { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [MaxLength(50)]
    public string? Code { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Insurance type is required")]
    public string? InsuranceType { get; set; }

    public string? Provider { get; set; }

    public string? Description { get; set; }

    public decimal MinCoverage { get; set; }

    public decimal MaxCoverage { get; set; }

    public string? PremiumCalculation { get; set; }

    public decimal BasePremiumRate { get; set; }

    public int MinTermMonths { get; set; }

    public int MaxTermMonths { get; set; }

    public decimal ClaimDeductible { get; set; }

    public int WaitingPeriodDays { get; set; }

    public string? Status { get; set; }
}
