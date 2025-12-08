namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Loans;

/// <summary>
/// ViewModel used by the Loans page for add/edit operations.
/// Mirrors the shape of the API's CreateLoanCommand and UpdateLoanCommand so Mapster/Adapt can map between them.
/// </summary>
public class LoanViewModel
{
    /// <summary>
    /// Primary identifier of the loan.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member who is taking this loan. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// The loan product type for this loan. Required.
    /// </summary>
    public DefaultIdType LoanProductId { get; set; }

    /// <summary>
    /// Selected loan product response for autocomplete binding.
    /// </summary>
    public LoanProductResponse? SelectedLoanProduct { get; set; }

    /// <summary>
    /// The requested loan amount.
    /// </summary>
    public decimal RequestedAmount { get; set; } = 50000m;

    /// <summary>
    /// The loan term in months.
    /// </summary>
    public int TermMonths { get; set; } = 12;

    /// <summary>
    /// Purpose of the loan.
    /// </summary>
    public string? Purpose { get; set; } = "Business expansion loan";

    /// <summary>
    /// Repayment frequency (e.g., MONTHLY, WEEKLY).
    /// </summary>
    public string? RepaymentFrequency { get; set; } = "MONTHLY";

    /// <summary>
    /// Sync IDs from selected autocomplete values.
    /// </summary>
    public void SyncIdsFromSelections()
    {
        if (SelectedMember != null)
            MemberId = SelectedMember.Id;
        if (SelectedLoanProduct != null)
            LoanProductId = SelectedLoanProduct.Id;
    }
}
