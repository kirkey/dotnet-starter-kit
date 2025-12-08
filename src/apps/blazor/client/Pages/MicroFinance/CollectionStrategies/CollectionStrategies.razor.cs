namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionStrategies;

public partial class CollectionStrategies
{
    protected EntityServerTableContext<CollectionStrategySummaryResponse, DefaultIdType, CollectionStrategyViewModel> Context { get; set; } = null!;
    private EntityTable<CollectionStrategySummaryResponse, DefaultIdType, CollectionStrategyViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CollectionStrategySummaryResponse, DefaultIdType, CollectionStrategyViewModel>(
            fields:
            [
                new EntityField<CollectionStrategySummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<CollectionStrategySummaryResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<CollectionStrategySummaryResponse>(dto => dto.ActionType, "Action Type", "ActionType"),
                new EntityField<CollectionStrategySummaryResponse>(dto => dto.TriggerDaysPastDue, "Trigger DPD", "TriggerDaysPastDue", typeof(int)),
                new EntityField<CollectionStrategySummaryResponse>(dto => dto.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<CollectionStrategySummaryResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollectionStrategiesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollectionStrategiesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollectionStrategySummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollectionStrategyAsync("1", viewModel.Adapt<CreateCollectionStrategyCommand>()).ConfigureAwait(false);
            },
            entityName: "Collection Strategy",
            entityNamePlural: "Collection Strategies",
            entityResource: FshResources.CollectionStrategies,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var strategy = await Client.GetCollectionStrategyAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Strategy", strategy } };
        await DialogService.ShowAsync<CollectionStrategyDetailsDialog>("Collection Strategy Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
