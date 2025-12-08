using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentAccounts.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentAccounts;

public partial class InvestmentAccounts
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;
    [Inject]
    private ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<InvestmentAccountResponse, DefaultIdType, InvestmentAccountViewModel> _context = null!;
    private EntityTable<InvestmentAccountResponse, DefaultIdType, InvestmentAccountViewModel>? _table;

    private int _elevation;
    private string _borderRadius = string.Empty;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canView;
    private bool _canInvest;
    private bool _canRedeem;
    private bool _canSetupSip;

    protected override async Task OnInitializedAsync()
    {
        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        var authState = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Create, FshResources.InvestmentAccounts))).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Update, FshResources.InvestmentAccounts))).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Delete, FshResources.InvestmentAccounts))).Succeeded;
        _canView = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.View, FshResources.InvestmentAccounts))).Succeeded;
        _canInvest = _canUpdate;
        _canRedeem = _canUpdate;
        _canSetupSip = _canUpdate;

        _context = new EntityServerTableContext<InvestmentAccountResponse, DefaultIdType, InvestmentAccountViewModel>(
            entityName: "Investment Account",
            entityNamePlural: "Investment Accounts",
            entityResource: FshResources.InvestmentAccounts,
            fields:
            [
                new EntityField<InvestmentAccountResponse>(e => e.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<InvestmentAccountResponse>(e => e.MemberName, "Member", "MemberName"),
                new EntityField<InvestmentAccountResponse>(e => e.RiskProfile, "Risk Profile", "RiskProfile"),
                new EntityField<InvestmentAccountResponse>(e => e.TotalInvested, "Total Invested", "TotalInvested", typeof(decimal)),
                new EntityField<InvestmentAccountResponse>(e => e.CurrentValue, "Current Value", "CurrentValue", typeof(decimal)),
                new EntityField<InvestmentAccountResponse>(e => e.UnrealizedGainLoss, "Unrealized G/L", "UnrealizedGainLoss", typeof(decimal)),
                new EntityField<InvestmentAccountResponse>(e => e.Status, "Status", "Status"),
            ],
            idFunc: e => e.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await Client.SearchInvestmentAccountsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<InvestmentAccountResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateInvestmentAccountCommand>();
                await Client.CreateInvestmentAccountAsync("1", command);
            },
            getDefaultsFunc: () => Task.FromResult(new InvestmentAccountViewModel()),
            hasExtraActionsFunc: () => true,
            canCreateEntityFunc: () => _canCreate,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private async Task ViewDetails(InvestmentAccountResponse account)
    {
        var parameters = new DialogParameters
        {
            { nameof(InvestmentAccountDetailsDialog.Account), account }
        };
        await DialogService.ShowAsync<InvestmentAccountDetailsDialog>("Investment Account Details", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }

    private async Task Invest(InvestmentAccountResponse account)
    {
        var parameters = new DialogParameters
        {
            { nameof(InvestmentAccountInvestDialog.AccountId), account.Id },
            { nameof(InvestmentAccountInvestDialog.AccountNumber), account.AccountNumber }
        };
        var dialog = await DialogService.ShowAsync<InvestmentAccountInvestDialog>("Add Investment", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task Redeem(InvestmentAccountResponse account)
    {
        var parameters = new DialogParameters
        {
            { nameof(InvestmentAccountRedeemDialog.AccountId), account.Id },
            { nameof(InvestmentAccountRedeemDialog.AccountNumber), account.AccountNumber },
            { nameof(InvestmentAccountRedeemDialog.CurrentValue), account.CurrentValue }
        };
        var dialog = await DialogService.ShowAsync<InvestmentAccountRedeemDialog>("Redeem Investment", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task SetupSip(InvestmentAccountResponse account)
    {
        var parameters = new DialogParameters
        {
            { nameof(InvestmentAccountSipDialog.AccountId), account.Id },
            { nameof(InvestmentAccountSipDialog.AccountNumber), account.AccountNumber }
        };
        var dialog = await DialogService.ShowAsync<InvestmentAccountSipDialog>("Setup SIP", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<InvestmentAccountsHelpDialog>("Investment Accounts Help",
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}
