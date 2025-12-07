using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralTypes.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralTypes;

public partial class CollateralTypes
{
    [Inject]
    protected IAuthorizationService AuthorizationService { get; set; } = null!;

    [Inject]
    protected ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<CollateralTypeResponse, DefaultIdType, CollateralTypeViewModel> _context = null!;
    private EntityTable<CollateralTypeResponse, DefaultIdType, CollateralTypeViewModel>? _table;

    private bool _canCreate;
    private bool _canUpdate;
    private bool _canView;
    private int _elevation;
    private string _borderRadius = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Create)).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canView = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.View)).Succeeded;

        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        _context = new EntityServerTableContext<CollateralTypeResponse, DefaultIdType, CollateralTypeViewModel>(
            entityName: "Collateral Type",
            entityNamePlural: "Collateral Types",
            entityResource: FshResources.MicroFinance,
            searchAction: FshActions.Search,
            fields:
            [
                new EntityField<CollateralTypeResponse>(ct => ct.Code!, "Code", "Code"),
                new EntityField<CollateralTypeResponse>(ct => ct.Name!, "Name", "Name"),
                new EntityField<CollateralTypeResponse>(ct => ct.Category!, "Category", "Category"),
                new EntityField<CollateralTypeResponse>(ct => ct.DefaultLtvPercent, "Default LTV %", "DefaultLtvPercent", typeof(decimal)),
                new EntityField<CollateralTypeResponse>(ct => ct.MaxLtvPercent, "Max LTV %", "MaxLtvPercent", typeof(decimal)),
                new EntityField<CollateralTypeResponse>(ct => ct.AnnualDepreciationRate, "Depreciation %", "AnnualDepreciationRate", typeof(decimal)),
            ],
            enableAdvancedSearch: false,
            idFunc: ct => ct.Id,
            searchFunc: async filter =>
            {
                var response = await Client.SearchCollateralTypesEndpointAsync("1", new SearchCollateralTypesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? []
                });

                return response.Adapt<PaginationResponse<CollateralTypeResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateCollateralTypeCommand>();
                await Client.CreateCollateralTypeAsync("1", command);
            },
            updateFunc: async (id, vm) =>
            {
                var command = vm.Adapt<UpdateCollateralTypeCommand>();
                await Client.UpdateCollateralTypeEndpointAsync("1", id, command);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetCollateralTypeEndpointAsync("1", id);
                return response.Adapt<CollateralTypeViewModel>();
            },
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(CollateralTypeResponse collateralType)
    {
        var parameters = new DialogParameters
        {
            { nameof(CollateralTypeDetailsDialog.CollateralType), collateralType }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CollateralTypeDetailsDialog>("Collateral Type Details", parameters, options);
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CollateralTypesHelpDialog>("Collateral Types Help", options);
    }
}
