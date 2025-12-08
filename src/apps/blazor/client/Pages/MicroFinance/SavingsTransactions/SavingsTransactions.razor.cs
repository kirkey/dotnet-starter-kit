namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsTransactions;

/// <summary>
/// SavingsTransactions page logic. Provides search and view operations for SavingsTransaction entities.
/// This is a read-only view - transactions are created via deposits, withdrawals, and other account operations.
/// </summary>
public partial class SavingsTransactions
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<SavingsTransactionResponse, DefaultIdType, SavingsTransactionViewModel> Context { get; set; } = null!;

    private EntityTable<SavingsTransactionResponse, DefaultIdType, SavingsTransactionViewModel> _table = null!;

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

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchTransactionType;
    private string? SearchTransactionType
    {
        get => _searchTransactionType;
        set
        {
            _searchTransactionType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchPaymentMethod;
    private string? SearchPaymentMethod
    {
        get => _searchPaymentMethod;
        set
        {
            _searchPaymentMethod = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with savings transaction-specific configuration.
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

        Context = new EntityServerTableContext<SavingsTransactionResponse, DefaultIdType, SavingsTransactionViewModel>(
            fields:
            [
                new EntityField<SavingsTransactionResponse>(dto => dto.Reference, "Reference", "Reference"),
                new EntityField<SavingsTransactionResponse>(dto => dto.TransactionType, "Type", "TransactionType"),
                new EntityField<SavingsTransactionResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<SavingsTransactionResponse>(dto => dto.BalanceAfter, "Balance After", "BalanceAfter", typeof(decimal)),
                new EntityField<SavingsTransactionResponse>(dto => dto.TransactionDate, "Date", "TransactionDate", typeof(DateTimeOffset)),
                new EntityField<SavingsTransactionResponse>(dto => dto.PaymentMethod, "Payment Method", "PaymentMethod"),
                new EntityField<SavingsTransactionResponse>(dto => dto.Description, "Description", "Description"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchSavingsTransactionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchSavingsTransactionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SavingsTransactionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Savings Transaction",
            entityNamePlural: "Savings Transactions",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => true);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<SavingsTransactionsHelpDialog>("Savings Transactions Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View transaction details in a dialog.
    /// </summary>
    private async Task ViewTransactionDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(SavingsTransactionDetailsDialog.TransactionId), id }
        };

        await DialogService.ShowAsync<SavingsTransactionDetailsDialog>("Transaction Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
