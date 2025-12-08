namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareAccounts;

/// <summary>
/// ShareAccounts page logic. Provides CRUD and search over ShareAccount entities using the generated API client.
/// Manages member share accounts including share purchases, redemptions, and dividends.
/// </summary>
public partial class ShareAccounts
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<ShareAccountResponse, DefaultIdType, ShareAccountViewModel> Context { get; set; } = null!;

    private EntityTable<ShareAccountResponse, DefaultIdType, ShareAccountViewModel> _table = null!;

    /// <summary>
    /// Authorization state for permission checks.
    /// </summary>
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Authorization service for permission checks.
    /// </summary>
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canManage;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Date picker backing field
    private DateTime? _openedDate = DateTime.Today;

    // Advanced search filters
    private string? _searchAccountNumber;
    private string? SearchAccountNumber
    {
        get => _searchAccountNumber;
        set
        {
            _searchAccountNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchStatus;
    private string? SearchStatus
    {
        get => _searchStatus;
        set
        {
            _searchStatus = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with share account-specific configuration.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<ShareAccountResponse, DefaultIdType, ShareAccountViewModel>(
            fields:
            [
                new EntityField<ShareAccountResponse>(dto => dto.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<ShareAccountResponse>(dto => dto.NumberOfShares, "Shares", "NumberOfShares", typeof(int)),
                new EntityField<ShareAccountResponse>(dto => dto.TotalShareValue, "Share Value", "TotalShareValue", typeof(decimal)),
                new EntityField<ShareAccountResponse>(dto => dto.TotalPurchases, "Purchases", "TotalPurchases", typeof(decimal)),
                new EntityField<ShareAccountResponse>(dto => dto.TotalRedemptions, "Redemptions", "TotalRedemptions", typeof(decimal)),
                new EntityField<ShareAccountResponse>(dto => dto.TotalDividendsEarned, "Dividends Earned", "TotalDividendsEarned", typeof(decimal)),
                new EntityField<ShareAccountResponse>(dto => dto.OpenedDate, "Opened", "OpenedDate", typeof(DateTimeOffset)),
                new EntityField<ShareAccountResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchShareAccountsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchShareAccountsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ShareAccountResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                // Sync IDs from autocomplete selections
                viewModel.SyncIdsFromSelections();
                
                // Set opened date from date picker
                if (_openedDate.HasValue)
                {
                    viewModel.OpenedDate = new DateTimeOffset(_openedDate.Value);
                }
                
                await Client.CreateShareAccountAsync("1", viewModel.Adapt<CreateShareAccountCommand>()).ConfigureAwait(false);
            },
            // No update or delete for share accounts - they are managed through share transactions
            entityName: "Share Account",
            entityNamePlural: "Share Accounts",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions for extra actions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show share accounts help dialog.
    /// </summary>
    private async Task ShowShareAccountsHelp()
    {
        await DialogService.ShowAsync<ShareAccountsHelpDialog>("Share Accounts Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View share account details in a dialog.
    /// </summary>
    private async Task ViewAccountDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ShareAccountDetailsDialog.AccountId), id }
        };

        await DialogService.ShowAsync<ShareAccountDetailsDialog>("Share Account Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Purchase shares for an account.
    /// </summary>
    private async Task PurchaseShares(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id);
        var product = await Client.GetShareProductAsync("1", account.ShareProductId);
        
        var confirmed = await DialogService.ShowMessageBox(
            "Purchase Shares",
            $"Purchase 10 shares at {product.CurrentPrice:C} per share for a total of {(10 * product.CurrentPrice):C}?",
            yesText: "Purchase",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.PurchaseSharesAsync("1", id, new PurchaseSharesCommand 
                { 
                    ShareAccountId = id,
                    NumberOfShares = 10,
                    PricePerShare = product.CurrentPrice 
                }),
                successMessage: "Shares purchased successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Redeem shares from an account.
    /// </summary>
    private async Task RedeemShares(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id);
        
        if (account.NumberOfShares <= 0)
        {
            await DialogService.ShowMessageBox(
                "No Shares to Redeem",
                "This account has no shares to redeem.",
                yesText: "OK");
            return;
        }
        
        var product = await Client.GetShareProductAsync("1", account.ShareProductId);
        var sharesToRedeem = Math.Min(10, account.NumberOfShares);
        
        var confirmed = await DialogService.ShowMessageBox(
            "Redeem Shares",
            $"Redeem {sharesToRedeem} shares at {product.CurrentPrice:C} per share for a total of {(sharesToRedeem * product.CurrentPrice):C}?",
            yesText: "Redeem",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.RedeemSharesAsync("1", id, new RedeemSharesCommand 
                { 
                    ShareAccountId = id,
                    NumberOfShares = sharesToRedeem,
                    PricePerShare = product.CurrentPrice 
                }),
                successMessage: "Shares redeemed successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Close a share account.
    /// </summary>
    private async Task CloseAccount(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id);
        
        if (account.NumberOfShares > 0)
        {
            await DialogService.ShowMessageBox(
                "Cannot Close Account",
                $"This account still has {account.NumberOfShares} shares. Redeem all shares before closing.",
                yesText: "OK");
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Close Account",
            "Are you sure you want to close this share account? This action cannot be undone.",
            yesText: "Close Account",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.CloseShareAccountAsync("1", id, new CloseShareAccountCommand 
                { 
                    Reason = "Closed by administrator" 
                }),
                successMessage: "Share account closed successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
