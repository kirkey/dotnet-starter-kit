namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CommunicationLogs;

public partial class CommunicationLogs
{
    protected EntityServerTableContext<CommunicationLogSummaryResponse, DefaultIdType, CommunicationLogViewModel> Context { get; set; } = null!;
    private EntityTable<CommunicationLogSummaryResponse, DefaultIdType, CommunicationLogViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CommunicationLogSummaryResponse, DefaultIdType, CommunicationLogViewModel>(
            fields:
            [
                new EntityField<CommunicationLogSummaryResponse>(dto => dto.Channel, "Channel", "Channel"),
                new EntityField<CommunicationLogSummaryResponse>(dto => dto.Recipient, "Recipient", "Recipient"),
                new EntityField<CommunicationLogSummaryResponse>(dto => dto.Subject, "Subject", "Subject"),
                new EntityField<CommunicationLogSummaryResponse>(dto => dto.DeliveryStatus, "Status", "DeliveryStatus"),
                new EntityField<CommunicationLogSummaryResponse>(dto => dto.SentAt, "Sent At", "SentAt", typeof(DateTime)),
                new EntityField<CommunicationLogSummaryResponse>(dto => dto.RetryCount, "Retries", "RetryCount", typeof(int)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCommunicationLogsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCommunicationLogsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CommunicationLogSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCommunicationLogAsync("1", viewModel.Adapt<CreateCommunicationLogCommand>()).ConfigureAwait(false);
            },
            entityName: "Communication Log",
            entityNamePlural: "Communication Logs",
            entityResource: FshResources.CommunicationLogs,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var log = await Client.GetCommunicationLogAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Log", log } };
        await DialogService.ShowAsync<CommunicationLogDetailsDialog>("Communication Log Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show communication log help dialog.
    /// </summary>
    private async Task ShowCommunicationLogHelp()
    {
        await DialogService.ShowAsync<CommunicationLogHelpDialog>("Communication Log Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
