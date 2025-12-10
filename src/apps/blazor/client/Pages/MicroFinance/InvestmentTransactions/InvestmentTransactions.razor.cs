namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentTransactions;

public partial class InvestmentTransactions
{
    static InvestmentTransactions()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for InvestmentTransactionResponse -> InvestmentTransactionViewModel mapping
        TypeAdapterConfig<InvestmentTransactionResponse, InvestmentTransactionViewModel>.NewConfig()
            .Map(dest => dest.TransactionDate, src => src.TransactionDate.HasValue ? src.TransactionDate.Value.DateTime : (DateTime?)null);
    }

    protected EntityServerTableContext<InvestmentTransactionResponse, DefaultIdType, InvestmentTransactionViewModel> Context { get; set; } = null!;
    private EntityTable<InvestmentTransactionResponse, DefaultIdType, InvestmentTransactionViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

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

        Context = new EntityServerTableContext<InvestmentTransactionResponse, DefaultIdType, InvestmentTransactionViewModel>(
            fields:
            [
                new EntityField<InvestmentTransactionResponse>(dto => dto.TransactionReference, "Reference", "TransactionReference"),
                new EntityField<InvestmentTransactionResponse>(dto => dto.TransactionType, "Type", "TransactionType"),
                new EntityField<InvestmentTransactionResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<InvestmentTransactionResponse>(dto => dto.Units, "Units", "Units", typeof(decimal)),
                new EntityField<InvestmentTransactionResponse>(dto => dto.NetAmount, "Net Amount", "NetAmount", typeof(decimal)),
                new EntityField<InvestmentTransactionResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInvestmentTransactionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInvestmentTransactionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InvestmentTransactionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Investment Transaction",
            entityNamePlural: "Investment Transactions",
            entityResource: FshResources.InvestmentTransactions,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var transaction = await Client.GetInvestmentTransactionAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Transaction", transaction } };
        await DialogService.ShowAsync<InvestmentTransactionDetailsDialog>("Investment Transaction Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show investment transactions help dialog.
    /// </summary>
    private async Task ShowInvestmentTransactionsHelp()
    {
        await DialogService.ShowAsync<InvestmentTransactionsHelpDialog>("Investment Transactions Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
