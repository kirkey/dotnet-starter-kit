namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AgentBankings;

public partial class AgentBankings
{
    protected EntityServerTableContext<AgentBankingResponse, DefaultIdType, AgentBankingViewModel> Context { get; set; } = null!;
    private EntityTable<AgentBankingResponse, DefaultIdType, AgentBankingViewModel> _table = null!;

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

        Context = new EntityServerTableContext<AgentBankingResponse, DefaultIdType, AgentBankingViewModel>(
            fields:
            [
                new EntityField<AgentBankingResponse>(dto => dto.AgentCode, "Code", "AgentCode"),
                new EntityField<AgentBankingResponse>(dto => dto.BusinessName, "Business Name", "BusinessName"),
                new EntityField<AgentBankingResponse>(dto => dto.ContactName, "Contact", "ContactName"),
                new EntityField<AgentBankingResponse>(dto => dto.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<AgentBankingResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<AgentBankingResponse>(dto => dto.FloatBalance, "Float Balance", "FloatBalance", typeof(decimal)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchAgentBankingsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchAgentBankingsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<AgentBankingResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateAgentBankingCommand
                {
                    AgentCode = vm.AgentCode,
                    BusinessName = vm.BusinessName,
                    ContactName = vm.ContactName,
                    PhoneNumber = vm.PhoneNumber,
                    Address = vm.Address,
                    CommissionRate = vm.CommissionRate,
                    DailyTransactionLimit = vm.DailyTransactionLimit,
                    MonthlyTransactionLimit = vm.MonthlyTransactionLimit,
                    ContractStartDate = vm.ContractStartDate,
                    BranchId = vm.BranchId,
                    Email = vm.Email,
                    GpsCoordinates = vm.GpsCoordinates,
                    OperatingHours = vm.OperatingHours
                };
                var result = await Client.CreateAgentBankingAsync("1", command).ConfigureAwait(false);
                return result.Id;
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateAgentBankingCommand
                {
                    Id = id,
                    AgentCode = vm.AgentCode,
                    BusinessName = vm.BusinessName,
                    ContactName = vm.ContactName,
                    PhoneNumber = vm.PhoneNumber,
                    Address = vm.Address,
                    CommissionRate = vm.CommissionRate,
                    DailyTransactionLimit = vm.DailyTransactionLimit,
                    MonthlyTransactionLimit = vm.MonthlyTransactionLimit,
                    Email = vm.Email,
                    GpsCoordinates = vm.GpsCoordinates,
                    OperatingHours = vm.OperatingHours
                };
                await Client.UpdateAgentBankingAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteAgentBankingAsync("1", id).ConfigureAwait(false),
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Agent Banking",
            entityNamePlural: "Agent Bankings",
            entityResource: FshResources.AgentBankings,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var agent = await Client.GetAgentBankingAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "AgentBanking", agent } };
        await DialogService.ShowAsync<AgentBankingDetailsDialog>("Agent Banking Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
