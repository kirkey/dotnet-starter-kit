namespace FSH.Starter.Blazor.Client.Pages.Warehouse;

/// <summary>
/// Cycle Counts page logic. Provides CRUD and workflow operations over CycleCount entities.
/// </summary>
public partial class CycleCounts : ComponentBase
{
    protected EntityServerTableContext<CycleCountResponse, DefaultIdType, CycleCountViewModel> Context { get; set; } = default!;
    private EntityTable<CycleCountResponse, DefaultIdType, CycleCountViewModel> _table = default!;

    private List<WarehouseResponse> _warehouses = new();
    private List<GetWarehouseLocationListResponse> _warehouseLocations = new();

    protected override Task OnInitializedAsync()
    {
        // Load reference data asynchronously
        _ = LoadWarehousesAsync();
        _ = LoadWarehouseLocationsAsync();
        
        Context = new EntityServerTableContext<CycleCountResponse, DefaultIdType, CycleCountViewModel>(
            entityName: "Cycle Count",
            entityNamePlural: "Cycle Counts",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<CycleCountResponse>(x => x.CountNumber, "Count #", "CountNumber"),
                new EntityField<CycleCountResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<CycleCountResponse>(x => x.WarehouseLocationName, "Location", "WarehouseLocationName"),
                new EntityField<CycleCountResponse>(x => x.CountDate, "Date", "CountDate", typeof(DateTime)),
                new EntityField<CycleCountResponse>(x => x.Status, "Status", "Status"),
                new EntityField<CycleCountResponse>(x => x.CountType, "Type", "CountType"),
                new EntityField<CycleCountResponse>(x => x.CountedBy, "Counted By", "CountedBy"),
                new EntityField<CycleCountResponse>(x => x.TotalItems, "Total Items", "TotalItems", typeof(int)),
                new EntityField<CycleCountResponse>(x => x.CountedItems, "Counted", "CountedItems", typeof(int)),
                new EntityField<CycleCountResponse>(x => x.VarianceItems, "Variance", "VarianceItems", typeof(int))
            ],
            enableAdvancedSearch: false,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                // Note: Cycle Count search uses GET with query params
                var result = await Client.SearchCycleCountsEndpointAsync("1");
                return result.Adapt<PaginationResponse<CycleCountResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateCycleCountEndpointAsync("1", viewModel.Adapt<CreateCycleCountCommand>());
            },
            updateFunc: null, // Cycle counts use workflow transitions instead of updates
            deleteFunc: null); // Cycle counts typically aren't deleted once created
        
        return Task.CompletedTask;
    }

    private async Task LoadWarehousesAsync()
    {
        var filter = new SearchWarehousesCommand { PageSize = 1000 };
        var result = await Client.SearchWarehousesEndpointAsync("1", filter);
        _warehouses = result.Items?.ToList() ?? new List<WarehouseResponse>();
    }

    private async Task LoadWarehouseLocationsAsync()
    {
        var filter = new SearchWarehouseLocationsCommand { PageSize = 1000 };
        var result = await Client.SearchWarehouseLocationsEndpointAsync("1", filter);
        _warehouseLocations = result.Items?.ToList() ?? new List<GetWarehouseLocationListResponse>();
    }

    private async Task<IEnumerable<DefaultIdType>> SearchWarehouses(string value, CancellationToken token)
    {
        await Task.CompletedTask;
        return string.IsNullOrWhiteSpace(value)
            ? _warehouses.Select(w => w.Id)
            : _warehouses.Where(w => w.Name?.Contains(value, StringComparison.OrdinalIgnoreCase) == true || w.Code?.Contains(value, StringComparison.OrdinalIgnoreCase) == true)
                .Select(w => w.Id);
    }

    private async Task<IEnumerable<DefaultIdType?>> SearchWarehouseLocations(string value, CancellationToken token)
    {
        await Task.CompletedTask;
        return string.IsNullOrWhiteSpace(value)
            ? _warehouseLocations.Select(l => (DefaultIdType?)l.Id)
            : _warehouseLocations.Where(l => l.Name?.Contains(value, StringComparison.OrdinalIgnoreCase) == true || l.Code?.Contains(value, StringComparison.OrdinalIgnoreCase) == true)
                .Select(l => (DefaultIdType?)l.Id);
    }

    private async Task StartCycleCount(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Start Cycle Count",
            "Are you sure you want to start this cycle count?",
            yesText: "Start",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                await Client.StartCycleCountEndpointAsync("1", id);
                Snackbar.Add("Cycle count started successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to start cycle count: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task CompleteCycleCount(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Complete Cycle Count",
            "Are you sure you want to complete this cycle count?",
            yesText: "Complete",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                await Client.CompleteCycleCountEndpointAsync("1", id);
                Snackbar.Add("Cycle count completed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to complete cycle count: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ReconcileCycleCount(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Reconcile Cycle Count",
            "Are you sure you want to reconcile this cycle count? This will adjust inventory levels.",
            yesText: "Reconcile",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                await Client.ReconcileCycleCountEndpointAsync("1", id);
                Snackbar.Add("Cycle count reconciled successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reconcile cycle count: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task AddCycleCountItem(DefaultIdType id)
    {
        // This would typically open a dialog to add items
        Snackbar.Add("Add item functionality to be implemented", Severity.Info);
        await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel used by the Cycle Counts page for add/edit operations.
/// Maps to CreateCycleCountCommand for create.
/// </summary>
public class CycleCountViewModel
{
    /// <summary>Unique identifier of the cycle count.</summary>
    public DefaultIdType Id { get; set; }

    /// <summary>Cycle count number (unique identifier).</summary>
    public string? CountNumber { get; set; }

    /// <summary>Warehouse where the count is performed.</summary>
    public DefaultIdType WarehouseId { get; set; }

    /// <summary>Optional specific location within the warehouse.</summary>
    public DefaultIdType? WarehouseLocationId { get; set; }

    /// <summary>Scheduled date for the count.</summary>
    public DateTime? ScheduledDate { get; set; } = DateTime.Now;

    /// <summary>Type of count (e.g., Full, Partial, Spot).</summary>
    public string? CountType { get; set; }

    /// <summary>Name of the person performing the count.</summary>
    public string? CounterName { get; set; }

    /// <summary>Name of the supervisor overseeing the count.</summary>
    public string? SupervisorName { get; set; }

    /// <summary>Additional notes or instructions.</summary>
    public string? Notes { get; set; }
}
