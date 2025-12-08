using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentProducts.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentProducts;

public partial class InvestmentProducts
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;
    [Inject]
    private ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<InvestmentProductResponse, DefaultIdType, InvestmentProductViewModel> _context = null!;
    private EntityTable<InvestmentProductResponse, DefaultIdType, InvestmentProductViewModel>? _table;

    private int _elevation;
    private string _borderRadius = string.Empty;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canView;
    private bool _canUpdateNav;

    protected override async Task OnInitializedAsync()
    {
        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        var authState = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Create, FshResources.InvestmentProducts))).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Update, FshResources.InvestmentProducts))).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.Delete, FshResources.InvestmentProducts))).Succeeded;
        _canView = (await AuthorizationService.AuthorizeAsync(authState.User, FshPermission.NameFor(FshActions.View, FshResources.InvestmentProducts))).Succeeded;
        _canUpdateNav = _canUpdate;

        _context = new EntityServerTableContext<InvestmentProductResponse, DefaultIdType, InvestmentProductViewModel>(
            entityName: "Investment Product",
            entityNamePlural: "Investment Products",
            entityResource: FshResources.InvestmentProducts,
            fields:
            [
                new EntityField<InvestmentProductResponse>(e => e.Code, "Code", "Code"),
                new EntityField<InvestmentProductResponse>(e => e.Name, "Name", "Name"),
                new EntityField<InvestmentProductResponse>(e => e.ProductType, "Type", "ProductType"),
                new EntityField<InvestmentProductResponse>(e => e.RiskLevel, "Risk Level", "RiskLevel"),
                new EntityField<InvestmentProductResponse>(e => e.MinimumInvestment, "Min Investment", "MinimumInvestment", typeof(decimal)),
                new EntityField<InvestmentProductResponse>(e => e.ManagementFeePercent, "Mgmt Fee %", "ManagementFeePercent", typeof(decimal)),
                new EntityField<InvestmentProductResponse>(e => $"{e.ExpectedReturnMin}% - {e.ExpectedReturnMax}%", "Expected Return", "ExpectedReturnMin"),
                new EntityField<InvestmentProductResponse>(e => e.CurrentNav, "NAV", "CurrentNav", typeof(decimal)),
            ],
            idFunc: e => e.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await Client.SearchInvestmentProductsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<InvestmentProductResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateInvestmentProductCommand>();
                await Client.CreateInvestmentProductAsync("1", command);
            },
            getDefaultsFunc: () => Task.FromResult(new InvestmentProductViewModel()),
            hasExtraActionsFunc: () => true,
            canCreateEntityFunc: () => _canCreate,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private async Task ViewDetails(InvestmentProductResponse product)
    {
        var parameters = new DialogParameters
        {
            { nameof(InvestmentProductDetailsDialog.Product), product }
        };
        await DialogService.ShowAsync<InvestmentProductDetailsDialog>("Investment Product Details", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }

    private async Task UpdateNav(InvestmentProductResponse product)
    {
        var parameters = new DialogParameters
        {
            { nameof(InvestmentProductNavDialog.ProductId), product.Id },
            { nameof(InvestmentProductNavDialog.ProductName), product.Name },
            { nameof(InvestmentProductNavDialog.CurrentNav), product.CurrentNav }
        };
        var dialog = await DialogService.ShowAsync<InvestmentProductNavDialog>("Update NAV", parameters,
            new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true });
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<InvestmentProductsHelpDialog>("Investment Products Help",
            new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}
