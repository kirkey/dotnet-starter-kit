using FSH.Starter.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Store.Suppliers;

public partial class SupplierDashboard
{
    [Parameter]
    public DefaultIdType Id { get; set; }

    private List<BreadcrumbItem> _breadcrumbs = new();
    private bool _loading = true;
    private string _supplierName = "Loading...";
    private SupplierDashboardData _dashboard = new();
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
            new("Suppliers", "/store/suppliers", false, Icons.Material.Filled.Business),
            new("Dashboard", null, true, Icons.Material.Filled.Dashboard)
        };

        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        _loading = true;
        try
        {
            var response = await Client.GetSupplierDashboardEndpointAsync(Id);
            
            if (response != null && !string.IsNullOrEmpty(response.SupplierName))
            {
                _dashboard = response.Adapt<SupplierDashboardData>();
                _supplierName = _dashboard.SupplierName;
            }
            else
            {
                _dashboard = GenerateMockDashboardData();
                _supplierName = _dashboard.SupplierName;
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            _dashboard = GenerateMockDashboardData();
            _supplierName = _dashboard.SupplierName;
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
        await DialogService.ShowAsync<SupplierDashboardHelpDialog>("Supplier Dashboard Help", options);
    }

    private SupplierDashboardData GenerateMockDashboardData()
    {
        var random = new Random();

        return new SupplierDashboardData
        {
            SupplierName = "Sample Supplier Inc.",
            Tier = new[] { "Platinum", "Gold", "Silver", "Bronze" }[random.Next(4)],
            PartnerSince = DateTime.Now.AddYears(-random.Next(1, 10)),

            TotalPurchases = random.Next(500000, 5000000),
            PurchaseGrowthPercent = (decimal)(random.NextDouble() * 40 - 10),

            OnTimeDeliveryPercent = (decimal)(random.NextDouble() * 15 + 85),
            QualityRatePercent = (decimal)(random.NextDouble() * 10 + 90),
            OverallScore = (decimal)(random.NextDouble() * 2 + 3),

            TotalOrders = random.Next(100, 2000),
            AvgOrderValue = (decimal)(random.NextDouble() * 5000 + 1000),
            PendingOrders = random.Next(0, 30),
            FulfillmentRatePercent = (decimal)(random.NextDouble() * 10 + 90),

            AvgLeadTimeDays = (decimal)(random.NextDouble() * 10 + 3),
            LateDeliveriesLast30Days = random.Next(0, 10),

            DefectRatePercent = (decimal)(random.NextDouble() * 3),
            ReturnsLast30Days = random.Next(0, 20),

            OutstandingBalance = (decimal)(random.NextDouble() * 50000),
            CreditLimit = random.Next(100000, 500000),
            CreditUtilizationPercent = (decimal)(random.NextDouble() * 60 + 10),

            TopItems = Enumerable.Range(1, 10).Select(i => new TopItemData
            {
                Name = $"Product {(char)('A' + i - 1)}{random.Next(100, 999)}",
                OrderCount = random.Next(10, 200),
                TotalQuantity = random.Next(100, 5000),
                TotalValue = (decimal)(random.NextDouble() * 50000 + 5000)
            }).OrderByDescending(x => x.TotalValue).ToList(),

            RecentOrders = Enumerable.Range(1, 10).Select(i => new RecentOrderData
            {
                PoNumber = $"PO-{DateTime.Now.Year}-{random.Next(1000, 9999)}",
                OrderDate = DateTime.Now.AddDays(-random.Next(1, 60)),
                TotalAmount = (decimal)(random.NextDouble() * 20000 + 1000),
                Status = new[] { "Delivered", "Shipped", "Processing", "Pending" }[random.Next(4)]
            }).OrderByDescending(x => x.OrderDate).ToList()
        };
    }

    private MudBlazor.Color GetTrendColor(decimal value) => value >= 0 ? MudBlazor.Color.Success : MudBlazor.Color.Error;

    private MudBlazor.Color GetTierColor(string tier) => tier switch
    {
        "Platinum" => MudBlazor.Color.Primary,
        "Gold" => MudBlazor.Color.Warning,
        "Silver" => MudBlazor.Color.Default,
        "Bronze" => MudBlazor.Color.Surface,
        _ => MudBlazor.Color.Default
    };

    private MudBlazor.Color GetPerformanceColor(decimal value) => value switch
    {
        >= 95 => MudBlazor.Color.Success,
        >= 85 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private string GetPerformanceLabel(decimal value) => value switch
    {
        >= 95 => "Excellent",
        >= 85 => "Good",
        >= 70 => "Fair",
        _ => "Poor"
    };

    private MudBlazor.Color GetScoreColor(decimal score) => score switch
    {
        >= 4.5m => MudBlazor.Color.Success,
        >= 3.5m => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private string GetScoreLabel(decimal score) => score switch
    {
        >= 4.5m => "Excellent",
        >= 3.5m => "Good",
        >= 2.5m => "Fair",
        _ => "Poor"
    };

    private MudBlazor.Color GetPendingOrdersColor(int count) => count switch
    {
        < 5 => MudBlazor.Color.Success,
        < 15 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetLateDeliveryColor(int count) => count switch
    {
        0 => MudBlazor.Color.Success,
        < 3 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetDefectRateColor(decimal rate) => rate switch
    {
        < 1 => MudBlazor.Color.Success,
        < 3 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetBalanceColor(decimal balance) => balance switch
    {
        0 => MudBlazor.Color.Success,
        < 10000 => MudBlazor.Color.Default,
        < 30000 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetCreditUtilizationColor(decimal percent) => percent switch
    {
        < 50 => MudBlazor.Color.Success,
        < 75 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetOrderStatusColor(string status) => status switch
    {
        "Delivered" => MudBlazor.Color.Success,
        "Shipped" => MudBlazor.Color.Info,
        "Processing" => MudBlazor.Color.Warning,
        "Pending" => MudBlazor.Color.Default,
        _ => MudBlazor.Color.Default
    };

    // Local DTOs
    private class SupplierDashboardData
    {
        public string SupplierName { get; set; } = "";
        public string Tier { get; set; } = "";
        public DateTime PartnerSince { get; set; }

        public decimal TotalPurchases { get; set; }
        public decimal PurchaseGrowthPercent { get; set; }

        public decimal OnTimeDeliveryPercent { get; set; }
        public decimal QualityRatePercent { get; set; }
        public decimal OverallScore { get; set; }

        public int TotalOrders { get; set; }
        public decimal AvgOrderValue { get; set; }
        public int PendingOrders { get; set; }
        public decimal FulfillmentRatePercent { get; set; }

        public decimal AvgLeadTimeDays { get; set; }
        public int LateDeliveriesLast30Days { get; set; }

        public decimal DefectRatePercent { get; set; }
        public int ReturnsLast30Days { get; set; }

        public decimal OutstandingBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditUtilizationPercent { get; set; }

        public List<TopItemData> TopItems { get; set; } = new();
        public List<RecentOrderData> RecentOrders { get; set; } = new();
    }

    private class TopItemData
    {
        public string Name { get; set; } = "";
        public int OrderCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }

    private class RecentOrderData
    {
        public string PoNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "";
    }
}
