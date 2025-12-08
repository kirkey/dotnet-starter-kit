namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareTransactions;

/// <summary>
/// ShareTransactions page logic. Provides search and view operations for ShareTransaction entities.
/// This is a read-only view - transactions are created via buy/sell operations on share accounts.
/// </summary>
public partial class ShareTransactions
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<ShareTransactionResponse, DefaultIdType, ShareTransactionViewModel> Context { get; set; } = null!;

    private EntityTable<ShareTransactionResponse, DefaultIdType, ShareTransactionViewModel> _table = null!;

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

    /// <summary>
    /// Initializes the table context with share transaction-specific configuration.
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

        Context = new EntityServerTableContext<ShareTransactionResponse, DefaultIdType, ShareTransactionViewModel>(
            fields:
            [
                new EntityField<ShareTransactionResponse>(dto => dto.Reference, "Reference", "Reference"),
                new EntityField<ShareTransactionResponse>(dto => dto.TransactionType, "Type", "TransactionType"),
                new EntityField<ShareTransactionResponse>(dto => dto.NumberOfShares, "Shares", "NumberOfShares", typeof(int)),
                new EntityField<ShareTransactionResponse>(dto => dto.PricePerShare, "Price/Share", "PricePerShare", typeof(decimal)),
                new EntityField<ShareTransactionResponse>(dto => dto.TotalAmount, "Total", "TotalAmount", typeof(decimal)),
                new EntityField<ShareTransactionResponse>(dto => dto.SharesBalanceAfter, "Balance After", "SharesBalanceAfter", typeof(int)),
                new EntityField<ShareTransactionResponse>(dto => dto.TransactionDate, "Date", "TransactionDate", typeof(DateTimeOffset)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchShareTransactionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchShareTransactionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ShareTransactionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Share Transaction",
            entityNamePlural: "Share Transactions",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => true);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<ShareTransactionsHelpDialog>("Share Transactions Help", new DialogParameters(), new DialogOptions
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
            { nameof(ShareTransactionDetailsDialog.TransactionId), id }
        };

        await DialogService.ShowAsync<ShareTransactionDetailsDialog>("Transaction Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
