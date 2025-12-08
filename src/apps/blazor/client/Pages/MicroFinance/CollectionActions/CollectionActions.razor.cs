namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionActions;

public partial class CollectionActions
{
    protected EntityServerTableContext<CollectionActionSummaryResponse, DefaultIdType, CollectionActionViewModel> Context { get; set; } = null!;
    private EntityTable<CollectionActionSummaryResponse, DefaultIdType, CollectionActionViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CollectionActionSummaryResponse, DefaultIdType, CollectionActionViewModel>(
            fields:
            [
                new EntityField<CollectionActionSummaryResponse>(dto => dto.ActionType, "Action Type", "ActionType"),
                new EntityField<CollectionActionSummaryResponse>(dto => dto.ActionDateTime, "Date/Time", "ActionDateTime", typeof(DateTime)),
                new EntityField<CollectionActionSummaryResponse>(dto => dto.ContactMethod, "Contact Method", "ContactMethod"),
                new EntityField<CollectionActionSummaryResponse>(dto => dto.ContactPerson, "Contact Person", "ContactPerson"),
                new EntityField<CollectionActionSummaryResponse>(dto => dto.Outcome, "Outcome", "Outcome"),
                new EntityField<CollectionActionSummaryResponse>(dto => dto.PromisedAmount, "Promised", "PromisedAmount", typeof(decimal)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollectionActionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollectionActionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollectionActionSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollectionActionAsync("1", viewModel.Adapt<CreateCollectionActionCommand>()).ConfigureAwait(false);
            },
            entityName: "Collection Action",
            entityNamePlural: "Collection Actions",
            entityResource: FshResources.CollectionActions,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var action = await Client.GetCollectionActionAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Action", action } };
        await DialogService.ShowAsync<CollectionActionDetailsDialog>("Collection Action Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show collection action help dialog.
    /// </summary>
    private async Task ShowCollectionActionHelp()
    {
        await DialogService.ShowAsync<CollectionActionsHelpDialog>("Collection Action Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
