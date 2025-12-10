namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AmlAlerts;

public partial class AmlAlerts
{
    static AmlAlerts()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for AmlAlertResponse -> AmlAlertViewModel mapping
        TypeAdapterConfig<AmlAlertResponse, AmlAlertViewModel>.NewConfig()
            .Map(dest => dest.DetectedDate, src => src.DetectedDate.HasValue ? src.DetectedDate.Value.DateTime : (DateTime?)null)
            .Map(dest => dest.ResolvedDate, src => src.ResolvedDate.HasValue ? src.ResolvedDate.Value.DateTime : (DateTime?)null);
    }

    protected EntityServerTableContext<AmlAlertResponse, DefaultIdType, AmlAlertViewModel> Context { get; set; } = null!;
    private EntityTable<AmlAlertResponse, DefaultIdType, AmlAlertViewModel> _table = null!;

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

        Context = new EntityServerTableContext<AmlAlertResponse, DefaultIdType, AmlAlertViewModel>(
            fields:
            [
                new EntityField<AmlAlertResponse>(dto => dto.AlertCode, "Alert Code", "AlertCode"),
                new EntityField<AmlAlertResponse>(dto => dto.AlertType, "Type", "AlertType"),
                new EntityField<AmlAlertResponse>(dto => dto.Severity, "Severity", "Severity"),
                new EntityField<AmlAlertResponse>(dto => dto.TransactionAmount, "Amount", "TransactionAmount", typeof(decimal)),
                new EntityField<AmlAlertResponse>(dto => dto.AlertedAt, "Alerted At", "AlertedAt", typeof(DateTime)),
                new EntityField<AmlAlertResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchAmlAlertsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchAmlAlertsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<AmlAlertResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateAmlAlertAsync("1", viewModel.Adapt<CreateAmlAlertCommand>()).ConfigureAwait(false);
            },
            entityName: "AML Alert",
            entityNamePlural: "AML Alerts",
            entityResource: FshResources.AmlAlerts,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var alert = await Client.GetAmlAlertAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Alert", alert } };
        await DialogService.ShowAsync<AmlAlertDetailsDialog>("AML Alert Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show AML Alerts help dialog.
    /// </summary>
    private async Task ShowAmlAlertsHelp()
    {
        await DialogService.ShowAsync<AmlAlertsHelpDialog>("AML Alerts Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
