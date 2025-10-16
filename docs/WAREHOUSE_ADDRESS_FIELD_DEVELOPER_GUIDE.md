# Warehouse Address Field Consolidation - Developer Quick Reference

## 🎯 What Changed?

The Warehouse entity has been simplified by removing separate `City`, `State`, `Country`, and `PostalCode` properties and consolidating them into a single `Address` property.

### Old vs New Format

| Aspect | Old | New |
|--------|-----|-----|
| Address Field | `Address` (street only) | `Address` (complete) |
| Separate City | `City` | ❌ Removed |
| Separate State | `State` | ❌ Removed |
| Separate Country | `Country` | ❌ Removed |
| Separate PostalCode | `PostalCode` | ❌ Removed |
| Max Length | 500 chars | 500 chars |
| Required | Yes | Yes |

---

## 📝 API Changes

### Creating a Warehouse

#### Before
```json
POST /api/warehouses
{
  "code": "WH-001",
  "name": "Main Warehouse",
  "address": "123 Industrial Blvd",
  "city": "Seattle",
  "state": "WA",
  "country": "USA",
  "postalCode": "98101",
  "managerName": "John Smith",
  "managerEmail": "john@warehouse.com",
  "managerPhone": "+1-555-0123",
  "totalCapacity": 50000,
  "capacityUnit": "sqft",
  "warehouseType": "Standard",
  "isActive": true,
  "isMainWarehouse": false
}
```

#### After
```json
POST /api/warehouses
{
  "code": "WH-001",
  "name": "Main Warehouse",
  "address": "123 Industrial Blvd, Seattle, WA 98101, USA",
  "managerName": "John Smith",
  "managerEmail": "john@warehouse.com",
  "managerPhone": "+1-555-0123",
  "totalCapacity": 50000,
  "capacityUnit": "sqft",
  "warehouseType": "Standard",
  "isActive": true,
  "isMainWarehouse": false
}
```

### Updating a Warehouse

#### Before
```json
PATCH /api/warehouses/{id}
{
  "address": "456 New Industrial Park",
  "city": "Portland",
  "state": "OR",
  "country": "USA",
  "postalCode": "97201"
}
```

#### After
```json
PATCH /api/warehouses/{id}
{
  "address": "456 New Industrial Park, Portland, OR 97201, USA"
}
```

### Getting a Warehouse

#### Before
```json
GET /api/warehouses/{id}
{
  "id": "...",
  "code": "WH-001",
  "name": "Main Warehouse",
  "address": "123 Industrial Blvd",
  "city": "Seattle",
  "state": "WA",
  "country": "USA",
  "postalCode": "98101",
  ...
}
```

#### After
```json
GET /api/warehouses/{id}
{
  "id": "...",
  "code": "WH-001",
  "name": "Main Warehouse",
  "address": "123 Industrial Blvd, Seattle, WA 98101, USA",
  ...
}
```

---

## 💻 C# Code Changes

### Domain Model Usage

#### Creating a Warehouse
```csharp
// OLD:
var warehouse = Warehouse.Create(
    name: "Main Warehouse",
    description: "Primary storage facility",
    code: "WH-001",
    address: "123 Industrial Blvd",
    city: "Seattle",
    state: "WA",
    country: "USA",
    postalCode: "98101",
    managerName: "John Smith",
    managerEmail: "john@example.com",
    managerPhone: "+1-555-0123",
    totalCapacity: 50000,
    capacityUnit: "sqft"
);

// NEW:
var warehouse = Warehouse.Create(
    name: "Main Warehouse",
    description: "Primary storage facility",
    code: "WH-001",
    address: "123 Industrial Blvd, Seattle, WA 98101, USA",
    managerName: "John Smith",
    managerEmail: "john@example.com",
    managerPhone: "+1-555-0123",
    totalCapacity: 50000,
    capacityUnit: "sqft"
);
```

#### Updating a Warehouse
```csharp
// OLD:
warehouse.Update(
    name: "Main Warehouse",
    address: "456 New Park",
    city: "Portland",
    state: "OR",
    country: "USA",
    postalCode: "97201",
    ...
);

// NEW:
warehouse.Update(
    name: "Main Warehouse",
    address: "456 New Park, Portland, OR 97201, USA",
    ...
);
```

#### Accessing Address
```csharp
// OLD:
var fullAddress = $"{warehouse.Address}, {warehouse.City}, {warehouse.State} {warehouse.Country} {warehouse.PostalCode}";

// NEW:
var fullAddress = warehouse.Address; // Already complete
```

---

## 🎨 Blazor/UI Changes

### Warehouse Form

#### Before
```html
<section>
  <h3>Address Information</h3>
  <MudTextField @bind-Value="model.Address" Label="Street Address" />
  <MudTextField @bind-Value="model.City" Label="City" />
  <MudTextField @bind-Value="model.State" Label="State/Province" />
  <MudTextField @bind-Value="model.Country" Label="Country" />
  <MudTextField @bind-Value="model.PostalCode" Label="Postal Code" />
</section>
```

#### After
```html
<section>
  <h3>Address Information</h3>
  <MudTextField @bind-Value="model.Address" Label="Complete Address" 
                Placeholder="e.g., 123 Blvd, Seattle, WA 98101, USA" />
</section>
```

---

## 📋 Validation Rules

### Address Field Rules
- **Required:** Yes
- **Max Length:** 500 characters
- **Format:** Complete address including street, city, state, country, postal code

### Address Format Recommendations
Suggested format for consistency:
```
{street address}, {city}, {state/province} {postal code}, {country}
```

Examples:
- `123 Industrial Blvd, Seattle, WA 98101, USA`
- `456 Commercial Drive, Toronto, ON M1A 1E1, Canada`
- `789 Business Park, London, SW1A 1AA, United Kingdom`

---

## 🔍 SQL Query Changes

### Before
```sql
SELECT Code, Name, Address, City, Country, ManagerName 
FROM Warehouses 
WHERE City = 'Seattle' AND Country = 'USA'
```

### After
```sql
SELECT Code, Name, Address, ManagerName 
FROM Warehouses 
WHERE Address LIKE '%Seattle%' AND Address LIKE '%USA%'
-- Or use TSQUERY/FTS if available
```

---

## 📋 Validation Changes

### CreateWarehouseCommand Validator

**Removed Validations:**
- ❌ `City` - Required, Max 100 chars
- ❌ `State` - Max 100 chars (optional)
- ❌ `Country` - Required, Max 100 chars
- ❌ `PostalCode` - Max 20 chars (optional)

**Retained Validations:**
- ✅ `Address` - Required, Max 500 chars

### UpdateWarehouseCommand Validator

Same changes as CreateWarehouseCommand validator

---

## 🧪 Test Updates

### Unit Tests
```csharp
// OLD:
[Fact]
public void Create_WithValidData_ShouldSucceed()
{
    var warehouse = Warehouse.Create(
        name: "Test",
        code: "WH-TEST",
        address: "123 Test St",
        city: "TestCity",
        state: "TS",
        country: "TC",
        postalCode: "12345",
        managerName: "Test Manager",
        managerEmail: "test@example.com",
        managerPhone: "+1-555-0000",
        totalCapacity: 1000
    );
    Assert.NotNull(warehouse);
}

// NEW:
[Fact]
public void Create_WithValidData_ShouldSucceed()
{
    var warehouse = Warehouse.Create(
        name: "Test",
        code: "WH-TEST",
        address: "123 Test St, TestCity, TS 12345, TC",
        managerName: "Test Manager",
        managerEmail: "test@example.com",
        managerPhone: "+1-555-0000",
        totalCapacity: 1000
    );
    Assert.NotNull(warehouse);
}
```

---

## 🚀 Migration Checklist

For migrating existing code:

- [ ] Update all `Warehouse.Create()` calls to use consolidated address
- [ ] Update all `warehouse.Update()` calls to remove location parameters
- [ ] Remove `City`, `State`, `Country`, `PostalCode` property access
- [ ] Update all form bindings in Blazor components
- [ ] Update all API client libraries
- [ ] Update unit tests for warehouse creation/update
- [ ] Update integration tests
- [ ] Update any reporting/query logic accessing these fields
- [ ] Update database migration scripts
- [ ] Update API documentation

---

## ❓ FAQ

### Q: How do I search by city now?
**A:** Parse the address field using string matching or regex:
```csharp
var warehouses = context.Warehouses
    .Where(w => w.Address.Contains("Seattle"))
    .ToList();
```

### Q: What about international addresses?
**A:** The consolidated format is more flexible and supports any format. Just ensure the complete address fits in 500 characters.

### Q: Do I need to update my database?
**A:** Yes. You'll need to migrate the data by combining the four location columns into one Address column.

### Q: What if I need to extract city/state from the address later?
**A:** You would need to parse the address string. Consider adding a helper method if this is needed frequently.

### Q: Is this a breaking change?
**A:** Yes. Any code or API clients sending the old format with separate location fields will fail validation.

---

## 📞 Support

For questions or issues related to this refactoring, refer to:
- `WAREHOUSE_REFACTORING_COMPLETE.md` - Comprehensive implementation details
- `WAREHOUSE_REFACTORING_CHECKLIST.md` - Verification checklist
- Domain tests for usage examples
