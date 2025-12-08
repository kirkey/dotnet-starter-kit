namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceProducts;

/// <summary>
/// Insurance Products page logic. Manages MFI insurance products.
/// </summary>
public partial class InsuranceProducts
{
    protected EntityServerTableContext<InsuranceProductResponse, DefaultIdType, InsuranceProductViewModel> Context { get; set; } = null!;
    private EntityTable<InsuranceProductResponse, DefaultIdType, InsuranceProductViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();
    private bool _canActivate;

    private string? _searchInsuranceType;
    private string? SearchInsuranceType
    {
        get => _searchInsuranceType;
        set
        {
            _searchInsuranceType = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

        Context = new EntityServerTableContext<InsuranceProductResponse, DefaultIdType, InsuranceProductViewModel>(
            fields:
            [
                new EntityField<InsuranceProductResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<InsuranceProductResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<InsuranceProductResponse>(dto => dto.InsuranceType, "Type", "InsuranceType"),
                new EntityField<InsuranceProductResponse>(dto => dto.Provider, "Provider", "Provider"),
                new EntityField<InsuranceProductResponse>(dto => dto.MinCoverage, "Min Coverage", "MinCoverage", typeof(decimal)),
                new EntityField<InsuranceProductResponse>(dto => dto.MaxCoverage, "Max Coverage", "MaxCoverage", typeof(decimal)),
                new EntityField<InsuranceProductResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInsuranceProductsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInsuranceProductsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InsuranceProductResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateInsuranceProductAsync("1", viewModel.Adapt<CreateInsuranceProductCommand>()).ConfigureAwait(false);
            },
            entityName: "Insurance Product",
            entityNamePlural: "Insurance Products",
            entityResource: FshResources.InsuranceProducts,
            hasExtraActionsFunc: () => true);

        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Activate, FshResources.InsuranceProducts);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var product = await Client.GetInsuranceProductAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Product", product }
        };

        await DialogService.ShowAsync<InsuranceProductDetailsDialog>("Insurance Product Details", parameters, new DialogOptions
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
            "Are you sure you want to activate this insurance product?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateInsuranceProductAsync("1", id),
                successMessage: "Insurance product activated successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
