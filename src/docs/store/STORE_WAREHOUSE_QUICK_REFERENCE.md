# 🚀 Store Warehouse Modules - Quick Reference Guide

## ✅ What Was Fixed

Applied best practices from Accounting module review to all 19 Store warehouse modules.

---

## 📝 Pattern Examples

### ✅ CORRECT: Update Command (Property-Based)
```csharp
public record UpdateWarehouseCommand : IRequest<UpdateWarehouseResponse>
{
    public DefaultIdType Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
}
```

### ✅ CORRECT: Get Request (Positional OK for Reads)
```csharp
public record GetWarehouseRequest(DefaultIdType Id) : IRequest<WarehouseResponse>;
```

### ✅ CORRECT: Search Request (Property-Based for Complex Filters)
```csharp
public class SearchWarehousesRequest : PaginationFilter, IRequest<PagedList<WarehouseResponse>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public bool? IsActive { get; set; }
}
```

### ✅ CORRECT: Update Endpoint (ID from URL)
```csharp
.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseCommand request, ISender sender) =>
{
    var command = request with { Id = id };
    var result = await sender.Send(command).ConfigureAwait(false);
    return Results.Ok(result);
})
```

### ✅ CORRECT: Get Endpoint
```csharp
.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
{
    var result = await sender.Send(new GetWarehouseRequest(id)).ConfigureAwait(false);
    return Results.Ok(result);
})
```

---

## ❌ What NOT to Do

### ❌ WRONG: Positional Parameters in Commands
```csharp
// DON'T DO THIS
public record UpdateWarehouseCommand(
    DefaultIdType Id,
    string Name
) : IRequest<UpdateWarehouseResponse>;
```
**Why?** NSwag cannot properly generate client code for positional parameters.

### ❌ WRONG: ID Validation in Endpoints
```csharp
// DON'T DO THIS
if (id != command.Id) return Results.BadRequest("ID mismatch");
```
**Why?** ID should come from URL only. Use `request with { Id = id }` pattern.

### ❌ WRONG: "Command" for Read Operations
```csharp
// DON'T DO THIS
public class SearchWarehousesCommand : IRequest<PagedList<WarehouseResponse>>
```
**Why?** Use "Request" for read operations to follow CQRS naming conventions.

---

## 🎯 Quick Decision Tree

```
Is this a WRITE operation (Create, Update, Delete)?
├─ YES → Use "Command"
│   └─ Use property-based syntax with { get; init; }
│
└─ NO → Is this a READ operation (Get, Search, List)?
    └─ YES → Use "Request"
        ├─ Simple (single ID)? → Use positional parameter OK
        └─ Complex (filters)? → Use property-based syntax
```

---

## 📦 Files to Update When Creating New Operations

### For Update Operations:
1. **Command** - `UpdateXCommand.cs` (property-based)
2. **Validator** - `UpdateXCommandValidator.cs`
3. **Handler** - `UpdateXHandler.cs`
4. **Response** - `UpdateXResponse.cs`
5. **Endpoint** - `UpdateXEndpoint.cs` (use `with { Id = id }`)
6. **Registration** - Add to module endpoints file

### For Get Operations:
1. **Request** - `GetXRequest.cs` (positional OK)
2. **Handler** - `GetXHandler.cs`
3. **Response** - `XResponse.cs`
4. **Spec** - `GetXSpec.cs`
5. **Endpoint** - `GetXEndpoint.cs`
6. **Registration** - Add to module endpoints file

---

## 🔍 Review Checklist

When reviewing Store module code, check:

- [ ] Commands use property-based syntax (`{ get; init; }`)
- [ ] Reads use "Request" naming
- [ ] Endpoints set ID from URL (`with { Id = id }`)
- [ ] No ID validation in endpoints
- [ ] XML documentation on all public members
- [ ] Proper error handling
- [ ] Appropriate permissions required

---

## 📚 Related Documentation

- `STORE_WAREHOUSE_BEST_PRACTICES_COMPLETE.md` - Full summary
- `STORE_WAREHOUSE_BEST_PRACTICES_REVIEW.md` - Detailed tracking
- `CQRS_IMPLEMENTATION_CHECKLIST.md` - General CQRS guidelines

---

**Last Updated:** November 9, 2025

