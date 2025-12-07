using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.AmlAlerts.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AmlAlerts;

public partial class AmlAlerts
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private IMicroFinanceClient MicroFinanceClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private ClientPreference _clientPreference = new();
    private EntityTable<AmlAlertResponse, DefaultIdType, AmlAlertViewModel>? _table;
    private EntityServerTableContext<AmlAlertResponse, DefaultIdType, AmlAlertViewModel> _context = null!;

    private bool _canCreate;
    private bool _canAssign;
    private bool _canConfirm;
    private bool _canClear;
    private bool _canEscalate;
    private bool _canFileSar;
    private bool _canClose;

    private string? _selectedStatus;
    private string? _selectedSeverity;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.Create)).Succeeded;
        _canAssign = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.Assign)).Succeeded;
        _canConfirm = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.Confirm)).Succeeded;
        _canClear = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.Clear)).Succeeded;
        _canEscalate = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.Escalate)).Succeeded;
        _canFileSar = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.FileSar)).Succeeded;
        _canClose = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.AmlAlerts.Close)).Succeeded;

        _context = new EntityServerTableContext<AmlAlertResponse, DefaultIdType, AmlAlertViewModel>(
            entityName: "AML Alert",
            entityNamePlural: "AML Alerts",
            entityResource: FSHResources.AmlAlerts,
            fields: new List<EntityField<AmlAlertResponse>>
            {
                new EntityField<AmlAlertResponse>(a => a.AlertCode, "Alert Code", "AlertCode"),
                new EntityField<AmlAlertResponse>(a => a.AlertType, "Type", "AlertType"),
                new EntityField<AmlAlertResponse>(a => a.Severity, "Severity", "Severity"),
                new EntityField<AmlAlertResponse>(a => a.MemberName, "Member", "MemberName"),
                new EntityField<AmlAlertResponse>(a => a.TransactionAmount, "Amount", "TransactionAmount", typeof(decimal)),
                new EntityField<AmlAlertResponse>(a => a.Status, "Status", "Status"),
                new EntityField<AmlAlertResponse>(a => a.AssignedTo, "Assigned To", "AssignedTo"),
                new EntityField<AmlAlertResponse>(a => a.CreatedOn, "Created", "CreatedOn", typeof(DateTime))
            },
            idFunc: a => a.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await MicroFinanceClient.SearchAmlAlertsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<AmlAlertResponse>>();
            },
            getDetailsFunc: async id => (await MicroFinanceClient.GetAmlAlertEndpointAsync("1", id)).Adapt<AmlAlertViewModel>(),
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateAmlAlertCommand>();
                await MicroFinanceClient.CreateAmlAlertEndpointAsync("1", command);
            },
            deleteFunc: async id => await MicroFinanceClient.DeleteAmlAlertEndpointAsync("1", id),
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: _ => false,
            canDeleteEntityFunc: _ => false);
    }

    private Color GetSeverityColor(string? severity) => severity switch
    {
        "Critical" => Color.Error,
        "High" => Color.Warning,
        "Medium" => Color.Info,
        "Low" => Color.Default,
        _ => Color.Default
    };

    private Color GetStatusColor(string? status) => status switch
    {
        "New" => Color.Warning,
        "Assigned" => Color.Info,
        "UnderReview" => Color.Primary,
        "Confirmed" => Color.Error,
        "Escalated" => Color.Error,
        "Cleared" => Color.Success,
        "Closed" => Color.Default,
        _ => Color.Default
    };

    private async Task ShowDetails(AmlAlertResponse alert)
    {
        var parameters = new DialogParameters<AmlAlertDetailsDialog>
        {
            { x => x.Alert, alert }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<AmlAlertDetailsDialog>("AML Alert Details", parameters, options);
    }

    private async Task AssignAlert(AmlAlertResponse alert)
    {
        var parameters = new DialogParameters<AmlAlertAssignDialog>
        {
            { x => x.AlertId, alert.Id },
            { x => x.AlertCode, alert.AlertCode }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AmlAlertAssignDialog>("Assign Alert", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ConfirmAlert(AmlAlertResponse alert)
    {
        var parameters = new DialogParameters<AmlAlertConfirmDialog>
        {
            { x => x.AlertId, alert.Id },
            { x => x.AlertCode, alert.AlertCode }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AmlAlertConfirmDialog>("Confirm Alert", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ClearAlert(AmlAlertResponse alert)
    {
        var parameters = new DialogParameters<AmlAlertClearDialog>
        {
            { x => x.AlertId, alert.Id },
            { x => x.AlertCode, alert.AlertCode }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AmlAlertClearDialog>("Clear Alert", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task EscalateAlert(AmlAlertResponse alert)
    {
        var parameters = new DialogParameters<AmlAlertEscalateDialog>
        {
            { x => x.AlertId, alert.Id },
            { x => x.AlertCode, alert.AlertCode }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AmlAlertEscalateDialog>("Escalate Alert", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task FileSar(AmlAlertResponse alert)
    {
        var parameters = new DialogParameters<AmlAlertFileSarDialog>
        {
            { x => x.AlertId, alert.Id },
            { x => x.AlertCode, alert.AlertCode },
            { x => x.MemberName, alert.MemberName }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AmlAlertFileSarDialog>("File Suspicious Activity Report", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task CloseAlert(AmlAlertResponse alert)
    {
        var command = new CloseAmlAlertCommand { Id = alert.Id };
        await ApiHelper.ExecuteCallGuardedAsync(
            async () => await MicroFinanceClient.CloseAmlAlertEndpointAsync("1", alert.Id, command),
            Snackbar,
            successMessage: "Alert closed.");
        await _table!.ReloadDataAsync();
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<AmlAlertsHelpDialog>("AML Alerts Help", options);
    }
}
