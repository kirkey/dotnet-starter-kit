namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalRequests;

public partial class ApprovalRequests
{
    protected EntityServerTableContext<ApprovalRequestSummaryResponse, DefaultIdType, ApprovalRequestViewModel> Context { get; set; } = null!;
    private EntityTable<ApprovalRequestSummaryResponse, DefaultIdType, ApprovalRequestViewModel> _table = null!;

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

        Context = new EntityServerTableContext<ApprovalRequestSummaryResponse, DefaultIdType, ApprovalRequestViewModel>(
            fields:
            [
                new EntityField<ApprovalRequestSummaryResponse>(dto => dto.RequestNumber, "Request #", "RequestNumber"),
                new EntityField<ApprovalRequestSummaryResponse>(dto => dto.EntityType, "Entity Type", "EntityType"),
                new EntityField<ApprovalRequestSummaryResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<ApprovalRequestSummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<ApprovalRequestSummaryResponse>(dto => dto.CurrentLevel, "Level", "CurrentLevel", typeof(int)),
                new EntityField<ApprovalRequestSummaryResponse>(dto => dto.SubmittedAt, "Submitted", "SubmittedAt", typeof(DateTime)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchApprovalRequestsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchApprovalRequestsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ApprovalRequestSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateApprovalRequestAsync("1", viewModel.Adapt<CreateApprovalRequestCommand>()).ConfigureAwait(false);
            },
            entityName: "Approval Request",
            entityNamePlural: "Approval Requests",
            entityResource: FshResources.ApprovalRequests,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var approvalRequest = await Client.GetApprovalRequestAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "ApprovalRequest", approvalRequest } };
        await DialogService.ShowAsync<ApprovalRequestDetailsDialog>("Approval Request Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show approval request help dialog.
    /// </summary>
    private async Task ShowApprovalRequestHelp()
    {
        await DialogService.ShowAsync<ApprovalRequestsHelpDialog>("Approval Request Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
