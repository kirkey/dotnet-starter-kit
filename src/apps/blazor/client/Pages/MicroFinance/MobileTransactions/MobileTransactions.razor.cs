namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileTransactions;

public partial class MobileTransactions
{
    static MobileTransactions()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for MobileTransactionResponse -> MobileTransactionViewModel mapping
        TypeAdapterConfig<MobileTransactionResponse, MobileTransactionViewModel>.NewConfig()
            .Map(dest => dest.TransactionDate, src => src.TransactionDate.HasValue ? src.TransactionDate.Value.DateTime : (DateTime?)null);
    }

    protected EntityServerTableContext<MobileTransactionResponse, DefaultIdType, MobileTransactionViewModel> Context { get; set; } = null!;
    private EntityTable<MobileTransactionResponse, DefaultIdType, MobileTransactionViewModel> _table = null!;

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

        Context = new EntityServerTableContext<MobileTransactionResponse, DefaultIdType, MobileTransactionViewModel>(
            fields:
            [
                new EntityField<MobileTransactionResponse>(dto => dto.TransactionReference, "Reference", "TransactionReference"),
                new EntityField<MobileTransactionResponse>(dto => dto.TransactionType, "Type", "TransactionType"),
                new EntityField<MobileTransactionResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<MobileTransactionResponse>(dto => dto.Fee, "Fee", "Fee", typeof(decimal)),
                new EntityField<MobileTransactionResponse>(dto => dto.SourcePhone, "Source", "SourcePhone"),
                new EntityField<MobileTransactionResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchMobileTransactionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchMobileTransactionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<MobileTransactionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateMobileTransactionAsync("1", viewModel.Adapt<CreateMobileTransactionCommand>()).ConfigureAwait(false);
            },
            entityName: "Mobile Transaction",
            entityNamePlural: "Mobile Transactions",
            entityResource: FshResources.MobileTransactions,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var transaction = await Client.GetMobileTransactionAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Transaction", transaction } };
        await DialogService.ShowAsync<MobileTransactionDetailsDialog>("Mobile Transaction Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show mobile transactions help dialog.
    /// </summary>
    private async Task ShowMobileTransactionsHelp()
    {
        await DialogService.ShowAsync<MobileTransactionsHelpDialog>("Mobile Transactions Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
