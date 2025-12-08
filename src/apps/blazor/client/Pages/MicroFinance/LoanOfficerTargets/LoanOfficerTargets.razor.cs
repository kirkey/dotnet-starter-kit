namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanOfficerTargets;

/// <summary>
/// Loan Officer Targets page logic. Manages performance targets for loan officers.
/// </summary>
public partial class LoanOfficerTargets
{
    protected EntityServerTableContext<LoanOfficerTargetSummaryResponse, DefaultIdType, LoanOfficerTargetViewModel> Context { get; set; } = null!;
    private EntityTable<LoanOfficerTargetSummaryResponse, DefaultIdType, LoanOfficerTargetViewModel> _table = null!;

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

        Context = new EntityServerTableContext<LoanOfficerTargetSummaryResponse, DefaultIdType, LoanOfficerTargetViewModel>(
            fields:
            [
                new EntityField<LoanOfficerTargetSummaryResponse>(dto => dto.StaffId, "Staff", "StaffId"),
                new EntityField<LoanOfficerTargetSummaryResponse>(dto => dto.TargetType, "Target Type", "TargetType"),
                new EntityField<LoanOfficerTargetSummaryResponse>(dto => dto.Period, "Period", "Period"),
                new EntityField<LoanOfficerTargetSummaryResponse>(dto => dto.TargetValue, "Target", "TargetValue", typeof(decimal)),
                new EntityField<LoanOfficerTargetSummaryResponse>(dto => dto.AchievedValue, "Achieved", "AchievedValue", typeof(decimal)),
                new EntityField<LoanOfficerTargetSummaryResponse>(dto => dto.MetricUnit, "Unit", "MetricUnit"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanOfficerTargetsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanOfficerTargetsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanOfficerTargetSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanOfficerTargetAsync("1", viewModel.Adapt<CreateLoanOfficerTargetCommand>()).ConfigureAwait(false);
            },
            entityName: "Loan Officer Target",
            entityNamePlural: "Loan Officer Targets",
            entityResource: FshResources.LoanOfficerTargets,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var target = await Client.GetLoanOfficerTargetAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Target", target }
        };

        await DialogService.ShowAsync<LoanOfficerTargetDetailsDialog>("Loan Officer Target Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show loan officer target help dialog.
    /// </summary>
    private async Task ShowLoanOfficerTargetHelp()
    {
        await DialogService.ShowAsync<LoanOfficerTargetHelpDialog>("Loan Officer Target Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
