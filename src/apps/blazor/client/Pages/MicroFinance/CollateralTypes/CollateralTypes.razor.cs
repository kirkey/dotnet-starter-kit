namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralTypes;

/// <summary>
/// CollateralTypes page logic. Provides CRUD and search over CollateralType entities using the generated API client.
/// Manages collateral type configuration for loan security requirements.
/// </summary>
public partial class CollateralTypes
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<CollateralTypeResponse, DefaultIdType, CollateralTypeViewModel> Context { get; set; } = null!;

    private EntityTable<CollateralTypeResponse, DefaultIdType, CollateralTypeViewModel> _table = null!;

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

    private string? _searchCategory;
    private string? SearchCategory
    {
        get => _searchCategory;
        set
        {
            _searchCategory = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with collateral type-specific configuration.
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

        Context = new EntityServerTableContext<CollateralTypeResponse, DefaultIdType, CollateralTypeViewModel>(
            fields:
            [
                new EntityField<CollateralTypeResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<CollateralTypeResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<CollateralTypeResponse>(dto => dto.Category, "Category", "Category"),
                new EntityField<CollateralTypeResponse>(dto => dto.DefaultLtvPercent, "Default LTV %", "DefaultLtvPercent", typeof(decimal)),
                new EntityField<CollateralTypeResponse>(dto => dto.MaxLtvPercent, "Max LTV %", "MaxLtvPercent", typeof(decimal)),
                new EntityField<CollateralTypeResponse>(dto => dto.RequiresInsurance, "Insurance", "RequiresInsurance", typeof(bool)),
                new EntityField<CollateralTypeResponse>(dto => dto.RequiresAppraisal, "Appraisal", "RequiresAppraisal", typeof(bool)),
                new EntityField<CollateralTypeResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollateralTypesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollateralTypesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollateralTypeResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollateralTypeAsync("1", viewModel.Adapt<CreateCollateralTypeCommand>()).ConfigureAwait(false);
            },
            getDetailsFunc: async id =>
            {
                var result = await Client.GetCollateralTypeAsync("1", id).ConfigureAwait(false);
                return result.Adapt<CollateralTypeViewModel>();
            },
            deleteFunc: null,
            entityName: "Collateral Type",
            entityNamePlural: "Collateral Types",
            entityResource: FshResources.CollateralTypes);
    }

    /// <summary>
    /// Show collateral types help dialog.
    /// </summary>
    private async Task ShowCollateralTypesHelp()
    {
        await DialogService.ShowAsync<CollateralTypesHelpDialog>("Collateral Types Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
