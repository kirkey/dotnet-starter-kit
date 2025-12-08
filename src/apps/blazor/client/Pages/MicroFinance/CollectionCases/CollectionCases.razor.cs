namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionCases;

public partial class CollectionCases
{
    protected EntityServerTableContext<CollectionCaseResponse, DefaultIdType, CollectionCaseViewModel> Context { get; set; } = null!;
    private EntityTable<CollectionCaseResponse, DefaultIdType, CollectionCaseViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CollectionCaseResponse, DefaultIdType, CollectionCaseViewModel>(
            fields:
            [
                new EntityField<CollectionCaseResponse>(dto => dto.CaseNumber, "Case #", "CaseNumber"),
                new EntityField<CollectionCaseResponse>(dto => dto.LoanId, "Loan", "LoanId"),
                new EntityField<CollectionCaseResponse>(dto => dto.Priority, "Priority", "Priority"),
                new EntityField<CollectionCaseResponse>(dto => dto.CurrentDaysPastDue, "DPD", "CurrentDaysPastDue", typeof(int)),
                new EntityField<CollectionCaseResponse>(dto => dto.AmountOverdue, "Overdue", "AmountOverdue", typeof(decimal)),
                new EntityField<CollectionCaseResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollectionCasesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollectionCasesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollectionCaseResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollectionCaseAsync("1", viewModel.Adapt<CreateCollectionCaseCommand>()).ConfigureAwait(false);
            },
            entityName: "Collection Case",
            entityNamePlural: "Collection Cases",
            entityResource: FshResources.CollectionCases,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var caseData = await Client.GetCollectionCaseAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Case", caseData } };
        await DialogService.ShowAsync<CollectionCaseDetailsDialog>("Collection Case Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show collection case help dialog.
    /// </summary>
    private async Task ShowCollectionCaseHelp()
    {
        await DialogService.ShowAsync<CollectionCasesHelpDialog>("Collection Case Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
