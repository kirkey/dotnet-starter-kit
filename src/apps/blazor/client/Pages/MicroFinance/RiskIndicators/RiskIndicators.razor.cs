namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.RiskIndicators;

public partial class RiskIndicators
{
    protected EntityServerTableContext<RiskIndicatorSummaryResponse, DefaultIdType, RiskIndicatorViewModel> Context { get; set; } = null!;
    private EntityTable<RiskIndicatorSummaryResponse, DefaultIdType, RiskIndicatorViewModel> _table = null!;

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

        Context = new EntityServerTableContext<RiskIndicatorSummaryResponse, DefaultIdType, RiskIndicatorViewModel>(
            fields:
            [
                new EntityField<RiskIndicatorSummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<RiskIndicatorSummaryResponse>(dto => dto.Formula, "Formula", "Formula"),
                new EntityField<RiskIndicatorSummaryResponse>(dto => dto.Unit, "Unit", "Unit"),
                new EntityField<RiskIndicatorSummaryResponse>(dto => dto.Direction, "Direction", "Direction"),
                new EntityField<RiskIndicatorSummaryResponse>(dto => dto.TargetValue, "Target", "TargetValue", typeof(decimal)),
                new EntityField<RiskIndicatorSummaryResponse>(dto => dto.Frequency, "Frequency", "Frequency"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchRiskIndicatorsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchRiskIndicatorsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<RiskIndicatorSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateRiskIndicatorAsync("1", viewModel.Adapt<CreateRiskIndicatorCommand>()).ConfigureAwait(false);
            },
            entityName: "Risk Indicator",
            entityNamePlural: "Risk Indicators",
            entityResource: FshResources.RiskIndicators,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var indicator = await Client.GetRiskIndicatorAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Indicator", indicator } };
        await DialogService.ShowAsync<RiskIndicatorDetailsDialog>("Risk Indicator Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
