namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeeDefinitions;

/// <summary>
/// FeeDefinitions page logic. Provides CRUD and search operations for FeeDefinition entities.
/// Manages fee types, calculation methods, and configurations for the microfinance system.
/// </summary>
public partial class FeeDefinitions
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<FeeDefinitionResponse, DefaultIdType, FeeDefinitionViewModel> Context { get; set; } = null!;

    private EntityTable<FeeDefinitionResponse, DefaultIdType, FeeDefinitionViewModel> _table = null!;

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

    // Permission flags
    private bool _canManage;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchFeeType;
    private string? SearchFeeType
    {
        get => _searchFeeType;
        set
        {
            _searchFeeType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchAppliesTo;
    private string? SearchAppliesTo
    {
        get => _searchAppliesTo;
        set
        {
            _searchAppliesTo = value;
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
    /// Initializes the table context with fee definition-specific configuration.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
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

        Context = new EntityServerTableContext<FeeDefinitionResponse, DefaultIdType, FeeDefinitionViewModel>(
            fields:
            [
                new EntityField<FeeDefinitionResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<FeeDefinitionResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<FeeDefinitionResponse>(dto => dto.FeeType, "Fee Type", "FeeType"),
                new EntityField<FeeDefinitionResponse>(dto => dto.CalculationType, "Calculation", "CalculationType"),
                new EntityField<FeeDefinitionResponse>(dto => dto.AppliesTo, "Applies To", "AppliesTo"),
                new EntityField<FeeDefinitionResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<FeeDefinitionResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchFeeDefinitionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchFeeDefinitionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<FeeDefinitionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateFeeDefinitionAsync("1", viewModel.Adapt<CreateFeeDefinitionCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateFeeDefinitionAsync("1", id, viewModel.Adapt<UpdateFeeDefinitionCommand>()).ConfigureAwait(false);
            },
            entityName: "Fee Definition",
            entityNamePlural: "Fee Definitions",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<FeeDefinitionsHelpDialog>("Fee Definitions Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View fee details in a dialog.
    /// </summary>
    private async Task ViewFeeDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeeDefinitionDetailsDialog.FeeId), id }
        };

        await DialogService.ShowAsync<FeeDefinitionDetailsDialog>("Fee Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
