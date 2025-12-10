namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRestructures;

/// <summary>
/// View model for loan restructure creation.
/// </summary>
public class LoanRestructureViewModel
{
    public DefaultIdType LoanId { get; set; }
    public string RestructureType { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? EffectiveDate { get; set; }
    public decimal NewPrincipal { get; set; }
    public decimal NewInterestRate { get; set; }
    public int NewTerm { get; set; }
    public decimal NewInstallmentAmount { get; set; }
    public int GracePeriodMonths { get; set; }
    public decimal WaivedAmount { get; set; }
    public decimal RestructureFee { get; set; }
}
