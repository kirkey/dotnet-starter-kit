namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsAccounts;

/// <summary>
/// ViewModel used by the SavingsAccounts page for add/edit operations.
/// Mirrors the shape of the API's CreateSavingsAccountCommand.
/// </summary>
public class SavingsAccountViewModel
{
    /// <summary>
    /// Primary identifier of the savings account.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member who owns this account.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// The savings product type for this account.
    /// </summary>
    public DefaultIdType SavingsProductId { get; set; }

    /// <summary>
    /// Initial deposit amount when opening the account.
    /// </summary>
    public decimal InitialDeposit { get; set; }
}
