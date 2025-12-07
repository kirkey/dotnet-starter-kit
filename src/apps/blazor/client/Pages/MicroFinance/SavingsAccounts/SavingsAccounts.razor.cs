namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsAccounts;

/// <summary>
/// SavingsAccounts page logic. Provides CRUD and search over SavingsAccount entities using the generated API client.
/// Manages member savings accounts including deposits, withdrawals, and interest.
/// </summary>
public partial class SavingsAccounts
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<SavingsAccountResponse, DefaultIdType, SavingsAccountViewModel> Context { get; set; } = null!;

    private EntityTable<SavingsAccountResponse, DefaultIdType, SavingsAccountViewModel> _table = null!;

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
    private bool _canDeposit;
    private bool _canWithdraw;
    private bool _canFreeze;
    private bool _canUnfreeze;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

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

    private string? _searchMemberName;
    private string? SearchMemberName
    {
        get => _searchMemberName;
        set
        {
            _searchMemberName = value;
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
    /// Initializes the table context with savings account-specific configuration.
    /// </summary>
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

        Context = new EntityServerTableContext<SavingsAccountResponse, DefaultIdType, SavingsAccountViewModel>(
            fields:
            [
                new EntityField<SavingsAccountResponse>(dto => dto.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<SavingsAccountResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<SavingsAccountResponse>(dto => dto.ProductName, "Product", "ProductName"),
                new EntityField<SavingsAccountResponse>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.TotalDeposits, "Total Deposits", "TotalDeposits", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.TotalWithdrawals, "Total Withdrawals", "TotalWithdrawals", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.TotalInterestEarned, "Interest Earned", "TotalInterestEarned", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.OpenedDate, "Opened", "OpenedDate", typeof(DateOnly)),
                new EntityField<SavingsAccountResponse>(dto => dto.Status, "Status", "Status",
                    Template: entity => @<MudChip T="string" Color="@GetStatusColor(entity.Status)" Size="Size.Small">@entity.Status</MudChip>),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchSavingsAccountsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchSavingsAccountsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SavingsAccountResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateSavingsAccountAsync("1", viewModel.Adapt<CreateSavingsAccountCommand>()).ConfigureAwait(false);
            },
            updateFunc: null, // Savings accounts are typically not updated directly
            deleteFunc: null, // Savings accounts should not be deleted
            entityName: "Savings Account",
            entityNamePlural: "Savings Accounts",
            entityResource: FshResources.SavingsAccounts,
            hasExtraActionsFunc: () => true); // Always show menu for View Details/Transactions

        // Check permissions for extra actions
        var state = await AuthState;
        _canDeposit = await AuthService.HasPermissionAsync(state.User, FshActions.Deposit, FshResources.SavingsTransactions);
        _canWithdraw = await AuthService.HasPermissionAsync(state.User, FshActions.Withdraw, FshResources.SavingsTransactions);
        _canFreeze = await AuthService.HasPermissionAsync(state.User, FshActions.Freeze, FshResources.SavingsAccounts);
        _canUnfreeze = await AuthService.HasPermissionAsync(state.User, FshActions.Unfreeze, FshResources.SavingsAccounts);
    }

    private Color GetStatusColor(string status) => status switch
    {
        "Active" => Color.Success,
        "Dormant" => Color.Warning,
        "Frozen" => Color.Error,
        "Closed" => Color.Default,
        _ => Color.Default
    };

    /// <summary>
    /// Show savings accounts help dialog.
    /// </summary>
    private async Task ShowSavingsAccountsHelp()
    {
        await DialogService.ShowAsync<SavingsAccountsHelpDialog>("Savings Accounts Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Record a deposit to the account.
    /// </summary>
    private async Task RecordDeposit(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(SavingsTransactionDialog.AccountId), id },
            { nameof(SavingsTransactionDialog.TransactionType), "Deposit" }
        };

        var dialog = await DialogService.ShowAsync<SavingsTransactionDialog>("Record Deposit", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        });

        var result = await dialog.Result;
        if (result?.Canceled == false)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Record a withdrawal from the account.
    /// </summary>
    private async Task RecordWithdrawal(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(SavingsTransactionDialog.AccountId), id },
            { nameof(SavingsTransactionDialog.TransactionType), "Withdrawal" }
        };

        var dialog = await DialogService.ShowAsync<SavingsTransactionDialog>("Record Withdrawal", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        });

        var result = await dialog.Result;
        if (result?.Canceled == false)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// View account details in a dialog.
    /// </summary>
    private async Task ViewAccountDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(SavingsAccountDetailsDialog.AccountId), id }
        };

        await DialogService.ShowAsync<SavingsAccountDetailsDialog>("Account Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View transaction history for the account.
    /// </summary>
    private async Task ViewTransactions(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(SavingsTransactionHistoryDialog.AccountId), id }
        };

        await DialogService.ShowAsync<SavingsTransactionHistoryDialog>("Transaction History", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Freeze an account.
    /// </summary>
    private async Task FreezeAccount(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Freeze Account",
            "Are you sure you want to freeze this account? All transactions will be blocked.",
            yesText: "Freeze",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.FreezeSavingsAccountAsync("1", id),
                Snackbar,
                successMessage: "Account frozen successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Unfreeze an account.
    /// </summary>
    private async Task UnfreezeAccount(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Unfreeze Account",
            "Are you sure you want to unfreeze this account? Transactions will be allowed again.",
            yesText: "Unfreeze",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.UnfreezeSavingsAccountAsync("1", id),
                Snackbar,
                successMessage: "Account unfrozen successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
