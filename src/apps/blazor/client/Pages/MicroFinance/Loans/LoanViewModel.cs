namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Loans;

/// <summary>
/// ViewModel used by the Loans page for add/edit operations.
/// Mirrors the shape of the API's CreateLoanCommand and UpdateLoanCommand.
/// </summary>
public class LoanViewModel
{
    /// <summary>
    /// Primary identifier of the loan.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member applying for the loan.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// The loan product selected for this application.
    /// </summary>
    public DefaultIdType LoanProductId { get; set; }

    /// <summary>
    /// Amount requested by the member.
    /// </summary>
    public decimal RequestedAmount { get; set; }

    /// <summary>
    /// Loan term in months.
    /// </summary>
    public int TermMonths { get; set; } = 12;

    /// <summary>
    /// Purpose or reason for the loan.
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// Repayment frequency. Values: "WEEKLY", "BI-WEEKLY", "MONTHLY", "QUARTERLY".
    /// </summary>
    public string RepaymentFrequency { get; set; } = "MONTHLY";
}
