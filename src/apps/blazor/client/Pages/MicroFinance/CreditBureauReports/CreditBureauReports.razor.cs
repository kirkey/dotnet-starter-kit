namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditBureauReports;

public partial class CreditBureauReports
{
    static CreditBureauReports()
    {
        // No TypeAdapterConfig needed - CreditBureauReportSummaryResponse uses DateTime which matches CreditBureauReportViewModel
    }

    protected EntityServerTableContext<CreditBureauReportSummaryResponse, DefaultIdType, CreditBureauReportViewModel> Context { get; set; } = null!;
    private EntityTable<CreditBureauReportSummaryResponse, DefaultIdType, CreditBureauReportViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CreditBureauReportSummaryResponse, DefaultIdType, CreditBureauReportViewModel>(
            fields:
            [
                new EntityField<CreditBureauReportSummaryResponse>(dto => dto.ReportNumber, "Report #", "ReportNumber"),
                new EntityField<CreditBureauReportSummaryResponse>(dto => dto.BureauName, "Bureau", "BureauName"),
                new EntityField<CreditBureauReportSummaryResponse>(dto => dto.ReportDate, "Report Date", "ReportDate", typeof(DateTime)),
                new EntityField<CreditBureauReportSummaryResponse>(dto => dto.CreditScore, "Credit Score", "CreditScore", typeof(int)),
                new EntityField<CreditBureauReportSummaryResponse>(dto => dto.RiskGrade, "Risk Grade", "RiskGrade"),
                new EntityField<CreditBureauReportSummaryResponse>(dto => dto.ActiveAccounts, "Active Accounts", "ActiveAccounts", typeof(int)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCreditBureauReportsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCreditBureauReportsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CreditBureauReportSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCreditBureauReportAsync("1", viewModel.Adapt<CreateCreditBureauReportCommand>()).ConfigureAwait(false);
            },
            entityName: "Credit Bureau Report",
            entityNamePlural: "Credit Bureau Reports",
            entityResource: FshResources.CreditBureauReports,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var report = await Client.GetCreditBureauReportAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Report", report } };
        await DialogService.ShowAsync<CreditBureauReportDetailsDialog>("Credit Bureau Report Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show Credit Bureau Reports help dialog.
    /// </summary>
    private async Task ShowCreditBureauReportsHelp()
    {
        await DialogService.ShowAsync<CreditBureauReportsHelpDialog>("Credit Bureau Reports Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
