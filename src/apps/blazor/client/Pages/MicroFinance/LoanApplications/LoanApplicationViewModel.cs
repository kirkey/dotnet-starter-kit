namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanApplications;

/// <summary>
/// ViewModel for creating loan application entities.
/// Maps to CreateLoanApplicationCommand.
/// </summary>
public class LoanApplicationViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType MemberId { get; set; }
    public DefaultIdType LoanProductId { get; set; }
    public DefaultIdType? MemberGroupId { get; set; }
    public decimal RequestedAmount { get; set; } = 100000m;
    public int RequestedTermMonths { get; set; } = 12;
    public string? Purpose { get; set; }
}
