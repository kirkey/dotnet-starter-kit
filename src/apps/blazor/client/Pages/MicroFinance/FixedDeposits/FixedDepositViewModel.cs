namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FixedDeposits;

/// <summary>
/// ViewModel used by the FixedDeposits page for add operations.
/// Mirrors the shape of the API's CreateFixedDepositCommand so Mapster/Adapt can map between them.
/// </summary>
public class FixedDepositViewModel
{
    /// <summary>
    /// Primary identifier of the fixed deposit.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Certificate number for the fixed deposit.
    /// </summary>
    public string? CertificateNumber { get; set; }

    /// <summary>
    /// The member who owns this fixed deposit. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// Principal amount deposited.
    /// </summary>
    public decimal PrincipalAmount { get; set; } = 10000m;

    /// <summary>
    /// Annual interest rate percentage.
    /// </summary>
    public decimal InterestRate { get; set; } = 8.5m;

    /// <summary>
    /// Term in months.
    /// </summary>
    public int TermMonths { get; set; } = 12;

    /// <summary>
    /// Optional savings product ID.
    /// </summary>
    public DefaultIdType? SavingsProductId { get; set; }

    /// <summary>
    /// Selected savings product response for autocomplete binding.
    /// </summary>
    public SavingsProductResponse? SelectedSavingsProduct { get; set; }

    /// <summary>
    /// Optional linked savings account ID.
    /// </summary>
    public DefaultIdType? LinkedSavingsAccountId { get; set; }

    /// <summary>
    /// Date of deposit.
    /// </summary>
    public DateTimeOffset? DepositDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Instruction at maturity (e.g., TransferToSavings, Reinvest, Payout).
    /// </summary>
    public string? MaturityInstruction { get; set; } = "TransferToSavings";

    /// <summary>
    /// Notes about the fixed deposit.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Sync IDs from selected autocomplete values.
    /// </summary>
    public void SyncIdsFromSelections()
    {
        if (SelectedMember != null)
            MemberId = SelectedMember.Id;
        if (SelectedSavingsProduct != null)
            SavingsProductId = SelectedSavingsProduct.Id;
    }
}
