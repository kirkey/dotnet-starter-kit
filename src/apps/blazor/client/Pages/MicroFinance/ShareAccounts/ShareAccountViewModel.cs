namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareAccounts;

/// <summary>
/// ViewModel used by the ShareAccounts page for add operations.
/// Mirrors the shape of the API's CreateShareAccountCommand so Mapster/Adapt can map between them.
/// </summary>
public class ShareAccountViewModel
{
    /// <summary>
    /// Primary identifier of the share account.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Account number for the share account.
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// The member who owns this share account. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// The share product for this account. Required.
    /// </summary>
    public DefaultIdType ShareProductId { get; set; }

    /// <summary>
    /// Selected share product response for autocomplete binding.
    /// </summary>
    public ShareProductResponse? SelectedShareProduct { get; set; }

    /// <summary>
    /// Date the account was opened.
    /// </summary>
    public DateTimeOffset? OpenedDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Notes about the share account.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Sync IDs from selected autocomplete values.
    /// </summary>
    public void SyncIdsFromSelections()
    {
        if (SelectedMember != null)
            MemberId = SelectedMember.Id;
        if (SelectedShareProduct != null)
            ShareProductId = SelectedShareProduct.Id;
    }
}
