# ✅ Action Plan: Remove Company Domain Implementation

**Date:** November 13, 2025  
**Action:** Remove Company entity implementation (already built)  
**Reason:** SAAS architecture - company info = tenant info  

---

## 🎯 What Needs to Be Removed

### Domain Layer (3 files to delete)
```
✅ HumanResources.Domain/Company.cs
✅ HumanResources.Domain/Exceptions/CompanyExceptions.cs
✅ CompanyEvents (need to remove company events from CompanyEvents.cs)
```

### Application Layer (14 files to delete)
```
✅ Companies/Create/v1/CreateCompanyCommand.cs
✅ Companies/Create/v1/CreateCompanyValidator.cs
✅ Companies/Create/v1/CreateCompanyHandler.cs
✅ Companies/Create/v1/CreateCompanyResponse.cs

✅ Companies/Get/v1/GetCompanyRequest.cs
✅ Companies/Get/v1/GetCompanyHandler.cs
✅ Companies/Get/v1/CompanyResponse.cs

✅ Companies/Search/v1/SearchCompaniesRequest.cs
✅ Companies/Search/v1/SearchCompaniesHandler.cs

✅ Companies/Update/v1/UpdateCompanyCommand.cs
✅ Companies/Update/v1/UpdateCompanyValidator.cs
✅ Companies/Update/v1/UpdateCompanyHandler.cs
✅ Companies/Update/v1/UpdateCompanyResponse.cs

✅ Companies/Delete/v1/DeleteCompanyCommand.cs
✅ Companies/Delete/v1/DeleteCompanyHandler.cs
✅ Companies/Delete/v1/DeleteCompanyResponse.cs

✅ Companies/Specifications/CompanyByIdSpec.cs
✅ Companies/Specifications/CompanyByCodeSpec.cs
✅ Companies/Specifications/SearchCompaniesSpec.cs
```

### Infrastructure Layer (7 files to delete)
```
✅ Endpoints/v1/CreateCompanyEndpoint.cs
✅ Endpoints/v1/GetCompanyEndpoint.cs
✅ Endpoints/v1/SearchCompaniesEndpoint.cs
✅ Endpoints/v1/UpdateCompanyEndpoint.cs
✅ Endpoints/v1/DeleteCompanyEndpoint.cs

✅ Persistence/Configurations/CompanyConfiguration.cs
```

### Configuration & Updates (multiple files)
```
⚠️ HumanResourcesModule.cs - Remove company service registration & endpoints
⚠️ HumanResourcesDbContext.cs - Remove DbSet<Company>
⚠️ HumanResourcesDbInitializer.cs - Remove Company seed data
⚠️ OrganizationalUnit.cs - Remove CompanyId references
⚠️ All OrganizationalUnit files - Update to remove CompanyId
```

---

## 📋 Step-by-Step Removal Process

### Step 1: Update OrganizationalUnit Domain
**File:** `OrganizationalUnit.cs`

**Changes:**
- Remove: `public DefaultIdType CompanyId { get; private set; }`
- Remove: `public Company Company { get; private set; } = default!;`
- Update: Create method to remove `companyId` parameter
- Update: Constructor to remove company assignment

**Before:**
```csharp
private OrganizationalUnit(
    DefaultIdType id,
    DefaultIdType companyId,        // ← Remove
    string code,
    string name,
    // ...
)
{
    Id = id;
    CompanyId = companyId;          // ← Remove
    Code = code;
    // ...
}

public static OrganizationalUnit Create(
    DefaultIdType companyId,        // ← Remove
    string code,
    // ...
)
```

**After:**
```csharp
private OrganizationalUnit(
    DefaultIdType id,
    string code,
    string name,
    // ...
)
{
    Id = id;
    Code = code;
    // ...
}

public static OrganizationalUnit Create(
    string code,
    // ...
)
```

### Step 2: Update All OrganizationalUnit Application Files

**Affected files:**
- `Positions/Specifications/SearchPositionsSpec.cs` - No change (doesn't reference Company)
- `OrganizationalUnits/Create/v1/CreateOrganizationalUnitCommand.cs` - **Remove CompanyId parameter**
- `OrganizationalUnits/Create/v1/CreateOrganizationalUnitValidator.cs` - No CompanyId validation
- `OrganizationalUnits/Create/v1/CreateOrganizationalUnitHandler.cs` - Update to remove CompanyId usage
- All OrganizationalUnit specifications - Remove CompanyId where used

### Step 3: Update Database Configuration

**File:** `Persistence/Configurations/OrganizationalUnitConfiguration.cs`

**Remove:**
```csharp
// ❌ Remove this
builder.Property(ou => ou.CompanyId).IsRequired();
builder.HasIndex(ou => new { ou.CompanyId, ou.Code }).IsUnique();
builder.HasOne(ou => ou.Company)
    .WithMany()
    .HasForeignKey(ou => ou.CompanyId)
    .OnDelete(DeleteBehavior.Restrict);
```

**Add:**
```csharp
// ✅ Add this for tenant-based uniqueness
builder.HasIndex(ou => new { ou.Code }).IsUnique();
```

### Step 4: Update Module & Context

**File:** `HumanResourcesModule.cs`

**Remove:**
```csharp
// ❌ Remove company service registration
builder.Services.AddKeyedScoped<IRepository<Company>, 
    HumanResourcesRepository<Company>>("hr:companies");
builder.Services.AddKeyedScoped<IReadRepository<Company>, 
    HumanResourcesRepository<Company>>("hr:companies");

// ❌ Remove company endpoints mapping
var companyGroup = app.MapGroup("companies").WithTags("companies");
companyGroup.MapCompanyCreateEndpoint();
companyGroup.MapCompanyGetEndpoint();
companyGroup.MapCompaniesSearchEndpoint();
companyGroup.MapCompanyUpdateEndpoint();
companyGroup.MapCompanyDeleteEndpoint();
```

**File:** `HumanResourcesDbContext.cs`

**Remove:**
```csharp
// ❌ Remove this
public DbSet<Company> Companies { get; set; } = null!;
```

**File:** `HumanResourcesDbInitializer.cs`

**Remove:**
```csharp
// ❌ Remove company seeding
var company = await context.Companies
    .FirstOrDefaultAsync(c => c.CompanyCode == CompanyCode, cancellationToken)
    .ConfigureAwait(false);

if (company is null)
{
    company = Company.Create(CompanyCode, CompanyName);
    await context.Companies.AddAsync(company, cancellationToken).ConfigureAwait(false);
    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}

// Update organizational unit creation to NOT use company.Id
```

### Step 5: Delete Company Implementations

**Delete these folders entirely:**
```
❌ HumanResources.Application/Companies/
❌ HumanResources.Infrastructure/Persistence/Configurations/CompanyConfiguration.cs
```

**Delete these files:**
```
❌ HumanResources.Domain/Company.cs
❌ HumanResources.Domain/Exceptions/CompanyExceptions.cs
```

---

## 🔧 Files That Need Updates

### OrganizationalUnit Create Command
**File:** `OrganizationalUnits/Create/v1/CreateOrganizationalUnitCommand.cs`

**Change:**
```csharp
// Before
public sealed record CreateOrganizationalUnitCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] 
    DefaultIdType CompanyId,  // ← Remove
    [property: DefaultValue("HR-001")] string Code,
    // ...

// After
public sealed record CreateOrganizationalUnitCommand(
    [property: DefaultValue("HR-001")] string Code,
    // ...
```

### OrganizationalUnit Create Handler
**File:** `OrganizationalUnits/Create/v1/CreateOrganizationalUnitHandler.cs`

**Change:**
```csharp
// Before
var organizationalUnit = OrganizationalUnit.Create(
    request.CompanyId,  // ← Remove this parameter
    request.Code,
    request.Name,
    // ...

// After
var organizationalUnit = OrganizationalUnit.Create(
    request.Code,
    request.Name,
    // ...
```

### OrganizationalUnit Specifications
**Files:** All spec files in `OrganizationalUnits/Specifications/`

**Change:**
```csharp
// Before
var spec = new SearchOrganizationalUnitsSpec(request);

if (request.CompanyId.HasValue)  // ← Remove
{
    Query.Where(ou => ou.CompanyId == request.CompanyId.Value);
}

// After
var spec = new SearchOrganizationalUnitsSpec(request);
// TenantId filtering done automatically by FshDbContext
```

### HumanResourcesDbInitializer
**File:** `HumanResourcesDbInitializer.cs`

**Change:**
```csharp
// Before
var hrDepartment = OrganizationalUnit.Create(
    company.Id,  // ← Remove this parameter
    "HR-001",
    "Human Resources Department",
    OrganizationalUnitType.Department);

// After
var hrDepartment = OrganizationalUnit.Create(
    "HR-001",
    "Human Resources Department",
    OrganizationalUnitType.Department);
```

---

## ✅ Verification Checklist

After removal, verify:

- [ ] Solution builds without errors
- [ ] No references to Company entity remain
- [ ] OrganizationalUnit works without CompanyId
- [ ] All organizational unit endpoints still work
- [ ] Position endpoints still work
- [ ] Database migrations can be reversed
- [ ] No orphaned code remains
- [ ] Documentation updated

---

## 📝 Summary

**Total files to delete: 25**
- Domain: 2 files
- Application: 20 files
- Infrastructure: 3 files

**Total files to update: 7**
- Domain: 1 file (OrganizationalUnit.cs)
- Application: 4 files (Org unit commands/handlers)
- Infrastructure: 2 files (Module, DbContext)

**Total development time:** ~3-4 hours

**Effort saved:** $2K

---

## 🚀 Next Steps

1. Update OrganizationalUnit entity (remove CompanyId)
2. Update all OrganizationalUnit CQRS files
3. Update database configurations
4. Delete Company implementations
5. Update module registration
6. Test and verify build
7. Update documentation

---

**Ready to clean up the architecture!** ✅

