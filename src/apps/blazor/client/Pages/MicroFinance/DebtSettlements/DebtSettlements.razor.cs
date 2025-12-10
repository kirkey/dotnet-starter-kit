namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.DebtSettlements;

public partial class DebtSettlements
{
    static DebtSettlements()
    {
        // Configure Mapster to convert DateTimeOffset to DateTime? for DebtSettlementSummaryResponse -> DebtSettlementViewModel mapping
        TypeAdapterConfig<DebtSettlementSummaryResponse, DebtSettlementViewModel>.NewConfig()
            .Map(dest => dest.ProposedDate, src => src.ProposedDate.DateTime)
            .Map(dest => dest.DueDate, src => src.DueDate.DateTime)
            .Map(dest => dest.CompletedDate, src => src.CompletedDate.HasValue ? src.CompletedDate.Value.DateTime : (DateTime?)null);
    }

    protected EntityServerTableContext<DebtSettlementSummaryResponse, DefaultIdType, DebtSettlementViewModel> Context { get; set; } = null!;
    private EntityTable<DebtSettlementSummaryResponse, DefaultIdType, DebtSettlementViewModel> _table = null!;

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

        Context = new EntityServerTableContext<DebtSettlementSummaryResponse, DefaultIdType, DebtSettlementViewModel>(
            fields:
            [
                new EntityField<DebtSettlementSummaryResponse>(dto => dto.ReferenceNumber, "Reference", "ReferenceNumber"),
                new EntityField<DebtSettlementSummaryResponse>(dto => dto.SettlementType, "Type", "SettlementType"),
                new EntityField<DebtSettlementSummaryResponse>(dto => dto.OriginalOutstanding, "Original", "OriginalOutstanding", typeof(decimal)),
                new EntityField<DebtSettlementSummaryResponse>(dto => dto.SettlementAmount, "Settlement", "SettlementAmount", typeof(decimal)),
                new EntityField<DebtSettlementSummaryResponse>(dto => dto.DiscountPercentage, "Discount %", "DiscountPercentage", typeof(decimal)),
                new EntityField<DebtSettlementSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchDebtSettlementsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchDebtSettlementsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<DebtSettlementSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateDebtSettlementAsync("1", viewModel.Adapt<CreateDebtSettlementCommand>()).ConfigureAwait(false);
            },
            entityName: "Debt Settlement",
            entityNamePlural: "Debt Settlements",
            entityResource: FshResources.DebtSettlements,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var settlement = await Client.GetDebtSettlementAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Settlement", settlement } };
        await DialogService.ShowAsync<DebtSettlementDetailsDialog>("Debt Settlement Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show debt settlement help dialog.
    /// </summary>
    private async Task ShowDebtSettlementHelp()
    {
        await DialogService.ShowAsync<DebtSettlementsHelpDialog>("Debt Settlement Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
