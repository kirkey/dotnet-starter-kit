using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalWorkflows.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalWorkflows;

public partial class ApprovalWorkflows
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;
    [Inject]
    private ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<ApprovalWorkflowResponse, DefaultIdType, ApprovalWorkflowViewModel> _context = null!;
    private EntityTable<ApprovalWorkflowResponse, DefaultIdType, ApprovalWorkflowViewModel>? _table;

    private int _elevation;
    private string _borderRadius = string.Empty;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canView;
    private bool _canActivate;
    private bool _canDeactivate;

    protected override async Task OnInitializedAsync()
    {
        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        var authState = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Create, FshResources.ApprovalWorkflows))).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Update, FshResources.ApprovalWorkflows))).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Delete, FshResources.ApprovalWorkflows))).Succeeded;
        _canView = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.View, FshResources.ApprovalWorkflows))).Succeeded;
        _canActivate = _canUpdate;
        _canDeactivate = _canUpdate;

        _context = new EntityServerTableContext<ApprovalWorkflowResponse, DefaultIdType, ApprovalWorkflowViewModel>(
            entityName: "Approval Workflow",
            entityNamePlural: "Approval Workflows",
            entityResource: FshResources.ApprovalWorkflows,
            fields:
            [
                new EntityField<ApprovalWorkflowResponse>(e => e.Code, "Code", "Code"),
                new EntityField<ApprovalWorkflowResponse>(e => e.Name, "Name", "Name"),
                new EntityField<ApprovalWorkflowResponse>(e => e.EntityType, "Entity Type", "EntityType"),
                new EntityField<ApprovalWorkflowResponse>(e => e.NumberOfLevels, "Levels", "NumberOfLevels", typeof(int)),
                new EntityField<ApprovalWorkflowResponse>(e => e.IsSequential ? "Sequential" : "Parallel", "Mode", "IsSequential"),
                new EntityField<ApprovalWorkflowResponse>(e => e.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<ApprovalWorkflowResponse>(e => e.IsActive ? "Active" : "Inactive", "Status", "IsActive"),
            ],
            idFunc: e => e.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await Client.SearchApprovalWorkflowsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<ApprovalWorkflowResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateApprovalWorkflowCommand>();
                await Client.CreateApprovalWorkflowAsync("1", command);
            },
            getDefaultsFunc: () => Task.FromResult(new ApprovalWorkflowViewModel { IsSequential = true, Priority = 100 }),
            hasExtraActionsFunc: () => true,
            canCreateEntityFunc: () => _canCreate,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private async Task ViewDetails(ApprovalWorkflowResponse workflow)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApprovalWorkflowDetailsDialog.Workflow), workflow }
        };
        await DialogService.ShowAsync<ApprovalWorkflowDetailsDialog>("Approval Workflow Details", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }

    private async Task Activate(ApprovalWorkflowResponse workflow)
    {
        var command = new ActivateApprovalWorkflowCommand { Id = workflow.Id };
        await ApiHelper.ExecuteCallGuardedAsync(
            async () => await Client.ActivateApprovalWorkflowEndpointAsync("1", workflow.Id, command),
            Snackbar,
            successMessage: "Workflow activated successfully.");
        await _table!.ReloadDataAsync();
    }

    private async Task Deactivate(ApprovalWorkflowResponse workflow)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApprovalWorkflowDeactivateDialog.WorkflowId), workflow.Id },
            { nameof(ApprovalWorkflowDeactivateDialog.WorkflowName), workflow.Name }
        };
        var dialog = await DialogService.ShowAsync<ApprovalWorkflowDeactivateDialog>("Deactivate Workflow", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<ApprovalWorkflowsHelpDialog>("Approval Workflows Help",
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}
