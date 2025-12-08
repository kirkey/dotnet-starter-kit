namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileWallets;

/// <summary>
/// MobileWallets page logic. Provides CRUD and search over MobileWallet entities.
/// Manages member mobile wallets for mobile money integration and transactions.
/// </summary>
public partial class MobileWallets
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<MobileWalletResponse, DefaultIdType, MobileWalletViewModel> Context { get; set; } = null!;

    private EntityTable<MobileWalletResponse, DefaultIdType, MobileWalletViewModel> _table = null!;

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

    // Advanced search filters
    private string? _searchProvider;
    private string? SearchProvider
    {
        get => _searchProvider;
        set
        {
            _searchProvider = value;
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

    private string? _searchTier;
    private string? SearchTier
    {
        get => _searchTier;
        set
        {
            _searchTier = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with mobile wallet-specific configuration.
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

        Context = new EntityServerTableContext<MobileWalletResponse, DefaultIdType, MobileWalletViewModel>(
            fields:
            [
                new EntityField<MobileWalletResponse>(dto => dto.PhoneNumber, "Phone Number", "PhoneNumber"),
                new EntityField<MobileWalletResponse>(dto => dto.Provider, "Provider", "Provider"),
                new EntityField<MobileWalletResponse>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
                new EntityField<MobileWalletResponse>(dto => dto.DailyLimit, "Daily Limit", "DailyLimit", typeof(decimal)),
                new EntityField<MobileWalletResponse>(dto => dto.MonthlyLimit, "Monthly Limit", "MonthlyLimit", typeof(decimal)),
                new EntityField<MobileWalletResponse>(dto => dto.Tier, "Tier", "Tier"),
                new EntityField<MobileWalletResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchMobileWalletsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchMobileWalletsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<MobileWalletResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateMobileWalletAsync("1", viewModel.Adapt<CreateMobileWalletCommand>()).ConfigureAwait(false);
            },
            entityName: "Mobile Wallet",
            entityNamePlural: "Mobile Wallets",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<MobileWalletsHelpDialog>("Mobile Wallets Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View wallet details in a dialog.
    /// </summary>
    private async Task ViewWalletDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(MobileWalletDetailsDialog.WalletId), id }
        };

        await DialogService.ShowAsync<MobileWalletDetailsDialog>("Wallet Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Suspend a wallet.
    /// </summary>
    private async Task SuspendWallet(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Suspend Wallet",
            "Are you sure you want to suspend this mobile wallet? The member will not be able to perform transactions.",
            "Suspend", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SuspendMobileWalletAsync("1", id, new SuspendMobileWalletRequest { Reason = "Suspended by administrator" }),
                successMessage: "Wallet suspended successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Activate a wallet.
    /// </summary>
    private async Task ActivateWallet(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Activate Wallet",
            "Are you sure you want to activate this mobile wallet?",
            "Activate", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateMobileWalletAsync("1", id),
                successMessage: "Wallet activated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Block a wallet.
    /// </summary>
    private async Task BlockWallet(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Block Wallet",
            "Are you sure you want to block this mobile wallet? This is typically done for security reasons (e.g., fraud detection).",
            "Block", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.BlockMobileWalletAsync("1", id, new BlockMobileWalletRequest { Reason = "Blocked by administrator" }),
                successMessage: "Wallet blocked successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Unblock a wallet.
    /// </summary>
    private async Task UnblockWallet(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Unblock Wallet",
            "Are you sure you want to unblock this mobile wallet?",
            "Unblock", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.UnblockMobileWalletAsync("1", id),
                successMessage: "Wallet unblocked successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Reactivate an inactive wallet.
    /// </summary>
    private async Task ReactivateWallet(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Reactivate Wallet",
            "Are you sure you want to reactivate this inactive mobile wallet?",
            "Reactivate", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ReactivateMobileWalletAsync("1", id),
                successMessage: "Wallet reactivated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Close a wallet permanently.
    /// </summary>
    private async Task CloseWallet(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Close Wallet",
            "Are you sure you want to close this mobile wallet permanently? This action cannot be undone.",
            "Close", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.CloseMobileWalletAsync("1", id, new CloseMobileWalletRequest { Reason = "Closed by administrator" }),
                successMessage: "Wallet closed successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
