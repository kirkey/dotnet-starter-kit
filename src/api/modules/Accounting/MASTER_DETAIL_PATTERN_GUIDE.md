# Master-Detail Entity Pattern - Quick Reference

## Overview

This guide provides a quick reference for implementing master-detail relationships in domain entities, based on the Budget-BudgetDetail reference pattern.

---

## When to Use This Pattern

Use the master-detail pattern when:
- ✅ An entity has a collection of related sub-entities (lines, items, details)
- ✅ The detail entities are meaningless without the master
- ✅ You need aggregate root behavior with transactional consistency
- ✅ The master calculates totals or aggregates from details

Examples:
- Budget → BudgetDetails (budget lines per account)
- Invoice → InvoiceLineItems (charges and line items)
- Bill → BillLineItems (vendor bill line items)
- JournalEntry → JournalEntryLines (debit/credit lines)
- Payment → PaymentAllocations (payment splits to invoices)

---

## Master Entity Template

```csharp
namespace YourModule.Domain.Entities;

/// <summary>
/// Represents [master entity description].
/// </summary>
/// <remarks>
/// Use cases:
/// - [Primary use case]
/// - [Secondary use case]
/// 
/// Default values:
/// - [Property]: [default value and description]
/// 
/// Business rules:
/// - [Critical business rule]
/// - [Validation rule]
/// </remarks>
public class Master : AuditableEntity, IAggregateRoot
{
    // Properties
    public string Name { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = "Draft";
    
    // Detail collection - REQUIRED
    private readonly List<Detail> _details = new();
    /// <summary>
    /// Collection of detail entities.
    /// </summary>
    public IReadOnlyCollection<Detail> Details => _details.AsReadOnly();
    
    // EF Core constructor
    private Master()
    {
        Name = string.Empty;
        Status = "Draft";
    }
    
    // Private constructor with parameters
    private Master(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");
        
        Name = name.Trim();
        Description = description?.Trim();
        Status = "Draft";
        TotalAmount = 0;
        
        // Queue domain event
        QueueDomainEvent(new MasterCreated(Id, Name));
    }
    
    /// <summary>
    /// Factory method to create a new master entity.
    /// </summary>
    public static Master Create(string name, string? description = null)
    {
        return new Master(name, description);
    }
    
    /// <summary>
    /// Update master properties.
    /// </summary>
    public Master Update(string? name, string? description, string? status)
    {
        bool isUpdated = false;
        
        if (!string.IsNullOrWhiteSpace(name) && Name != name)
        {
            Name = name.Trim();
            isUpdated = true;
        }
        
        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(status) && Status != status)
        {
            Status = status.Trim();
            isUpdated = true;
        }
        
        if (isUpdated)
        {
            QueueDomainEvent(new MasterUpdated(this));
        }
        
        return this;
    }
    
    /// <summary>
    /// Business method to update aggregate totals from details.
    /// Called by application layer after detail changes.
    /// </summary>
    public Master UpdateTotals(decimal totalAmount)
    {
        TotalAmount = totalAmount;
        return this;
    }
}
```

---

## Detail Entity Template

```csharp
namespace YourModule.Domain.Entities;

/// <summary>
/// Represents a single detail item within [master entity].
/// </summary>
/// <remarks>
/// Use cases:
/// - [Primary use case]
/// - [Detail-specific use case]
/// 
/// Default values:
/// - [Property]: [default value and description]
/// 
/// Business rules:
/// - [Validation rule]
/// - [Relationship rule]
/// </remarks>
public class Detail : AuditableEntity, IAggregateRoot
{
    private const int MaxDescriptionLength = 500;
    
    /// <summary>
    /// Parent master identifier - REQUIRED
    /// </summary>
    public DefaultIdType MasterId { get; private set; }
    
    /// <summary>
    /// Description of the detail item.
    /// </summary>
    public new string Description { get; private set; } = string.Empty;
    
    /// <summary>
    /// Amount for this detail item.
    /// </summary>
    public decimal Amount { get; private set; }
    
    /// <summary>
    /// Optional foreign key to related entity.
    /// </summary>
    public DefaultIdType? RelatedEntityId { get; private set; }
    
    // EF Core constructor
    private Detail()
    {
    }
    
    // Private constructor with parameters
    private Detail(DefaultIdType masterId, string description, decimal amount, DefaultIdType? relatedEntityId = null)
    {
        if (masterId == default)
            throw new ArgumentException("MasterId is required", nameof(masterId));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));
        
        var desc = description.Trim();
        if (desc.Length > MaxDescriptionLength)
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters");
        
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        
        MasterId = masterId;
        Description = desc;
        Amount = amount;
        RelatedEntityId = relatedEntityId;
    }
    
    /// <summary>
    /// Factory method to create a new detail entity.
    /// </summary>
    public static Detail Create(DefaultIdType masterId, string description, decimal amount, DefaultIdType? relatedEntityId = null)
    {
        return new Detail(masterId, description, amount, relatedEntityId);
    }
    
    /// <summary>
    /// Update detail properties.
    /// </summary>
    public Detail Update(string? description, decimal? amount, DefaultIdType? relatedEntityId)
    {
        bool isUpdated = false;
        
        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            var desc = description.Trim();
            if (desc.Length > MaxDescriptionLength)
                throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters");
            Description = desc;
            isUpdated = true;
        }
        
        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value < 0)
                throw new ArgumentException("Amount cannot be negative");
            Amount = amount.Value;
            isUpdated = true;
        }
        
        if (relatedEntityId != RelatedEntityId)
        {
            RelatedEntityId = relatedEntityId;
            isUpdated = true;
        }
        
        return this;
    }
}
```

---

## Checklist

### Master Entity Requirements
- [ ] Inherits from `AuditableEntity`
- [ ] Implements `IAggregateRoot`
- [ ] Has `private readonly List<Detail> _details = new();`
- [ ] Has `public IReadOnlyCollection<Detail> Details => _details.AsReadOnly();`
- [ ] Has private parameterless constructor for EF Core
- [ ] Has private constructor with parameters
- [ ] Has `static Create()` factory method
- [ ] Has `Update()` method for business logic
- [ ] Queues domain events for lifecycle changes
- [ ] Has comprehensive XML documentation

### Detail Entity Requirements
- [ ] In **separate file** (not nested in master)
- [ ] Inherits from `AuditableEntity`
- [ ] Implements `IAggregateRoot`
- [ ] Has `DefaultIdType MasterId { get; private set; }` property
- [ ] Uses `new` keyword for `Description` if needed
- [ ] Has private parameterless constructor for EF Core
- [ ] Has private constructor with parameters
- [ ] Has `static Create()` factory method
- [ ] Has `Update()` method for business logic
- [ ] Has comprehensive XML documentation
- [ ] Validates all inputs in constructor and Update method

---

## Common Patterns

### 1. Recalculating Totals

**Master entity method:**
```csharp
public Master RecalculateTotals()
{
    TotalAmount = _details.Sum(d => d.Amount);
    return this;
}
```

**Or via application layer:**
```csharp
public Master UpdateTotals(decimal totalAmount)
{
    TotalAmount = totalAmount;
    QueueDomainEvent(new MasterTotalsUpdated(Id, TotalAmount));
    return this;
}
```

### 2. Adding Detail Items

**Option A: Via application layer (recommended)**
```csharp
// In command handler
var master = await repository.GetByIdAsync(request.MasterId);
var detail = Detail.Create(master.Id, request.Description, request.Amount);
await detailRepository.AddAsync(detail);
master.UpdateTotals(/* recalculated total */);
```

**Option B: Via master entity**
```csharp
public Master AddDetail(string description, decimal amount, DefaultIdType? relatedEntityId = null)
{
    var detail = Detail.Create(Id, description, amount, relatedEntityId);
    _details.Add(detail);
    RecalculateTotals();
    QueueDomainEvent(new DetailAdded(Id, detail.Id));
    return this;
}
```

### 3. Preventing Modification

```csharp
public Master Update(...)
{
    if (Status is "Approved" or "Posted")
        throw new MasterCannotBeModifiedException(Id);
    
    // ... update logic
}
```

### 4. Cascade Operations

```csharp
public Master Delete()
{
    if (_details.Any())
        throw new MasterHasDetailsException(Id);
    
    IsDeleted = true;
    QueueDomainEvent(new MasterDeleted(Id));
    return this;
}
```

---

## Naming Conventions

### Master-Detail Pairs
| Master | Detail | ✅ Good | ❌ Bad |
|--------|--------|---------|--------|
| Budget | BudgetDetail | ✅ | BudgetLine |
| Invoice | InvoiceLineItem | ✅ | InvoiceLine |
| Bill | BillLineItem | ✅ | BillItem |
| JournalEntry | JournalEntryLine | ✅ | JournalLine |
| Order | OrderLineItem | ✅ | OrderItem |

### Properties
- Master collection: `Details`, `Lines`, `LineItems`, `Items`
- Detail foreign key: `{Master}Id` (e.g., `BudgetId`, `InvoiceId`)
- Backing field: `_details`, `_lines`, `_lineItems`, `_items`

---

## EF Core Configuration

### Master Entity Configuration
```csharp
public class MasterConfiguration : IEntityTypeConfiguration<Master>
{
    public void Configure(EntityTypeBuilder<Master> builder)
    {
        builder.ToTable("Masters", "module");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        // Configure one-to-many relationship
        builder.HasMany(x => x.Details)
            .WithOne()
            .HasForeignKey(d => d.MasterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### Detail Entity Configuration
```csharp
public class DetailConfiguration : IEntityTypeConfiguration<Detail>
{
    public void Configure(EntityTypeBuilder<Detail> builder)
    {
        builder.ToTable("Details", "module");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.MasterId)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)");
        
        // Index for foreign key
        builder.HasIndex(x => x.MasterId);
    }
}
```

---

## Application Layer Usage

### Creating Master with Details

```csharp
public class CreateMasterCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public List<CreateDetailRequest> Details { get; set; }
}

public class CreateMasterHandler : ICommandHandler<CreateMasterCommand, Guid>
{
    public async Task<Guid> Handle(CreateMasterCommand request, CancellationToken ct)
    {
        // Create master
        var master = Master.Create(request.Name);
        await _masterRepository.AddAsync(master, ct);
        
        // Create details
        decimal totalAmount = 0;
        foreach (var detailRequest in request.Details)
        {
            var detail = Detail.Create(master.Id, detailRequest.Description, detailRequest.Amount);
            await _detailRepository.AddAsync(detail, ct);
            totalAmount += detail.Amount;
        }
        
        // Update master totals
        master.UpdateTotals(totalAmount);
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        return master.Id;
    }
}
```

### Updating Details

```csharp
public class UpdateDetailCommand : ICommand<Unit>
{
    public Guid DetailId { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
}

public class UpdateDetailHandler : ICommandHandler<UpdateDetailCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDetailCommand request, CancellationToken ct)
    {
        var detail = await _detailRepository.GetByIdAsync(request.DetailId, ct);
        
        detail.Update(request.Description, request.Amount, null);
        
        // Recalculate master totals
        var details = await _detailRepository.GetByMasterIdAsync(detail.MasterId, ct);
        var totalAmount = details.Sum(d => d.Amount);
        
        var master = await _masterRepository.GetByIdAsync(detail.MasterId, ct);
        master.UpdateTotals(totalAmount);
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}
```

---

## Testing Examples

### Unit Test - Detail Creation

```csharp
[Fact]
public void Create_ValidDetail_ShouldSucceed()
{
    // Arrange
    var masterId = Guid.NewGuid();
    var description = "Test Item";
    var amount = 100.00m;
    
    // Act
    var detail = Detail.Create(masterId, description, amount);
    
    // Assert
    Assert.Equal(masterId, detail.MasterId);
    Assert.Equal(description, detail.Description);
    Assert.Equal(amount, detail.Amount);
}

[Fact]
public void Create_InvalidAmount_ShouldThrow()
{
    // Arrange
    var masterId = Guid.NewGuid();
    var description = "Test Item";
    var amount = -100.00m;
    
    // Act & Assert
    Assert.Throws<ArgumentException>(() => 
        Detail.Create(masterId, description, amount));
}
```

---

## Best Practices

### ✅ DO
- Keep master and detail in separate files
- Use factory methods (Create) for instantiation
- Validate in constructors and Update methods
- Use private setters for properties
- Document use cases and business rules
- Queue domain events for important changes
- Use IReadOnlyCollection for exposing collections

### ❌ DON'T
- Don't use nested classes for detail entities
- Don't expose List<T> directly
- Don't allow invalid state in entities
- Don't put application logic in entities
- Don't use public setters
- Don't forget to recalculate master totals

---

## Migration from Nested Classes

If you have existing nested detail classes:

1. **Create new file** for detail entity
2. **Copy class definition** to new file
3. **Add inheritance**: `AuditableEntity, IAggregateRoot`
4. **Add MasterId property**
5. **Add Update method**
6. **Fix Description property** with `new` keyword if needed
7. **Remove nested class** from master file
8. **Update master methods** to pass MasterId to Create
9. **Update application layer** code
10. **Run and fix compilation errors**

---

## References

- Budget-BudgetDetail: Reference implementation
- Bill-BillLineItem: Example implementation
- Invoice-InvoiceLineItem: Example implementation
- JournalEntry-JournalEntryLine: Example implementation
- Payment-PaymentAllocation: Example implementation

For detailed implementation, see: [MASTER_DETAIL_IMPLEMENTATION_COMPLETE.md](./MASTER_DETAIL_IMPLEMENTATION_COMPLETE.md)

