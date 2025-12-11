using FSH.Starter.WebApi.Store.Application.Dashboard;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific supplier.
/// </summary>
public sealed record GetSupplierDashboardQuery(Guid SupplierId) : IRequest<SupplierDashboardResponse>;

public sealed class GetSupplierDashboardHandler(
    IReadRepository<Supplier> supplierRepository,
    IReadRepository<PurchaseOrder> purchaseOrderRepository,
    IReadRepository<PurchaseOrderItem> purchaseOrderItemRepository,
    IReadRepository<GoodsReceipt> goodsReceiptRepository,
    IReadRepository<ItemSupplier> itemSupplierRepository,
    IReadRepository<Item> itemRepository,
    IReadRepository<Category> categoryRepository,
    ICacheService cacheService,
    ILogger<GetSupplierDashboardHandler> logger)
    : IRequestHandler<GetSupplierDashboardQuery, SupplierDashboardResponse>
{
    public async Task<SupplierDashboardResponse> Handle(GetSupplierDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"supplier-dashboard:{request.SupplierId}";

        var cachedResult = await cacheService.GetAsync<SupplierDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for supplier {SupplierId}", request.SupplierId);

        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new NotFoundException($"Supplier {request.SupplierId} not found");

        // Get all purchase orders for this supplier
        var allOrders = await purchaseOrderRepository
            .ListAsync(new PurchaseOrdersBySupplierSpec(request.SupplierId), cancellationToken);

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var lastYearStart = startOfYear.AddYears(-1);
        var lastYearEnd = startOfYear;

        var ordersYTD = allOrders.Where(o => o.CreatedOn >= startOfYear).ToList();
        var ordersLastYear = allOrders.Where(o => o.CreatedOn >= lastYearStart && o.CreatedOn < lastYearEnd).ToList();

        // Get all goods receipts for delivery metrics (linked via purchase orders)
        var orderIds = allOrders.Select(o => o.Id).ToList();
        var goodsReceipts = await goodsReceiptRepository
            .ListAsync(new GoodsReceiptsByOrderIdsSpec(orderIds), cancellationToken);

        // Get items supplied by this supplier
        var itemSuppliers = await itemSupplierRepository
            .ListAsync(new ItemSuppliersBySupplierSpec(request.SupplierId), cancellationToken);

        // Calculate financial metrics
        var financials = CalculateFinancialMetrics(allOrders, ordersYTD, ordersLastYear, supplier);

        // Calculate order metrics
        var orderMetrics = CalculateOrderMetrics(allOrders, ordersYTD, ordersLastYear);

        // Calculate delivery performance
        var deliveryPerformance = CalculateDeliveryPerformance(allOrders, goodsReceipts);

        // Calculate quality metrics
        var qualityMetrics = await CalculateQualityMetrics(request.SupplierId, goodsReceipts, cancellationToken);

        // Calculate item portfolio
        var itemPortfolio = await CalculateItemPortfolio(itemSuppliers, cancellationToken);

        // Generate trend data
        var orderValueTrend = GenerateMonthlyValueTrend(allOrders, 12);
        var orderCountTrend = GenerateMonthlyCountTrend(allOrders, 12);
        var leadTimeTrend = GenerateLeadTimeTrend(allOrders, goodsReceipts, 12);
        var onTimeDeliveryTrend = GenerateOnTimeDeliveryTrend(allOrders, goodsReceipts, 12);

        // Category breakdown
        var categoryBreakdown = await CalculateCategoryBreakdown(request.SupplierId, cancellationToken);

        // Top items
        var topItems = await GetTopItems(request.SupplierId, cancellationToken);

        // Recent orders
        var recentOrders = allOrders
            .OrderByDescending(o => o.CreatedOn)
            .Take(10)
            .Select(po => new SupplierRecentPurchaseOrder
            {
                PurchaseOrderId = po.Id,
                OrderNumber = po.OrderNumber ?? $"PO-{po.Id.ToString()[..8]}",
                OrderDate = po.OrderDate,
                Status = po.Status ?? "Unknown",
                TotalAmount = po.TotalAmount,
                ItemCount = po.Items?.Count ?? 0,
                ExpectedDelivery = po.ExpectedDeliveryDate,
                ActualDelivery = null // Would need to track from goods receipts
            }).ToList();

        // Monthly comparison
        var monthlyPerformance = GenerateMonthlyComparison(allOrders, goodsReceipts, 6);

        // Supplier ranking
        var ranking = await CalculateSupplierRanking(request.SupplierId, cancellationToken);

        var response = new SupplierDashboardResponse
        {
            SupplierId = supplier.Id,
            SupplierName = supplier.Name ?? "Unknown",
            Code = supplier.Code,
            ContactPerson = supplier.ContactPerson,
            Email = supplier.Email,
            Phone = supplier.Phone,
            ImageUrl = supplier.ImageUrl,
            IsActive = supplier.IsActive,
            Rating = supplier.Rating,
            Financials = financials,
            Orders = orderMetrics,
            Delivery = deliveryPerformance,
            Quality = qualityMetrics,
            Items = itemPortfolio,
            OrderValueTrend = orderValueTrend,
            OrderCountTrend = orderCountTrend,
            LeadTimeTrend = leadTimeTrend,
            OnTimeDeliveryTrend = onTimeDeliveryTrend,
            OrdersByCategory = categoryBreakdown,
            TopItems = topItems,
            RecentOrders = recentOrders,
            MonthlyPerformance = monthlyPerformance,
            Ranking = ranking
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static SupplierFinancialMetrics CalculateFinancialMetrics(
        List<PurchaseOrder> allOrders,
        List<PurchaseOrder> ordersYTD,
        List<PurchaseOrder> ordersLastYear,
        Supplier supplier)
    {
        var totalValue = allOrders.Sum(o => o.TotalAmount);
        var totalValueYTD = ordersYTD.Sum(o => o.TotalAmount);
        var totalValueLastYear = ordersLastYear.Sum(o => o.TotalAmount);
        var avgOrderValue = allOrders.Count > 0 ? totalValue / allOrders.Count : 0;
        var growth = totalValueLastYear > 0 ? (totalValueYTD - totalValueLastYear) / totalValueLastYear * 100 : 0;

        var creditUtilization = supplier.CreditLimit > 0
            ? 0 / supplier.CreditLimit.Value * 100 // Would need actual outstanding balance
            : 0;

        return new SupplierFinancialMetrics
        {
            TotalPurchaseValue = totalValue,
            TotalPurchaseValueYTD = totalValueYTD,
            TotalPurchaseValueLastYear = totalValueLastYear,
            AverageOrderValue = avgOrderValue,
            OutstandingBalance = 0, // Would need integration with AP
            CreditLimit = supplier.CreditLimit ?? 0,
            CreditUtilization = creditUtilization,
            PaymentTermsDays = supplier.PaymentTermsDays,
            GrowthPercentage = growth
        };
    }

    private static SupplierOrderMetrics CalculateOrderMetrics(
        List<PurchaseOrder> allOrders,
        List<PurchaseOrder> ordersYTD,
        List<PurchaseOrder> ordersLastYear)
    {
        var pendingCount = allOrders.Count(o => o.Status == "Pending" || o.Status == "Draft");
        var approvedCount = allOrders.Count(o => o.Status == "Approved" || o.Status == "Sent");
        var completedCount = allOrders.Count(o => o.Status == "Completed" || o.Status == "Received");
        var cancelledCount = allOrders.Count(o => o.Status == "Cancelled");

        var completionRate = allOrders.Count > 0
            ? (decimal)completedCount / allOrders.Count * 100
            : 0;

        var cancellationRate = allOrders.Count > 0
            ? (decimal)cancelledCount / allOrders.Count * 100
            : 0;

        var avgItemsPerOrder = allOrders.Count > 0
            ? allOrders.Average(o => o.Items?.Count ?? 0)
            : 0;

        return new SupplierOrderMetrics
        {
            TotalOrders = allOrders.Count,
            TotalOrdersYTD = ordersYTD.Count,
            TotalOrdersLastYear = ordersLastYear.Count,
            PendingOrders = pendingCount,
            ApprovedOrders = approvedCount,
            CompletedOrders = completedCount,
            CancelledOrders = cancelledCount,
            CompletionRate = completionRate,
            CancellationRate = cancellationRate,
            AverageItemsPerOrder = (decimal)avgItemsPerOrder
        };
    }

    private static SupplierDeliveryPerformance CalculateDeliveryPerformance(
        List<PurchaseOrder> allOrders,
        List<GoodsReceipt> goodsReceipts)
    {
        var ordersWithDelivery = allOrders
            .Where(o => o.ExpectedDeliveryDate.HasValue)
            .ToList();

        var deliveredOrders = ordersWithDelivery
            .Where(o => o.Status == "Completed" || o.Status == "Received")
            .ToList();

        // Calculate lead times from goods receipts
        var leadTimes = new List<int>();
        foreach (var receipt in goodsReceipts)
        {
            var relatedOrder = allOrders.FirstOrDefault(o => o.Id == receipt.PurchaseOrderId);
            if (relatedOrder != null)
            {
                var leadTime = (receipt.ReceivedDate - relatedOrder.OrderDate).Days;
                leadTimes.Add(leadTime);
            }
        }

        var avgLeadTime = leadTimes.Count > 0 ? leadTimes.Average() : 0;
        var minLeadTime = leadTimes.Count > 0 ? leadTimes.Min() : 0;
        var maxLeadTime = leadTimes.Count > 0 ? leadTimes.Max() : 0;

        // On-time calculation
        int onTime = 0, early = 0, late = 0;
        foreach (var order in deliveredOrders)
        {
            if (!order.ExpectedDeliveryDate.HasValue) continue;

            var receipt = goodsReceipts.FirstOrDefault(r => r.PurchaseOrderId == order.Id);
            if (receipt == null) continue;

            var expected = order.ExpectedDeliveryDate.Value;
            var actual = receipt.ReceivedDate;

            if (actual <= expected.AddDays(-1)) early++;
            else if (actual <= expected) onTime++;
            else late++;
        }

        var totalDelivered = early + onTime + late;
        var onTimeRate = totalDelivered > 0 ? (decimal)(early + onTime) / totalDelivered * 100 : 100;

        return new SupplierDeliveryPerformance
        {
            OnTimeDeliveryRate = onTimeRate,
            AverageLeadTimeDays = (decimal)avgLeadTime,
            ShortestLeadTimeDays = minLeadTime,
            LongestLeadTimeDays = maxLeadTime,
            EarlyDeliveries = early,
            OnTimeDeliveries = onTime,
            LateDeliveries = late,
            AverageDelayDays = 0, // Would calculate from late deliveries
            DeliveryReliabilityScore = onTimeRate
        };
    }

    private async Task<SupplierQualityMetrics> CalculateQualityMetrics(
        Guid supplierId,
        List<GoodsReceipt> goodsReceipts,
        CancellationToken ct)
    {
        var totalReceived = 0;
        var accepted = 0;
        var rejected = 0;

        foreach (var receipt in goodsReceipts)
        {
            foreach (var item in receipt.Items)
            {
                // GoodsReceiptItem only has Quantity - treat all received as accepted for now
                totalReceived += item.Quantity;
                accepted += item.Quantity;
                // rejected would need separate tracking
            }
        }

        var acceptanceRate = totalReceived > 0 ? (decimal)accepted / totalReceived * 100 : 100;
        var defectRate = totalReceived > 0 ? (decimal)rejected / totalReceived * 100 : 0;

        return new SupplierQualityMetrics
        {
            AcceptanceRate = acceptanceRate,
            TotalItemsReceived = totalReceived,
            ItemsAccepted = accepted,
            ItemsRejected = rejected,
            ItemsWithDefects = rejected,
            DefectRate = defectRate,
            ReturnCount = 0, // Would need return tracking
            ReturnRate = 0,
            QualityScore = acceptanceRate
        };
    }

    private async Task<SupplierItemPortfolio> CalculateItemPortfolio(
        List<ItemSupplier> itemSuppliers,
        CancellationToken ct)
    {
        var activeItems = itemSuppliers.Count(i => i.IsPreferred);
        var categories = new List<string>();

        foreach (var itemSupplier in itemSuppliers.Take(10))
        {
            var item = await itemRepository.GetByIdAsync(itemSupplier.ItemId, ct);
            if (item != null && item.CategoryId != Guid.Empty)
            {
                var category = await categoryRepository.GetByIdAsync(item.CategoryId, ct);
                if (category?.Name != null && !categories.Contains(category.Name))
                {
                    categories.Add(category.Name);
                }
            }
        }

        return new SupplierItemPortfolio
        {
            TotalItemsSupplied = itemSuppliers.Count,
            ActiveItems = activeItems,
            ExclusiveItems = itemSuppliers.Count(i => i.IsPreferred),
            AveragePriceCompetitiveness = 85, // Would need market comparison
            TopCategories = categories.Take(5).ToList()
        };
    }

    private static List<StoreTimeSeriesDataPoint> GenerateMonthlyValueTrend(List<PurchaseOrder> orders, int months)
    {
        var result = new List<StoreTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthOrders = orders.Where(o =>
                o.CreatedOn >= monthStart && o.CreatedOn < monthEnd).ToList();

            result.Add(new StoreTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthOrders.Sum(o => o.TotalAmount),
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<StoreTimeSeriesDataPoint> GenerateMonthlyCountTrend(List<PurchaseOrder> orders, int months)
    {
        var result = new List<StoreTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthOrders = orders.Where(o =>
                o.CreatedOn >= monthStart && o.CreatedOn < monthEnd).ToList();

            result.Add(new StoreTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthOrders.Count,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<StoreTimeSeriesDataPoint> GenerateLeadTimeTrend(
        List<PurchaseOrder> orders,
        List<GoodsReceipt> receipts,
        int months)
    {
        var result = new List<StoreTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthReceipts = receipts.Where(r =>
                r.ReceivedDate >= monthStart &&
                r.ReceivedDate < monthEnd).ToList();

            var leadTimes = new List<int>();
            foreach (var receipt in monthReceipts)
            {
                var order = orders.FirstOrDefault(o => o.Id == receipt.PurchaseOrderId);
                if (order != null)
                {
                    var leadTime = (receipt.ReceivedDate - order.OrderDate).Days;
                    leadTimes.Add(leadTime);
                }
            }

            result.Add(new StoreTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = leadTimes.Count > 0 ? (decimal)leadTimes.Average() : 0,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<StoreTimeSeriesDataPoint> GenerateOnTimeDeliveryTrend(
        List<PurchaseOrder> orders,
        List<GoodsReceipt> receipts,
        int months)
    {
        var result = new List<StoreTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthReceipts = receipts.Where(r =>
                r.ReceivedDate >= monthStart &&
                r.ReceivedDate < monthEnd).ToList();

            int onTime = 0, total = 0;
            foreach (var receipt in monthReceipts)
            {
                var order = orders.FirstOrDefault(o => o.Id == receipt.PurchaseOrderId);
                if (order?.ExpectedDeliveryDate != null)
                {
                    total++;
                    if (receipt.ReceivedDate <= order.ExpectedDeliveryDate.Value)
                        onTime++;
                }
            }

            result.Add(new StoreTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = total > 0 ? (decimal)onTime / total * 100 : 100,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private async Task<List<StoreCategoryBreakdown>> CalculateCategoryBreakdown(Guid supplierId, CancellationToken ct)
    {
        var result = new Dictionary<string, (int count, decimal value)>();

        var poItems = await purchaseOrderItemRepository
            .ListAsync(new POItemsBySupplierSpec(supplierId), ct);

        foreach (var poItem in poItems)
        {
            var item = await itemRepository.GetByIdAsync(poItem.ItemId, ct);
            if (item == null) continue;

            var category = await categoryRepository.GetByIdAsync(item.CategoryId, ct);
            var categoryName = category?.Name ?? "Uncategorized";

            if (!result.ContainsKey(categoryName))
                result[categoryName] = (0, 0);

            var current = result[categoryName];
            result[categoryName] = (current.count + 1, current.value + poItem.TotalPrice);
        }

        var totalValue = result.Values.Sum(v => v.value);

        return result
            .OrderByDescending(r => r.Value.value)
            .Take(10)
            .Select(r => new StoreCategoryBreakdown
            {
                CategoryName = r.Key,
                OrderCount = r.Value.count,
                TotalValue = r.Value.value,
                Percentage = totalValue > 0 ? r.Value.value / totalValue * 100 : 0
            }).ToList();
    }

    private async Task<List<SupplierTopItemInfo>> GetTopItems(Guid supplierId, CancellationToken ct)
    {
        var result = new Dictionary<Guid, SupplierTopItemInfo>();

        var poItems = await purchaseOrderItemRepository
            .ListAsync(new POItemsBySupplierSpec(supplierId), ct);

        foreach (var poItem in poItems)
        {
            if (!result.ContainsKey(poItem.ItemId))
            {
                var item = await itemRepository.GetByIdAsync(poItem.ItemId, ct);
                result[poItem.ItemId] = new SupplierTopItemInfo
                {
                    ItemId = poItem.ItemId,
                    ItemName = item?.Name ?? "Unknown",
                    Sku = item?.Sku,
                    TotalQuantityOrdered = 0,
                    TotalValue = 0,
                    OrderCount = 0,
                    AverageUnitPrice = 0
                };
            }

            var current = result[poItem.ItemId];
            result[poItem.ItemId] = current with
            {
                TotalQuantityOrdered = current.TotalQuantityOrdered + poItem.Quantity,
                TotalValue = current.TotalValue + poItem.TotalPrice,
                OrderCount = current.OrderCount + 1
            };
        }

        return result.Values
            .OrderByDescending(i => i.TotalValue)
            .Take(10)
            .Select(i => i with
            {
                AverageUnitPrice = i.TotalQuantityOrdered > 0 ? i.TotalValue / i.TotalQuantityOrdered : 0
            }).ToList();
    }

    private static List<SupplierMonthlyComparison> GenerateMonthlyComparison(
        List<PurchaseOrder> orders,
        List<GoodsReceipt> receipts,
        int months)
    {
        var result = new List<SupplierMonthlyComparison>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthOrders = orders.Where(o =>
                o.CreatedOn >= monthStart && o.CreatedOn < monthEnd).ToList();

            var monthReceipts = receipts.Where(r =>
                r.ReceivedDate >= monthStart &&
                r.ReceivedDate < monthEnd).ToList();

            // Calculate on-time rate
            int onTime = 0, total = 0;
            foreach (var receipt in monthReceipts)
            {
                var order = orders.FirstOrDefault(o => o.Id == receipt.PurchaseOrderId);
                if (order?.ExpectedDeliveryDate != null)
                {
                    total++;
                    if (receipt.ReceivedDate <= order.ExpectedDeliveryDate.Value)
                        onTime++;
                }
            }

            result.Add(new SupplierMonthlyComparison
            {
                Month = monthStart.ToString("MMM"),
                Year = monthStart.Year,
                OrderValue = monthOrders.Sum(o => o.TotalAmount),
                OrderCount = monthOrders.Count,
                OnTimeRate = total > 0 ? (decimal)onTime / total * 100 : 100,
                QualityScore = 95 // Would need actual quality tracking
            });
        }

        return result;
    }

    private async Task<SupplierRanking> CalculateSupplierRanking(Guid supplierId, CancellationToken ct)
    {
        // Get all suppliers for ranking comparison
        var allSuppliers = await supplierRepository.ListAsync(ct);
        var totalSuppliers = allSuppliers.Count;

        // Simplified ranking - would need more complex calculation
        var currentSupplier = allSuppliers.FirstOrDefault(s => s.Id == supplierId);
        var ranking = allSuppliers
            .OrderByDescending(s => s.Rating)
            .ToList()
            .FindIndex(s => s.Id == supplierId) + 1;

        var tier = ranking switch
        {
            <= 3 => "Preferred",
            <= 10 => "Approved",
            <= 20 => "Standard",
            _ => "Under Review"
        };

        return new SupplierRanking
        {
            OverallRank = ranking,
            TotalSuppliers = totalSuppliers,
            VolumeRank = ranking,
            QualityRank = ranking,
            DeliveryRank = ranking,
            PriceRank = ranking,
            PerformanceTier = tier
        };
    }
}

// Specification classes
public class PurchaseOrdersBySupplierSpec : Specification<PurchaseOrder>
{
    public PurchaseOrdersBySupplierSpec(Guid supplierId)
    {
        Query.Where(p => p.SupplierId == supplierId)
             .Include(p => p.Items);
    }
}

public class GoodsReceiptsByOrderIdsSpec : Specification<GoodsReceipt>
{
    public GoodsReceiptsByOrderIdsSpec(List<Guid> orderIds)
    {
        Query.Where(g => g.PurchaseOrderId.HasValue && orderIds.Contains(g.PurchaseOrderId.Value))
             .Include(g => g.Items);
    }
}

public class ItemSuppliersBySupplierSpec : Specification<ItemSupplier>
{
    public ItemSuppliersBySupplierSpec(Guid supplierId)
    {
        Query.Where(i => i.SupplierId == supplierId);
    }
}

public class GoodsReceiptItemsByReceiptSpec : Specification<GoodsReceiptItem>
{
    public GoodsReceiptItemsByReceiptSpec(Guid receiptId)
    {
        Query.Where(g => g.GoodsReceiptId == receiptId);
    }
}

public class POItemsBySupplierSpec : Specification<PurchaseOrderItem>
{
    public POItemsBySupplierSpec(Guid supplierId)
    {
        Query.Include(p => p.PurchaseOrder)
             .Where(p => p.PurchaseOrder!.SupplierId == supplierId);
    }
}
