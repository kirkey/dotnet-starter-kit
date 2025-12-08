namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MfiConfigurations;

/// <summary>
/// MFI Configurations page logic. Provides CRUD and search over MfiConfiguration entities.
/// Manages system configuration settings for the microfinance institution.
/// </summary>
public partial class MfiConfigurations
{
    /// <summary>
    /// Table context that drives the generic EntityTable.
    /// </summary>
    protected EntityServerTableContext<MfiConfigurationSummaryResponse, DefaultIdType, MfiConfigurationViewModel> Context { get; set; } = null!;

    private EntityTable<MfiConfigurationSummaryResponse, DefaultIdType, MfiConfigurationViewModel> _table = null!;

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
    private string? _searchKey;
    private string? SearchKey
    {
        get => _searchKey;
        set
        {
            _searchKey = value;
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
    /// Initializes the table context with configuration-specific settings.
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

        Context = new EntityServerTableContext<MfiConfigurationSummaryResponse, DefaultIdType, MfiConfigurationViewModel>(
            fields:
            [
                new EntityField<MfiConfigurationSummaryResponse>(dto => dto.Key, "Key", "Key"),
                new EntityField<MfiConfigurationSummaryResponse>(dto => dto.Value, "Value", "Value"),
                new EntityField<MfiConfigurationSummaryResponse>(dto => dto.Category, "Category", "Category"),
                new EntityField<MfiConfigurationSummaryResponse>(dto => dto.DataType, "Data Type", "DataType"),
                new EntityField<MfiConfigurationSummaryResponse>(dto => dto.IsEditable, "Editable", "IsEditable", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchMfiConfigurationsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchMfiConfigurationsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<MfiConfigurationSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateMfiConfigurationAsync("1", viewModel.Adapt<CreateMfiConfigurationCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateMfiConfigurationAsync("1", id, viewModel.Adapt<UpdateMfiConfigurationCommand>()).ConfigureAwait(false);
            },
            entityName: "MFI Configuration",
            entityNamePlural: "MFI Configurations",
            entityResource: FshResources.MfiConfigurations,
            hasExtraActionsFunc: () => true);
    }

    /// <summary>
    /// View configuration details in a dialog.
    /// </summary>
    private async Task ViewDetails(DefaultIdType id)
    {
        var config = await Client.GetMfiConfigurationAsync("1", id).ConfigureAwait(false);
        
        var parameters = new DialogParameters
        {
            { "Configuration", config }
        };

        await DialogService.ShowAsync<MfiConfigurationDetailsDialog>("Configuration Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
