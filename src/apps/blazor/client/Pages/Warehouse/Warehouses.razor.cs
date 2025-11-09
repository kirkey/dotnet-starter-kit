namespace FSH.Starter.Blazor.Client.Pages.Warehouse;

/// <summary>
/// Warehouses page component for managing warehouse facilities.
/// Provides CRUD operations for warehouse entities including address management,
/// capacity tracking, and manager assignments.
/// </summary>
public partial class Warehouses
{
    protected EntityServerTableContext<WarehouseResponse, DefaultIdType, WarehouseViewModel> Context { get; set; } = null!;

    private EntityTable<WarehouseResponse, DefaultIdType, WarehouseViewModel> _table = null!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<WarehouseResponse, DefaultIdType, WarehouseViewModel>(
            entityName: "Warehouse",
            entityNamePlural: "Warehouses",
            entityResource: FshResources.Warehouse,
            fields:
            [
                new EntityField<WarehouseResponse>(response => response.Code, "Code", "Code"),
                new EntityField<WarehouseResponse>(response => response.Name, "Name", "Name"),
                new EntityField<WarehouseResponse>(response => response.Address, "Address", "Address"),
                new EntityField<WarehouseResponse>(response => response.ManagerName, "Manager", "ManagerName"),
                new EntityField<WarehouseResponse>(response => response.TotalCapacity, "Capacity", "TotalCapacity", typeof(decimal)),
                new EntityField<WarehouseResponse>(response => response.CapacityUnit, "Unit", "CapacityUnit"),
                new EntityField<WarehouseResponse>(response => response.WarehouseType, "Warehouse Type", "WarehouseType"),
                new EntityField<WarehouseResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<WarehouseResponse>(response => response.IsMainWarehouse, "Main", "IsMainWarehouse", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchWarehousesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchWarehousesEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<WarehouseResponse>>();
            },
            createFunc: async warehouse =>
            {
                await Client.CreateWarehouseEndpointAsync("1", warehouse.Adapt<CreateWarehouseCommand>());
            },
            updateFunc: async (id, warehouse) =>
            {
                await Client.UpdateWarehouseEndpointAsync("1", id, warehouse.Adapt<UpdateWarehouseCommand>());
            },
            deleteFunc: async id => await Client.DeleteWarehouseEndpointAsync("1", id),
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }
}

/// <summary>
/// View model for warehouse form operations.
/// Includes validation attributes and data annotations for comprehensive warehouse management.
/// </summary>
public class WarehouseViewModel
{
    /// <summary>
    /// Warehouse unique identifier.
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Warehouse name for identification and display purposes.
    /// Required field with maximum length of 200 characters.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Optional description providing additional warehouse details.
    /// Maximum length of 1000 characters.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Unique warehouse code for identification and referencing.
    /// Required field with a maximum length of 50 characters.
    /// Examples: "WH-MAIN", "WH-NYC-01", "WH-SEA"
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Street address of the warehouse facility.
    /// Required field with a maximum length of 500 characters.
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Name of the warehouse manager responsible for operations.
    /// Required field with a maximum length of 100 characters.
    /// </summary>
    public string ManagerName { get; set; } = null!;

    /// <summary>
    /// Email address of the warehouse manager.
    /// Required field with a maximum length of 255 characters and valid email format.
    /// </summary>
    public string ManagerEmail { get; set; } = null!;

    /// <summary>
    /// Phone number of the warehouse manager.
    /// Required field with a maximum length of 50 characters.
    /// </summary>
    public string ManagerPhone { get; set; } = null!;

    /// <summary>
    /// Total storage capacity of the warehouse.
    /// Must be greater than 0.
    /// </summary>
    public decimal TotalCapacity { get; set; }

    /// <summary>
    /// Currently used capacity of the warehouse.
    /// Read-only field updated by inventory movements.
    /// </summary>
    public decimal UsedCapacity { get; set; }

    /// <summary>
    /// Unit of measurement for capacity (e.g., sqft, pallets, cubic_meters).
    /// Required field with a maximum length of 20 characters.
    /// </summary>
    public string CapacityUnit { get; set; } = "sqft";

    /// <summary>
    /// Indicates whether the warehouse is active and operational.
    /// Default value is true.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Indicates whether this is the main/primary warehouse.
    /// The default value is false. Only one warehouse should be marked as main.
    /// </summary>
    public bool IsMainWarehouse { get; set; }

    /// <summary>
    /// Type of warehouse indicating storage capabilities and restrictions.
    /// Required field with a maximum length of 50 characters.
    /// The default value is "Standard".
    /// </summary>
    public string WarehouseType { get; set; } = "Standard";
}
