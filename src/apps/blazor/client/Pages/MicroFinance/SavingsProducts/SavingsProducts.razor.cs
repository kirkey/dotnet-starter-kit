namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsProducts;

/// <summary>
/// SavingsProducts page logic. Provides CRUD and search over SavingsProduct entities using the generated API client.
/// Manages savings product configuration including interest rates and withdrawal limits.
/// </summary>
public partial class SavingsProducts
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<SavingsProductResponse, DefaultIdType, SavingsProductViewModel> Context { get; set; } = null!;

    private EntityTable<SavingsProductResponse, DefaultIdType, SavingsProductViewModel> _table = null!;

    /// <summary>
    /// Authorization state for permission checks.
    /// </summary>
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Authorization service for permission checks.
    /// </summary>
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
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

    private bool? _searchIsActive;
    private bool? SearchIsActive
    {
        get => _searchIsActive;
        set
        {
            _searchIsActive = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with savings product-specific configuration.
    /// </summary>
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

        Context = new EntityServerTableContext<SavingsProductResponse, DefaultIdType, SavingsProductViewModel>(
            fields:
            [
                new EntityField<SavingsProductResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<SavingsProductResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<SavingsProductResponse>(dto => dto.InterestRate, "Interest Rate %", "InterestRate", typeof(decimal)),
                new EntityField<SavingsProductResponse>(dto => dto.InterestCalculation, "Calculation", "InterestCalculation"),
                new EntityField<SavingsProductResponse>(dto => dto.InterestPostingFrequency, "Posting", "InterestPostingFrequency"),
                new EntityField<SavingsProductResponse>(dto => dto.MinOpeningBalance, "Min Opening", "MinOpeningBalance", typeof(decimal)),
                new EntityField<SavingsProductResponse>(dto => dto.MinBalanceForInterest, "Min for Interest", "MinBalanceForInterest", typeof(decimal)),
                new EntityField<SavingsProductResponse>(dto => dto.AllowOverdraft, "Overdraft", "AllowOverdraft", typeof(bool)),
                new EntityField<SavingsProductResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchSavingsProductsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchSavingsProductsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SavingsProductResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateSavingsProductAsync("1", viewModel.Adapt<CreateSavingsProductCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateSavingsProductAsync("1", id, viewModel.Adapt<UpdateSavingsProductCommand>()).ConfigureAwait(false);
            },
            deleteFunc: null, // No delete endpoint for savings products
            entityName: "Savings Product",
            entityNamePlural: "Savings Products",
            entityResource: FshResources.SavingsProducts);
    }

    /// <summary>
    /// Show savings products help dialog.
    /// </summary>
    private async Task ShowSavingsProductsHelp()
    {
        await DialogService.ShowAsync<SavingsProductsHelpDialog>("Savings Products Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Navigate to savings product dashboard.
    /// </summary>
    private void OnViewDashboard(DefaultIdType id)
    {
        Navigation.NavigateTo($"/microfinance/savings-products/{id}/dashboard");
    }
}
