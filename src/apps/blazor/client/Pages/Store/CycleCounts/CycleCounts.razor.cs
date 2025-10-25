namespace FSH.Starter.Blazor.Client.Pages.Store.CycleCounts;

/// <summary>
/// Cycle Counts page logic. Provides CRUD and search over CycleCount entities using the generated API client.
/// Supports workflow operations including start, complete, cancel, and reconcile with variance tracking.
/// </summary>
/// <remarks>
/// Note: After making backend changes, regenerate the API client using NSwag to ensure SearchCycleCountsCommand is available.
/// </remarks>
public partial class CycleCounts
{
    private EntityServerTableContext<CycleCountResponse, DefaultIdType, CycleCountViewModel> Context = default!;
    private EntityTable<CycleCountResponse, DefaultIdType, CycleCountViewModel> _table = default!;

    private List<WarehouseResponse> _warehouses = new();
    
    // Search filter properties - bound to UI controls in the razor file
    private DefaultIdType? SearchWarehouseId { get; set; }
    private string? SearchStatus { get; set; }
    private string? SearchCountType { get; set; }  // Available in UI but not yet in SearchCommand
    private DateTime? SearchCountDateFrom { get; set; }  // Available in UI but not yet in SearchCommand
    private DateTime? SearchCountDateTo { get; set; }  // Available in UI but not yet in SearchCommand

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<CycleCountResponse, DefaultIdType, CycleCountViewModel>(
            entityName: "Cycle Count",
            entityNamePlural: "Cycle Counts",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<CycleCountResponse>(x => x.CountNumber, "Count #", "CountNumber"),
                new EntityField<CycleCountResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<CycleCountResponse>(x => x.CountDate, "Count Date", "CountDate", typeof(DateTime)),
                new EntityField<CycleCountResponse>(x => x.Status, "Status", "Status"),
                new EntityField<CycleCountResponse>(x => x.CountType, "Type", "CountType"),
                new EntityField<CycleCountResponse>(x => x.TotalItems, "Total Items", "TotalItems", typeof(int)),
                new EntityField<CycleCountResponse>(x => x.CountedItems, "Counted", "CountedItems", typeof(int)),
                new EntityField<CycleCountResponse>(x => x.VarianceItems, "Variances", "VarianceItems", typeof(int))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                // Temporary: Simple search until API client includes SearchCycleCountsCommand
                var result = await Client.SearchCycleCountsEndpointAsync("1").ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CycleCountResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateCycleCountEndpointAsync("1", viewModel.Adapt<CreateCycleCountCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateCycleCountEndpointAsync("1", id, viewModel.Adapt<UpdateCycleCountCommand>()).ConfigureAwait(false);
            },
            deleteFunc: null); // Cycle counts should not be deleted, only cancelled
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadWarehousesAsync();
    }

    /// <summary>
    /// Loads warehouses for the search filter dropdown.
    /// </summary>
    private async Task LoadWarehousesAsync()
    {
        try
        {
            var command = new SearchWarehousesCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchWarehousesEndpointAsync("1", command).ConfigureAwait(false);
            _warehouses = result.Items?.ToList() ?? new List<WarehouseResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load warehouses: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Views the full details of a cycle count in a dialog.
    /// </summary>
    private async Task ViewCountDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<CycleCountDetailsDialog>
        {
            { x => x.CycleCountId, id }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large, 
        };

        var dialog = await DialogService.ShowAsync<CycleCountDetailsDialog>("Cycle Count Details", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Starts a scheduled cycle count.
    /// </summary>
    private async Task StartCount(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Start Cycle Count",
            "Are you sure you want to start this cycle count? This will change the status to In Progress.",
            yesText: "Start Count",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                var command = new StartCycleCountCommand { Id = id };
                await Client.StartCycleCountEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Cycle count started successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to start cycle count: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Completes an in-progress cycle count.
    /// </summary>
    private async Task CompleteCount(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Complete Cycle Count",
            "Are you sure you want to complete this cycle count? This will calculate variances and finalize the count.",
            yesText: "Complete Count",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                var command = new CompleteCycleCountCommand { Id = id };
                await Client.CompleteCycleCountEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Cycle count completed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to complete cycle count: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Cancels a cycle count.
    /// </summary>
    private async Task CancelCount(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Are you sure you want to cancel this cycle count? This action cannot be undone." },
            { "ButtonText", "Cancel Count" },
            { "Color", Color.Error }
        };

        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Cancel Cycle Count", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            try
            {
                var command = new CancelCycleCountCommand
                {
                    Id = id,
                    Reason = "Cancelled by user"
                };
                await Client.CancelCycleCountEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Cycle count cancelled successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to cancel cycle count: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Reconciles variances in a completed cycle count.
    /// </summary>
    private async Task ReconcileCount(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Reconcile Cycle Count",
            "Are you sure you want to reconcile this cycle count? This will adjust inventory levels to match the counted quantities.",
            yesText: "Reconcile",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                var command = new ReconcileCycleCountCommand { Id = id };
                await Client.ReconcileCycleCountEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Cycle count reconciled successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reconcile cycle count: {ex.Message}", Severity.Error);
            }
        }
    }
}

/// <summary>
/// ViewModel used by the CycleCounts page for add/edit operations.
/// </summary>
public class CycleCountViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string CountNumber { get; set; } = default!;
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DateTime? CountDate { get; set; }
    public string CountType { get; set; } = "Full";
    public string? CountedBy { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
}

