namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentProducts;

/// <summary>
/// Investment Products page logic. Manages MFI investment products.
/// </summary>
public partial class InvestmentProducts
{
    protected EntityServerTableContext<InvestmentProductResponse, DefaultIdType, InvestmentProductViewModel> Context { get; set; } = null!;
    private EntityTable<InvestmentProductResponse, DefaultIdType, InvestmentProductViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();
    private bool _canActivate;
    private bool _canDeactivate;

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

        Context = new EntityServerTableContext<InvestmentProductResponse, DefaultIdType, InvestmentProductViewModel>(
            fields:
            [
                new EntityField<InvestmentProductResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<InvestmentProductResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<InvestmentProductResponse>(dto => dto.ProductType, "Type", "ProductType"),
                new EntityField<InvestmentProductResponse>(dto => dto.MinimumInvestment, "Min Investment", "MinimumInvestment", typeof(decimal)),
                new EntityField<InvestmentProductResponse>(dto => dto.ExpectedReturnMin, "Expected Return", "ExpectedReturnMin", typeof(decimal)),
                new EntityField<InvestmentProductResponse>(dto => dto.RiskLevel, "Risk Level", "RiskLevel"),
                new EntityField<InvestmentProductResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInvestmentProductsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInvestmentProductsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InvestmentProductResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateInvestmentProductAsync("1", viewModel.Adapt<CreateInvestmentProductCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateInvestmentProductAsync("1", id, viewModel.Adapt<UpdateInvestmentProductRequest>()).ConfigureAwait(false);
            },
            entityName: "Investment Product",
            entityNamePlural: "Investment Products",
            entityResource: FshResources.InvestmentProducts,
            hasExtraActionsFunc: () => true);

        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Activate, FshResources.InvestmentProducts);
        _canDeactivate = await AuthService.HasPermissionAsync(state.User, FshActions.Deactivate, FshResources.InvestmentProducts);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var product = await Client.GetInvestmentProductAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Product", product }
        };

        await DialogService.ShowAsync<InvestmentProductDetailsDialog>("Investment Product Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ActivateProduct(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Activate Product",
            "Are you sure you want to activate this investment product?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateInvestmentProductAsync("1", id),
                successMessage: "Investment product activated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task DeactivateProduct(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Deactivate Product",
            "Are you sure you want to deactivate this investment product?",
            yesText: "Deactivate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.DeactivateInvestmentProductAsync("1", id),
                successMessage: "Investment product deactivated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Show investment products help dialog.
    /// </summary>
    private async Task ShowInvestmentProductsHelp()
    {
        await DialogService.ShowAsync<InvestmentProductsHelpDialog>("Investment Products Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
