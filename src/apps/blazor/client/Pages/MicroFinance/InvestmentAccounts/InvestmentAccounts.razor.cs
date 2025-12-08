namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentAccounts;

/// <summary>
/// Investment Accounts page logic. Manages customer investment accounts.
/// </summary>
public partial class InvestmentAccounts
{
    protected EntityServerTableContext<InvestmentAccountResponse, DefaultIdType, InvestmentAccountViewModel> Context { get; set; } = null!;
    private EntityTable<InvestmentAccountResponse, DefaultIdType, InvestmentAccountViewModel> _table = null!;

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

        Context = new EntityServerTableContext<InvestmentAccountResponse, DefaultIdType, InvestmentAccountViewModel>(
            fields:
            [
                new EntityField<InvestmentAccountResponse>(dto => dto.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<InvestmentAccountResponse>(dto => dto.MemberId, "Member", "MemberId"),
                new EntityField<InvestmentAccountResponse>(dto => dto.TotalInvested, "Invested", "TotalInvested", typeof(decimal)),
                new EntityField<InvestmentAccountResponse>(dto => dto.CurrentValue, "Current Value", "CurrentValue", typeof(decimal)),
                new EntityField<InvestmentAccountResponse>(dto => dto.FirstInvestmentDate, "First Investment", "FirstInvestmentDate", typeof(DateTimeOffset)),
                new EntityField<InvestmentAccountResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInvestmentAccountsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInvestmentAccountsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InvestmentAccountResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateInvestmentAccountAsync("1", viewModel.Adapt<CreateInvestmentAccountCommand>()).ConfigureAwait(false);
            },
            entityName: "Investment Account",
            entityNamePlural: "Investment Accounts",
            entityResource: FshResources.InvestmentAccounts,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var account = await Client.GetInvestmentAccountAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Account", account }
        };

        await DialogService.ShowAsync<InvestmentAccountDetailsDialog>("Investment Account Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
