namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsProducts;

/// <summary>
/// ViewModel used by the SavingsProducts page for add/edit operations.
/// Mirrors the shape of the API's CreateSavingsProductCommand and UpdateSavingsProductCommand.
/// </summary>
public class SavingsProductViewModel
{
    /// <summary>
    /// Primary identifier of the savings product.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique product code. Example: "REGULAR", "FIXED", "MINOR".
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Product name displayed to users. Example: "Regular Savings", "Fixed Deposit".
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the savings product and its features.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Annual interest rate percentage.
    /// </summary>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// How interest is calculated. Values: "Daily", "Monthly", "Quarterly", "Annual".
    /// </summary>
    public string InterestCalculation { get; set; } = "Monthly";

    /// <summary>
    /// How often interest is posted to accounts. Values: "Monthly", "Quarterly", "Semi-Annual", "Annual".
    /// </summary>
    public string InterestPostingFrequency { get; set; } = "Monthly";

    /// <summary>
    /// Minimum amount required to open an account.
    /// </summary>
    public decimal MinOpeningBalance { get; set; }

    /// <summary>
    /// Minimum balance required to earn interest.
    /// </summary>
    public decimal MinBalanceForInterest { get; set; }

    /// <summary>
    /// Minimum amount for a single withdrawal.
    /// </summary>
    public decimal MinWithdrawalAmount { get; set; }

    /// <summary>
    /// Maximum amount that can be withdrawn per day. Null for unlimited.
    /// </summary>
    public decimal? MaxWithdrawalPerDay { get; set; }

    /// <summary>
    /// Whether the account can have a negative balance.
    /// </summary>
    public bool AllowOverdraft { get; set; }

    /// <summary>
    /// Maximum overdraft amount allowed if overdraft is enabled.
    /// </summary>
    public decimal? OverdraftLimit { get; set; }
}
