# Technical Implementation Guide: Multi-Location Inventory Counting

## 🎯 Quick Start Implementation

This is the **technical companion** to the main Multi-Warehouse Inventory Counting Guide, focusing on API usage and code implementation.

---

## 📋 Core API Endpoints (Already Available)

### Cycle Count Management

```http
# Create a new cycle count
POST /api/v1/cycle-counts
Content-Type: application/json

{
  "countNumber": "CC-2025-11-001",
  "warehouseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "warehouseLocationId": "optional-location-id",
  "scheduledDate": "2025-11-15T06:00:00Z",
  "countType": "ABC",
  "counterName": "John Smith",
  "supervisorName": "Jane Doe",
  "notes": "Weekly high-value items count"
}
```

```http
# Get cycle count details
GET /api/v1/cycle-counts/{id}
```

```http
# Search cycle counts
POST /api/v1/cycle-counts/search
Content-Type: application/json

{
  "pageNumber": 1,
  "pageSize": 20,
  "keyword": "WH-MAIN",
  "warehouseId": "warehouse-guid",
  "status": "Scheduled",
  "countType": "ABC",
  "dateFrom": "2025-11-01",
  "dateTo": "2025-11-30"
}
```

```http
# Update cycle count (only when status = "Scheduled")
PUT /api/v1/cycle-counts/{id}
Content-Type: application/json

{
  "warehouseId": "warehouse-guid",
  "scheduledDate": "2025-11-16T06:00:00Z",
  "countType": "Full",
  ...
}
```

```http
# Start counting
POST /api/v1/cycle-counts/{id}/start
```

```http
# Complete count
POST /api/v1/cycle-counts/{id}/complete
```

```http
# Cancel count
POST /api/v1/cycle-counts/{id}/cancel
Content-Type: application/json

{
  "reason": "Warehouse closed for maintenance"
}
```

### Cycle Count Items

```http
# Add item to count
POST /api/v1/cycle-count-items
Content-Type: application/json

{
  "cycleCountId": "cycle-count-guid",
  "itemId": "item-guid",
  "systemQuantity": 150,
  "countedQuantity": null,
  "notes": "High-value item"
}
```

```http
# Update counted quantity
PUT /api/v1/cycle-count-items/{id}/count
Content-Type: application/json

{
  "countedQuantity": 148,
  "countedBy": "john.smith",
  "notes": "2 units damaged, removed from stock"
}
```

```http
# Mark for recount
POST /api/v1/cycle-count-items/{id}/recount
Content-Type: application/json

{
  "recountReason": "Variance exceeds 10% threshold"
}
```

```http
# Get items for a cycle count
GET /api/v1/cycle-counts/{cycleCountId}/items
```

---

## 🔄 Complete Workflow Implementation

### Scenario: Schedule Weekly Count for Main Warehouse

```typescript
// Step 1: Get warehouse ID
const warehouse = await client.searchWarehouses({
  keyword: "WH-MAIN"
});
const warehouseId = warehouse.items[0].id;

// Step 2: Create cycle count
const cycleCount = await client.createCycleCount({
  countNumber: generateCountNumber(), // "CC-2025-11-001"
  warehouseId: warehouseId,
  warehouseLocationId: null, // Full warehouse count
  scheduledDate: new Date("2025-11-15T06:00:00Z"),
  countType: "ABC",
  counterName: "John Smith",
  supervisorName: "Jane Doe",
  notes: "Weekly A-items count"
});

// Step 3: Get stock levels for A-items
const stockLevels = await client.searchStockLevels({
  warehouseId: warehouseId,
  // Filter logic for A-items (high-value)
  minValue: 100 // Items valued at $100+
});

// Step 4: Add items to cycle count
for (const stock of stockLevels.items) {
  await client.addCycleCountItem({
    cycleCountId: cycleCount.id,
    itemId: stock.itemId,
    systemQuantity: stock.quantityOnHand,
    countedQuantity: null
  });
}

// Step 5: Notify counter
await sendNotification({
  to: "john.smith@company.com",
  subject: `Cycle Count Scheduled: ${cycleCount.countNumber}`,
  body: `You have been assigned to count ${stockLevels.items.length} items on Nov 15, 2025 at 6:00 AM.`
});
```

### Scenario: Counter Performs Count via Mobile App

```typescript
// Mobile App: Counter logs in and starts count
async function startCounting(cycleCountId: string) {
  // 1. Start the count
  await client.startCycleCount(cycleCountId);
  
  // 2. Get all items to count
  const items = await client.getCycleCountItems(cycleCountId);
  
  // 3. Load into local storage for offline support
  await localDB.saveCountItems(items);
  
  return items;
}

// Mobile App: Scan and count each item
async function recordCount(itemId: string, countedQty: number, notes?: string) {
  const countItem = await localDB.getCountItem(itemId);
  
  // Calculate variance
  const variance = countedQty - countItem.systemQuantity;
  const variancePct = (variance / countItem.systemQuantity) * 100;
  
  // Flag if variance exceeds threshold
  const requiresRecount = Math.abs(variancePct) > 5;
  
  if (requiresRecount) {
    showWarning(`Large variance detected: ${variancePct.toFixed(1)}%`);
  }
  
  // Save count (sync when online)
  await client.updateCycleCountItemCount(countItem.id, {
    countedQuantity: countedQty,
    countedBy: getCurrentUser(),
    notes: notes
  });
  
  // Update local progress
  await localDB.markItemCounted(itemId);
  
  // Move to next item
  return getNextItem();
}

// Mobile App: Complete count
async function completeCount(cycleCountId: string) {
  // 1. Check all items counted
  const uncountedItems = await localDB.getUncountedItems(cycleCountId);
  
  if (uncountedItems.length > 0) {
    throw new Error(`${uncountedItems.length} items not yet counted`);
  }
  
  // 2. Sync any pending updates
  await syncPendingCounts();
  
  // 3. Complete the count
  await client.completeCycleCount(cycleCountId);
  
  // 4. Show summary
  const summary = await client.getCycleCountSummary(cycleCountId);
  showSummary(summary);
}
```

### Scenario: Supervisor Reviews Variances

```typescript
// Get count with variances
const cycleCount = await client.getCycleCount(cycleCountId);
const items = await client.getCycleCountItems(cycleCountId);

// Filter items requiring attention
const varianceItems = items.filter(item => 
  item.varianceQuantity !== 0
);

const recountItems = varianceItems.filter(item => 
  item.requiresRecount === true
);

// Display for supervisor review
console.log(`
Cycle Count: ${cycleCount.countNumber}
Status: ${cycleCount.status}
Accuracy: ${cycleCount.accuracyPercentage}%

Items with Variances: ${varianceItems.length}
Requires Recount: ${recountItems.length}
`);

// Approve adjustments for small variances
for (const item of varianceItems) {
  const variancePct = Math.abs(
    (item.varianceQuantity! / item.systemQuantity) * 100
  );
  
  if (variancePct < 5 && !item.requiresRecount) {
    // Auto-approve small variances
    await approveVariance(item.id, "Auto-approved < 5% variance");
  } else {
    // Require manual review
    await flagForReview(item.id);
  }
}
```

---

## 📊 Auto-Population Logic for Count Items

### Strategy 1: Full Warehouse Count

```csharp
// Backend: CreateCycleCountHandler
public async Task<Guid> Handle(CreateCycleCountCommand request, CancellationToken ct)
{
    // Create cycle count
    var cycleCount = CycleCount.Create(...);
    await _repository.AddAsync(cycleCount, ct);
    
    // If count type is "Full", auto-add all items from warehouse
    if (request.CountType == "Full")
    {
        var stockLevels = await _stockLevelRepository.ListAsync(
            new StockLevelsByWarehouseSpec(request.WarehouseId),
            ct
        );
        
        foreach (var stock in stockLevels)
        {
            var item = CycleCountItem.Create(
                cycleCount.Id,
                stock.ItemId,
                stock.QuantityOnHand,
                countedQuantity: null,
                notes: null
            );
            await _cycleCountItemRepository.AddAsync(item, ct);
        }
    }
    
    await _unitOfWork.SaveChangesAsync(ct);
    return cycleCount.Id;
}
```

### Strategy 2: ABC Classification

```csharp
// Specification for A-items
public class HighValueItemsSpec : Specification<StockLevel>
{
    public HighValueItemsSpec(Guid warehouseId)
    {
        Query
            .Where(s => s.WarehouseId == warehouseId)
            .Include(s => s.Item)
            .Where(s => s.Item.UnitPrice * s.QuantityOnHand > 1000); // $1000+ value
    }
}

// Usage in handler
var stockLevels = await _stockLevelRepository.ListAsync(
    new HighValueItemsSpec(request.WarehouseId),
    ct
);
```

### Strategy 3: Location-Based Count

```csharp
// Specification for specific location
public class StockLevelsByLocationSpec : Specification<StockLevel>
{
    public StockLevelsByLocationSpec(Guid warehouseId, Guid locationId)
    {
        Query
            .Where(s => s.WarehouseId == warehouseId)
            .Where(s => s.WarehouseLocationId == locationId);
    }
}
```

### Strategy 4: Random Sample

```csharp
// Random selection for spot checks
public class RandomStockLevelsSpec : Specification<StockLevel>
{
    public RandomStockLevelsSpec(Guid warehouseId, int count = 50)
    {
        Query
            .Where(s => s.WarehouseId == warehouseId)
            .OrderBy(s => Guid.NewGuid()) // Random ordering
            .Take(count);
    }
}
```

---

## 🔄 Inventory Adjustment After Count

### Automatic Stock Level Update

```csharp
// Handler: CompleteCycleCountCommand
public async Task Handle(CompleteCycleCountCommand request, CancellationToken ct)
{
    var cycleCount = await _repository.GetByIdAsync(request.Id, ct);
    
    // Complete the count
    cycleCount.Complete();
    
    // Get all count items with variances
    var countItems = await _cycleCountItemRepository.ListAsync(
        new CycleCountItemsWithVarianceSpec(request.Id),
        ct
    );
    
    foreach (var countItem in countItems)
    {
        if (countItem.VarianceQuantity == 0) continue;
        
        // Get stock level
        var stockLevel = await _stockLevelRepository.FirstOrDefaultAsync(
            new StockLevelByItemAndWarehouseSpec(
                countItem.ItemId, 
                cycleCount.WarehouseId
            ),
            ct
        );
        
        if (stockLevel == null) continue;
        
        // Adjust quantity
        stockLevel.AdjustQuantity(
            countItem.VarianceQuantity!.Value,
            reason: $"Cycle Count Adjustment: {cycleCount.CountNumber}",
            adjustedBy: cycleCount.CounterName
        );
        
        // Create inventory transaction
        var transaction = InventoryTransaction.Create(
            itemId: countItem.ItemId,
            warehouseId: cycleCount.WarehouseId,
            transactionType: "Adjustment",
            quantity: countItem.VarianceQuantity!.Value,
            referenceType: "CycleCount",
            referenceId: cycleCount.Id,
            notes: countItem.Notes
        );
        
        await _inventoryTransactionRepository.AddAsync(transaction, ct);
    }
    
    await _unitOfWork.SaveChangesAsync(ct);
}
```

---

## 📱 Offline Support for Mobile App

### Using IndexedDB for Offline Storage

```typescript
// Mobile App: Offline database schema
interface OfflineCycleCount {
  id: string;
  countNumber: string;
  warehouseId: string;
  status: string;
  items: OfflineCycleCountItem[];
  syncStatus: 'synced' | 'pending' | 'error';
  lastSyncDate?: Date;
}

interface OfflineCycleCountItem {
  id: string;
  itemId: string;
  itemName: string;
  itemSku: string;
  systemQuantity: number;
  countedQuantity?: number;
  notes?: string;
  syncStatus: 'synced' | 'pending' | 'error';
}

// Download for offline use
async function downloadForOffline(cycleCountId: string) {
  const cycleCount = await client.getCycleCount(cycleCountId);
  const items = await client.getCycleCountItems(cycleCountId);
  
  // Enrich with item details
  const enrichedItems = await Promise.all(
    items.map(async (item) => {
      const itemDetails = await client.getItem(item.itemId);
      return {
        ...item,
        itemName: itemDetails.name,
        itemSku: itemDetails.sku,
        syncStatus: 'synced' as const
      };
    })
  );
  
  // Save to IndexedDB
  await offlineDB.saveCycleCount({
    ...cycleCount,
    items: enrichedItems,
    syncStatus: 'synced'
  });
}

// Record count offline
async function recordCountOffline(itemId: string, countedQty: number) {
  // Update in local DB
  await offlineDB.updateCountItem(itemId, {
    countedQuantity: countedQty,
    countDate: new Date(),
    syncStatus: 'pending'
  });
  
  // Try to sync if online
  if (navigator.onLine) {
    await syncToServer();
  }
}

// Sync when back online
async function syncToServer() {
  const pendingCounts = await offlineDB.getPendingCounts();
  
  for (const count of pendingCounts) {
    try {
      await client.updateCycleCountItemCount(count.id, {
        countedQuantity: count.countedQuantity!,
        countedBy: getCurrentUser(),
        notes: count.notes
      });
      
      // Mark as synced
      await offlineDB.markSynced(count.id);
    } catch (error) {
      // Mark as error, will retry later
      await offlineDB.markSyncError(count.id, error.message);
    }
  }
}
```

---

## 📊 Reporting Queries

### Query 1: Count Accuracy by Warehouse

```sql
SELECT 
    w.Code AS WarehouseCode,
    w.Name AS WarehouseName,
    COUNT(cc.Id) AS TotalCounts,
    AVG(cc.AccuracyPercentage) AS AvgAccuracy,
    SUM(cc.ItemsWithDiscrepancies) AS TotalDiscrepancies,
    SUM(cc.TotalItemsToCount) AS TotalItemsCounted
FROM CycleCounts cc
JOIN Warehouses w ON cc.WarehouseId = w.Id
WHERE cc.Status = 'Completed'
  AND cc.CompletionDate >= DATEADD(month, -1, GETDATE())
GROUP BY w.Code, w.Name
ORDER BY AvgAccuracy DESC;
```

### Query 2: Items with Frequent Variances

```sql
SELECT 
    i.SKU,
    i.Name,
    COUNT(DISTINCT cci.CycleCountId) AS CountOccurrences,
    COUNT(*) AS TotalTimesIn Counts,
    SUM(CASE WHEN cci.VarianceQuantity <> 0 THEN 1 ELSE 0 END) AS VarianceCount,
    AVG(ABS(cci.VarianceQuantity)) AS AvgAbsVariance,
    (CAST(SUM(CASE WHEN cci.VarianceQuantity <> 0 THEN 1 ELSE 0 END) AS FLOAT) 
     / COUNT(*) * 100) AS VarianceRate
FROM CycleCountItems cci
JOIN Items i ON cci.ItemId = i.Id
JOIN CycleCounts cc ON cci.CycleCountId = cc.Id
WHERE cc.Status = 'Completed'
  AND cc.CompletionDate >= DATEADD(month, -3, GETDATE())
GROUP BY i.SKU, i.Name
HAVING SUM(CASE WHEN cci.VarianceQuantity <> 0 THEN 1 ELSE 0 END) >= 3
ORDER BY VarianceRate DESC;
```

### Query 3: Counter Performance Metrics

```sql
SELECT 
    cc.CounterName,
    COUNT(*) AS CountsCompleted,
    AVG(cc.AccuracyPercentage) AS AvgAccuracy,
    SUM(cc.TotalItemsToCount) AS TotalItemsCounted,
    AVG(DATEDIFF(minute, cc.ActualStartDate, cc.CompletionDate)) AS AvgMinutesToComplete,
    AVG(CAST(cc.TotalItemsToCount AS FLOAT) / 
        NULLIF(DATEDIFF(minute, cc.ActualStartDate, cc.CompletionDate), 0)) AS ItemsPerMinute
FROM CycleCounts cc
WHERE cc.Status = 'Completed'
  AND cc.CompletionDate >= DATEADD(month, -1, GETDATE())
  AND cc.CounterName IS NOT NULL
GROUP BY cc.CounterName
ORDER BY AvgAccuracy DESC;
```

---

## 🔔 Notification Integration

### Send Notifications for Key Events

```csharp
// Domain Event Handler
public class CycleCountCompletedEventHandler : INotificationHandler<CycleCountCompleted>
{
    private readonly INotificationService _notificationService;
    
    public async Task Handle(CycleCountCompleted notification, CancellationToken ct)
    {
        var cycleCount = notification.CycleCount;
        
        // Notify supervisor if accuracy is low
        if (cycleCount.AccuracyPercentage < 95m)
        {
            await _notificationService.SendAsync(new NotificationMessage
            {
                To = cycleCount.SupervisorName,
                Subject = $"Low Accuracy Alert: {cycleCount.CountNumber}",
                Body = $@"
                    Cycle count completed with {cycleCount.AccuracyPercentage:F1}% accuracy.
                    
                    Items counted: {cycleCount.TotalItemsToCount}
                    Discrepancies: {cycleCount.ItemsWithDiscrepancies}
                    
                    Please review variances.
                ",
                Priority = "High"
            });
        }
        
        // Notify finance if large variance value
        var totalVarianceValue = await CalculateVarianceValue(cycleCount.Id, ct);
        
        if (Math.Abs(totalVarianceValue) > 5000m)
        {
            await _notificationService.SendAsync(new NotificationMessage
            {
                To = "finance@company.com",
                Subject = $"Large Variance Detected: {cycleCount.CountNumber}",
                Body = $@"
                    Cycle count completed with ${Math.Abs(totalVarianceValue):N2} variance.
                    
                    Location: {cycleCount.Warehouse.Name}
                    Accuracy: {cycleCount.AccuracyPercentage:F1}%
                    
                    Approval required for inventory adjustment.
                ",
                Priority = "High"
            });
        }
    }
}
```

---

## ✅ Implementation Checklist

### Backend (Already Complete ✅)
- [x] CycleCount entity with status workflow
- [x] CycleCountItem entity with variance tracking
- [x] CRUD operations and search
- [x] Start/Complete/Cancel commands
- [x] Domain events for workflow tracking
- [x] StockLevel integration for system quantities
- [x] Audit trail

### Frontend (Existing ✅)
- [x] Cycle Counts page (list/search)
- [x] Create cycle count form
- [x] Cycle count items management

### To Implement 🔄
- [ ] Mobile counting interface
- [ ] Barcode scanning integration
- [ ] Offline support with sync
- [ ] Variance approval workflow UI
- [ ] Counter performance dashboard
- [ ] Automated item population based on count type
- [ ] Inventory adjustment integration
- [ ] Email/SMS notifications
- [ ] Reporting dashboards

---

**Ready to start counting!** The core infrastructure is in place. Focus on building the mobile interface for best results.

