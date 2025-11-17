# Taxes Module - Implementation Plan & Status

**Date:** November 17, 2025  
**Module:** Taxes (HumanResources subsystem)  
**Purpose:** Tax master configuration features, workflows, application layers, configurations and endpoints

---

## 📊 Executive Summary

The Taxes module is partially implemented with tax bracket (income tax bracket) support but **lacks a comprehensive Tax Master entity** for general tax configuration (sales tax, VAT, excise tax, withholding taxes, etc.).

### Current Implementation Status:
- ✅ **Tax Brackets (Income Tax Brackets)** - Fully implemented (Domain + Application + Endpoints)
- ❌ **Tax Master** - Not implemented (needed for tax types configuration)
- ❌ **Taxes Endpoints** - Not created (only TaxBrackets endpoints exist)
- ⚠️  **Tax Application Layer** - Partially incomplete (Create/Search handlers exist but missing Get/Update/Delete patterns)

### Implementation Gaps:

| Component | Status | Details |
|-----------|--------|---------|
| **Domain Entity: TaxMaster** | ❌ Missing | Should define tax configuration (code, rate, jurisdiction, accounts) |
| **TaxBracket Entity** | ✅ Complete | Income tax bracket with min/max income and rates |
| **Create Handler** | ✅ Complete | CreateTaxCommand/CreateTaxBracketCommand with handlers |
| **Get Handler** | ⚠️  Partial | GetTaxHandler exists but might need refinement |
| **Update Handler** | ❌ Missing | UpdateTaxCommand/UpdateTaxBracketCommand handlers |
| **Delete Handler** | ❌ Missing | DeleteTaxCommand/DeleteTaxBracketCommand handlers |
| **Search Handler** | ✅ Complete | SearchTaxesHandler and SearchTaxBracketsHandler exist |
| **Validators** | ❌ Partial | CreateTaxValidator exists but Update/Delete validators missing |
| **Endpoints** | ❌ Missing | Taxes folder not created in Infrastructure/Endpoints |
| **DbContext** | ❌ Missing | Need to add TaxMaster DbSet (only TaxBrackets exist) |
| **Repository Registration** | ⚠️  Partial | TaxBrackets registered but missing TaxMaster registration |
| **Module Integration** | ⚠️  Partial | MapTaxBracketEndpoints mapped but no Taxes endpoints |

---

## 🎯 Implementation Roadmap

### Phase 1: Create Missing Domain Entity (Tax Master)

**File:** `/src/api/modules/HumanResources/HumanResources.Domain/Entities/TaxMaster.cs`

Create a comprehensive Tax entity for all tax types (not just income brackets):

```csharp
public class TaxMaster : AuditableEntity, IAggregateRoot
{
    // Properties:
    // - Code (unique tax identifier)
    // - Name
    // - TaxType (SalesTax, VAT, GST, Excise, Withholding, Property)
    // - Rate (0-1)
    // - IsCompound
    // - Jurisdiction
    // - EffectiveDate / ExpiryDate
    // - TaxCollectedAccountId / TaxPaidAccountId
    // - TaxAuthority
    // - TaxRegistrationNumber
    // - ReportingCategory
    // - IsActive
}
```

### Phase 2: Implement Application Layer Commands & Handlers

**Files to create:**

1. **Create Command**
   - `HumanResources.Application/Taxes/Create/v1/CreateTaxCommand.cs`
   - `HumanResources.Application/Taxes/Create/v1/CreateTaxValidator.cs`
   - `HumanResources.Application/Taxes/Create/v1/CreateTaxHandler.cs`
   - `HumanResources.Application/Taxes/Create/v1/CreateTaxResponse.cs`

2. **Update Command**
   - `HumanResources.Application/Taxes/Update/v1/UpdateTaxCommand.cs`
   - `HumanResources.Application/Taxes/Update/v1/UpdateTaxValidator.cs`
   - `HumanResources.Application/Taxes/Update/v1/UpdateTaxHandler.cs`

3. **Delete Command**
   - `HumanResources.Application/Taxes/Delete/v1/DeleteTaxCommand.cs`
   - `HumanResources.Application/Taxes/Delete/v1/DeleteTaxHandler.cs`

4. **Get Query** (verify existing implementation)
   - `HumanResources.Application/Taxes/Get/v1/GetTaxRequest.cs`
   - `HumanResources.Application/Taxes/Get/v1/GetTaxHandler.cs`
   - `HumanResources.Application/Taxes/Get/v1/TaxResponse.cs`

5. **Search Query** (verify existing implementation)
   - `HumanResources.Application/Taxes/Search/v1/SearchTaxesRequest.cs`
   - `HumanResources.Application/Taxes/Search/v1/SearchTaxesHandler.cs`

### Phase 3: Implement Infrastructure Layer Endpoints

**Folder:** `/src/api/modules/HumanResources/HumanResources.Infrastructure/Endpoints/Taxes/`

**Files to create:**

```
Endpoints/
├── Taxes/
    ├── TaxEndpoints.cs (coordinator)
    ├── CreateTaxEndpoint.cs
    ├── UpdateTaxEndpoint.cs
    ├── GetTaxEndpoint.cs
    ├── DeleteTaxEndpoint.cs
    ├── SearchTaxesEndpoint.cs
    └── v1/
        ├── CreateTaxEndpoint.cs (implementation)
        ├── UpdateTaxEndpoint.cs (implementation)
        ├── GetTaxEndpoint.cs (implementation)
        ├── DeleteTaxEndpoint.cs (implementation)
        └── SearchTaxesEndpoint.cs (implementation)
```

### Phase 4: Update Infrastructure Configuration

1. **DbContext** - Add TaxMaster DbSet
2. **HumanResourcesModule** - Register TaxMaster repository
3. **HumanResourcesModule.Endpoints** - Map Taxes endpoints

### Phase 5: Create Migration

**File:** `migrations/postgresql/HumanResources_AddTaxMaster.sql`

---

## 📋 Detailed Implementation

### Step 1: Domain Entity - TaxMaster

Follow the pattern from `TaxBracket` and `Accounting.TaxCode`:

```csharp
namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

public class TaxMaster : AuditableEntity, IAggregateRoot
{
    // Private constructor for EF Core
    private TaxMaster() { }

    // Private constructor for factory method
    private TaxMaster(
        string code,
        string name,
        string taxType,
        decimal rate,
        DefaultIdType taxCollectedAccountId)
    {
        Code = code;
        Name = name;
        TaxType = taxType;
        Rate = rate;
        TaxCollectedAccountId = taxCollectedAccountId;
        IsActive = true;
        EffectiveDate = DateTime.UtcNow;
    }

    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string TaxType { get; private set; } = default!;
    public decimal Rate { get; private set; }
    public bool IsCompound { get; private set; }
    public string? Jurisdiction { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public DefaultIdType TaxCollectedAccountId { get; private set; }
    public DefaultIdType? TaxPaidAccountId { get; private set; }
    public string? TaxAuthority { get; private set; }
    public string? TaxRegistrationNumber { get; private set; }
    public string? ReportingCategory { get; private set; }
    public bool IsActive { get; private set; }

    // Factory method
    public static TaxMaster Create(
        string code,
        string name,
        string taxType,
        decimal rate,
        DefaultIdType taxCollectedAccountId,
        DateTime? effectiveDate = null,
        bool isCompound = false,
        string? jurisdiction = null,
        DateTime? expiryDate = null,
        DefaultIdType? taxPaidAccountId = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Tax code cannot be empty", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tax name cannot be empty", nameof(name));
        if (rate < 0 || rate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1", nameof(rate));
        if (expiryDate.HasValue && expiryDate <= effectiveDate)
            throw new ArgumentException("Expiry date must be after effective date", nameof(expiryDate));

        var tax = new TaxMaster(code, name, taxType, rate, taxCollectedAccountId)
        {
            IsCompound = isCompound,
            Jurisdiction = jurisdiction,
            EffectiveDate = effectiveDate ?? DateTime.UtcNow,
            ExpiryDate = expiryDate,
            TaxPaidAccountId = taxPaidAccountId,
            TaxAuthority = taxAuthority,
            TaxRegistrationNumber = taxRegistrationNumber,
            ReportingCategory = reportingCategory
        };

        return tax;
    }

    // Update method
    public TaxMaster Update(
        string? name = null,
        string? taxType = null,
        decimal? rate = null,
        bool? isCompound = null,
        string? jurisdiction = null,
        DateTime? expiryDate = null,
        DefaultIdType? taxPaidAccountId = null,
        string? taxAuthority = null,
        string? taxRegistrationNumber = null,
        string? reportingCategory = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
        if (!string.IsNullOrWhiteSpace(taxType))
            TaxType = taxType;
        if (rate.HasValue && (rate < 0 || rate > 1))
            throw new ArgumentException("Tax rate must be between 0 and 1", nameof(rate));
        if (rate.HasValue)
            Rate = rate.Value;
        if (isCompound.HasValue)
            IsCompound = isCompound.Value;
        if (jurisdiction != null)
            Jurisdiction = jurisdiction;
        if (expiryDate.HasValue && expiryDate <= EffectiveDate)
            throw new ArgumentException("Expiry date must be after effective date", nameof(expiryDate));
        if (expiryDate.HasValue)
            ExpiryDate = expiryDate;
        if (taxPaidAccountId.HasValue)
            TaxPaidAccountId = taxPaidAccountId;
        if (taxAuthority != null)
            TaxAuthority = taxAuthority;
        if (taxRegistrationNumber != null)
            TaxRegistrationNumber = taxRegistrationNumber;
        if (reportingCategory != null)
            ReportingCategory = reportingCategory;

        return this;
    }

    // Activate/Deactivate
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
```

### Step 2: Application Layer - Commands & Handlers

**Pattern Reference:** Follow Todo.Features.Create and Accounting.TaxCode patterns

#### CreateTaxCommand.cs
```csharp
namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

public sealed record CreateTaxCommand(
    [property: DefaultValue("VAT-STD")] string Code,
    [property: DefaultValue("Standard VAT")] string Name,
    [property: DefaultValue("VAT")] string TaxType,
    [property: DefaultValue(0.10)] decimal Rate,
    [property: DefaultValue(null)] DefaultIdType? TaxCollectedAccountId = null,
    [property: DefaultValue(null)] DateTime? EffectiveDate = null,
    [property: DefaultValue(false)] bool IsCompound = false,
    [property: DefaultValue(null)] string? Jurisdiction = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue(null)] DefaultIdType? TaxPaidAccountId = null,
    [property: DefaultValue(null)] string? TaxAuthority = null,
    [property: DefaultValue(null)] string? TaxRegistrationNumber = null,
    [property: DefaultValue(null)] string? ReportingCategory = null) : IRequest<CreateTaxResponse>;
```

#### CreateTaxValidator.cs
```csharp
namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

public sealed class CreateTaxValidator : AbstractValidator<CreateTaxCommand>
{
    public CreateTaxValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Tax code is required")
            .MaximumLength(50).WithMessage("Tax code cannot exceed 50 characters")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Tax code must be uppercase alphanumeric with hyphens/underscores only");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tax name is required")
            .MaximumLength(200).WithMessage("Tax name cannot exceed 200 characters");

        RuleFor(x => x.TaxType)
            .NotEmpty().WithMessage("Tax type is required")
            .Must(x => IsValidTaxType(x)).WithMessage("Invalid tax type");

        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Tax rate cannot be negative")
            .LessThanOrEqualTo(1).WithMessage("Tax rate cannot exceed 100%");

        RuleFor(x => x.TaxCollectedAccountId)
            .NotEmpty().WithMessage("Tax collected account is required");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.EffectiveDate).WithMessage("Expiry date must be after effective date")
            .When(x => x.ExpiryDate.HasValue && x.EffectiveDate.HasValue);
    }

    private static bool IsValidTaxType(string taxType)
    {
        var validTypes = new[] { "SalesTax", "VAT", "GST", "Excise", "Withholding", "Property", "Other" };
        return validTypes.Contains(taxType);
    }
}
```

#### CreateTaxHandler.cs
```csharp
namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

public sealed class CreateTaxHandler(
    ILogger<CreateTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxMaster> repository)
    : IRequestHandler<CreateTaxCommand, CreateTaxResponse>
{
    public async Task<CreateTaxResponse> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tax = TaxMaster.Create(
            code: request.Code,
            name: request.Name,
            taxType: request.TaxType,
            rate: request.Rate,
            taxCollectedAccountId: request.TaxCollectedAccountId!.Value,
            effectiveDate: request.EffectiveDate,
            isCompound: request.IsCompound,
            jurisdiction: request.Jurisdiction,
            expiryDate: request.ExpiryDate,
            taxPaidAccountId: request.TaxPaidAccountId,
            taxAuthority: request.TaxAuthority,
            taxRegistrationNumber: request.TaxRegistrationNumber,
            reportingCategory: request.ReportingCategory);

        await repository.AddAsync(tax, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Tax master created with ID {TaxId}, Code {TaxCode}, Type {TaxType}, Rate {Rate}%",
            tax.Id,
            request.Code,
            request.TaxType,
            request.Rate * 100);

        return new CreateTaxResponse(tax.Id);
    }
}
```

#### CreateTaxResponse.cs
```csharp
namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

public record CreateTaxResponse(DefaultIdType? Id);
```

### Step 3: Endpoints Layer

**Pattern Reference:** Follow Todo.Features and HumanResources.TaxBrackets patterns

#### TaxEndpoints.cs (Coordinator)
```csharp
namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes;

using v1;

public static class TaxEndpoints
{
    public static void MapTaxEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("taxes").WithTags("Taxes");
        group.MapCreateTaxEndpoint();
        group.MapUpdateTaxEndpoint();
        group.MapGetTaxEndpoint();
        group.MapDeleteTaxEndpoint();
        group.MapSearchTaxesEndpoint();
    }
}
```

#### CreateTaxEndpoint.cs (v1)
```csharp
namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes.v1;

public static class CreateTaxEndpoint
{
    public static RouteHandlerBuilder MapCreateTaxEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/", async (CreateTaxCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateTaxEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateTaxEndpoint))
        .WithSummary("Create tax master configuration")
        .WithDescription("Creates a new tax master configuration for various tax types (VAT, GST, Excise, Withholding, etc.)")
        .Produces<CreateTaxResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Taxes))
        .MapToApiVersion(1);
    }
}
```

---

## 🔄 Workflow: Tax Master Configuration

```
┌──────────────────────────────────────────┐
│ Tax Master Configuration Workflow         │
├──────────────────────────────────────────┤
│                                          │
│  1. Admin Portal                         │
│     ↓                                    │
│  2. Create Tax Master                    │
│     - Code, Name, Type                   │
│     - Rate, Jurisdiction                 │
│     - Accounts (Collected/Paid)          │
│     ↓                                    │
│  3. Set Effective Date                   │
│     - From date                          │
│     - Optional expiry date               │
│     ↓                                    │
│  4. Configure Accounts                   │
│     - Tax collected GL account           │
│     - Tax paid GL account (optional)     │
│     ↓                                    │
│  5. Save & Activate                      │
│     - Tax becomes active                 │
│     - Available for transactions         │
│     ↓                                    │
│  6. Apply to Transactions                │
│     - Use in sales invoices              │
│     - Use in purchases                   │
│     - Auto-calculate on amounts          │
│     ↓                                    │
│  7. Tax Reports                          │
│     - By jurisdiction                    │
│     - By period                          │
│     - Remittance calculation             │
│                                          │
└──────────────────────────────────────────┘
```

---

## 📦 Database Schema

### TaxMaster Table
```sql
CREATE TABLE HumanResources.TaxMasters (
    Id UUID PRIMARY KEY,
    TenantId UUID NOT NULL,
    Code VARCHAR(50) NOT NULL UNIQUE,
    Name VARCHAR(200) NOT NULL,
    TaxType VARCHAR(100) NOT NULL,
    Rate DECIMAL(5,4) NOT NULL,
    IsCompound BOOLEAN NOT NULL DEFAULT false,
    Jurisdiction VARCHAR(100),
    EffectiveDate TIMESTAMP NOT NULL,
    ExpiryDate TIMESTAMP,
    TaxCollectedAccountId UUID NOT NULL,
    TaxPaidAccountId UUID,
    TaxAuthority VARCHAR(200),
    TaxRegistrationNumber VARCHAR(100),
    ReportingCategory VARCHAR(100),
    IsActive BOOLEAN NOT NULL DEFAULT true,
    CreatedBy UUID NOT NULL,
    CreatedOn TIMESTAMP NOT NULL,
    LastModifiedBy UUID,
    LastModifiedOn TIMESTAMP,
    DeletedOn TIMESTAMP,
    DeletedBy UUID,
    CONSTRAINT FK_TaxMasters_TenantId FOREIGN KEY (TenantId) REFERENCES Tenants(Id)
);

CREATE INDEX IDX_TaxMasters_Code ON HumanResources.TaxMasters(Code);
CREATE INDEX IDX_TaxMasters_TaxType ON HumanResources.TaxMasters(TaxType);
CREATE INDEX IDX_TaxMasters_IsActive ON HumanResources.TaxMasters(IsActive);
CREATE INDEX IDX_TaxMasters_Jurisdiction ON HumanResources.TaxMasters(Jurisdiction);
```

---

## 🔐 Permissions

```csharp
FshPermission.NameFor(FshActions.Create, FshResources.Taxes)
FshPermission.NameFor(FshActions.Read, FshResources.Taxes)
FshPermission.NameFor(FshActions.Update, FshResources.Taxes)
FshPermission.NameFor(FshActions.Delete, FshResources.Taxes)
FshPermission.NameFor(FshActions.Search, FshResources.Taxes)
```

---

## ✅ Testing Checklist

- [ ] Domain entity validations work correctly
- [ ] Create command validates all required fields
- [ ] Create handler persists tax to database
- [ ] Get endpoint retrieves tax by ID
- [ ] Update endpoint modifies tax properties
- [ ] Delete endpoint soft-deletes or removes tax
- [ ] Search endpoint filters and paginates taxes
- [ ] Permissions are enforced on all endpoints
- [ ] Effective/Expiry dates work as expected
- [ ] Compound tax calculations work (if used)
- [ ] Historical tax rates preserved (temporal queries)
- [ ] Tax codes are unique
- [ ] Jurisdiction filtering works
- [ ] Active/Inactive status filtering works

---

## 🚀 Implementation Order

1. **Create Domain Entity** (TaxMaster.cs)
2. **Update DbContext** (add DbSet)
3. **Create Application Layer**
   - CreateTax command, validator, handler, response
   - UpdateTax command, validator, handler
   - DeleteTax command, handler
   - GetTax query, handler
   - SearchTaxes query, handler
4. **Create Infrastructure Endpoints**
   - TaxEndpoints.cs coordinator
   - CreateTaxEndpoint.cs, UpdateTaxEndpoint.cs, etc.
5. **Update Module Registration**
   - Register TaxMaster repository in HumanResourcesModule
   - Map Tax endpoints in HumanResourcesModule.Endpoints
6. **Update DbContext DbSets** (add TaxMaster)
7. **Create Migration** (add TaxMaster table)
8. **Test All Workflows**

---

## 📌 Key Patterns to Follow

### From Todo Module:
- ✅ Sealed record commands with IRequest
- ✅ Sealed class handlers
- ✅ Keyed service injection for repositories
- ✅ Proper logging
- ✅ Validator using AbstractValidator<T>

### From Accounting.TaxCode:
- ✅ Comprehensive XML documentation
- ✅ Temporal effectiveness (EffectiveDate, ExpiryDate)
- ✅ Account linking (TaxCollectedAccountId, TaxPaidAccountId)
- ✅ Tax authority tracking
- ✅ Compound tax support

### From HumanResources.TaxBrackets:
- ✅ Private constructors (EF Core + factory pattern)
- ✅ Factory methods with validation
- ✅ Update methods for partial updates
- ✅ Activation/Deactivation logic
- ✅ Carter module endpoint organization

---

## 📚 References

- **Todo Module:** `/src/api/modules/Todo/`
- **Catalog Module:** `/src/api/modules/Catalog/`
- **Accounting.TaxCode:** `/src/api/modules/Accounting/Accounting.Application/TaxCodes/`
- **HumanResources.TaxBrackets:** `/src/api/modules/HumanResources/HumanResources.Application/Taxes/`
- **HR Gap Analysis:** `/docs/HR_GAP_ANALYSIS_COMPLETE.md`


