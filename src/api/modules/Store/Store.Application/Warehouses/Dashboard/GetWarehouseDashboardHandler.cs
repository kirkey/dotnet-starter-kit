using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.Store.Application.Dashboard;

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific warehouse.
/// </summary>
public sealed record GetWarehouseDashboardQuery(Guid WarehouseId) : IRequest<WarehouseDashboardResponse>;

public sealed class GetWarehouseDashboardHandler(
    IReadRepository<Warehouse> warehouseRepository,
    IReadRepository<WarehouseLocation> warehouseLocationRepository,
    IReadRepository<StockLevel> stockLevelRepository,
    IReadRepository<InventoryTransaction> inventoryTransactionRepository,
    IReadRepository<PutAwayTask> putAwayTaskRepository,
    IReadRepository<PickList> pickListRepository,
    IReadRepository<CycleCount> cycleCountRepository,
    IReadRepository<GoodsReceipt> goodsReceiptRepository,
    IReadRepository<Item> itemRepository,
    IReadRepository<Category> categoryRepository,
    ICacheService cacheService,
    ILogger<GetWarehouseDashboardHandler> logger)
    : IRequestHandler<GetWarehouseDashboardQuery, WarehouseDashboardResponse>
{
    public async Task<WarehouseDashboardResponse> Handle(GetWarehouseDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"warehouse-dashboard:{request.WarehouseId}";

        var cachedResult = await cacheService.GetAsync<WarehouseDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for warehouse {WarehouseId}", request.WarehouseId);

        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken)
            ?? throw new NotFoundException($"Warehouse {request.WarehouseId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);

        // Get stock levels for this warehouse
        var stockLevels = await stockLevelRepository
            .ListAsync(new StockLevelsByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get warehouse locations
        var locations = await warehouseLocationRepository
            .ListAsync(new WarehouseLocationsByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get inventory transactions for this warehouse
        var transactions = await inventoryTransactionRepository
            .ListAsync(new InventoryTransactionsByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get pending put-away tasks
        var putAwayTasks = await putAwayTaskRepository
            .ListAsync(new PutAwayTasksByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get pending pick lists
        var pickLists = await pickListRepository
            .ListAsync(new PickListsByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get pending cycle counts
        var cycleCounts = await cycleCountRepository
            .ListAsync(new CycleCountsByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get pending goods receipts
        var goodsReceipts = await goodsReceiptRepository
            .ListAsync(new GoodsReceiptsByWarehouseSpec(request.WarehouseId), cancellationToken);

        // Get all items for reference
        var allItems = await itemRepository.ListAsync(cancellationToken);
        var itemDict = allItems.ToDictionary(i => i.Id, i => i);

        // Calculate metrics
        var capacityMetrics = CalculateCapacityMetrics(warehouse, locations, stockLevels);
        var inventorySummary = CalculateInventorySummary(stockLevels, itemDict);
        var movementMetrics = CalculateMovementMetrics(transactions, today, startOfYear);
        var operationsMetrics = CalculateOperationsMetrics(putAwayTasks, pickLists, cycleCounts, goodsReceipts);
        var locationUtilization = CalculateLocationUtilization(locations);

        // Generate trend data
        var inventoryValueTrend = GenerateInventoryValueTrend(transactions, 12);
        var receivingTrend = GenerateReceivingTrend(transactions, 12);
        var shippingTrend = GenerateShippingTrend(transactions, 12);
        var turnoverTrend = GenerateTurnoverTrend(transactions, stockLevels, 12);

        // Category breakdown
        var categoryBreakdown = await CalculateCategoryBreakdown(stockLevels, itemDict, cancellationToken);

        // Top items
        var topItems = CalculateTopItems(stockLevels, itemDict);

        // Recent transactions
        var recentTransactions = transactions
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t => new WarehouseRecentTransaction
            {
                TransactionId = t.Id,
                TransactionType = t.TransactionType ?? "Unknown",
                ItemName = itemDict.TryGetValue(t.ItemId, out var item) ? item.Name ?? "Unknown" : "Unknown",
                Quantity = t.Quantity,
                TransactionDate = t.TransactionDate,
                Reference = t.Reference,
                PerformedBy = t.CreatedBy.ToString()
            }).ToList();

        // Pending tasks
        var pendingTasks = GetPendingTasks(putAwayTasks, pickLists, cycleCounts);

        // Monthly comparison
        var monthlyPerformance = GenerateMonthlyComparison(transactions, stockLevels, 6);

        // Alerts
        var alerts = GenerateAlerts(stockLevels, itemDict, putAwayTasks, pickLists);

        var response = new WarehouseDashboardResponse
        {
            WarehouseId = warehouse.Id,
            WarehouseName = warehouse.Name ?? "Unknown",
            Code = warehouse.Code,
            Address = warehouse.Address,
            ManagerName = warehouse.ManagerName,
            ManagerEmail = warehouse.ManagerEmail,
            Phone = warehouse.ManagerPhone,
            WarehouseType = warehouse.WarehouseType,
            IsActive = warehouse.IsActive,
            Capacity = capacityMetrics,
            Inventory = inventorySummary,
            Movements = movementMetrics,
            Operations = operationsMetrics,
            Locations = locationUtilization,
            InventoryValueTrend = inventoryValueTrend,
            ReceivingTrend = receivingTrend,
            ShippingTrend = shippingTrend,
            TurnoverTrend = turnoverTrend,
            InventoryByCategory = categoryBreakdown,
            TopItems = topItems,
            RecentTransactions = recentTransactions,
            PendingTasks = pendingTasks,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static CapacityMetrics CalculateCapacityMetrics(
        Warehouse warehouse,
        List<WarehouseLocation> locations,
        List<StockLevel> stockLevels)
    {
        var totalCapacity = warehouse.TotalCapacity;
        var usedCapacity = warehouse.UsedCapacity;
        var availableCapacity = totalCapacity - usedCapacity;
        var utilizationPercentage = totalCapacity > 0 ? (usedCapacity / totalCapacity * 100) : 0;

        var occupiedLocations = locations.Count(l => stockLevels.Any(s => s.WarehouseLocationId == l.Id && s.QuantityOnHand > 0));
        var locationUtilization = locations.Count > 0 ? (decimal)occupiedLocations / locations.Count * 100 : 0;

        return new CapacityMetrics
        {
            TotalCapacity = totalCapacity,
            UsedCapacity = usedCapacity,
            AvailableCapacity = availableCapacity,
            UtilizationPercentage = utilizationPercentage,
            TotalLocations = locations.Count,
            OccupiedLocations = occupiedLocations,
            EmptyLocations = locations.Count - occupiedLocations,
            LocationUtilizationPercentage = locationUtilization
        };
    }

    private static InventorySummary CalculateInventorySummary(
        List<StockLevel> stockLevels,
        Dictionary<Guid, Item> itemDict)
    {
        var totalQuantity = stockLevels.Sum(s => s.QuantityOnHand);
        var totalValue = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.UnitPrice;
            }
            return 0;
        });
        var totalCost = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.Cost;
            }
            return 0;
        });

        var distinctItems = stockLevels.Select(s => s.ItemId).Distinct().Count();
        var lowStockItems = stockLevels.Count(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand > 0 && s.QuantityOnHand <= item.ReorderPoint;
            }
            return false;
        });
        var outOfStockItems = stockLevels.Count(s => s.QuantityOnHand <= 0);
        var overstockItems = stockLevels.Count(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand > item.MaximumStock;
            }
            return false;
        });

        return new InventorySummary
        {
            TotalItems = stockLevels.Count,
            TotalSku = distinctItems,
            TotalQuantityOnHand = totalQuantity,
            TotalInventoryValue = totalValue,
            TotalInventoryCost = totalCost,
            LowStockItems = lowStockItems,
            OutOfStockItems = outOfStockItems,
            OverstockItems = overstockItems,
            ExpiringItems = 0,
            AverageItemValue = distinctItems > 0 ? totalValue / distinctItems : 0
        };
    }

    private static MovementMetrics CalculateMovementMetrics(
        List<InventoryTransaction> transactions,
        DateTime today,
        DateTime startOfYear)
    {
        var todayTransactions = transactions.Where(t => t.TransactionDate.Date == today).ToList();
        var ytdTransactions = transactions.Where(t => t.TransactionDate >= startOfYear).ToList();

        var inboundToday = todayTransactions.Count(t => t.Quantity > 0);
        var outboundToday = todayTransactions.Count(t => t.Quantity < 0);
        var inboundYTD = ytdTransactions.Count(t => t.Quantity > 0);
        var outboundYTD = ytdTransactions.Count(t => t.Quantity < 0);

        var inboundValueYTD = ytdTransactions.Where(t => t.Quantity > 0).Sum(t => t.TotalCost);
        var outboundValueYTD = ytdTransactions.Where(t => t.Quantity < 0).Sum(t => Math.Abs(t.TotalCost));

        var daysSinceYearStart = (today - startOfYear).Days + 1;
        var avgDailyMovements = (decimal)(inboundYTD + outboundYTD) / daysSinceYearStart;

        return new MovementMetrics
        {
            TotalInboundToday = inboundToday,
            TotalOutboundToday = outboundToday,
            TotalInboundYTD = inboundYTD,
            TotalOutboundYTD = outboundYTD,
            InboundValueYTD = inboundValueYTD,
            OutboundValueYTD = outboundValueYTD,
            TransfersInToday = 0,
            TransfersOutToday = 0,
            AverageDailyMovements = avgDailyMovements,
            InventoryTurnover = 0
        };
    }

    private static OperationsMetrics CalculateOperationsMetrics(
        List<PutAwayTask> putAwayTasks,
        List<PickList> pickLists,
        List<CycleCount> cycleCounts,
        List<GoodsReceipt> goodsReceipts)
    {
        var pendingPutAways = putAwayTasks.Count(t => t.Status == "Pending" || t.Status == "InProgress");
        var pendingPickLists = pickLists.Count(p => p.Status == "Created" || p.Status == "Assigned" || p.Status == "InProgress");
        var pendingCycleCounts = cycleCounts.Count(c => c.Status == "Scheduled" || c.Status == "InProgress");
        var pendingGoodsReceipts = goodsReceipts.Count(g => g.Status == "Pending");

        var completedPutAways = putAwayTasks.Count(t => t.Status == "Completed");
        var putAwayEfficiency = putAwayTasks.Count > 0 ? (decimal)completedPutAways / putAwayTasks.Count * 100 : 100;

        var completedPickLists = pickLists.Count(p => p.Status == "Completed");
        var pickingAccuracy = pickLists.Count > 0 ? (decimal)completedPickLists / pickLists.Count * 100 : 100;

        var completedCycleCounts = cycleCounts.Count(c => c.Status == "Completed");
        var cycleCountAccuracy = cycleCounts.Count > 0 ? (decimal)completedCycleCounts / cycleCounts.Count * 100 : 100;

        var today = DateTime.UtcNow.Date;
        var tasksCompletedToday = putAwayTasks.Count(t => t.Status == "Completed" && t.CompletedDate?.Date == today)
            + pickLists.Count(p => p.Status == "Completed" && p.CompletedDate?.Date == today);

        var tasksOverdue = putAwayTasks.Count(t => t.Status != "Completed" && t.CreatedOn.Date < today.AddDays(-1))
            + pickLists.Count(p => p.Status != "Completed" && p.CreatedOn.Date < today.AddDays(-1));

        return new OperationsMetrics
        {
            PendingPutAways = pendingPutAways,
            PendingPickLists = pendingPickLists,
            PendingCycleCounts = pendingCycleCounts,
            PendingGoodsReceipts = pendingGoodsReceipts,
            PutAwayEfficiency = putAwayEfficiency,
            PickingAccuracy = pickingAccuracy,
            CycleCountAccuracy = cycleCountAccuracy,
            AverageProcessingTimeHours = 0,
            TasksCompletedToday = tasksCompletedToday,
            TasksOverdue = tasksOverdue
        };
    }

    private static LocationUtilization CalculateLocationUtilization(List<WarehouseLocation> locations)
    {
        var occupiedLocations = locations.Count(l => l.UsedCapacity > 0);
        var locationUtilization = locations.Count > 0 ? (decimal)occupiedLocations / locations.Count * 100 : 0;

        var zones = locations
            .Where(l => !string.IsNullOrEmpty(l.Aisle))
            .GroupBy(l => l.Aisle)
            .Select(g => new ZoneUtilization
            {
                ZoneName = g.Key,
                TotalLocations = g.Count(),
                OccupiedLocations = g.Count(l => l.UsedCapacity > 0),
                UtilizationPercentage = g.Count() > 0 ? (decimal)g.Count(l => l.UsedCapacity > 0) / g.Count() * 100 : 0
            }).ToList();

        return new LocationUtilization
        {
            TotalBins = locations.Count,
            OccupiedBins = occupiedLocations,
            EmptyBins = locations.Count - occupiedLocations,
            BinUtilizationPercentage = locationUtilization,
            ZoneBreakdown = zones
        };
    }

    private static List<TimeSeriesDataPoint> GenerateInventoryValueTrend(List<InventoryTransaction> transactions, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthTransactions = transactions.Where(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEnd);
            var value = monthTransactions.Sum(t => t.TotalCost);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = value,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GenerateReceivingTrend(List<InventoryTransaction> transactions, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var count = transactions.Count(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEnd && t.Quantity > 0);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = count,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GenerateShippingTrend(List<InventoryTransaction> transactions, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var count = transactions.Count(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEnd && t.Quantity < 0);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = count,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GenerateTurnoverTrend(
        List<InventoryTransaction> transactions,
        List<StockLevel> stockLevels,
        int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var outbound = transactions
                .Where(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEnd && t.Quantity < 0)
                .Sum(t => Math.Abs(t.Quantity));

            var avgInventory = stockLevels.Sum(s => s.QuantityOnHand);
            var turnover = avgInventory > 0 ? (decimal)outbound / avgInventory : 0;

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = turnover,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private async Task<List<WarehouseCategoryBreakdown>> CalculateCategoryBreakdown(
        List<StockLevel> stockLevels,
        Dictionary<Guid, Item> itemDict,
        CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.ListAsync(cancellationToken);
        var categoryDict = categories.ToDictionary(c => c.Id, c => c);

        var totalValue = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.UnitPrice;
            }
            return 0;
        });

        var breakdown = stockLevels
            .Where(s => itemDict.ContainsKey(s.ItemId))
            .GroupBy(s => itemDict[s.ItemId].CategoryId)
            .Where(g => categoryDict.ContainsKey(g.Key))
            .Select(g =>
            {
                var categoryId = g.Key;
                var category = categoryDict[categoryId];
                var value = g.Sum(s => s.QuantityOnHand * itemDict[s.ItemId].UnitPrice);
                return new WarehouseCategoryBreakdown
                {
                    CategoryId = categoryId,
                    CategoryName = category.Name ?? "Unknown",
                    ItemCount = g.Count(),
                    TotalQuantity = g.Sum(s => s.QuantityOnHand),
                    TotalValue = value,
                    Percentage = totalValue > 0 ? value / totalValue * 100 : 0
                };
            })
            .OrderByDescending(c => c.TotalValue)
            .Take(10)
            .ToList();

        return breakdown;
    }

    private static List<TopWarehouseItem> CalculateTopItems(
        List<StockLevel> stockLevels,
        Dictionary<Guid, Item> itemDict)
    {
        return stockLevels
            .Where(s => itemDict.ContainsKey(s.ItemId))
            .OrderByDescending(s => s.QuantityOnHand * itemDict[s.ItemId].UnitPrice)
            .Take(10)
            .Select(s =>
            {
                var item = itemDict[s.ItemId];
                var stockStatus = s.QuantityOnHand <= 0 ? "Out of Stock"
                    : s.QuantityOnHand <= item.ReorderPoint ? "Low Stock"
                    : s.QuantityOnHand > item.MaximumStock ? "Overstock"
                    : "Healthy";

                return new TopWarehouseItem
                {
                    ItemId = item.Id,
                    ItemName = item.Name ?? "Unknown",
                    Sku = item.Sku,
                    QuantityOnHand = s.QuantityOnHand,
                    TotalValue = s.QuantityOnHand * item.UnitPrice,
                    LocationCode = null,
                    StockStatus = stockStatus
                };
            }).ToList();
    }

    private static List<PendingWarehouseTask> GetPendingTasks(
        List<PutAwayTask> putAwayTasks,
        List<PickList> pickLists,
        List<CycleCount> cycleCounts)
    {
        var tasks = new List<PendingWarehouseTask>();

        tasks.AddRange(putAwayTasks
            .Where(t => t.Status == "Pending" || t.Status == "InProgress")
            .Take(5)
            .Select(t => new PendingWarehouseTask
            {
                TaskId = t.Id,
                TaskType = "Put Away",
                Description = $"Put away task #{t.TaskNumber ?? t.Id.ToString()[..8]}",
                Priority = t.Priority.ToString(),
                CreatedDate = t.CreatedOn.DateTime,
                DueDate = null,
                Status = t.Status ?? "Pending"
            }));

        tasks.AddRange(pickLists
            .Where(p => p.Status == "Created" || p.Status == "Assigned" || p.Status == "InProgress")
            .Take(5)
            .Select(p => new PendingWarehouseTask
            {
                TaskId = p.Id,
                TaskType = "Pick List",
                Description = $"Pick list #{p.PickListNumber ?? p.Id.ToString()[..8]}",
                Priority = p.Priority.ToString(),
                CreatedDate = p.CreatedOn.DateTime,
                DueDate = p.ExpectedCompletionDate,
                Status = p.Status ?? "Created"
            }));

        tasks.AddRange(cycleCounts
            .Where(c => c.Status == "Scheduled" || c.Status == "InProgress")
            .Take(5)
            .Select(c => new PendingWarehouseTask
            {
                TaskId = c.Id,
                TaskType = "Cycle Count",
                Description = $"Cycle count #{c.CountNumber ?? c.Id.ToString()[..8]}",
                Priority = "Normal",
                CreatedDate = c.CreatedOn.DateTime,
                DueDate = c.ScheduledDate,
                Status = c.Status ?? "Scheduled"
            }));

        return tasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue).Take(10).ToList();
    }

    private static List<WarehouseMonthlyComparison> GenerateMonthlyComparison(
        List<InventoryTransaction> transactions,
        List<StockLevel> stockLevels,
        int months)
    {
        var result = new List<WarehouseMonthlyComparison>();
        var today = DateTime.UtcNow.Date;
        var avgInventory = stockLevels.Sum(s => s.QuantityOnHand);

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthTransactions = transactions.Where(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEnd).ToList();

            var inbound = monthTransactions.Where(t => t.Quantity > 0).ToList();
            var outbound = monthTransactions.Where(t => t.Quantity < 0).ToList();
            var outboundQty = outbound.Sum(t => Math.Abs(t.Quantity));
            var turnover = avgInventory > 0 ? (decimal)outboundQty / avgInventory : 0;

            result.Add(new WarehouseMonthlyComparison
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                InboundCount = inbound.Count,
                OutboundCount = outbound.Count,
                InboundValue = inbound.Sum(t => t.TotalCost),
                OutboundValue = outbound.Sum(t => Math.Abs(t.TotalCost)),
                UtilizationPercentage = 0,
                TurnoverRate = turnover
            });
        }

        return result;
    }

    private static List<WarehouseAlert> GenerateAlerts(
        List<StockLevel> stockLevels,
        Dictionary<Guid, Item> itemDict,
        List<PutAwayTask> putAwayTasks,
        List<PickList> pickLists)
    {
        var alerts = new List<WarehouseAlert>();
        var today = DateTime.UtcNow;

        // Low stock alerts
        var lowStockItems = stockLevels
            .Where(s => itemDict.ContainsKey(s.ItemId))
            .Where(s => s.QuantityOnHand > 0 && s.QuantityOnHand <= itemDict[s.ItemId].ReorderPoint)
            .Take(5)
            .Select(s => new WarehouseAlert
            {
                AlertType = "Low Stock",
                Severity = "Warning",
                Message = $"Low stock for {itemDict[s.ItemId].Name}: {s.QuantityOnHand} units remaining",
                CreatedDate = today,
                RelatedItemId = s.ItemId,
                RelatedItemName = itemDict[s.ItemId].Name
            });
        alerts.AddRange(lowStockItems);

        // Out of stock alerts
        var outOfStockItems = stockLevels
            .Where(s => itemDict.ContainsKey(s.ItemId))
            .Where(s => s.QuantityOnHand <= 0)
            .Take(5)
            .Select(s => new WarehouseAlert
            {
                AlertType = "Out of Stock",
                Severity = "Critical",
                Message = $"Out of stock: {itemDict[s.ItemId].Name}",
                CreatedDate = today,
                RelatedItemId = s.ItemId,
                RelatedItemName = itemDict[s.ItemId].Name
            });
        alerts.AddRange(outOfStockItems);

        // Overdue tasks
        var overdueTasks = putAwayTasks
            .Where(t => t.Status != "Completed" && t.CreatedOn.Date < today.Date.AddDays(-1))
            .Take(3)
            .Select(t => new WarehouseAlert
            {
                AlertType = "Overdue Task",
                Severity = "Warning",
                Message = $"Put away task overdue: #{t.TaskNumber ?? t.Id.ToString()[..8]}",
                CreatedDate = today,
                RelatedItemId = null,
                RelatedItemName = null
            });
        alerts.AddRange(overdueTasks);

        return alerts.Take(10).ToList();
    }
}

// Specification classes for warehouse dashboard
public sealed class StockLevelsByWarehouseSpec : Specification<StockLevel>
{
    public StockLevelsByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(s => s.WarehouseId == warehouseId);
    }
}

public sealed class WarehouseLocationsByWarehouseSpec : Specification<WarehouseLocation>
{
    public WarehouseLocationsByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(l => l.WarehouseId == warehouseId);
    }
}

public sealed class InventoryTransactionsByWarehouseSpec : Specification<InventoryTransaction>
{
    public InventoryTransactionsByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(t => t.WarehouseId == warehouseId)
            .OrderByDescending(t => t.TransactionDate);
    }
}

public sealed class PutAwayTasksByWarehouseSpec : Specification<PutAwayTask>
{
    public PutAwayTasksByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(t => t.WarehouseId == warehouseId);
    }
}

public sealed class PickListsByWarehouseSpec : Specification<PickList>
{
    public PickListsByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(p => p.WarehouseId == warehouseId);
    }
}

public sealed class CycleCountsByWarehouseSpec : Specification<CycleCount>
{
    public CycleCountsByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(c => c.WarehouseId == warehouseId);
    }
}

public sealed class GoodsReceiptsByWarehouseSpec : Specification<GoodsReceipt>
{
    public GoodsReceiptsByWarehouseSpec(Guid warehouseId)
    {
        Query.Where(g => g.WarehouseId == warehouseId);
    }
}
