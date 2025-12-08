namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalWorkflows;

public partial class ApprovalWorkflows
{
    protected EntityServerTableContext<ApprovalWorkflowSummaryResponse, DefaultIdType, ApprovalWorkflowViewModel> Context { get; set; } = null!;
    private EntityTable<ApprovalWorkflowSummaryResponse, DefaultIdType, ApprovalWorkflowViewModel> _table = null!;

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

        Context = new EntityServerTableContext<ApprovalWorkflowSummaryResponse, DefaultIdType, ApprovalWorkflowViewModel>(
            fields:
            [
                new EntityField<ApprovalWorkflowSummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<ApprovalWorkflowSummaryResponse>(dto => dto.EntityType, "Entity Type", "EntityType"),
                new EntityField<ApprovalWorkflowSummaryResponse>(dto => dto.NumberOfLevels, "Levels", "NumberOfLevels", typeof(int)),
                new EntityField<ApprovalWorkflowSummaryResponse>(dto => dto.MinAmount, "Min Amount", "MinAmount", typeof(decimal)),
                new EntityField<ApprovalWorkflowSummaryResponse>(dto => dto.MaxAmount, "Max Amount", "MaxAmount", typeof(decimal)),
                new EntityField<ApprovalWorkflowSummaryResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchApprovalWorkflowsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchApprovalWorkflowsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ApprovalWorkflowSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateApprovalWorkflowAsync("1", viewModel.Adapt<CreateApprovalWorkflowCommand>()).ConfigureAwait(false);
            },
            entityName: "Approval Workflow",
            entityNamePlural: "Approval Workflows",
            entityResource: FshResources.ApprovalWorkflows,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var workflow = await Client.GetApprovalWorkflowAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Workflow", workflow } };
        await DialogService.ShowAsync<ApprovalWorkflowDetailsDialog>("Approval Workflow Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
