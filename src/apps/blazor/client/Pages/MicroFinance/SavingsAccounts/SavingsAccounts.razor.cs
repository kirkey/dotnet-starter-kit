namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsAccounts;

/// <summary>
/// Savings Accounts page logic. Provides Create and Search operations over SavingsAccount entities.
/// Manages member savings accounts including deposits and withdrawals.
/// Note: No Update/Delete operations available for savings accounts.
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

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Dialog state
    private bool _depositDialogVisible;
    private bool _withdrawDialogVisible;
    private SavingsAccountResponse? _selectedAccount;
    private decimal _depositAmount;
    private decimal _withdrawAmount;
    private string? _transactionNotes;
    private readonly DialogOptions _dialogOptions = new() { MaxWidth = MaxWidth.Small, FullWidth = true };

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
    /// Initializes the table context with savings account-specific configuration.
    /// Note: Only Create and Search operations - no Update or Delete.
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

        Context = new EntityServerTableContext<SavingsAccountResponse, DefaultIdType, SavingsAccountViewModel>(
            fields:
            [
                new EntityField<SavingsAccountResponse>(dto => dto.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<SavingsAccountResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<SavingsAccountResponse>(dto => dto.ProductName, "Product", "ProductName"),
                new EntityField<SavingsAccountResponse>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.TotalDeposits, "Total Deposits", "TotalDeposits", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.TotalWithdrawals, "Total Withdrawals", "TotalWithdrawals", typeof(decimal)),
                new EntityField<SavingsAccountResponse>(dto => dto.OpenedDate, "Opened", "OpenedDate", typeof(DateTimeOffset)),
                new EntityField<SavingsAccountResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchSavingsAccountsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    Status = _searchStatus
                };
                var result = await Client.SearchSavingsAccountsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SavingsAccountResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                // Sync IDs from autocomplete selections
                viewModel.SyncIdsFromSelections();
                await Client.CreateSavingsAccountAsync("1", viewModel.Adapt<CreateSavingsAccountCommand>()).ConfigureAwait(false);
            },
            // No update or delete for savings accounts
            entityName: "Savings Account",
            entityNamePlural: "Savings Accounts",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canDeposit || _canWithdraw);

        // Check permissions for extra actions
        var state = await AuthState;
        _canDeposit = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canWithdraw = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

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
    /// View account details in a dialog.
    /// </summary>
    private async Task ViewAccountDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(SavingsAccountDetailsDialog.AccountId), id }
        };

        await DialogService.ShowAsync<SavingsAccountDetailsDialog>("Savings Account Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Show deposit dialog.
    /// </summary>
    private void ShowDepositDialog(SavingsAccountResponse account)
    {
        _selectedAccount = account;
        _depositAmount = 0;
        _transactionNotes = null;
        _depositDialogVisible = true;
    }

    /// <summary>
    /// Show withdraw dialog.
    /// </summary>
    private void ShowWithdrawDialog(SavingsAccountResponse account)
    {
        _selectedAccount = account;
        _withdrawAmount = 0;
        _transactionNotes = null;
        _withdrawDialogVisible = true;
    }

    /// <summary>
    /// Execute deposit transaction.
    /// </summary>
    private async Task ExecuteDeposit()
    {
        if (_selectedAccount == null || _depositAmount <= 0) return;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.DepositToSavingsAccountAsync("1", _selectedAccount.Id, new DepositCommand
            {
                Amount = _depositAmount,
                Notes = _transactionNotes
            }),
            successMessage: $"Successfully deposited {_depositAmount:C} to account {_selectedAccount.AccountNumber}.");
        
        _depositDialogVisible = false;
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Execute withdraw transaction.
    /// </summary>
    private async Task ExecuteWithdraw()
    {
        if (_selectedAccount == null || _withdrawAmount <= 0) return;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.WithdrawFromSavingsAccountAsync("1", _selectedAccount.Id, new WithdrawCommand
            {
                Amount = _withdrawAmount,
                Notes = _transactionNotes
            }),
            successMessage: $"Successfully withdrew {_withdrawAmount:C} from account {_selectedAccount.AccountNumber}.");
        
        _withdrawDialogVisible = false;
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Activate a pending savings account.
    /// </summary>
    private async Task ActivateAccount(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Activate Account",
            "Are you sure you want to activate this savings account? The member will be able to perform deposits and withdrawals.",
            yesText: "Activate",
            cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateSavingsAccountAsync("1", id),
                successMessage: "Savings account activated successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
