namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralInsurances;

public class CollateralInsuranceViewModel
{
    public DefaultIdType CollateralId { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string InsurerName { get; set; } = string.Empty;
    public string InsuranceType { get; set; } = string.Empty;
    public decimal CoverageAmount { get; set; }
    public decimal PremiumAmount { get; set; }
    public decimal Deductible { get; set; }
    public DateTimeOffset EffectiveDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset ExpiryDate { get; set; }
    public string? InsurerContact { get; set; }
    public string? InsurerPhone { get; set; }
    public string? InsurerEmail { get; set; }
    public bool IsMfiAsBeneficiary { get; set; } = true;
    public string? BeneficiaryName { get; set; }
    public int RenewalReminderDays { get; set; } = 30;
    public bool AutoRenewal { get; set; }
}
