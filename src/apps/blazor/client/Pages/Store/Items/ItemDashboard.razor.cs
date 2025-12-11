using FSH.Starter.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Store.Items;

public partial class ItemDashboard
{
    [Parameter]
    public DefaultIdType Id { get; set; }

    private List<BreadcrumbItem> _breadcrumbs = new();
    private bool _loading = true;
    private string _itemName = "Loading...";
    private string _itemSku = "";
    private ItemDashboardData _dashboard = new();
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
        
        _breadcrumbs = new List<BreadcrumbItem>
        {
            new("Home", "/", false, Icons.Material.Filled.Home),
            new("Store", "/store", false, Icons.Material.Filled.Store),
            new("Items", "/store/items", false, Icons.Material.Filled.Inventory),
            new("Dashboard", null, true, Icons.Material.Filled.Dashboard)
        };

        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        _loading = true;
        try
        {
            var response = await Client.GetItemDashboardEndpointAsync(Id);
            
            if (response != null && !string.IsNullOrEmpty(response.ItemName))
            {
                _dashboard = response.Adapt<ItemDashboardData>();
                _itemName = _dashboard.ItemName;
                _itemSku = _dashboard.Sku;
            }
            else
            {
                _dashboard = GenerateMockDashboardData();
                _itemName = _dashboard.ItemName;
                _itemSku = _dashboard.Sku;
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            _dashboard = GenerateMockDashboardData();
            _itemName = _dashboard.ItemName;
            _itemSku = _dashboard.Sku;
            Snackbar.Add($"Error loading dashboard. Using sample data: {ex.Message}", Severity.Warning);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task RefreshData()
    {
        await LoadDashboardData();
    }

    private async Task OpenHelpDialog()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };
        await DialogService.ShowAsync<ItemDashboardHelpDialog>("Item Dashboard Help", options);
    }

    private ItemDashboardData GenerateMockDashboardData()
    {
        var random = new Random();

        return new ItemDashboardData
        {
            ItemName = "Sample Product XYZ",
            Sku = $"SKU-{Id.ToString()[..8].ToUpper()}",

            TotalSales = random.Next(5000, 50000),
            SalesGrowthPercent = (decimal)(random.NextDouble() * 40 - 10),
            TotalRevenue = random.Next(100000, 500000),
            RevenueGrowthPercent = (decimal)(random.NextDouble() * 30 - 5),

            CurrentStock = random.Next(100, 5000),
            StockStatus = new[] { "Healthy", "Low", "Critical", "Overstock" }[random.Next(4)],

            TurnoverRate = (decimal)(random.NextDouble() * 10 + 2),
            MovementCategory = new[] { "Fast Moving", "Normal", "Slow Moving", "Dead Stock" }[random.Next(4)],

            DailySalesAvg = (decimal)(random.NextDouble() * 50 + 10),
            DailyRevenueAvg = (decimal)(random.NextDouble() * 1000 + 200),
            DailyGrowth = (decimal)(random.NextDouble() * 20 - 10),
            AvgSellingPrice = (decimal)(random.NextDouble() * 50 + 10),

            WeeklySalesAvg = (decimal)(random.NextDouble() * 350 + 70),
            WeeklyRevenueAvg = (decimal)(random.NextDouble() * 7000 + 1400),
            WeeklyGrowth = (decimal)(random.NextDouble() * 25 - 10),

            MonthlySalesAvg = (decimal)(random.NextDouble() * 1500 + 300),
            MonthlyRevenueAvg = (decimal)(random.NextDouble() * 30000 + 6000),
            MonthlyGrowth = (decimal)(random.NextDouble() * 30 - 10),

            YearlySalesProjected = random.Next(15000, 60000),
            YearlyRevenueProjected = random.Next(300000, 1200000),
            YearlyGrowth = (decimal)(random.NextDouble() * 35 - 5),

            PendingBackorders = random.Next(0, 100),
            BackorderRate = (decimal)(random.NextDouble() * 15),
            AvgFulfillmentDays = (decimal)(random.NextDouble() * 5 + 1),

            WarehouseStock = Enumerable.Range(1, 4).Select(i => new WarehouseStockData
            {
                WarehouseName = $"Warehouse {(char)('A' + i - 1)}",
                OnHand = random.Next(50, 1500),
                Reserved = random.Next(0, 200),
                Available = random.Next(50, 1300),
                Status = new[] { "Healthy", "Low", "Critical" }[random.Next(3)],
                ReorderPoint = random.Next(50, 200)
            }).ToList(),

            RecentTransactions = Enumerable.Range(1, 15).Select(i => new TransactionData
            {
                Date = DateTime.Now.AddDays(-random.Next(1, 30)),
                Type = new[] { "Sale", "Purchase", "Transfer", "Adjustment" }[random.Next(4)],
                Quantity = random.Next(1, 100),
                Value = (decimal)(random.NextDouble() * 5000 + 100)
            }).OrderByDescending(t => t.Date).ToList(),

            SupplierPerformance = Enumerable.Range(1, 5).Select(i => new SupplierPerformanceData
            {
                Name = $"Supplier {i}",
                TotalOrders = random.Next(10, 200),
                OnTimeDeliveryPercent = (decimal)(random.NextDouble() * 20 + 80),
                QualityPercent = (decimal)(random.NextDouble() * 15 + 85)
            }).ToList()
        };
    }

    private MudBlazor.Color GetTrendColor(decimal value) => value >= 0 ? MudBlazor.Color.Success : MudBlazor.Color.Error;

    private MudBlazor.Color GetStockLevelColor(string status) => status switch
    {
        "Healthy" => MudBlazor.Color.Success,
        "Low" => MudBlazor.Color.Warning,
        "Critical" => MudBlazor.Color.Error,
        "Overstock" => MudBlazor.Color.Info,
        _ => MudBlazor.Color.Default
    };

    private MudBlazor.Color GetTurnoverColor(string category) => category switch
    {
        "Fast Moving" => MudBlazor.Color.Success,
        "Normal" => MudBlazor.Color.Info,
        "Slow Moving" => MudBlazor.Color.Warning,
        "Dead Stock" => MudBlazor.Color.Error,
        _ => MudBlazor.Color.Default
    };

    private MudBlazor.Color GetBackorderColor(int count) => count switch
    {
        0 => MudBlazor.Color.Success,
        < 10 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetBackorderRateColor(decimal rate) => rate switch
    {
        < 5 => MudBlazor.Color.Success,
        < 10 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetTransactionTypeColor(string type) => type switch
    {
        "Sale" => MudBlazor.Color.Success,
        "Purchase" => MudBlazor.Color.Primary,
        "Transfer" => MudBlazor.Color.Info,
        "Adjustment" => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Default
    };

    private MudBlazor.Color GetPerformanceColor(decimal value) => value switch
    {
        >= 95 => MudBlazor.Color.Success,
        >= 85 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    // Local DTOs
    private class ItemDashboardData
    {
        public string ItemName { get; set; } = "";
        public string Sku { get; set; } = "";

        public int TotalSales { get; set; }
        public decimal SalesGrowthPercent { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenueGrowthPercent { get; set; }

        public int CurrentStock { get; set; }
        public string StockStatus { get; set; } = "";

        public decimal TurnoverRate { get; set; }
        public string MovementCategory { get; set; } = "";

        public decimal DailySalesAvg { get; set; }
        public decimal DailyRevenueAvg { get; set; }
        public decimal DailyGrowth { get; set; }
        public decimal AvgSellingPrice { get; set; }

        public decimal WeeklySalesAvg { get; set; }
        public decimal WeeklyRevenueAvg { get; set; }
        public decimal WeeklyGrowth { get; set; }

        public decimal MonthlySalesAvg { get; set; }
        public decimal MonthlyRevenueAvg { get; set; }
        public decimal MonthlyGrowth { get; set; }

        public int YearlySalesProjected { get; set; }
        public decimal YearlyRevenueProjected { get; set; }
        public decimal YearlyGrowth { get; set; }

        public int PendingBackorders { get; set; }
        public decimal BackorderRate { get; set; }
        public decimal AvgFulfillmentDays { get; set; }

        public List<WarehouseStockData> WarehouseStock { get; set; } = new();
        public List<TransactionData> RecentTransactions { get; set; } = new();
        public List<SupplierPerformanceData> SupplierPerformance { get; set; } = new();
    }

    private class WarehouseStockData
    {
        public string WarehouseName { get; set; } = "";
        public int OnHand { get; set; }
        public int Reserved { get; set; }
        public int Available { get; set; }
        public string Status { get; set; } = "";
        public int ReorderPoint { get; set; }
    }

    private class TransactionData
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Value { get; set; }
    }

    private class SupplierPerformanceData
    {
        public string Name { get; set; } = "";
        public int TotalOrders { get; set; }
        public decimal OnTimeDeliveryPercent { get; set; }
        public decimal QualityPercent { get; set; }
    }
}
