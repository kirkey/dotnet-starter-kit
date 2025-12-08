namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanDisbursementTranches;

/// <summary>
/// Loan Disbursement Tranches page logic. Manages loan disbursement schedules.
/// </summary>
public partial class LoanDisbursementTranches
{
    protected EntityServerTableContext<LoanDisbursementTrancheSummaryResponse, DefaultIdType, LoanDisbursementTrancheViewModel> Context { get; set; } = null!;
    private EntityTable<LoanDisbursementTrancheSummaryResponse, DefaultIdType, LoanDisbursementTrancheViewModel> _table = null!;

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

        Context = new EntityServerTableContext<LoanDisbursementTrancheSummaryResponse, DefaultIdType, LoanDisbursementTrancheViewModel>(
            fields:
            [
                new EntityField<LoanDisbursementTrancheSummaryResponse>(dto => dto.LoanId, "Loan ID", "LoanId"),
                new EntityField<LoanDisbursementTrancheSummaryResponse>(dto => dto.TrancheNumber, "Tranche #", "TrancheNumber", typeof(int)),
                new EntityField<LoanDisbursementTrancheSummaryResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<LoanDisbursementTrancheSummaryResponse>(dto => dto.ScheduledDate, "Scheduled Date", "ScheduledDate", typeof(DateTimeOffset)),
                new EntityField<LoanDisbursementTrancheSummaryResponse>(dto => dto.DisbursedDate, "Disbursed Date", "DisbursedDate", typeof(DateTimeOffset)),
                new EntityField<LoanDisbursementTrancheSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanDisbursementTranchesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanDisbursementTranchesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanDisbursementTrancheSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanDisbursementTrancheAsync("1", viewModel.Adapt<CreateLoanDisbursementTrancheCommand>()).ConfigureAwait(false);
            },
            entityName: "Loan Disbursement Tranche",
            entityNamePlural: "Loan Disbursement Tranches",
            entityResource: FshResources.LoanDisbursementTranches,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var tranche = await Client.GetLoanDisbursementTrancheAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Tranche", tranche }
        };

        await DialogService.ShowAsync<LoanDisbursementTrancheDetailsDialog>("Loan Disbursement Tranche Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
