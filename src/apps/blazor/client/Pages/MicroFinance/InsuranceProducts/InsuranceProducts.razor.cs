namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceProducts;

/// <summary>
/// InsuranceProducts page logic. Provides CRUD and search over InsuranceProduct entities.
/// Manages micro-insurance products for member protection.
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

    // Advanced search filters
    private string? _searchCode;
    private string? SearchCode
    {
        get => _searchCode;
        set
        {
            _searchCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchName;
    private string? SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            _ = _table.ReloadDataAsync();
        }
    }

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
                new EntityField<InsuranceProductResponse>(dto => dto.PremiumRate, "Premium Rate", "PremiumRate", typeof(decimal)),
                new EntityField<InsuranceProductResponse>(dto => dto.MandatoryWithLoan, "Mandatory", "MandatoryWithLoan", typeof(bool)),
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
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateInsuranceProductAsync("1", id, viewModel.Adapt<UpdateInsuranceProductCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteInsuranceProductAsync("1", id).ConfigureAwait(false),
            entityName: "Insurance Product",
            entityNamePlural: "Insurance Products",
            entityResource: FshResources.InsuranceProducts);

        await AuthState;
    }

    private async Task ShowInsuranceProductsHelp()
    {
        await DialogService.ShowAsync<InsuranceProductsHelpDialog>("Insurance Products Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewProductDetails(DefaultIdType id)
    {
        var product = await Client.GetInsuranceProductAsync("1", id).ConfigureAwait(false);
        if (product != null)
        {
            var parameters = new DialogParameters { ["Product"] = product };
            await DialogService.ShowAsync<InsuranceProductDetailsDialog>("Insurance Product Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private void ViewPolicies(DefaultIdType productId)
    {
        Navigation.NavigateTo($"/microfinance/insurance-policies?productId={productId}");
    }
}
