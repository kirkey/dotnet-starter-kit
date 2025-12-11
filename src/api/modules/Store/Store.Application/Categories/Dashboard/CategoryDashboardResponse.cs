using FSH.Starter.WebApi.Store.Application.Dashboard;

namespace FSH.Starter.WebApi.Store.Application.Categories.Dashboard;

/// <summary>
/// Response containing comprehensive category performance metrics and analytics.
/// </summary>
public sealed record CategoryDashboardResponse
{
    // Basic Category Info
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = default!;
    public string? Code { get; init; }
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
    public int SortOrder { get; init; }
    public Guid? ParentCategoryId { get; init; }
    public string? ParentCategoryName { get; init; }

    // Item Metrics
    public CategoryItemMetrics Items { get; init; } = new();

    // Inventory Metrics
    public CategoryInventoryMetrics Inventory { get; init; } = new();

    // Sales Metrics
    public CategorySalesMetrics Sales { get; init; } = new();

    // Purchase Metrics
    public CategoryPurchaseMetrics Purchases { get; init; } = new();

    // Subcategories Summary
    public CategorySubcategorySummary Subcategories { get; init; } = new();

    // Trend Data for Charts
    public List<StoreTimeSeriesDataPoint> SalesTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> InventoryTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> PurchaseTrend { get; init; } = [];

    // Top Items in Category
    public List<TopCategoryItem> TopItemsBySales { get; init; } = [];
    public List<TopCategoryItem> TopItemsByInventoryValue { get; init; } = [];

    // Supplier Distribution
    public List<CategorySupplierDistribution> SupplierBreakdown { get; init; } = [];

    // Warehouse Distribution
    public List<CategoryWarehouseDistribution> WarehouseBreakdown { get; init; } = [];

    // Recent Transactions
    public List<CategoryRecentTransaction> RecentTransactions { get; init; } = [];

    // Monthly Performance
    public List<CategoryMonthlyComparison> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<CategoryAlert> Alerts { get; init; } = [];
}

public sealed record CategoryItemMetrics
{
    public int TotalItems { get; init; }
    public int ActiveItems { get; init; }
    public int InactiveItems { get; init; }
    public int NewItemsThisMonth { get; init; }
    public decimal AverageUnitPrice { get; init; }
    public decimal AverageCost { get; init; }
    public decimal AverageMargin { get; init; }
    public decimal HighestPrice { get; init; }
    public decimal LowestPrice { get; init; }
    public string? BestSellingItem { get; init; }
}

public sealed record CategoryInventoryMetrics
{
    public decimal TotalQuantityOnHand { get; init; }
    public decimal TotalQuantityAvailable { get; init; }
    public decimal TotalQuantityReserved { get; init; }
    public decimal TotalInventoryValue { get; init; }
    public decimal TotalInventoryCost { get; init; }
    public int ItemsInStock { get; init; }
    public int ItemsLowStock { get; init; }
    public int ItemsOutOfStock { get; init; }
    public int ItemsOverstock { get; init; }
    public decimal InventoryTurnover { get; init; }
    public int AverageDaysOfSupply { get; init; }
}

public sealed record CategorySalesMetrics
{
    public decimal TotalSalesValue { get; init; }
    public decimal TotalSalesValueYTD { get; init; }
    public decimal TotalSalesValueLastYear { get; init; }
    public int TotalUnitsSold { get; init; }
    public int TotalUnitsSoldYTD { get; init; }
    public int TransactionCount { get; init; }
    public decimal AverageSaleValue { get; init; }
    public decimal SalesGrowthPercentage { get; init; }
    public decimal MarginPercentage { get; init; }
}

public sealed record CategoryPurchaseMetrics
{
    public decimal TotalPurchaseValue { get; init; }
    public decimal TotalPurchaseValueYTD { get; init; }
    public int TotalUnitsReceived { get; init; }
    public int TotalUnitsReceivedYTD { get; init; }
    public int PurchaseOrderCount { get; init; }
    public decimal AveragePurchaseValue { get; init; }
    public int UniqueSuppliers { get; init; }
    public decimal AverageLeadTimeDays { get; init; }
}

public sealed record CategorySubcategorySummary
{
    public int TotalSubcategories { get; init; }
    public int ActiveSubcategories { get; init; }
    public List<CategorySubcategoryInfo> TopSubcategories { get; init; } = [];
}

public sealed record CategorySubcategoryInfo
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = default!;
    public int ItemCount { get; init; }
    public decimal InventoryValue { get; init; }
    public decimal SalesValue { get; init; }
}

public sealed record TopCategoryItem
{
    public Guid ItemId { get; init; }
    public string ItemName { get; init; } = default!;
    public string? Sku { get; init; }
    public string? ImageUrl { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalSales { get; init; }
    public int UnitsSold { get; init; }
    public decimal QuantityOnHand { get; init; }
    public decimal InventoryValue { get; init; }
    public string StockStatus { get; init; } = default!;
}

public sealed record CategorySupplierDistribution
{
    public Guid SupplierId { get; init; }
    public string SupplierName { get; init; } = default!;
    public int ItemCount { get; init; }
    public decimal TotalPurchaseValue { get; init; }
    public decimal Percentage { get; init; }
}

public sealed record CategoryWarehouseDistribution
{
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = default!;
    public decimal QuantityOnHand { get; init; }
    public decimal InventoryValue { get; init; }
    public decimal Percentage { get; init; }
}

public sealed record CategoryRecentTransaction
{
    public Guid TransactionId { get; init; }
    public string TransactionType { get; init; } = default!;
    public string ItemName { get; init; } = default!;
    public decimal Quantity { get; init; }
    public decimal Value { get; init; }
    public DateTime TransactionDate { get; init; }
    public string? WarehouseName { get; init; }
}

public sealed record CategoryMonthlyComparison
{
    public string Month { get; init; } = default!;
    public int Year { get; init; }
    public decimal SalesValue { get; init; }
    public int UnitsSold { get; init; }
    public decimal PurchaseValue { get; init; }
    public int UnitsReceived { get; init; }
    public decimal InventoryValue { get; init; }
    public decimal MarginPercentage { get; init; }
}

public sealed record CategoryAlert
{
    public string AlertType { get; init; } = default!;
    public string Severity { get; init; } = default!;
    public string Message { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public Guid? RelatedItemId { get; init; }
    public string? RelatedItemName { get; init; }
}
