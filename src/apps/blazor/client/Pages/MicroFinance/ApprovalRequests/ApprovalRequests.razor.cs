using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalRequests.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ApprovalRequests;

public partial class ApprovalRequests
{
    [Inject]
    private IMicroFinanceClient MicroFinanceClient { get; set; } = null!;
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<ApprovalRequestResponse, DefaultIdType, ApprovalRequestViewModel> _context = null!;
    private EntityTable<ApprovalRequestResponse, DefaultIdType, ApprovalRequestViewModel>? _table;

    private int _elevation;
    private string _borderRadius = string.Empty;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canView;
    private bool _canApprove;
    private bool _canCancel;

    protected override async Task OnInitializedAsync()
    {
        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        var authState = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermissions.ApprovalRequestsCreate)).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermissions.ApprovalRequestsUpdate)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermissions.ApprovalRequestsDelete)).Succeeded;
        _canView = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermissions.ApprovalRequestsView)).Succeeded;
        _canApprove = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermissions.ApprovalRequestsApprove)).Succeeded;
        _canCancel = _canUpdate;

        _context = new EntityServerTableContext<ApprovalRequestResponse, DefaultIdType, ApprovalRequestViewModel>(
            entityName: "Approval Request",
            entityNamePlural: "Approval Requests",
            entityResource: FshResources.MicroFinance,
            searchAction: FshActions.View,
            fields:
            [
                new EntityField<ApprovalRequestResponse>(e => e.RequestNumber, "Request #", "RequestNumber"),
                new EntityField<ApprovalRequestResponse>(e => e.EntityType, "Entity Type", "EntityType"),
                new EntityField<ApprovalRequestResponse>(e => e.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<ApprovalRequestResponse>(e => $"{e.CurrentLevel}/{e.TotalLevels}", "Level", "CurrentLevel"),
                new EntityField<ApprovalRequestResponse>(e => e.SubmittedByName, "Submitted By", "SubmittedByName"),
                new EntityField<ApprovalRequestResponse>(e => e.Status, "Status", "Status"),
                new EntityField<ApprovalRequestResponse>(e => e.SubmittedAt, "Submitted At", "SubmittedAt", typeof(DateTimeOffset)),
            ],
            idFunc: e => e.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await MicroFinanceClient.SearchApprovalRequestsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<ApprovalRequestResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateApprovalRequestCommand>();
                await MicroFinanceClient.CreateApprovalRequestAsync("1", command);
            },
            getDefaultsFunc: () => Task.FromResult(new ApprovalRequestViewModel { TotalLevels = 1, CurrentLevel = 1 }),
            hasExtraActionsFunc: () => true,
            canCreateEntityFunc: () => _canCreate,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private async Task ViewDetails(ApprovalRequestResponse request)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApprovalRequestDetailsDialog.Request), request }
        };
        await DialogService.ShowAsync<ApprovalRequestDetailsDialog>("Approval Request Details", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }

    private async Task Approve(ApprovalRequestResponse request)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApprovalRequestApproveDialog.RequestId), request.Id },
            { nameof(ApprovalRequestApproveDialog.RequestNumber), request.RequestNumber },
            { nameof(ApprovalRequestApproveDialog.CurrentLevel), request.CurrentLevel },
            { nameof(ApprovalRequestApproveDialog.TotalLevels), request.TotalLevels }
        };
        var dialog = await DialogService.ShowAsync<ApprovalRequestApproveDialog>("Approve Request", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task Reject(ApprovalRequestResponse request)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApprovalRequestRejectDialog.RequestId), request.Id },
            { nameof(ApprovalRequestRejectDialog.RequestNumber), request.RequestNumber }
        };
        var dialog = await DialogService.ShowAsync<ApprovalRequestRejectDialog>("Reject Request", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task Cancel(ApprovalRequestResponse request)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApprovalRequestCancelDialog.RequestId), request.Id },
            { nameof(ApprovalRequestCancelDialog.RequestNumber), request.RequestNumber }
        };
        var dialog = await DialogService.ShowAsync<ApprovalRequestCancelDialog>("Cancel Request", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<ApprovalRequestsHelpDialog>("Approval Requests Help",
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}
