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
    public FinancialMetrics Financials { get; init; } = new();

    // Order Metrics
    public OrderMetrics Orders { get; init; } = new();

    // Delivery Performance
    public DeliveryPerformance Delivery { get; init; } = new();

    // Quality Metrics
    public QualityMetrics Quality { get; init; } = new();

    // Item Portfolio
    public ItemPortfolio Items { get; init; } = new();

    // Trend Data for Charts
    public List<TimeSeriesDataPoint> OrderValueTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> OrderCountTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> LeadTimeTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> OnTimeDeliveryTrend { get; init; } = [];

    // Category Breakdown
    public List<CategoryBreakdown> OrdersByCategory { get; init; } = [];

    // Top Items from this Supplier
    public List<TopItemInfo> TopItems { get; init; } = [];

    // Recent Purchase Orders
    public List<RecentPurchaseOrder> RecentOrders { get; init; } = [];

    // Monthly Comparison
    public List<MonthlyComparison> MonthlyPerformance { get; init; } = [];

    // Comparative Ranking
    public SupplierRanking Ranking { get; init; } = new();
}

public sealed record FinancialMetrics
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

public sealed record OrderMetrics
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

public sealed record DeliveryPerformance
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

public sealed record QualityMetrics
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

public sealed record ItemPortfolio
{
    public int TotalItemsSupplied { get; init; }
    public int ActiveItems { get; init; }
    public int ExclusiveItems { get; init; }
    public decimal AveragePriceCompetitiveness { get; init; }
    public List<string> TopCategories { get; init; } = [];
}

public sealed record TimeSeriesDataPoint
{
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
    public string? Label { get; init; }
}

public sealed record CategoryBreakdown
{
    public string CategoryName { get; init; } = default!;
    public int OrderCount { get; init; }
    public decimal TotalValue { get; init; }
    public decimal Percentage { get; init; }
}

public sealed record TopItemInfo
{
    public Guid ItemId { get; init; }
    public string ItemName { get; init; } = default!;
    public string? Sku { get; init; }
    public int TotalQuantityOrdered { get; init; }
    public decimal TotalValue { get; init; }
    public int OrderCount { get; init; }
    public decimal AverageUnitPrice { get; init; }
}

public sealed record RecentPurchaseOrder
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

public sealed record MonthlyComparison
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
