namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanApplications;

/// <summary>
/// ViewModel used by the LoanApplications page for add operations.
/// Mirrors the shape of the API's CreateLoanApplicationCommand so Mapster/Adapt can map between them.
/// </summary>
public class LoanApplicationViewModel
{
    /// <summary>
    /// Primary identifier of the loan application.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member applying for the loan. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// The loan product for this application. Required.
    /// </summary>
    public DefaultIdType ProductId { get; set; }

    /// <summary>
    /// Selected loan product response for autocomplete binding.
    /// </summary>
    public LoanProductResponse? SelectedLoanProduct { get; set; }

    /// <summary>
    /// Requested loan amount.
    /// </summary>
    public decimal RequestedAmount { get; set; } = 100000m;

    /// <summary>
    /// Requested term in months.
    /// </summary>
    public int RequestedTermMonths { get; set; } = 12;

    /// <summary>
    /// Purpose of the loan.
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// Optional member group for group lending.
    /// </summary>
    public DefaultIdType? GroupId { get; set; }

    /// <summary>
    /// Sync IDs from selected autocomplete values.
    /// </summary>
    public void SyncIdsFromSelections()
    {
        if (SelectedMember != null)
            MemberId = SelectedMember.Id;
        if (SelectedLoanProduct != null)
            ProductId = SelectedLoanProduct.Id;
    }
}
