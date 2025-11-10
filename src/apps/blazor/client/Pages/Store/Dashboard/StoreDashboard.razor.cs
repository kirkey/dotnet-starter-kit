namespace FSH.Starter.Blazor.Client.Pages.Store.Dashboard;

/// <summary>
/// Store Dashboard page logic. Loads live metrics from Store API endpoints.
/// </summary>
public partial class StoreDashboard
{
    /// <summary>Flag indicating whether the dashboard is still loading metrics.</summary>
    private bool _loading = true;

    /// <summary>Holds top-level KPI values displayed on the dashboard.</summary>
    private readonly StoreDashboardMetrics _metrics = new();

    /// <summary>Holds datasets and labels for small inline charts.</summary>
    private readonly StoreDashboardCharts _charts = new();

    /// <summary>Low stock items for the data table.</summary>
    private readonly List<StockItem> _stockItems = [];

    /// <summary>Recent goods receipts list.</summary>
    private readonly List<GoodsReceiptItem> _goodsReceipts = [];

    /// <summary>Active inventory transfers list.</summary>
    private readonly List<InventoryTransferItem> _inventoryTransfers = [];

    /// <summary>Active pick lists.</summary>
    private readonly List<PickListItem> _pickLists = [];

    /// <summary>Active put away tasks.</summary>
    private readonly List<PutAwayTaskItem> _putAwayTasks = [];

    /// <summary>
    /// Initialize the dashboard - load real data from API.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadDashboardDataAsync();
    }

    /// <summary>
    /// Load all dashboard data from Store API endpoints.
    /// </summary>
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            _loading = true;

            // Load metrics in parallel for better performance
            await Task.WhenAll(
                LoadItemMetricsAsync(),
                LoadPurchaseOrderMetricsAsync(),
                LoadWarehouseMetricsAsync(),
                LoadSupplierMetricsAsync(),
                LoadStockLevelsAsync(),
                LoadGoodsReceiptsAsync(),
                LoadInventoryTransfersAsync(),
                LoadPickListsAsync(),
                LoadPutAwayTasksAsync(),
                LoadCycleCountsAsync()
            );

            // Initialize chart data after metrics are loaded
            InitializeCharts();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading dashboard data: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Load item-related metrics (total items, low stock, expiring items).
    /// </summary>
    private async Task LoadItemMetricsAsync()
    {
        try
        {
            // Get total items count
            var itemsResult = await Client.SearchItemsEndpointAsync("1", new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.TotalItems = itemsResult.TotalCount;

            // Get perishable items count (these could potentially expire)
            var perishableResult = await Client.SearchItemsEndpointAsync("1", new SearchItemsCommand
            {
                IsPerishable = true,
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.PerishableItemsCount = perishableResult.TotalCount;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading item metrics: {ex.Message}", Severity.Warning);
            _metrics.TotalItems = 0;
            _metrics.PerishableItemsCount = 0;
        }
    }

    /// <summary>
    /// Load purchase order metrics by status.
    /// </summary>
    private async Task LoadPurchaseOrderMetricsAsync()
    {
        try
        {
            // Get pending purchase orders (Draft + Submitted)
            var draftResult = await Client.SearchPurchaseOrdersEndpointAsync("1", new SearchPurchaseOrdersCommand
            {
                Status = "Draft",
                PageNumber = 1,
                PageSize = 1
            });
            var submittedResult = await Client.SearchPurchaseOrdersEndpointAsync("1", new SearchPurchaseOrdersCommand
            {
                Status = "Submitted",
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.PurchaseOrdersPending = draftResult.TotalCount + submittedResult.TotalCount;

            // Get approved/sent purchase orders
            var approvedResult = await Client.SearchPurchaseOrdersEndpointAsync("1", new SearchPurchaseOrdersCommand
            {
                Status = "Approved",
                PageNumber = 1,
                PageSize = 1
            });
            var sentResult = await Client.SearchPurchaseOrdersEndpointAsync("1", new SearchPurchaseOrdersCommand
            {
                Status = "Sent",
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.PurchaseOrdersApproved = approvedResult.TotalCount + sentResult.TotalCount;

            // Get received purchase orders
            var receivedResult = await Client.SearchPurchaseOrdersEndpointAsync("1", new SearchPurchaseOrdersCommand
            {
                Status = "Received",
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.PurchaseOrdersCompleted = receivedResult.TotalCount;

            // Get cancelled purchase orders
            var cancelledResult = await Client.SearchPurchaseOrdersEndpointAsync("1", new SearchPurchaseOrdersCommand
            {
                Status = "Cancelled",
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.PurchaseOrdersCancelled = cancelledResult.TotalCount;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading purchase order metrics: {ex.Message}", Severity.Warning);
            _metrics.PurchaseOrdersPending = 0;
            _metrics.PurchaseOrdersApproved = 0;
            _metrics.PurchaseOrdersCompleted = 0;
            _metrics.PurchaseOrdersCancelled = 0;
        }
    }

    /// <summary>
    /// Load warehouse metrics.
    /// </summary>
    private async Task LoadWarehouseMetricsAsync()
    {
        try
        {
            var warehousesResult = await Client.SearchWarehousesEndpointAsync("1", new SearchWarehousesRequest
            {
                PageNumber = 1,
                PageSize = 10,
            });
            _metrics.WarehousesCount = warehousesResult.TotalCount;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading warehouse metrics: {ex.Message}", Severity.Warning);
            _metrics.WarehousesCount = 0;
        }
    }

    /// <summary>
    /// Load supplier count.
    /// </summary>
    private async Task LoadSupplierMetricsAsync()
    {
        try
        {
            var suppliersResult = await Client.SearchSuppliersEndpointAsync("1", new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 1
            });
            _metrics.SuppliersCount = suppliersResult.TotalCount;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading supplier metrics: {ex.Message}", Severity.Warning);
            _metrics.SuppliersCount = 0;
        }
    }

    /// <summary>
    /// Load stock level data and identify low stock items.
    /// </summary>
    private async Task LoadStockLevelsAsync()
    {
        try
        {
            // Get all items to check stock levels
            var itemsResult = await Client.SearchItemsEndpointAsync("1", new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 10
                // No OrderBy - let specification use its built-in ordering (Name)
            });

            // Process items to find low stock
            _stockItems.Clear();
            int lowStockCount = 0;

            if (itemsResult?.Items != null)
            {
                foreach (var item in itemsResult.Items)
                {
                    // Get stock levels for this item
                    var stockResult = await Client.SearchStockLevelsEndpointAsync("1", new SearchStockLevelsCommand
                    {
                        ItemId = item.Id,
                        PageNumber = 1,
                        PageSize = 1
                    });

                    int totalStock = stockResult?.Items?.Sum(s => s.QuantityOnHand) ?? 0;
                    bool isLowStock = totalStock <= item.ReorderPoint;

                if (isLowStock)
                {
                    lowStockCount++;
                }

                    // Add to table (show first 10 items)
                    if (_stockItems.Count < 10)
                    {
                        _stockItems.Add(new StockItem
                        {
                            Id = _stockItems.Count + 1,
                            Product = item.Name ?? "Unknown",
                            Sku = item.Sku ?? "N/A",
                            CurrentStock = totalStock,
                            Threshold = item.ReorderPoint,
                            ReorderQuantity = item.ReorderQuantity,
                            Status = isLowStock ? "Low Stock" : "In Stock",
                            Supplier = "N/A",
                            LeadTime = $"{item.LeadTimeDays} days"
                        });
                    }
                }

                _metrics.LowStockItems = lowStockCount;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading stock levels: {ex.Message}", Severity.Warning);
            _metrics.LowStockItems = 0;
        }
    }

    /// <summary>
    /// Initialize chart data based on loaded metrics.
    /// </summary>
    private void InitializeCharts()
    {
        // Purchase Order Status Chart (Donut)
        _charts.OrderSourceLabels = ["Pending", "Approved/Sent", "Completed", "Cancelled"];
        var total = _metrics.PurchaseOrdersPending + _metrics.PurchaseOrdersApproved + 
                    _metrics.PurchaseOrdersCompleted + _metrics.PurchaseOrdersCancelled;
        
        if (total > 0)
        {
            _charts.OrderSourceDatasets =
            [
                new()
                {
                    Name = "Purchase Orders",
                    Data =
                    [
                        _metrics.PurchaseOrdersPending, _metrics.PurchaseOrdersApproved,
                        _metrics.PurchaseOrdersCompleted, _metrics.PurchaseOrdersCancelled
                    ]
                }
            ];
        }
        else
        {
            _charts.OrderSourceDatasets = [new() { Name = "Purchase Orders", Data = [1] }];
        }

        // Stock Level Chart (Bar) - Show top items by stock
        if (_stockItems.Count > 0)
        {
            _charts.StockLabels = _stockItems.Take(10).Select(s => s.Product).ToArray();
            _charts.StockDatasets =
            [
                new()
                {
                    Name = "Current Stock",
                    Data = _stockItems.Take(10).Select(s => (double)s.CurrentStock).ToArray()
                },

                new() { Name = "Reorder Point", Data = _stockItems.Take(10).Select(s => (double)s.Threshold).ToArray() }
            ];
        }
    }

    /// <summary>
    /// Load recent goods receipts.
    /// </summary>
    private async Task LoadGoodsReceiptsAsync()
    {
        try
        {
            var result = await Client.SearchGoodsReceiptsEndpointAsync("1", new SearchGoodsReceiptsCommand
            {
                PageNumber = 1,
                PageSize = 10
                // No OrderBy - let specification use its built-in ordering (ReceivedDate desc, ReceiptNumber)
            });

            _metrics.GoodsReceiptsCount = result.TotalCount;

            _goodsReceipts.Clear();
            if (result.Items != null)
            {
                foreach (var receipt in result.Items)
                {
                    _goodsReceipts.Add(new GoodsReceiptItem
                    {
                        ReceiptNumber = receipt.ReceiptNumber ?? "N/A",
                        PurchaseOrderNumber = receipt.PurchaseOrderId?.ToString() ?? "N/A",
                        WarehouseName = receipt.Name ?? "N/A",
                        ReceivedDate = receipt.ReceivedDate.ToString("MM/dd/yyyy"),
                        Status = receipt.Status ?? "Draft"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading goods receipts: {ex.Message}", Severity.Warning);
            _metrics.GoodsReceiptsCount = 0;
        }
    }

    /// <summary>
    /// Load active inventory transfers.
    /// </summary>
    private async Task LoadInventoryTransfersAsync()
    {
        try
        {
            // Get all transfers without status filter to avoid permission issues
            var result = await Client.SearchInventoryTransfersEndpointAsync("1", new SearchInventoryTransfersCommand
            {
                PageNumber = 1,
                PageSize = 10
                // No OrderBy - let specification use its built-in ordering (TransferDate desc, TransferNumber)
            });

            _inventoryTransfers.Clear();
            
            // Filter for pending and in-transit on client side
            var activeTransfers = result.Items?
                .Where(t => t.Status is "Pending" or "InTransit" or "Approved")
                .ToList() ?? [];

            _metrics.InventoryTransfersPending = activeTransfers.Count;

            foreach (var transfer in activeTransfers.Take(10))
            {
                _inventoryTransfers.Add(new InventoryTransferItem
                {
                    TransferNumber = transfer.TransferNumber ?? "N/A",
                    FromWarehouse = transfer.FromWarehouseName ?? "N/A",
                    ToWarehouse = transfer.ToWarehouseName ?? "N/A",
                    ItemCount = 0, // Item count not available in list response
                    Status = transfer.Status ?? "Pending"
                });
            }
        }
        catch (Exception ex)
        {
            // If unauthorized, silently set to 0 (user may not have permission)
            if (ex.Message.Contains("403") || ex.Message.Contains("unauthorized"))
            {
                _metrics.InventoryTransfersPending = 0;
            }
            else
            {
                Snackbar.Add($"Error loading inventory transfers: {ex.Message}", Severity.Warning);
                _metrics.InventoryTransfersPending = 0;
            }
        }
    }

    /// <summary>
    /// Load active pick lists.
    /// </summary>
    private async Task LoadPickListsAsync()
    {
        try
        {
            var result = await Client.SearchPickListsEndpointAsync("1", new SearchPickListsCommand
            {
                Status = "InProgress",
                PageNumber = 1,
                PageSize = 10
                // No OrderBy - let specification use its built-in ordering (Priority desc, CreatedOn desc)
            });

            _metrics.ActivePickLists = result.TotalCount;

            _pickLists.Clear();
            if (result.Items != null)
            {
                foreach (var pickList in result.Items)
                {
                    _pickLists.Add(new PickListItem
                    {
                        PickListNumber = pickList.PickListNumber ?? "N/A",
                        WarehouseName = pickList.Name ?? "N/A",
                        AssignedTo = pickList.AssignedTo ?? "Unassigned",
                        Priority = pickList.Priority switch
                        {
                            1 => "Low",
                            2 => "Normal",
                            3 => "High",
                            _ => "Normal"
                        },
                        Status = pickList.Status ?? "Pending"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading pick lists: {ex.Message}", Severity.Warning);
            _metrics.ActivePickLists = 0;
        }
    }

    /// <summary>
    /// Load active put away tasks.
    /// </summary>
    private async Task LoadPutAwayTasksAsync()
    {
        try
        {
            var result = await Client.SearchPutAwayTasksEndpointAsync("1", new SearchPutAwayTasksCommand
            {
                PageNumber = 1,
                PageSize = 10
                // No OrderBy - let specification use its built-in ordering
            });

            _putAwayTasks.Clear();
            if (result.Items != null)
            {
                foreach (var task in result.Items.Where(t => t.Status != "Completed"))
                {
                    _putAwayTasks.Add(new PutAwayTaskItem
                    {
                        TaskNumber = task.TaskNumber ?? "N/A",
                        WarehouseName = task.Name ?? "N/A",
                        ItemCount = task.TotalLines,
                        AssignedTo = task.AssignedTo ?? "Unassigned",
                        Status = task.Status ?? "Pending"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading put away tasks: {ex.Message}", Severity.Warning);
        }
    }

    /// <summary>
    /// Load cycle counts in progress.
    /// </summary>
    private async Task LoadCycleCountsAsync()
    {
        try
        {
            var request = new SearchCycleCountsRequest
            {
                PageNumber = 1,
                PageSize = 100,
                Status = "InProgress"
            };
            
            var result = await Client.SearchCycleCountsEndpointAsync("1", request);

            // Count only "InProgress" status cycle counts
            _metrics.CycleCountsInProgress = result.Items?.Count(c => c.Status == "InProgress") ?? 0;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading cycle counts: {ex.Message}", Severity.Warning);
            _metrics.CycleCountsInProgress = 0;
        }
    }

    /// <summary>
    /// Get color for transfer status.
    /// </summary>
    private static Color GetTransferStatusColor(string status) => status switch
    {
        "Pending" => Color.Warning,
        "Approved" => Color.Info,
        "InTransit" => Color.Primary,
        "Completed" => Color.Success,
        "Cancelled" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Get color for priority.
    /// </summary>
    private static Color GetPriorityColor(string priority) => priority switch
    {
        "High" => Color.Error,
        "Medium" => Color.Warning,
        "Normal" => Color.Info,
        _ => Color.Default
    };

    /// <summary>
    /// Get color for pick list status.
    /// </summary>
    private static Color GetPickListStatusColor(string status) => status switch
    {
        "Pending" => Color.Warning,
        "InProgress" => Color.Primary,
        "Completed" => Color.Success,
        "Cancelled" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Get color for put away status.
    /// </summary>
    private static Color GetPutAwayStatusColor(string status) => status switch
    {
        "Pending" => Color.Warning,
        "Assigned" => Color.Info,
        "InProgress" => Color.Primary,
        "Completed" => Color.Success,
        _ => Color.Default
    };
}

/// <summary>
/// Dashboard KPI container for Store module (Inventory, Warehouse, Purchase Orders).
/// </summary>
internal sealed class StoreDashboardMetrics
{
    /// <summary>Total number of items in inventory.</summary>
    public int TotalItems { get; set; }

    /// <summary>Number of perishable items in inventory.</summary>
    public int PerishableItemsCount { get; set; }

    /// <summary>Number of items whose current stock is at or below reorder threshold.</summary>
    public int LowStockItems { get; set; }

    /// <summary>Pending purchase orders (Draft + Submitted).</summary>
    public int PurchaseOrdersPending { get; set; }

    /// <summary>Approved/Sent purchase orders.</summary>
    public int PurchaseOrdersApproved { get; set; }

    /// <summary>Completed purchase orders (Received).</summary>
    public int PurchaseOrdersCompleted { get; set; }

    /// <summary>Cancelled purchase orders.</summary>
    public int PurchaseOrdersCancelled { get; set; }

    /// <summary>Total number of suppliers.</summary>
    public int SuppliersCount { get; set; }

    /// <summary>Total number of warehouses.</summary>
    public int WarehousesCount { get; set; }

    /// <summary>Total number of goods receipts.</summary>
    public int GoodsReceiptsCount { get; set; }

    /// <summary>Number of pending inventory transfers.</summary>
    public int InventoryTransfersPending { get; set; }

    /// <summary>Number of active pick lists.</summary>
    public int ActivePickLists { get; set; }

    /// <summary>Number of cycle counts in progress.</summary>
    public int CycleCountsInProgress { get; set; }
}

/// <summary>
/// Dashboard chart data container.
/// </summary>
internal sealed class StoreDashboardCharts
{
    /// <summary>Labels for order sources donut chart.</summary>
    public string[] OrderSourceLabels { get; set; } = [];

    /// <summary>Datasets for order sources donut chart.</summary>
    public List<ChartSeries> OrderSourceDatasets { get; set; } = [];

    /// <summary>Labels for stock level bar chart.</summary>
    public string[] StockLabels { get; set; } = [];

    /// <summary>Datasets for stock level bar chart.</summary>
    public List<ChartSeries> StockDatasets { get; set; } = [];

    /// <summary>Labels for weekly line chart.</summary>
    public string[] WeeklyLabels { get; set; } = [];

    /// <summary>Datasets for the weekly line chart (Sales vs Restocks).</summary>
    public List<ChartSeries> WeeklyDatasets { get; set; } = [];

    /// <summary>Labels for the Monthly Visits bar chart.</summary>
    public string[] MonthlyVisitsLabels { get; set; } = [];

    /// <summary>Datasets for Monthly Visits bar chart.</summary>
    public List<ChartSeries> MonthlyVisitsDatasets { get; set; } = [];

    /// <summary>Labels for the Users bar chart.</summary>
    public string[] UsersLabels { get; set; } = [];

    /// <summary>Datasets for Users bar chart.</summary>
    public List<ChartSeries> UsersDatasets { get; set; } = [];
}

/// <summary>
/// Stock item model for the current stock overview table.
/// </summary>
internal sealed class StockItem
{
    /// <summary>Stock item identifier.</summary>
    public int Id { get; set; }

    /// <summary>Product SKU.</summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>Product name.</summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>Current stock quantity.</summary>
    public int CurrentStock { get; set; }

    /// <summary>Reorder threshold.</summary>
    public int Threshold { get; set; }

    /// <summary>Recommended reorder quantity.</summary>
    public int ReorderQuantity { get; set; }

    /// <summary>Stock status (e.g., "In Stock", "Low Stock").</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Supplier name.</summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>Lead time for restocking.</summary>
    public string LeadTime { get; set; } = string.Empty;
}

/// <summary>
/// Goods receipt item model for dashboard display.
/// </summary>
internal sealed class GoodsReceiptItem
{
    /// <summary>Receipt number.</summary>
    public string ReceiptNumber { get; set; } = string.Empty;

    /// <summary>Purchase order number.</summary>
    public string PurchaseOrderNumber { get; set; } = string.Empty;

    /// <summary>Warehouse name.</summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>Received date.</summary>
    public string ReceivedDate { get; set; } = string.Empty;

    /// <summary>Receipt status.</summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Inventory transfer item model for dashboard display.
/// </summary>
internal sealed class InventoryTransferItem
{
    /// <summary>Transfer number.</summary>
    public string TransferNumber { get; set; } = string.Empty;

    /// <summary>Source warehouse name.</summary>
    public string FromWarehouse { get; set; } = string.Empty;

    /// <summary>Destination warehouse name.</summary>
    public string ToWarehouse { get; set; } = string.Empty;

    /// <summary>Number of items in transfer.</summary>
    public int ItemCount { get; set; }

    /// <summary>Transfer status.</summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Pick list item model for dashboard display.
/// </summary>
internal sealed class PickListItem
{
    /// <summary>Pick list number.</summary>
    public string PickListNumber { get; set; } = string.Empty;

    /// <summary>Warehouse name.</summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>Assigned picker name.</summary>
    public string AssignedTo { get; set; } = string.Empty;

    /// <summary>Pick list priority.</summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>Pick list status.</summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Put away task item model for dashboard display.
/// </summary>
internal sealed class PutAwayTaskItem
{
    /// <summary>Task number.</summary>
    public string TaskNumber { get; set; } = string.Empty;

    /// <summary>Warehouse name.</summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>Number of items to put away.</summary>
    public int ItemCount { get; set; }

    /// <summary>Assigned worker name.</summary>
    public string AssignedTo { get; set; } = string.Empty;

    /// <summary>Task status.</summary>
    public string Status { get; set; } = string.Empty;
}

