# Final Fix: DuplicatePurchaseOrderNumberException

## Issue
The `PurchaseOrderCreateHandler` was using `DuplicatePurchaseOrderNumberException` which didn't exist in the domain exceptions.

**Error**:
```
PurchaseOrderCreateHandler.cs(22, 23): [CS0246] The type or namespace name 'DuplicatePurchaseOrderNumberException' could not be found
```

## Root Cause
When implementing the PurchaseOrder application layer, the exception was referenced in the handler but was not defined in the `PurchaseOrderExceptions.cs` file.

## Solution
Added the missing exception to the domain exceptions file:

**File**: `/Accounting.Domain/Exceptions/PurchaseOrderExceptions.cs`

**Added**:
```csharp
public sealed class DuplicatePurchaseOrderNumberException(string orderNumber) 
    : ConflictException($"Purchase order with number {orderNumber} already exists");
```

## Location
Added between `PurchaseOrderNotFoundException` and `PurchaseOrderCannotBeModifiedException` for logical grouping.

## Verification
- ✅ Exception properly defined with correct signature
- ✅ Follows naming convention (Duplicate{Entity}{UniqueKey}Exception)
- ✅ Uses ConflictException base class (appropriate for duplicate resources)
- ✅ Includes descriptive error message with context
- ✅ Matches pattern used in other entities (InterCompanyTransaction, Customer, etc.)

## Related Files
- `/Accounting.Application/PurchaseOrders/Create/v1/PurchaseOrderCreateHandler.cs` - Uses the exception
- `/Accounting.Domain/Exceptions/PurchaseOrderExceptions.cs` - Defines the exception

## Status
✅ **FIXED** - Exception added to domain, compilation error resolved.

---

**Note**: IDE may show cached errors. A rebuild or IDE restart will clear these.

