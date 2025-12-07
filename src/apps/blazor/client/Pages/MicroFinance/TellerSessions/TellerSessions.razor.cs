using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.TellerSessions.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.TellerSessions;

public partial class TellerSessions
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;

    private ClientPreference _clientPreference = new();
    private EntityTable<TellerSessionResponse, DefaultIdType, TellerSessionViewModel>? _table;
    private EntityServerTableContext<TellerSessionResponse, DefaultIdType, TellerSessionViewModel> _context = null!;

    private bool _canOpen;
    private bool _canClose;
    private bool _canPause;
    private bool _canResume;
    private bool _canVerify;
    private bool _canRecordTransaction;

    private string? _selectedStatus;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canOpen = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.TellerSessions.Open)).Succeeded;
        _canClose = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.TellerSessions.Close)).Succeeded;
        _canPause = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.TellerSessions.Pause)).Succeeded;
        _canResume = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.TellerSessions.Resume)).Succeeded;
        _canVerify = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.TellerSessions.Verify)).Succeeded;
        _canRecordTransaction = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.TellerSessions.RecordTransaction)).Succeeded;

        _context = new EntityServerTableContext<TellerSessionResponse, DefaultIdType, TellerSessionViewModel>(
            entityName: "Teller Session",
            entityNamePlural: "Teller Sessions",
            entityResource: FSHResources.TellerSessions,
            fields: new List<EntityField<TellerSessionResponse>>
            {
                new EntityField<TellerSessionResponse>(s => s.SessionNumber, "Session #", "SessionNumber"),
                new EntityField<TellerSessionResponse>(s => s.TellerName, "Teller", "TellerName"),
                new EntityField<TellerSessionResponse>(s => s.CurrentBalance, "Balance", "CurrentBalance", typeof(decimal)),
                new EntityField<TellerSessionResponse>(s => s.TransactionCount, "Transactions", "TransactionCount", typeof(int)),
                new EntityField<TellerSessionResponse>(s => s.Status, "Status", "Status"),
                new EntityField<TellerSessionResponse>(s => s.OpenedAt, "Opened", "OpenedAt", typeof(DateTime)),
                new EntityField<TellerSessionResponse>(s => s.Variance, "Variance", "Variance", typeof(decimal))
            },
            idFunc: s => s.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await Client.SearchTellerSessionsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<TellerSessionResponse>>();
            },
            getDetailsFunc: async id => (await Client.GetTellerSessionEndpointAsync("1", id)).Adapt<TellerSessionViewModel>(),
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: _ => false,
            canDeleteEntityFunc: _ => false);
    }

    private Color GetStatusColor(string? status) => status switch
    {
        "Open" => Color.Success,
        "Paused" => Color.Warning,
        "Closed" => Color.Default,
        "Verified" => Color.Info,
        _ => Color.Default
    };

    private async Task ShowDetails(TellerSessionResponse session)
    {
        var parameters = new DialogParameters<TellerSessionDetailsDialog>
        {
            { x => x.Session, session }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<TellerSessionDetailsDialog>("Teller Session Details", parameters, options);
    }

    private async Task OpenSession()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<TellerSessionOpenDialog>("Open Teller Session", options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task RecordCashIn(TellerSessionResponse session)
    {
        var parameters = new DialogParameters<TellerSessionCashInDialog>
        {
            { x => x.SessionId, session.Id },
            { x => x.SessionNumber, session.SessionNumber },
            { x => x.CurrentBalance, session.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<TellerSessionCashInDialog>("Record Cash In", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task RecordCashOut(TellerSessionResponse session)
    {
        var parameters = new DialogParameters<TellerSessionCashOutDialog>
        {
            { x => x.SessionId, session.Id },
            { x => x.SessionNumber, session.SessionNumber },
            { x => x.CurrentBalance, session.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<TellerSessionCashOutDialog>("Record Cash Out", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task PauseSession(TellerSessionResponse session)
    {
        var command = new PauseTellerSessionCommand { Id = session.Id };
        await ApiHelper.ExecuteCallGuardedAsync(
            async () => await Client.PauseTellerSessionEndpointAsync("1", session.Id, command),
            Snackbar,
            successMessage: "Session paused.");
        await _table!.ReloadDataAsync();
    }

    private async Task ResumeSession(TellerSessionResponse session)
    {
        var command = new ResumeTellerSessionCommand { Id = session.Id };
        await ApiHelper.ExecuteCallGuardedAsync(
            async () => await Client.ResumeTellerSessionEndpointAsync("1", session.Id, command),
            Snackbar,
            successMessage: "Session resumed.");
        await _table!.ReloadDataAsync();
    }

    private async Task CloseSession(TellerSessionResponse session)
    {
        var parameters = new DialogParameters<TellerSessionCloseDialog>
        {
            { x => x.SessionId, session.Id },
            { x => x.SessionNumber, session.SessionNumber },
            { x => x.CurrentBalance, session.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<TellerSessionCloseDialog>("Close Session", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task VerifySession(TellerSessionResponse session)
    {
        var parameters = new DialogParameters<TellerSessionVerifyDialog>
        {
            { x => x.SessionId, session.Id },
            { x => x.SessionNumber, session.SessionNumber },
            { x => x.ClosingBalance, session.ClosingBalance ?? 0 },
            { x => x.Variance, session.Variance ?? 0 }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<TellerSessionVerifyDialog>("Verify Session", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<TellerSessionsHelpDialog>("Teller Sessions Help", options);
    }
}
