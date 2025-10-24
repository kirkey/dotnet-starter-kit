# Store Domain Exceptions Summary

This document provides a comprehensive overview of all domain exceptions created for the Store Module's Inventory and Warehouse Management System.

## Overview

All exception classes follow the FSH Framework exception pattern using primary constructors and inheriting from framework base exceptions:
- `NotFoundException` - For resource not found scenarios (404)
- `ConflictException` - For duplicate/conflict scenarios (409)
- `BadRequestException` - For business rule violations (400)

## Exception Hierarchy

```
FSH.Framework.Core.Exceptions.FshException
├── NotFoundException (404)
├── ConflictException (409)
└── BadRequestException (400)
```

## Exception Catalog

### 1. Item Exceptions
**Namespace:** `Store.Domain.Exceptions.Item`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `ItemNotFoundException` | NotFoundException | Item not found by ID or SKU |
| `DuplicateItemSkuException` | ConflictException | Attempt to create item with existing SKU |
| `DuplicateItemBarcodeException` | ConflictException | Attempt to create item with existing barcode |
| `InvalidItemStockLevelException` | BadRequestException | Invalid stock level parameters (e.g., MinStock > MaxStock) |
| `ItemCannotBeDeletedException` | BadRequestException | Item deletion blocked due to dependencies |

**Example Usage:**
```csharp
// In Application layer handler
var item = await repository.GetByIdAsync(itemId, cancellationToken) 
    ?? throw new ItemNotFoundException(itemId);

// In Domain entity
if (minStock > maxStock)
    throw new InvalidItemStockLevelException("Minimum stock cannot exceed maximum stock");
```

---

### 2. StockLevel Exceptions
**Namespace:** `Store.Domain.Exceptions.StockLevel`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `StockLevelNotFoundException` | NotFoundException | Stock level not found by ID or Item+Warehouse combination |
| `InsufficientStockException` | BadRequestException | Attempted operation exceeds available stock |
| `InvalidStockLevelOperationException` | BadRequestException | Invalid stock operation (e.g., negative quantity) |

**Example Usage:**
```csharp
// In StockLevel.DecreaseQuantity() method
if (quantity > QuantityAvailable)
    throw new InsufficientStockException(ItemId, WarehouseId, QuantityAvailable, quantity);

// In Application handler
var stockLevel = await repository.GetByItemAndWarehouseAsync(itemId, warehouseId, cancellationToken)
    ?? throw new StockLevelNotFoundException(itemId, warehouseId);
```

---

### 3. Bin Exceptions
**Namespace:** `Store.Domain.Exceptions.Bin`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `BinNotFoundException` | NotFoundException | Bin not found by ID or Code+Location |
| `DuplicateBinCodeException` | ConflictException | Bin code already exists in location |
| `BinCapacityExceededException` | BadRequestException | Operation would exceed bin capacity |
| `BinCannotBeDeactivatedException` | BadRequestException | Bin deactivation blocked (e.g., contains stock) |

**Example Usage:**
```csharp
// In Bin.UpdateUtilization() method
if (newUtilization > Capacity)
    throw new BinCapacityExceededException(Id, Capacity, newUtilization);

// In Application handler
var bin = await repository.GetByCodeAsync(code, warehouseLocationId, cancellationToken)
    ?? throw new BinNotFoundException(code, warehouseLocationId);
```

---

### 4. LotNumber Exceptions
**Namespace:** `Store.Domain.Exceptions.LotNumber`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `LotNumberNotFoundException` | NotFoundException | Lot not found by ID or LotCode+Item |
| `DuplicateLotNumberException` | ConflictException | Lot code already exists for item |
| `LotNumberExpiredException` | BadRequestException | Attempt to use expired lot |
| `InsufficientLotQuantityException` | BadRequestException | Lot quantity insufficient for operation |

**Example Usage:**
```csharp
// In LotNumber.UpdateQuantity() method
if (quantityToRemove > QuantityRemaining)
    throw new InsufficientLotQuantityException(LotCode, QuantityRemaining, quantityToRemove);

// In picking logic
if (lot.IsExpired())
    throw new LotNumberExpiredException(lot.LotCode, lot.ExpirationDate);
```

---

### 5. SerialNumber Exceptions
**Namespace:** `Store.Domain.Exceptions.SerialNumber`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `SerialNumberNotFoundException` | NotFoundException | Serial number not found by ID or value |
| `DuplicateSerialNumberException` | ConflictException | Serial number already exists |
| `InvalidSerialNumberStatusException` | BadRequestException | Invalid status transition or operation |

**Example Usage:**
```csharp
// In SerialNumber.UpdateStatus() method
if (Status == SerialNumberStatus.Sold && newStatus != SerialNumberStatus.Returned)
    throw new InvalidSerialNumberStatusException(SerialNumberValue, Status.ToString(), "Returned");

// In Application handler
var serial = await repository.GetByValueAsync(serialValue, cancellationToken)
    ?? throw new SerialNumberNotFoundException(serialValue);
```

---

### 6. PickList Exceptions
**Namespace:** `Store.Domain.Exceptions.PickList`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `PickListNotFoundException` | NotFoundException | Pick list not found by ID or number |
| `PickListCannotBeModifiedException` | BadRequestException | Modification blocked due to status |
| `InvalidPickListStatusException` | BadRequestException | Invalid status transition |

**Example Usage:**
```csharp
// In PickList.AddItem() method
if (Status != PickListStatus.Created)
    throw new PickListCannotBeModifiedException(Id, Status.ToString());

// In status transition validation
if (currentStatus == PickListStatus.Completed && newStatus == PickListStatus.Created)
    throw new InvalidPickListStatusException(currentStatus.ToString(), newStatus.ToString());
```

---

### 7. PutAwayTask Exceptions
**Namespace:** `Store.Domain.Exceptions.PutAwayTask`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `PutAwayTaskNotFoundException` | NotFoundException | Put-away task not found by ID or number |
| `PutAwayTaskCannotBeModifiedException` | BadRequestException | Modification blocked due to status |
| `InvalidPutAwayTaskStatusException` | BadRequestException | Invalid status transition |

**Example Usage:**
```csharp
// In PutAwayTask.AddItem() method
if (Status != PutAwayTaskStatus.Created)
    throw new PutAwayTaskCannotBeModifiedException(Id, Status.ToString());

// In Application handler
var task = await repository.GetByNumberAsync(taskNumber, cancellationToken)
    ?? throw new PutAwayTaskNotFoundException(taskNumber);
```

---

### 8. ItemSupplier Exceptions
**Namespace:** `Store.Domain.Exceptions.ItemSupplier`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `ItemSupplierNotFoundException` | NotFoundException | Item-supplier relationship not found |
| `DuplicateItemSupplierException` | ConflictException | Item-supplier relationship already exists |

**Example Usage:**
```csharp
// In Application handler
var itemSupplier = await repository.GetByItemAndSupplierAsync(itemId, supplierId, cancellationToken)
    ?? throw new ItemSupplierNotFoundException(itemId, supplierId);

// In domain service
if (await repository.ExistsAsync(itemId, supplierId, cancellationToken))
    throw new DuplicateItemSupplierException(itemId, supplierId);
```

---

### 9. InventoryReservation Exceptions
**Namespace:** `Store.Domain.Exceptions.InventoryReservation`

| Exception Class | Base Type | Use Case |
|----------------|-----------|----------|
| `InventoryReservationNotFoundException` | NotFoundException | Reservation not found by ID or number |
| `InventoryReservationCannotBeModifiedException` | BadRequestException | Modification blocked due to status |
| `InvalidInventoryReservationStatusException` | BadRequestException | Invalid status transition |
| `InsufficientInventoryForReservationException` | BadRequestException | Insufficient inventory to reserve |

**Example Usage:**
```csharp
// In reservation creation
if (availableQuantity < requestedQuantity)
    throw new InsufficientInventoryForReservationException(itemId, availableQuantity, requestedQuantity);

// In InventoryReservation.Allocate() method
if (Status != ReservationStatus.Active)
    throw new InventoryReservationCannotBeModifiedException(Id, Status.ToString());
```

---

## Exception Handling Pattern

### Application Layer (Command/Query Handlers)

```csharp
public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto>
{
    public async Task<ItemDto> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        // Framework automatically converts exceptions to appropriate HTTP responses
        var item = await _repository.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new ItemNotFoundException(request.ItemId);
            
        return _mapper.Map<ItemDto>(item);
    }
}
```

### Domain Layer (Entity Methods)

```csharp
public class StockLevel : AuditableEntity<DefaultIdType>, IAggregateRoot
{
    public StockLevel DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidStockLevelOperationException("Quantity must be greater than zero");
            
        if (quantity > QuantityAvailable)
            throw new InsufficientStockException(ItemId, WarehouseId, QuantityAvailable, quantity);
            
        QuantityOnHand -= quantity;
        QuantityAvailable -= quantity;
        LastMovementDate = DateTime.UtcNow;
        
        QueueDomainEvent(new StockLevelDecreased { StockLevel = this });
        return this;
    }
}
```

### API Response Mapping

The FSH Framework automatically maps exceptions to HTTP responses:

| Exception Type | HTTP Status Code | Response Body |
|---------------|------------------|---------------|
| `NotFoundException` | 404 Not Found | `{ "message": "Item with ID 'xxx' was not found." }` |
| `ConflictException` | 409 Conflict | `{ "message": "An item with SKU 'ABC123' already exists." }` |
| `BadRequestException` | 400 Bad Request | `{ "message": "Insufficient stock..." }` |

---

## Best Practices

### 1. **Use Specific Exceptions**
```csharp
// ✅ GOOD: Specific exception with context
throw new ItemNotFoundException(itemId);

// ❌ BAD: Generic exception
throw new Exception("Item not found");
```

### 2. **Validate at Entry Points**
```csharp
// Domain entity constructor
private Item(DefaultIdType id, string sku, string name, ...)
{
    if (string.IsNullOrWhiteSpace(sku))
        throw new ArgumentException("SKU is required", nameof(sku));
        
    if (unitPrice < cost)
        throw new InvalidItemStockLevelException("Unit price cannot be less than cost");
}
```

### 3. **Check Before Operating**
```csharp
// Before decreasing stock
public StockLevel DecreaseQuantity(int quantity)
{
    if (quantity > QuantityAvailable)
        throw new InsufficientStockException(ItemId, WarehouseId, QuantityAvailable, quantity);
    // ... proceed with operation
}
```

### 4. **Use Descriptive Messages**
```csharp
// ✅ GOOD: Descriptive with context
throw new InvalidPickListStatusException(
    currentStatus.ToString(), 
    attemptedStatus.ToString()
);

// ❌ BAD: Vague message
throw new BadRequestException("Invalid status");
```

### 5. **Handle Cascading Failures**
```csharp
public async Task DeleteItemAsync(DefaultIdType itemId)
{
    var hasStockLevels = await _stockLevelRepository.HasAnyAsync(itemId);
    if (hasStockLevels)
        throw new ItemCannotBeDeletedException(itemId, "Item has stock levels in warehouses");
        
    var hasPendingOrders = await _orderRepository.HasPendingOrdersAsync(itemId);
    if (hasPendingOrders)
        throw new ItemCannotBeDeletedException(itemId, "Item has pending purchase orders");
}
```

---

## Exception Testing

### Unit Test Example
```csharp
[Fact]
public void DecreaseQuantity_WithInsufficientStock_ThrowsException()
{
    // Arrange
    var stockLevel = StockLevel.Create(itemId, warehouseId, 100);
    
    // Act & Assert
    var exception = Assert.Throws<InsufficientStockException>(
        () => stockLevel.DecreaseQuantity(150)
    );
    
    Assert.Contains("Insufficient stock", exception.Message);
    Assert.Equal(100, exception.Data["available"]);
    Assert.Equal(150, exception.Data["required"]);
}
```

### Integration Test Example
```csharp
[Fact]
public async Task GetItem_WithInvalidId_ReturnsNotFound()
{
    // Arrange
    var invalidId = DefaultIdType.NewGuid();
    
    // Act
    var response = await _client.GetAsync($"/api/items/{invalidId}");
    
    // Assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("was not found", content);
}
```

---

## Summary

### Exception Count by Category
- **Item**: 5 exceptions
- **StockLevel**: 3 exceptions
- **Bin**: 4 exceptions
- **LotNumber**: 4 exceptions
- **SerialNumber**: 3 exceptions
- **PickList**: 3 exceptions
- **PutAwayTask**: 3 exceptions
- **ItemSupplier**: 2 exceptions
- **InventoryReservation**: 4 exceptions

**Total**: 31 domain-specific exceptions across 9 entity categories

### Exception Types Distribution
- **NotFoundException**: 12 exceptions (39%)
- **ConflictException**: 5 exceptions (16%)
- **BadRequestException**: 14 exceptions (45%)

All exceptions follow the FSH Framework pattern with primary constructors, providing clear, actionable error messages for API consumers and maintaining separation of concerns between domain, application, and presentation layers.
