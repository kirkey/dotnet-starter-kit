namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanProducts;

/// <summary>
/// ViewModel used by the LoanProducts page for add/edit operations.
/// Mirrors the shape of the API's CreateLoanProductCommand and UpdateLoanProductCommand.
/// </summary>
public class LoanProductViewModel
{
    /// <summary>
    /// Primary identifier of the loan product.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique product code. Example: "LP001", "PERSONAL-LOAN".
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Product name displayed to users. Example: "Personal Loan", "Business Expansion Loan".
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the loan product and its features.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Minimum loan amount that can be requested.
    /// </summary>
    public decimal MinLoanAmount { get; set; }

    /// <summary>
    /// Maximum loan amount that can be requested.
    /// </summary>
    public decimal MaxLoanAmount { get; set; }

    /// <summary>
    /// Annual interest rate percentage.
    /// </summary>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Method used to calculate interest. Values: "Declining", "Flat", "Reducing".
    /// </summary>
    public string InterestMethod { get; set; } = "Declining";

    /// <summary>
    /// Minimum loan term in months.
    /// </summary>
    public int MinTermMonths { get; set; } = 1;

    /// <summary>
    /// Maximum loan term in months.
    /// </summary>
    public int MaxTermMonths { get; set; } = 60;

    /// <summary>
    /// How often repayments are due. Values: "Weekly", "Bi-Weekly", "Monthly", "Quarterly".
    /// </summary>
    public string RepaymentFrequency { get; set; } = "Monthly";

    /// <summary>
    /// Number of days after due date before late penalty is applied.
    /// </summary>
    public int GracePeriodDays { get; set; }

    /// <summary>
    /// Penalty rate percentage applied to overdue amounts.
    /// </summary>
    public decimal LatePenaltyRate { get; set; }
}
