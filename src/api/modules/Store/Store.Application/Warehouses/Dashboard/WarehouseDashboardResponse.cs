using FSH.Starter.WebApi.Store.Application.Dashboard;

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Dashboard;

/// <summary>
/// Response containing comprehensive warehouse performance metrics and analytics.
/// </summary>
public sealed record WarehouseDashboardResponse
{
    // Basic Warehouse Info
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = default!;
    public string? Code { get; init; }
    public string? Address { get; init; }
    public string? ManagerName { get; init; }
    public string? ManagerEmail { get; init; }
    public string? Phone { get; init; }
    public string? WarehouseType { get; init; }
    public bool IsActive { get; init; }

    // Capacity Metrics
    public WarehouseCapacityMetrics Capacity { get; init; } = new();

    // Inventory Summary
    public WarehouseInventorySummary Inventory { get; init; } = new();

    // Movement Metrics
    public WarehouseMovementMetrics Movements { get; init; } = new();

    // Operations Metrics
    public WarehouseOperationsMetrics Operations { get; init; } = new();

    // Location Utilization
    public WarehouseLocationUtilization Locations { get; init; } = new();

    // Trend Data for Charts
    public List<StoreTimeSeriesDataPoint> InventoryValueTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> ReceivingTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> ShippingTrend { get; init; } = [];
    public List<StoreTimeSeriesDataPoint> TurnoverTrend { get; init; } = [];

    // Category Breakdown
    public List<WarehouseCategoryBreakdown> InventoryByCategory { get; init; } = [];

    // Top Items in Warehouse
    public List<TopWarehouseItem> TopItems { get; init; } = [];

    // Recent Transactions
    public List<WarehouseRecentTransaction> RecentTransactions { get; init; } = [];

    // Pending Tasks
    public List<PendingWarehouseTask> PendingTasks { get; init; } = [];

    // Monthly Comparison
    public List<WarehouseMonthlyComparison> MonthlyPerformance { get; init; } = [];

    // Alerts and Issues
    public List<WarehouseAlert> Alerts { get; init; } = [];
}

public sealed record WarehouseCapacityMetrics
{
    public decimal TotalCapacity { get; init; }
    public decimal UsedCapacity { get; init; }
    public decimal AvailableCapacity { get; init; }
    public decimal UtilizationPercentage { get; init; }
    public int TotalLocations { get; init; }
    public int OccupiedLocations { get; init; }
    public int EmptyLocations { get; init; }
    public decimal LocationUtilizationPercentage { get; init; }
}

public sealed record WarehouseInventorySummary
{
    public int TotalItems { get; init; }
    public int TotalSku { get; init; }
    public decimal TotalQuantityOnHand { get; init; }
    public decimal TotalInventoryValue { get; init; }
    public decimal TotalInventoryCost { get; init; }
    public int LowStockItems { get; init; }
    public int OutOfStockItems { get; init; }
    public int OverstockItems { get; init; }
    public int ExpiringItems { get; init; }
    public decimal AverageItemValue { get; init; }
}

public sealed record WarehouseMovementMetrics
{
    public int TotalInboundToday { get; init; }
    public int TotalOutboundToday { get; init; }
    public int TotalInboundYTD { get; init; }
    public int TotalOutboundYTD { get; init; }
    public decimal InboundValueYTD { get; init; }
    public decimal OutboundValueYTD { get; init; }
    public int TransfersInToday { get; init; }
    public int TransfersOutToday { get; init; }
    public decimal AverageDailyMovements { get; init; }
    public decimal InventoryTurnover { get; init; }
}

public sealed record WarehouseOperationsMetrics
{
    public int PendingPutAways { get; init; }
    public int PendingPickLists { get; init; }
    public int PendingCycleCounts { get; init; }
    public int PendingGoodsReceipts { get; init; }
    public decimal PutAwayEfficiency { get; init; }
    public decimal PickingAccuracy { get; init; }
    public decimal CycleCountAccuracy { get; init; }
    public decimal AverageProcessingTimeHours { get; init; }
    public int TasksCompletedToday { get; init; }
    public int TasksOverdue { get; init; }
}

public sealed record WarehouseLocationUtilization
{
    public int TotalBins { get; init; }
    public int OccupiedBins { get; init; }
    public int EmptyBins { get; init; }
    public decimal BinUtilizationPercentage { get; init; }
    public List<WarehouseZoneUtilization> ZoneBreakdown { get; init; } = [];
}

public sealed record WarehouseZoneUtilization
{
    public string ZoneName { get; init; } = default!;
    public int TotalLocations { get; init; }
    public int OccupiedLocations { get; init; }
    public decimal UtilizationPercentage { get; init; }
}

public sealed record WarehouseCategoryBreakdown
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = default!;
    public int ItemCount { get; init; }
    public decimal TotalQuantity { get; init; }
    public decimal TotalValue { get; init; }
    public decimal Percentage { get; init; }
}

public sealed record TopWarehouseItem
{
    public Guid ItemId { get; init; }
    public string ItemName { get; init; } = default!;
    public string? Sku { get; init; }
    public decimal QuantityOnHand { get; init; }
    public decimal TotalValue { get; init; }
    public string? LocationCode { get; init; }
    public string StockStatus { get; init; } = default!;
}

public sealed record WarehouseRecentTransaction
{
    public Guid TransactionId { get; init; }
    public string TransactionType { get; init; } = default!;
    public string ItemName { get; init; } = default!;
    public decimal Quantity { get; init; }
    public DateTime TransactionDate { get; init; }
    public string? Reference { get; init; }
    public string? PerformedBy { get; init; }
}

public sealed record PendingWarehouseTask
{
    public Guid TaskId { get; init; }
    public string TaskType { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Priority { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public DateTime? DueDate { get; init; }
    public string Status { get; init; } = default!;
}

public sealed record WarehouseMonthlyComparison
{
    public string Month { get; init; } = default!;
    public int Year { get; init; }
    public int InboundCount { get; init; }
    public int OutboundCount { get; init; }
    public decimal InboundValue { get; init; }
    public decimal OutboundValue { get; init; }
    public decimal UtilizationPercentage { get; init; }
    public decimal TurnoverRate { get; init; }
}

public sealed record WarehouseAlert
{
    public string AlertType { get; init; } = default!;
    public string Severity { get; init; } = default!;
    public string Message { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public Guid? RelatedItemId { get; init; }
    public string? RelatedItemName { get; init; }
}
