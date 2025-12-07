using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileWallets.Dialogs;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileWallets;

/// <summary>
/// Mobile Wallets page logic. Provides CRUD and workflow operations for mobile wallet entities.
/// </summary>
public partial class MobileWallets
{
    protected EntityServerTableContext<MobileWalletResponse, DefaultIdType, MobileWalletViewModel> Context { get; set; } = null!;

    private EntityTable<MobileWalletResponse, DefaultIdType, MobileWalletViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canActivate;
    private bool _canSuspend;
    private bool _canManageBalance;
    private bool _canLinkSavings;

    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchPhoneNumber;
    private string? SearchPhoneNumber
    {
        get => _searchPhoneNumber;
        set
        {
            _searchPhoneNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

    protected override async Task OnInitializedAsync()
    {
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

        // Check permissions
        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshPermissions.MobileWallets.Activate);
        _canSuspend = await AuthService.HasPermissionAsync(state.User, FshPermissions.MobileWallets.Suspend);
        _canManageBalance = await AuthService.HasPermissionAsync(state.User, FshPermissions.MobileWallets.ManageBalance);
        _canLinkSavings = await AuthService.HasPermissionAsync(state.User, FshPermissions.MobileWallets.LinkSavings);

        Context = new EntityServerTableContext<MobileWalletResponse, DefaultIdType, MobileWalletViewModel>(
            fields:
            [
                new EntityField<MobileWalletResponse>(dto => dto.PhoneNumber, "Phone Number", "PhoneNumber"),
                new EntityField<MobileWalletResponse>(dto => dto.Provider, "Provider", "Provider"),
                new EntityField<MobileWalletResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<MobileWalletResponse>(dto => dto.Tier, "Tier", "Tier"),
                new EntityField<MobileWalletResponse>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
                new EntityField<MobileWalletResponse>(dto => dto.DailyLimit, "Daily Limit", "DailyLimit", typeof(decimal)),
                new EntityField<MobileWalletResponse>(dto => dto.IsLinkedToBankAccount, "Bank Linked", "IsLinkedToBankAccount", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            getDefaultsFunc: () => Task.FromResult(new MobileWalletViewModel
            {
                DailyLimit = 50000m,
                MonthlyLimit = 500000m
            }),
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchMobileWalletsCommand>();

                if (!string.IsNullOrWhiteSpace(_searchPhoneNumber))
                    request = request with { PhoneNumber = _searchPhoneNumber };
                if (!string.IsNullOrWhiteSpace(_searchProvider))
                    request = request with { Provider = _searchProvider };
                if (!string.IsNullOrWhiteSpace(_searchStatus))
                    request = request with { Status = _searchStatus };

                var response = await Client.SearchMobileWalletsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<MobileWalletResponse>>();
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetMobileWalletEndpointAsync("1", id);
                return response.Adapt<MobileWalletViewModel>();
            },
            createFunc: async vm =>
            {
                var command = new CreateMobileWalletCommand
                {
                    MemberId = vm.MemberId,
                    PhoneNumber = vm.PhoneNumber,
                    Provider = vm.Provider,
                    DailyLimit = vm.DailyLimit,
                    MonthlyLimit = vm.MonthlyLimit
                };
                var response = await Client.CreateMobileWalletEndpointAsync("1", command);
                return response.Id;
            },
            deleteFunc: async id => await Client.DeleteMobileWalletEndpointAsync("1", id),
            exportAction: string.Empty,
            entityTypeName: "Mobile Wallet",
            entityTypeNamePlural: "Mobile Wallets",
            createPermission: FshPermissions.MobileWallets.Create,
            updatePermission: FshPermissions.MobileWallets.Update,
            deletePermission: FshPermissions.MobileWallets.Delete,
            searchPermission: FshPermissions.MobileWallets.View
        );
    }

    private async Task ActivateWallet(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Activate Wallet",
            "Are you sure you want to activate this wallet?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new ActivateMobileWalletCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await Client.ActivateMobileWalletEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Wallet activated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task SuspendWallet(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Suspend Wallet",
            "Are you sure you want to suspend this wallet? The member will not be able to use it until reactivated.",
            yesText: "Suspend",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new SuspendMobileWalletCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await Client.SuspendMobileWalletEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Wallet suspended successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowCreditDialog(MobileWalletResponse entity)
    {
        var parameters = new DialogParameters<MobileWalletTransactionDialog>
        {
            { x => x.WalletId, entity.Id },
            { x => x.PhoneNumber, entity.PhoneNumber },
            { x => x.CurrentBalance, entity.Balance },
            { x => x.IsCredit, true }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<MobileWalletTransactionDialog>("Credit Wallet", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowDebitDialog(MobileWalletResponse entity)
    {
        var parameters = new DialogParameters<MobileWalletTransactionDialog>
        {
            { x => x.WalletId, entity.Id },
            { x => x.PhoneNumber, entity.PhoneNumber },
            { x => x.CurrentBalance, entity.Balance },
            { x => x.IsCredit, false }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<MobileWalletTransactionDialog>("Debit Wallet", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowLinkSavingsDialog(MobileWalletResponse entity)
    {
        var parameters = new DialogParameters<MobileWalletLinkSavingsDialog>
        {
            { x => x.WalletId, entity.Id },
            { x => x.PhoneNumber, entity.PhoneNumber },
            { x => x.MemberId, entity.MemberId }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<MobileWalletLinkSavingsDialog>("Link to Savings", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ViewWalletDetails(DefaultIdType id)
    {
        var entity = await Client.GetMobileWalletEndpointAsync("1", id);

        var parameters = new DialogParameters<MobileWalletDetailsDialog>
        {
            { x => x.Entity, entity }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<MobileWalletDetailsDialog>("Wallet Details", parameters, options);
    }

    private async Task ShowMobileWalletsHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<MobileWalletsHelpDialog>("Mobile Wallets Help", options);
    }
}
