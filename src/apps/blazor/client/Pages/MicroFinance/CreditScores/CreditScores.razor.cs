namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditScores;

public partial class CreditScores
{
    static CreditScores()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for CreditScoreSummaryResponse -> CreditScoreViewModel mapping
        TypeAdapterConfig<CreditScoreSummaryResponse, CreditScoreViewModel>.NewConfig()
            .Map(dest => dest.ScoreDate, src => src.ScoreDate.HasValue ? src.ScoreDate.Value.DateTime : (DateTime?)null);
    }

    protected EntityServerTableContext<CreditScoreSummaryResponse, DefaultIdType, CreditScoreViewModel> Context { get; set; } = null!;
    private EntityTable<CreditScoreSummaryResponse, DefaultIdType, CreditScoreViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CreditScoreSummaryResponse, DefaultIdType, CreditScoreViewModel>(
            fields:
            [
                new EntityField<CreditScoreSummaryResponse>(dto => dto.MemberId, "Member", "MemberId"),
                new EntityField<CreditScoreSummaryResponse>(dto => dto.ScoreType, "Type", "ScoreType"),
                new EntityField<CreditScoreSummaryResponse>(dto => dto.ScoreModel, "Model", "ScoreModel"),
                new EntityField<CreditScoreSummaryResponse>(dto => dto.Score, "Score", "Score", typeof(decimal)),
                new EntityField<CreditScoreSummaryResponse>(dto => dto.Grade, "Grade", "Grade"),
                new EntityField<CreditScoreSummaryResponse>(dto => dto.ProbabilityOfDefault, "PD%", "ProbabilityOfDefault", typeof(decimal)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCreditScoresCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCreditScoresAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CreditScoreSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCreditScoreAsync("1", viewModel.Adapt<CreateCreditScoreCommand>()).ConfigureAwait(false);
            },
            entityName: "Credit Score",
            entityNamePlural: "Credit Scores",
            entityResource: FshResources.CreditScores,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var score = await Client.GetCreditScoreAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Score", score } };
        await DialogService.ShowAsync<CreditScoreDetailsDialog>("Credit Score Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show Credit Scores help dialog.
    /// </summary>
    private async Task ShowCreditScoresHelp()
    {
        await DialogService.ShowAsync<CreditScoresHelpDialog>("Credit Scores Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
