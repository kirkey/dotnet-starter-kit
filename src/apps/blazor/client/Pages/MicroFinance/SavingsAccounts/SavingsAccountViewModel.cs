namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsAccounts;

/// <summary>
/// ViewModel used by the SavingsAccounts page for add operations.
/// Mirrors the shape of the API's CreateSavingsAccountCommand so Mapster/Adapt can map between them.
/// </summary>
public class SavingsAccountViewModel
{
    /// <summary>
    /// Primary identifier of the savings account.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member who owns this savings account. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// The savings product type for this account. Required.
    /// </summary>
    public DefaultIdType SavingsProductId { get; set; }

    /// <summary>
    /// Selected savings product response for autocomplete binding.
    /// </summary>
    public SavingsProductResponse? SelectedSavingsProduct { get; set; }

    /// <summary>
    /// Initial deposit amount when opening the account.
    /// Defaults to 100.
    /// </summary>
    public decimal InitialDeposit { get; set; } = 100m;

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
