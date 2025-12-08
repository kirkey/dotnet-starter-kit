namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRestructures;

/// <summary>
/// Loan Restructures page logic. Manages loan restructuring requests.
/// </summary>
public partial class LoanRestructures
{
    protected EntityServerTableContext<LoanRestructureSummaryResponse, DefaultIdType, LoanRestructureViewModel> Context { get; set; } = null!;
    private EntityTable<LoanRestructureSummaryResponse, DefaultIdType, LoanRestructureViewModel> _table = null!;

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

        Context = new EntityServerTableContext<LoanRestructureSummaryResponse, DefaultIdType, LoanRestructureViewModel>(
            fields:
            [
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.RestructureNumber, "Number", "RestructureNumber"),
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.LoanId, "Loan", "LoanId"),
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.RestructureType, "Type", "RestructureType"),
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.RequestDate, "Request Date", "RequestDate", typeof(DateTimeOffset)),
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.OriginalPrincipal, "Original Principal", "OriginalPrincipal", typeof(decimal)),
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.NewPrincipal, "New Principal", "NewPrincipal", typeof(decimal)),
                new EntityField<LoanRestructureSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanRestructuresCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanRestructuresAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanRestructureSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanRestructureAsync("1", viewModel.Adapt<CreateLoanRestructureCommand>()).ConfigureAwait(false);
            },
            entityName: "Loan Restructure",
            entityNamePlural: "Loan Restructures",
            entityResource: FshResources.LoanRestructures,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var restructure = await Client.GetLoanRestructureAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Restructure", restructure }
        };

        await DialogService.ShowAsync<LoanRestructureDetailsDialog>("Loan Restructure Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show loan restructures help dialog.
    /// </summary>
    private async Task ShowLoanRestructuresHelp()
    {
        await DialogService.ShowAsync<LoanRestructuresHelpDialog>("Loan Restructures Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
