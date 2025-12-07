using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanCollaterals.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanCollaterals;

public partial class LoanCollaterals
{
    [Inject]
    protected IMicroFinanceClient MicroFinanceClient { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthorizationService { get; set; } = null!;

    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    protected ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<LoanCollateralResponse, DefaultIdType, LoanCollateralViewModel> _context = null!;
    private EntityTable<LoanCollateralResponse, DefaultIdType, LoanCollateralViewModel>? _table;

    private bool _canCreate;
    private bool _canVerify;
    private bool _canUpdateValuation;
    private bool _canPledge;
    private bool _canRelease;
    private bool _canSeize;
    private int _elevation;
    private string _borderRadius = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Create)).Succeeded;
        _canVerify = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canUpdateValuation = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canPledge = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canRelease = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canSeize = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;

        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        _context = new EntityServerTableContext<LoanCollateralResponse, DefaultIdType, LoanCollateralViewModel>(
            entityName: "Loan Collateral",
            entityNamePlural: "Loan Collaterals",
            entityResource: FshResources.MicroFinance,
            searchAction: FshActions.Search,
            fields:
            [
                new EntityField<LoanCollateralResponse>(c => c.LoanNumber!, "Loan #", "LoanNumber"),
                new EntityField<LoanCollateralResponse>(c => c.CollateralType!, "Type", "CollateralType"),
                new EntityField<LoanCollateralResponse>(c => c.Description!, "Description", "Description"),
                new EntityField<LoanCollateralResponse>(c => c.EstimatedValue, "Est. Value", "EstimatedValue", typeof(decimal)),
                new EntityField<LoanCollateralResponse>(c => c.ForcedSaleValue, "FSV", "ForcedSaleValue", typeof(decimal)),
                new EntityField<LoanCollateralResponse>(c => c.Status!, "Status", "Status"),
            ],
            enableAdvancedSearch: false,
            idFunc: c => c.Id,
            searchFunc: async filter =>
            {
                var response = await MicroFinanceClient.SearchLoanCollateralsEndpointAsync("1", new SearchLoanCollateralsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? []
                });

                return response.Adapt<PaginationResponse<LoanCollateralResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateLoanCollateralCommand>();
                await MicroFinanceClient.CreateLoanCollateralAsync("1", command);
            },
            getDetailsFunc: async id =>
            {
                var response = await MicroFinanceClient.GetLoanCollateralEndpointAsync("1", id);
                return response.Adapt<LoanCollateralViewModel>();
            },
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(LoanCollateralResponse collateral)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralDetailsDialog.Collateral), collateral }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanCollateralDetailsDialog>("Loan Collateral Details", parameters, options);
    }

    private async Task VerifyCollateral(LoanCollateralResponse collateral)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralVerifyDialog.CollateralId), collateral.Id },
            { nameof(LoanCollateralVerifyDialog.Description), collateral.Description }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanCollateralVerifyDialog>("Verify Collateral", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task UpdateValuation(LoanCollateralResponse collateral)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralValuationDialog.CollateralId), collateral.Id },
            { nameof(LoanCollateralValuationDialog.CurrentValue), collateral.EstimatedValue }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanCollateralValuationDialog>("Update Valuation", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task PledgeCollateral(LoanCollateralResponse collateral)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralPledgeDialog.CollateralId), collateral.Id },
            { nameof(LoanCollateralPledgeDialog.Description), collateral.Description }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanCollateralPledgeDialog>("Pledge Collateral", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ReleaseCollateral(LoanCollateralResponse collateral)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralReleaseDialog.CollateralId), collateral.Id },
            { nameof(LoanCollateralReleaseDialog.Description), collateral.Description }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanCollateralReleaseDialog>("Release Collateral", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task SeizeCollateral(LoanCollateralResponse collateral)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralSeizeDialog.CollateralId), collateral.Id },
            { nameof(LoanCollateralSeizeDialog.Description), collateral.Description }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanCollateralSeizeDialog>("Seize Collateral", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanCollateralsHelpDialog>("Loan Collaterals Help", options);
    }
}
