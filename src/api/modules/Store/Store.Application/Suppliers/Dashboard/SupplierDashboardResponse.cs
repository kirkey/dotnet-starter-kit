using FSH.Starter.WebApi.Store.Application.Dashboard;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Dashboard;

/// <summary>
/// Response containing comprehensive supplier performance metrics and analytics.
/// </summary>
public sealed record SupplierDashboardResponse
{
    // Basic Supplier Info
    public Guid SupplierId { get; init; }
    public string SupplierName { get; init; } = default!;
    public string? Code { get; init; }
    public string? ContactPerson { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
    public decimal Rating { get; init; }

    // Financial Overview
    public SupplierFinancialMetrics Financials { get; init; } = new();

    // Order Metrics
    public SupplierOrderMetrics Orders { get; init; } = new();

    // Delivery Performance
    public SupplierDeliveryPerformance Delivery { get; init; } = new();

    // Quality Metrics
    public SupplierQualityMetrics Quality { get; init; } = new();

    // Item Portfolio
    public SupplierItemPortfolio Items { get; init; } = new();

    // Trend Data for Charts
    public List<StoreTimeSeriesDataPoint> OrderValueTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> OrderCountTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> LeadTimeTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> OnTimeDeliveryTrend { get; init; } = [];

    // Category Breakdown
    public List<StoreCategoryBreakdown> OrdersByCategory { get; init; } = [];

    // Top Items from this Supplier
    public List<SupplierTopItemInfo> TopItems { get; init; } = [];

    // Recent Purchase Orders
    public List<SupplierRecentPurchaseOrder> RecentOrders { get; init; } = [];

    // Monthly Comparison
    public List<SupplierMonthlyComparison> MonthlyPerformance { get; init; } = [];

    // Comparative Ranking
    public SupplierRanking Ranking { get; init; } = new();
}

public sealed record SupplierFinancialMetrics
{
    public decimal TotalPurchaseValue { get; init; }
    public decimal TotalPurchaseValueYTD { get; init; }
    public decimal TotalPurchaseValueLastYear { get; init; }
    public decimal AverageOrderValue { get; init; }
    public decimal OutstandingBalance { get; init; }
    public decimal CreditLimit { get; init; }
    public decimal CreditUtilization { get; init; }
    public int PaymentTermsDays { get; init; }
    public decimal GrowthPercentage { get; init; }
}

public sealed record SupplierOrderMetrics
{
    public int TotalOrders { get; init; }
    public int TotalOrdersYTD { get; init; }
    public int TotalOrdersLastYear { get; init; }
    public int PendingOrders { get; init; }
    public int ApprovedOrders { get; init; }
    public int CompletedOrders { get; init; }
    public int CancelledOrders { get; init; }
    public decimal CompletionRate { get; init; }
    public decimal CancellationRate { get; init; }
    public decimal AverageItemsPerOrder { get; init; }
}

public sealed record SupplierDeliveryPerformance
{
    public decimal OnTimeDeliveryRate { get; init; }
    public decimal AverageLeadTimeDays { get; init; }
    public decimal ShortestLeadTimeDays { get; init; }
    public decimal LongestLeadTimeDays { get; init; }
    public int EarlyDeliveries { get; init; }
    public int OnTimeDeliveries { get; init; }
    public int LateDeliveries { get; init; }
    public decimal AverageDelayDays { get; init; }
    public decimal DeliveryReliabilityScore { get; init; }
}

public sealed record SupplierQualityMetrics
{
    public decimal AcceptanceRate { get; init; }
    public int TotalItemsReceived { get; init; }
    public int ItemsAccepted { get; init; }
    public int ItemsRejected { get; init; }
    public int ItemsWithDefects { get; init; }
    public decimal DefectRate { get; init; }
    public int ReturnCount { get; init; }
    public decimal ReturnRate { get; init; }
    public decimal QualityScore { get; init; }
}

public sealed record SupplierItemPortfolio
{
    public int TotalItemsSupplied { get; init; }
    public int ActiveItems { get; init; }
    public int ExclusiveItems { get; init; }
    public decimal AveragePriceCompetitiveness { get; init; }
    public List<string> TopCategories { get; init; } = [];
}

public sealed record SupplierTopItemInfo
{
    public Guid ItemId { get; init; }
    public string ItemName { get; init; } = default!;
    public string? Sku { get; init; }
    public int TotalQuantityOrdered { get; init; }
    public decimal TotalValue { get; init; }
    public int OrderCount { get; init; }
    public decimal AverageUnitPrice { get; init; }
}

public sealed record SupplierRecentPurchaseOrder
{
    public Guid PurchaseOrderId { get; init; }
    public string OrderNumber { get; init; } = default!;
    public DateTime OrderDate { get; init; }
    public string Status { get; init; } = default!;
    public decimal TotalAmount { get; init; }
    public int ItemCount { get; init; }
    public DateTime? ExpectedDelivery { get; init; }
    public DateTime? ActualDelivery { get; init; }
}

public sealed record SupplierMonthlyComparison
{
    public string Month { get; init; } = default!;
    public int Year { get; init; }
    public decimal OrderValue { get; init; }
    public int OrderCount { get; init; }
    public decimal OnTimeRate { get; init; }
    public decimal QualityScore { get; init; }
}

public sealed record SupplierRanking
{
    public int OverallRank { get; init; }
    public int TotalSuppliers { get; init; }
    public int VolumeRank { get; init; }
    public int QualityRank { get; init; }
    public int DeliveryRank { get; init; }
    public int PriceRank { get; init; }
    public string PerformanceTier { get; init; } = default!; // "Preferred", "Approved", "Probation", "Under Review"
}
