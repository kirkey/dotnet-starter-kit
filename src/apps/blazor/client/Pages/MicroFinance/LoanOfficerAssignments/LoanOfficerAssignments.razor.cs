namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanOfficerAssignments;

/// <summary>
/// Loan Officer Assignments page logic. Manages loan officer to customer/loan assignments.
/// </summary>
public partial class LoanOfficerAssignments
{
    protected EntityServerTableContext<LoanOfficerAssignmentSummaryResponse, DefaultIdType, LoanOfficerAssignmentViewModel> Context { get; set; } = null!;
    private EntityTable<LoanOfficerAssignmentSummaryResponse, DefaultIdType, LoanOfficerAssignmentViewModel> _table = null!;

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

        Context = new EntityServerTableContext<LoanOfficerAssignmentSummaryResponse, DefaultIdType, LoanOfficerAssignmentViewModel>(
            fields:
            [
                new EntityField<LoanOfficerAssignmentSummaryResponse>(dto => dto.StaffId, "Staff", "StaffId"),
                new EntityField<LoanOfficerAssignmentSummaryResponse>(dto => dto.AssignmentType, "Type", "AssignmentType"),
                new EntityField<LoanOfficerAssignmentSummaryResponse>(dto => dto.MemberId, "Member", "MemberId"),
                new EntityField<LoanOfficerAssignmentSummaryResponse>(dto => dto.AssignmentDate, "Assignment Date", "AssignmentDate", typeof(DateTimeOffset)),
                new EntityField<LoanOfficerAssignmentSummaryResponse>(dto => dto.IsPrimary, "Primary", "IsPrimary", typeof(bool)),
                new EntityField<LoanOfficerAssignmentSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanOfficerAssignmentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanOfficerAssignmentsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanOfficerAssignmentSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Loan Officer Assignment",
            entityNamePlural: "Loan Officer Assignments",
            entityResource: FshResources.LoanOfficerAssignments,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var assignment = await Client.GetLoanOfficerAssignmentAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Assignment", assignment }
        };

        await DialogService.ShowAsync<LoanOfficerAssignmentDetailsDialog>("Loan Officer Assignment Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
