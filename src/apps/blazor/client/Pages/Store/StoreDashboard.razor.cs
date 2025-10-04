namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Store Dashboard page logic. Loads live metrics where available (e.g., grocery item total) and uses mock data for other widgets.
/// </summary>
public partial class StoreDashboard
{
    

    /// <summary>Flag indicating whether the dashboard is still loading metrics.</summary>
    private bool _loading = true;

    /// <summary>Holds top-level KPI values displayed on the dashboard.</summary>
    private readonly StoreDashboardMetrics _metrics = new();

    /// <summary>Holds datasets and labels for small inline charts.</summary>
    private readonly StoreDashboardCharts _charts = new();

    /// <summary>Mock stock items for the data table.</summary>
    private readonly List<StockItem> _stockItems = new();

    /// <summary>
    /// Initialize the dashboard - load mock data immediately to avoid hanging.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Initialize with mock data immediately so something shows
        LoadMockData();
        await Task.CompletedTask;
    }

    /// <summary>
    /// After first render, try to load real data.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadRealDataAsync();
        }
    }

    /// <summary>
    /// Load mock data immediately to prevent hanging skeletons.
    /// </summary>
    private void LoadMockData()
    {
        // Mock/prototype data
        _metrics.TotalItems = 35500;
        _metrics.LowStockItems = 5;
        _metrics.ExpiringToday = 3;
        _metrics.ExpiringThisWeek = 18;
        _metrics.ExpiringThisMonth = 42;

        _metrics.PurchaseOrdersPending = 10;
        _metrics.PurchaseOrdersStaged = 7;
        _metrics.PurchaseOrdersCompleted = 20;

        _metrics.TransfersInTransit = 2;
        _metrics.TransfersCompleted = 14;
        _metrics.SuppliersCount = 9;
        _metrics.WarehousesCount = 3;
        _metrics.WarehouseCapacityUsedPercent = 68f;

        _metrics.PosTodaySales = 3245.75m;
        _metrics.PosTodayTransactions = 112;
        _metrics.PosAverageBasket = 28.98m;

        // Order sources donut chart
        _charts.OrderSourceLabels = new[] { "Website Orders", "Mobile App Orders", "In-Store Orders" };
        _charts.OrderSourceDatasets = new List<ChartSeries>
        {
            new() { 
                Name = "Order Sources", 
                Data = new double[] { 40, 35, 25 },
                // BackgroundColors = new[] { "#4285f4", "#34a853", "#ea4335" }
            }
        };

        // Stock level bar chart
        _charts.StockLabels = new[] { "Milk", "Rice", "Beef", "Eggs", "Tuna", "Salt", "Corn", "Soup", "Kale", "Pork", "Lamb", "Beans" };
        _charts.StockDatasets = new List<ChartSeries>
        {
            new() { 
                Name = "Stock Level", 
                Data = new double[] { 80, 90, 120, 40, 100, 60, 150, 30, 110, 90, 130, 20 },
                // BackgroundColors = new[] { "#4285f4" }
            }
        };

        // Legacy charts for compatibility
        _charts.WeeklyLabels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        _charts.WeeklyDatasets = new List<ChartSeries>
        {
            new() { Name = "Sales", Data = new double[] { 420, 510, 480, 560, 610, 720, 680 } },
            new() { Name = "Restocks", Data = new double[] { 120, 140, 130, 150, 170, 200, 180 } },
        };

        _charts.MonthlyVisitsLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        _charts.MonthlyVisitsDatasets = new List<ChartSeries>
        {
            new() { Name = "Visits", Data = new double[] { 52, 49, 60, 70, 68, 75, 80, 78, 85, 90, 88, 95 } }
        };

        _charts.UsersLabels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        _charts.UsersDatasets = new List<ChartSeries>
        {
            new() { Name = "Users", Data = new double[] { 120, 140, 130, 160, 180, 200, 190 } }
        };

        // Stock table items
        _stockItems.Clear();
        _stockItems.AddRange(new[]
        {
            new StockItem { Id = 1, Product = "Apples", CurrentStock = 50, Threshold = 100, ReorderQuantity = 50, Status = "Needs Reordering", Supplier = "ABC Fruit Suppliers", LeadTime = "2 days" },
            new StockItem { Id = 2, Product = "Bananas", CurrentStock = 120, Threshold = 80, ReorderQuantity = 40, Status = "In Stock", Supplier = "Fresh Produce Co", LeadTime = "1 day" },
            new StockItem { Id = 3, Product = "Oranges", CurrentStock = 75, Threshold = 60, ReorderQuantity = 30, Status = "In Stock", Supplier = "Citrus Farm Ltd", LeadTime = "3 days" },
            new StockItem { Id = 4, Product = "Milk", CurrentStock = 25, Threshold = 50, ReorderQuantity = 60, Status = "Needs Reordering", Supplier = "Dairy Fresh", LeadTime = "1 day" },
            new StockItem { Id = 5, Product = "Bread", CurrentStock = 40, Threshold = 30, ReorderQuantity = 20, Status = "In Stock", Supplier = "Local Bakery", LeadTime = "Same day" }
        });

        // Set loading to false so data shows immediately
        _loading = false;
    }

    /// <summary>
    /// Try to load real data from API and update the display.
    /// </summary>
    private async Task LoadRealDataAsync()
    {
        try
        {
            // Try to get real grocery item count
            var request = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 1,
            };
            
            var result = await Client.SearchItemsEndpointAsync("1", request);
            
            // Update only the total items with real data
            _metrics.TotalItems = result.TotalCount;
            
            // Trigger UI update
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            // If API call fails, keep mock data
            _metrics.TotalItems = 35500; // Fallback to mock
            await InvokeAsync(StateHasChanged);
        }
    }
}

/// <summary>
/// Dashboard KPI container for Store module (Grocery, Warehouse, POS).
/// </summary>
public sealed class StoreDashboardMetrics
{
    /// <summary>Total number of grocery items.</summary>
    public int TotalItems { get; set; }

    /// <summary>Number of items whose current stock is at or below reorder threshold.</summary>
    public int LowStockItems { get; set; }

    /// <summary>Number of perishable items expiring today.</summary>
    public int ExpiringToday { get; set; }

    /// <summary>Number of perishable items expiring this calendar week.</summary>
    public int ExpiringThisWeek { get; set; }

    /// <summary>Number of perishable items expiring this calendar month.</summary>
    public int ExpiringThisMonth { get; set; }

    /// <summary>Pending purchase orders.</summary>
    public int PurchaseOrdersPending { get; set; }

    /// <summary>Staged purchase orders (e.g., confirmed/staging).</summary>
    public int PurchaseOrdersStaged { get; set; }

    /// <summary>Completed purchase orders.</summary>
    public int PurchaseOrdersCompleted { get; set; }

    /// <summary>Inventory transfers currently in transit between warehouses.</summary>
    public int TransfersInTransit { get; set; }

    /// <summary>Completed inventory transfers.</summary>
    public int TransfersCompleted { get; set; }

    /// <summary>Total number of suppliers.</summary>
    public int SuppliersCount { get; set; }

    /// <summary>Total number of warehouses.</summary>
    public int WarehousesCount { get; set; }

    /// <summary>Current warehouse capacity usage percentage (0-100).</summary>
    public float WarehouseCapacityUsedPercent { get; set; }

    /// <summary>Total POS sales for today.</summary>
    public decimal PosTodaySales { get; set; }

    /// <summary>Total number of POS transactions for today.</summary>
    public int PosTodayTransactions { get; set; }

    /// <summary>Average basket size for today.</summary>
    public decimal PosAverageBasket { get; set; }
}

/// <summary>
/// Dashboard chart data container.
/// </summary>
public sealed class StoreDashboardCharts
{
    /// <summary>Labels for order sources donut chart.</summary>
    public string[] OrderSourceLabels { get; set; } = Array.Empty<string>();

    /// <summary>Datasets for order sources donut chart.</summary>
    public List<ChartSeries> OrderSourceDatasets { get; set; } = new();

    /// <summary>Labels for stock level bar chart.</summary>
    public string[] StockLabels { get; set; } = Array.Empty<string>();

    /// <summary>Datasets for stock level bar chart.</summary>
    public List<ChartSeries> StockDatasets { get; set; } = new();

    /// <summary>Labels for weekly line chart.</summary>
    public string[] WeeklyLabels { get; set; } = Array.Empty<string>();

    /// <summary>Datasets for the weekly line chart (Sales vs Restocks).</summary>
    public List<ChartSeries> WeeklyDatasets { get; set; } = new();

    /// <summary>Labels for the Monthly Visits bar chart.</summary>
    public string[] MonthlyVisitsLabels { get; set; } = Array.Empty<string>();

    /// <summary>Datasets for Monthly Visits bar chart.</summary>
    public List<ChartSeries> MonthlyVisitsDatasets { get; set; } = new();

    /// <summary>Labels for the Users bar chart.</summary>
    public string[] UsersLabels { get; set; } = Array.Empty<string>();

    /// <summary>Datasets for Users bar chart.</summary>
    public List<ChartSeries> UsersDatasets { get; set; } = new();
}

/// <summary>
/// Stock item model for the current stock overview table.
/// </summary>
public sealed class StockItem
{
    /// <summary>Stock item identifier.</summary>
    public int Id { get; set; }

    /// <summary>Product name.</summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>Current stock quantity.</summary>
    public int CurrentStock { get; set; }

    /// <summary>Reorder threshold.</summary>
    public int Threshold { get; set; }

    /// <summary>Recommended reorder quantity.</summary>
    public int ReorderQuantity { get; set; }

    /// <summary>Stock status (e.g., "In Stock", "Needs Reordering").</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Supplier name.</summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>Lead time for restocking.</summary>
    public string LeadTime { get; set; } = string.Empty;
}
