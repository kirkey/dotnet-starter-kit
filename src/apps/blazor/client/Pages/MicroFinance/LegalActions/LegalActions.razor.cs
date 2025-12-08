namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LegalActions;

public partial class LegalActions
{
    protected EntityServerTableContext<LegalActionResponse, DefaultIdType, LegalActionViewModel> Context { get; set; } = null!;
    private EntityTable<LegalActionResponse, DefaultIdType, LegalActionViewModel> _table = null!;

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

        Context = new EntityServerTableContext<LegalActionResponse, DefaultIdType, LegalActionViewModel>(
            fields:
            [
                new EntityField<LegalActionResponse>(dto => dto.CaseReference, "Case Ref", "CaseReference"),
                new EntityField<LegalActionResponse>(dto => dto.ActionType, "Action Type", "ActionType"),
                new EntityField<LegalActionResponse>(dto => dto.CourtName, "Court", "CourtName"),
                new EntityField<LegalActionResponse>(dto => dto.ClaimAmount, "Claim", "ClaimAmount", typeof(decimal)),
                new EntityField<LegalActionResponse>(dto => dto.NextHearingDate, "Next Hearing", "NextHearingDate", typeof(DateTimeOffset)),
                new EntityField<LegalActionResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLegalActionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLegalActionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LegalActionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLegalActionAsync("1", viewModel.Adapt<CreateLegalActionCommand>()).ConfigureAwait(false);
            },
            entityName: "Legal Action",
            entityNamePlural: "Legal Actions",
            entityResource: FshResources.LegalActions,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var action = await Client.GetLegalActionAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "LegalAction", action } };
        await DialogService.ShowAsync<LegalActionDetailsDialog>("Legal Action Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show legal action help dialog.
    /// </summary>
    private async Task ShowLegalActionHelp()
    {
        await DialogService.ShowAsync<LegalActionsHelpDialog>("Legal Action Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
