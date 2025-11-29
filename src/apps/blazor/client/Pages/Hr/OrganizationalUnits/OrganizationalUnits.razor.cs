namespace FSH.Starter.Blazor.Client.Pages.Hr.OrganizationalUnits;

/// <summary>
/// Organizational Units page for managing the organizational hierarchy.
/// Provides CRUD operations and search functionality for departments, divisions, and sections.
/// Follows the EntityTable pattern from Todos, Store, Catalog, and Accounting modules.
/// </summary>
public partial class OrganizationalUnits
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<OrganizationalUnitResponse, DefaultIdType, OrganizationalUnitViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<OrganizationalUnitResponse, DefaultIdType, OrganizationalUnitViewModel> _table = null!;

    private List<OrganizationalUnitResponse> _allUnits = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<OrganizationalUnitResponse, DefaultIdType, OrganizationalUnitViewModel>(
            entityName: "Organizational Unit",
            entityNamePlural: "Organizational Units",
            entityResource: FshResources.Organization,
            fields:
            [
                new EntityField<OrganizationalUnitResponse>(response => response.Code, "Code", "Code"),
                new EntityField<OrganizationalUnitResponse>(response => response.Name, "Name", "Name"),
                new EntityField<OrganizationalUnitResponse>(response => response.Type, "Type", "Type"),
                new EntityField<OrganizationalUnitResponse>(response => response.Level, "Level", "Level", typeof(int)),
                new EntityField<OrganizationalUnitResponse>(response => response.ParentName, "Parent", "ParentName"),
                new EntityField<OrganizationalUnitResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchOrganizationalUnitsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchOrganizationalUnitsEndpointAsync("1", request).ConfigureAwait(false);

                // Cache all units for parent dropdown filtering
                _allUnits = result.Items?.ToList() ?? new();

                return result.Adapt<PaginationResponse<OrganizationalUnitResponse>>();
            },
            createFunc: async unit =>
            {
                await Client.CreateOrganizationalUnitEndpointAsync("1", unit.Adapt<CreateOrganizationalUnitCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, unit) =>
            {
                await Client.UpdateOrganizationalUnitEndpointAsync("1", id, unit.Adapt<UpdateOrganizationalUnitCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteOrganizationalUnitEndpointAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Gets the helper text for parent type based on organizational unit type.
    /// </summary>
    private static string GetParentTypeHelper(OrganizationalUnitType? type) => type switch
    {
        (OrganizationalUnitType)2 => "Parent must be a Department",
        (OrganizationalUnitType)3 => "Parent must be a Division",
        _ => ""
    };

    /// <summary>
    /// Gets filtered parents based on organizational unit type for dropdown selection.
    /// </summary>
    private List<OrganizationalUnitResponse> GetFilteredParents(OrganizationalUnitType? type, DefaultIdType? currentId)
    {
        return type switch
        {
            (OrganizationalUnitType)2 => _allUnits
                .Where(u => u.Type == (OrganizationalUnitType)1 && u.Id != currentId)
                .OrderBy(u => u.Name)
                .ToList(),
            (OrganizationalUnitType)3 => _allUnits
                .Where(u => u.Type == (OrganizationalUnitType)2 && u.Id != currentId)
                .OrderBy(u => u.Name)
                .ToList(),
            _ => new List<OrganizationalUnitResponse>()
        };
    }

    /// <summary>
    /// Shows the organizational units help dialog.
    /// </summary>
    private async Task ShowOrganizationalUnitsHelp()
    {
        await DialogService.ShowAsync<OrganizationalUnitsHelpDialog>("Organizational Units Help", new DialogParameters(), _helpDialogOptions);
    }
}

