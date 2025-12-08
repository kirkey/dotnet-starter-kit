namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.UssdSessions;

public partial class UssdSessions
{
    protected EntityServerTableContext<UssdSessionResponse, DefaultIdType, UssdSessionViewModel> Context { get; set; } = null!;
    private EntityTable<UssdSessionResponse, DefaultIdType, UssdSessionViewModel> _table = null!;

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

        Context = new EntityServerTableContext<UssdSessionResponse, DefaultIdType, UssdSessionViewModel>(
            fields:
            [
                new EntityField<UssdSessionResponse>(dto => dto.SessionId, "Session ID", "SessionId"),
                new EntityField<UssdSessionResponse>(dto => dto.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<UssdSessionResponse>(dto => dto.ServiceCode, "Service Code", "ServiceCode"),
                new EntityField<UssdSessionResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<UssdSessionResponse>(dto => dto.CurrentMenu, "Current Menu", "CurrentMenu"),
                new EntityField<UssdSessionResponse>(dto => dto.StartedAt, "Started", "StartedAt", typeof(DateTime)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchUssdSessionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchUssdSessionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<UssdSessionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async vm =>
            {
                var command = new CreateUssdSessionCommand
                {
                    SessionId = vm.SessionId,
                    PhoneNumber = vm.PhoneNumber,
                    ServiceCode = vm.ServiceCode,
                    MemberId = vm.MemberId,
                    WalletId = vm.WalletId,
                    Language = vm.Language
                };
                await Client.CreateUssdSessionAsync("1", command).ConfigureAwait(false);
            },
            entityName: "USSD Session",
            entityNamePlural: "USSD Sessions",
            entityResource: FshResources.UssdSessions,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var session = await Client.GetUssdSessionAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "UssdSession", session } };
        await DialogService.ShowAsync<UssdSessionDetailsDialog>("USSD Session Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show USSD session help dialog.
    /// </summary>
    private async Task ShowUssdSessionHelp()
    {
        await DialogService.ShowAsync<UssdSessionHelpDialog>("USSD Session Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
