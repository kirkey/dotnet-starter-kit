using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.CashVaults.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CashVaults;

public partial class CashVaults
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;

    private ClientPreference _clientPreference = new();
    private EntityTable<CashVaultResponse, DefaultIdType, CashVaultViewModel>? _table;
    private EntityServerTableContext<CashVaultResponse, DefaultIdType, CashVaultViewModel> _context = null!;

    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canDeposit;
    private bool _canWithdraw;
    private bool _canReconcile;
    private bool _canOpenDay;

    private string? _selectedStatus;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Create, FshResources.CashVaults))).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Update, FshResources.CashVaults))).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Delete, FshResources.CashVaults))).Succeeded;
        _canDeposit = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Deposit, FshResources.CashVaults))).Succeeded;
        _canWithdraw = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Withdraw, FshResources.CashVaults))).Succeeded;
        _canReconcile = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Update, FshResources.CashVaults))).Succeeded;
        _canOpenDay = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Update, FshResources.CashVaults))).Succeeded;

        _context = new EntityServerTableContext<CashVaultResponse, DefaultIdType, CashVaultViewModel>(
            entityName: "Cash Vault",
            entityNamePlural: "Cash Vaults",
            entityResource: FshResources.CashVaults,
            fields: new List<EntityField<CashVaultResponse>>
            {
                new EntityField<CashVaultResponse>(vault => vault.Code, "Code", "Code"),
                new EntityField<CashVaultResponse>(vault => vault.Name, "Name", "Name"),
                new EntityField<CashVaultResponse>(vault => vault.VaultType, "Type", "VaultType"),
                new EntityField<CashVaultResponse>(vault => vault.CurrentBalance, "Balance", "CurrentBalance", typeof(decimal)),
                new EntityField<CashVaultResponse>(vault => vault.Status, "Status", "Status"),
                new EntityField<CashVaultResponse>(vault => vault.CustodianName, "Custodian", "CustodianName"),
                new EntityField<CashVaultResponse>(vault => vault.LastOpenedAt, "Last Opened", "LastOpenedAt", typeof(DateTime))
            },
            idFunc: vault => vault.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await Client.SearchCashVaultsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<CashVaultResponse>>();
            },
            getDetailsFunc: async id => (await Client.GetCashVaultEndpointAsync("1", id)).Adapt<CashVaultViewModel>(),
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateCashVaultCommand>();
                await Client.CreateCashVaultEndpointAsync("1", command);
            },
            updateFunc: async (id, vm) =>
            {
                var command = vm.Adapt<UpdateCashVaultCommand>();
                await Client.UpdateCashVaultEndpointAsync("1", id, command);
            },
            deleteFunc: async id => await Client.DeleteCashVaultEndpointAsync("1", id),
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private Color GetStatusColor(string? status) => status switch
    {
        "Open" => Color.Success,
        "Closed" => Color.Default,
        "Reconciling" => Color.Warning,
        _ => Color.Default
    };

    private async Task ShowDetails(CashVaultResponse vault)
    {
        var parameters = new DialogParameters<CashVaultDetailsDialog>
        {
            { x => x.Vault, vault }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CashVaultDetailsDialog>("Cash Vault Details", parameters, options);
    }

    private async Task OpenDay(CashVaultResponse vault)
    {
        var parameters = new DialogParameters<CashVaultOpenDayDialog>
        {
            { x => x.VaultId, vault.Id },
            { x => x.VaultName, vault.Name },
            { x => x.LastBalance, vault.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CashVaultOpenDayDialog>("Open Day", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task Deposit(CashVaultResponse vault)
    {
        var parameters = new DialogParameters<CashVaultDepositDialog>
        {
            { x => x.VaultId, vault.Id },
            { x => x.VaultName, vault.Name },
            { x => x.CurrentBalance, vault.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CashVaultDepositDialog>("Deposit Cash", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task Withdraw(CashVaultResponse vault)
    {
        var parameters = new DialogParameters<CashVaultWithdrawDialog>
        {
            { x => x.VaultId, vault.Id },
            { x => x.VaultName, vault.Name },
            { x => x.CurrentBalance, vault.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CashVaultWithdrawDialog>("Withdraw Cash", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task Reconcile(CashVaultResponse vault)
    {
        var parameters = new DialogParameters<CashVaultReconcileDialog>
        {
            { x => x.VaultId, vault.Id },
            { x => x.VaultName, vault.Name },
            { x => x.CurrentBalance, vault.CurrentBalance }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CashVaultReconcileDialog>("Reconcile Vault", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CashVaultsHelpDialog>("Cash Vaults Help", options);
    }
}
