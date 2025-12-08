namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanApplications;

/// <summary>
/// ViewModel used by the LoanApplications page for add/edit operations.
/// Mirrors the shape of the API's CreateLoanApplicationCommand so Mapster/Adapt can map between them.
/// </summary>
public class LoanApplicationViewModel
{
    /// <summary>
    /// Primary identifier of the loan application.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member applying for this loan. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// The loan product type for this application. Required.
    /// Maps to ProductId in CreateLoanApplicationCommand.
    /// </summary>
    public DefaultIdType ProductId { get; set; }

    /// <summary>
    /// Selected loan product response for autocomplete binding.
    /// </summary>
    public LoanProductResponse? SelectedLoanProduct { get; set; }

    /// <summary>
    /// Optional member group for group lending.
    /// Maps to GroupId in CreateLoanApplicationCommand.
    /// </summary>
    public DefaultIdType? GroupId { get; set; }

    /// <summary>
    /// Selected member group response for autocomplete binding.
    /// </summary>
    public MemberGroupResponse? SelectedMemberGroup { get; set; }

    /// <summary>
    /// The requested loan amount.
    /// </summary>
    public decimal RequestedAmount { get; set; } = 100000m;

    /// <summary>
    /// The requested loan term in months.
    /// </summary>
    public int RequestedTermMonths { get; set; } = 12;

    /// <summary>
    /// Purpose of the loan.
    /// </summary>
    public string? Purpose { get; set; } = "Business Expansion";

    /// <summary>
    /// Sync IDs from selected autocomplete values.
    /// </summary>
    public void SyncIdsFromSelections()
    {
        if (SelectedMember != null)
            MemberId = SelectedMember.Id;
        if (SelectedLoanProduct != null)
            ProductId = SelectedLoanProduct.Id;
        if (SelectedMemberGroup != null)
            GroupId = SelectedMemberGroup.Id;
    }
}
