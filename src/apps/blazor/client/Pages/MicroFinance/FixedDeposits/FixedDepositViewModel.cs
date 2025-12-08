namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FixedDeposits;

/// <summary>
/// ViewModel used by the FixedDeposits page for add/edit operations.
/// Mirrors the shape of the API's CreateFixedDepositCommand and UpdateFixedDepositCommand.
/// </summary>
public class FixedDepositViewModel
{
    /// <summary>
    /// Primary identifier of the fixed deposit.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique certificate number. Example: "FD-0001".
    /// </summary>
    public string? CertificateNumber { get; set; }

    /// <summary>
    /// Member who owns this fixed deposit.
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Principal amount deposited.
    /// </summary>
    public decimal PrincipalAmount { get; set; }

    /// <summary>
    /// Annual interest rate (e.g., 5.5 for 5.5%).
    /// </summary>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Term length in months.
    /// </summary>
    public int TermMonths { get; set; }

    /// <summary>
    /// Date when the deposit was made.
    /// </summary>
    public DateOnly? DepositDate { get; set; }

    /// <summary>
    /// DateTime wrapper for DepositDate to work with MudDatePicker.
    /// </summary>
    public DateTime? DepositDateDate
    {
        get => DepositDate?.ToDateTime(TimeOnly.MinValue);
        set => DepositDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Date when the deposit matures.
    /// </summary>
    public DateOnly? MaturityDate { get; set; }

    /// <summary>
    /// DateTime wrapper for MaturityDate to work with MudDatePicker.
    /// </summary>
    public DateTime? MaturityDateDate
    {
        get => MaturityDate?.ToDateTime(TimeOnly.MinValue);
        set => MaturityDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }

    /// <summary>
    /// Interest payout frequency: "AtMaturity", "Monthly", "Quarterly", "Annually".
    /// </summary>
    public string? InterestPayoutFrequency { get; set; }

    /// <summary>
    /// Whether to auto-renew at maturity.
    /// </summary>
    public bool AutoRenew { get; set; }

    /// <summary>
    /// Renewal term in months (if auto-renew is enabled).
    /// </summary>
    public int? RenewalTermMonths { get; set; }

    /// <summary>
    /// Currency code. Default: "PHP".
    /// </summary>
    public string CurrencyCode { get; set; } = "PHP";

    /// <summary>
    /// Notes or comments about this fixed deposit.
    /// </summary>
    public string? Notes { get; set; }
}
