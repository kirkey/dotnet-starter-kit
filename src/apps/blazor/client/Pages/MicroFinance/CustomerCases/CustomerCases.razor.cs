namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerCases;

public partial class CustomerCases
{
    protected EntityServerTableContext<CustomerCaseSummaryResponse, DefaultIdType, CustomerCaseViewModel> Context { get; set; } = null!;
    private EntityTable<CustomerCaseSummaryResponse, DefaultIdType, CustomerCaseViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CustomerCaseSummaryResponse, DefaultIdType, CustomerCaseViewModel>(
            fields:
            [
                new EntityField<CustomerCaseSummaryResponse>(dto => dto.CaseNumber, "Case #", "CaseNumber"),
                new EntityField<CustomerCaseSummaryResponse>(dto => dto.Subject, "Subject", "Subject"),
                new EntityField<CustomerCaseSummaryResponse>(dto => dto.Category, "Category", "Category"),
                new EntityField<CustomerCaseSummaryResponse>(dto => dto.Priority, "Priority", "Priority"),
                new EntityField<CustomerCaseSummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<CustomerCaseSummaryResponse>(dto => dto.Channel, "Channel", "Channel"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCustomerCasesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCustomerCasesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CustomerCaseSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCustomerCaseAsync("1", viewModel.Adapt<CreateCustomerCaseCommand>()).ConfigureAwait(false);
            },
            entityName: "Customer Case",
            entityNamePlural: "Customer Cases",
            entityResource: FshResources.CustomerCases,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var customerCase = await Client.GetCustomerCaseAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "CustomerCase", customerCase } };
        await DialogService.ShowAsync<CustomerCaseDetailsDialog>("Customer Case Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show customer case help dialog.
    /// </summary>
    private async Task ShowCustomerCaseHelp()
    {
        await DialogService.ShowAsync<CustomerCasesHelpDialog>("Customer Case Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
