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
    private readonly List<StockItem> _stockItems = new();

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
                LoadStockLevelsAsync()
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
            var warehousesResult = await Client.SearchWarehousesEndpointAsync("1", new SearchWarehousesCommand
            {
                PageNumber = 1,
                PageSize = 100
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
                PageSize = 10,
                OrderBy = new[] { "ReorderPoint desc" }
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
        _charts.OrderSourceLabels = new[] { "Pending", "Approved/Sent", "Completed", "Cancelled" };
        var total = _metrics.PurchaseOrdersPending + _metrics.PurchaseOrdersApproved + 
                    _metrics.PurchaseOrdersCompleted + _metrics.PurchaseOrdersCancelled;
        
        if (total > 0)
        {
            _charts.OrderSourceDatasets = new List<ChartSeries>
            {
                new() { 
                    Name = "Purchase Orders", 
                    Data = new double[] 
                    { 
                        _metrics.PurchaseOrdersPending,
                        _metrics.PurchaseOrdersApproved,
                        _metrics.PurchaseOrdersCompleted,
                        _metrics.PurchaseOrdersCancelled
                    }
                }
            };
        }
        else
        {
            _charts.OrderSourceDatasets = new List<ChartSeries>
            {
                new() { Name = "Purchase Orders", Data = new double[] { 1 } }
            };
        }

        // Stock Level Chart (Bar) - Show top items by stock
        if (_stockItems.Count > 0)
        {
            _charts.StockLabels = _stockItems.Take(10).Select(s => s.Product).ToArray();
            _charts.StockDatasets = new List<ChartSeries>
            {
                new() { 
                    Name = "Current Stock", 
                    Data = _stockItems.Take(10).Select(s => (double)s.CurrentStock).ToArray()
                },
                new() { 
                    Name = "Reorder Point", 
                    Data = _stockItems.Take(10).Select(s => (double)s.Threshold).ToArray()
                }
            };
        }
    }
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
