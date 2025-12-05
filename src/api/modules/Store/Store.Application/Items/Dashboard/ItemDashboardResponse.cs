namespace FSH.Starter.WebApi.Store.Application.Items.Dashboard;

/// <summary>
/// Response containing comprehensive item performance metrics and analytics.
/// </summary>
public sealed record ItemDashboardResponse
{
    // Basic Item Info
    public Guid ItemId { get; init; }
    public string ItemName { get; init; } = default!;
    public string? Sku { get; init; }
    public string? Barcode { get; init; }
    public string? CategoryName { get; init; }
    public string? ImageUrl { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Cost { get; init; }
    public decimal ProfitMargin { get; init; }

    // Current Stock Status
    public int TotalQuantityOnHand { get; init; }
    public int TotalQuantityAvailable { get; init; }
    public int TotalQuantityReserved { get; init; }
    public int MinimumStock { get; init; }
    public int ReorderPoint { get; init; }
    public string StockStatus { get; init; } = default!; // "Healthy", "Low", "Critical", "Out of Stock"

    // Movement Classification
    public string MovementClass { get; init; } = default!; // "Fast Moving", "Medium Moving", "Slow Moving", "Dead Stock"
    public decimal TurnoverRate { get; init; }
    public int DaysOfSupply { get; init; }

    // Sales Metrics
    public SalesMetrics DailySales { get; init; } = new();
    public SalesMetrics WeeklySales { get; init; } = new();
    public SalesMetrics MonthlySales { get; init; } = new();
    public SalesMetrics YearlySales { get; init; } = new();

    // Purchase/Receiving Metrics
    public PurchaseMetrics PurchaseStats { get; init; } = new();

    // Trend Data for Charts
    public List<TimeSeriesDataPoint> SalesTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> StockTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> PurchaseTrend { get; init; } = [];

    // Top Warehouses for this Item
    public List<WarehouseStockInfo> TopWarehouses { get; init; } = [];

    // Recent Transactions
    public List<RecentTransaction> RecentTransactions { get; init; } = [];

    // Supplier Performance for this Item
    public List<SupplierPerformanceInfo> SupplierPerformance { get; init; } = [];

    // Backorder/Stockout Info
    public BackorderMetrics BackorderStats { get; init; } = new();
}

public sealed record SalesMetrics
{
    public decimal TotalQuantitySold { get; init; }
    public decimal TotalRevenue { get; init; }
    public decimal TotalProfit { get; init; }
    public decimal AverageQuantityPerDay { get; init; }
    public int TransactionCount { get; init; }
    public decimal GrowthPercentage { get; init; }
}

public sealed record PurchaseMetrics
{
    public decimal TotalQuantityPurchased { get; init; }
    public decimal TotalPurchaseCost { get; init; }
    public decimal AverageUnitCost { get; init; }
    public int PurchaseOrderCount { get; init; }
    public decimal AverageLeadTimeDays { get; init; }
    public decimal OnTimeDeliveryRate { get; init; }
}

public sealed record TimeSeriesDataPoint
{
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
    public string? Label { get; init; }
}

public sealed record WarehouseStockInfo
{
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = default!;
    public int QuantityOnHand { get; init; }
    public int QuantityAvailable { get; init; }
    public decimal PercentageOfTotal { get; init; }
}

public sealed record RecentTransaction
{
    public Guid TransactionId { get; init; }
    public DateTime TransactionDate { get; init; }
    public string TransactionType { get; init; } = default!;
    public decimal Quantity { get; init; }
    public string? WarehouseName { get; init; }
    public string? Reference { get; init; }
}

public sealed record SupplierPerformanceInfo
{
    public Guid SupplierId { get; init; }
    public string SupplierName { get; init; } = default!;
    public int TotalOrders { get; init; }
    public decimal TotalQuantitySupplied { get; init; }
    public decimal AverageLeadTime { get; init; }
    public decimal OnTimeRate { get; init; }
    public decimal QualityScore { get; init; }
}

public sealed record BackorderMetrics
{
    public int CurrentBackorderQuantity { get; init; }
    public int StockoutDaysLast30 { get; init; }
    public int StockoutDaysLast90 { get; init; }
    public decimal StockoutCostEstimate { get; init; }
    public decimal FillRate { get; init; } // Percentage of demand fulfilled
}
