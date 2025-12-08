namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.RiskAlerts;

public partial class RiskAlerts
{
    protected EntityServerTableContext<RiskAlertSummaryResponse, DefaultIdType, RiskAlertViewModel> Context { get; set; } = null!;
    private EntityTable<RiskAlertSummaryResponse, DefaultIdType, RiskAlertViewModel> _table = null!;

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

        Context = new EntityServerTableContext<RiskAlertSummaryResponse, DefaultIdType, RiskAlertViewModel>(
            fields:
            [
                new EntityField<RiskAlertSummaryResponse>(dto => dto.AlertNumber, "Alert #", "AlertNumber"),
                new EntityField<RiskAlertSummaryResponse>(dto => dto.Title, "Title", "Title"),
                new EntityField<RiskAlertSummaryResponse>(dto => dto.Severity, "Severity", "Severity"),
                new EntityField<RiskAlertSummaryResponse>(dto => dto.Source, "Source", "Source"),
                new EntityField<RiskAlertSummaryResponse>(dto => dto.AlertedAt, "Alerted At", "AlertedAt", typeof(DateTime)),
                new EntityField<RiskAlertSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchRiskAlertsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchRiskAlertsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<RiskAlertSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateRiskAlertAsync("1", viewModel.Adapt<CreateRiskAlertCommand>()).ConfigureAwait(false);
            },
            entityName: "Risk Alert",
            entityNamePlural: "Risk Alerts",
            entityResource: FshResources.RiskAlerts,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var alert = await Client.GetRiskAlertAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Alert", alert } };
        await DialogService.ShowAsync<RiskAlertDetailsDialog>("Risk Alert Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
