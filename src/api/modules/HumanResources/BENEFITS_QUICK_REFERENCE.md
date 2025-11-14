# 📋 BENEFITS DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 15 complete files

---

## 🚀 Quick Start

### Create Benefit
```csharp
var command = new CreateBenefitCommand(
    BenefitName: "Health Insurance",
    BenefitType: "Health",
    EmployeeContribution: 500m,
    EmployerContribution: 1000m,
    Description: "Comprehensive health coverage",
    AnnualLimit: 50000m);

var result = await mediator.Send(command);
// Returns: CreateBenefitResponse with Id
```

### Search Benefits
```csharp
var request = new SearchBenefitsRequest
{
    BenefitType = "Health",
    IsActive = true,
    SearchString = "Health",
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<BenefitResponse>
```

### Get Single Benefit
```csharp
var request = new GetBenefitRequest(benefitId);
var result = await mediator.Send(request);
// Returns: BenefitResponse
```

### Update Benefit
```csharp
var command = new UpdateBenefitCommand(
    Id: benefitId,
    BenefitName: "Premium Health Insurance",
    EmployeeContribution: 600m,
    EmployerContribution: 1200m,
    IsRequired: true,
    IsActive: true);

var result = await mediator.Send(command);
// Returns: UpdateBenefitResponse
```

### Delete Benefit
```csharp
var command = new DeleteBenefitCommand(benefitId);
var result = await mediator.Send(command);
// Returns: DeleteBenefitResponse
```

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **SearchString** | string? | "Health" |
| **BenefitType** | string? | "Health", "Retirement", "Life Insurance" |
| **IsActive** | bool? | true / false |
| **IsRequired** | bool? | true / false |
| **PageNumber** | int | 1 |
| **PageSize** | int | 10 |

---

## ✅ Validations

### Create Benefit
- ✅ BenefitName required, max 100 chars
- ✅ BenefitType required, max 50 chars
- ✅ EmployeeContribution >= 0
- ✅ EmployerContribution >= 0
- ✅ AnnualLimit >= 0 (optional)
- ✅ MinimumEligibleEmployees > 0 (optional)
- ✅ Description max 500 chars (optional)

### Update Benefit
- ✅ Id required
- ✅ BenefitName max 100 chars (when provided)
- ✅ Contributions >= 0 (when provided)
- ✅ Description max 500 chars (when provided)

---

## 🎯 BenefitResponse Properties

```csharp
BenefitResponse
├── Id: DefaultIdType
├── BenefitName: string
├── BenefitType: string
├── EmployeeContribution: decimal
├── EmployerContribution: decimal
├── IsRequired: bool
├── IsActive: bool
├── Description: string?
├── AnnualLimit: decimal?
├── IsCarryoverAllowed: bool
├── MinimumEligibleEmployees: int?
└── PayComponentId: DefaultIdType?
```

---

## 📊 Benefit Types

| Type | Examples | Annual Limit |
|------|----------|--------------|
| **Health** | Medical, Dental, Vision | Varies |
| **Retirement** | 401(k), IRA | $23,500 (2024) |
| **Life Insurance** | Basic, Supplemental | Varies |
| **Wellness** | Gym, FSA, HSA | $3,300 (FSA, 2024) |
| **Other** | Parking, Transit | Varies |

---

## 🔧 Configuration

### Register Keyed Services
```csharp
services.AddKeyedScoped<IRepository<Benefit>>("hr:benefits");
services.AddKeyedScoped<IReadRepository<Benefit>>("hr:benefits");
```

### Register Handlers
```csharp
services.AddMediatR(typeof(CreateBenefitHandler));
services.AddMediatR(typeof(SearchBenefitsHandler));
services.AddMediatR(typeof(GetBenefitHandler));
services.AddMediatR(typeof(UpdateBenefitHandler));
services.AddMediatR(typeof(DeleteBenefitHandler));
```

### Register Validators
```csharp
services.AddValidatorsFromAssembly(typeof(CreateBenefitValidator).Assembly);
```

---

## 📁 Folder Structure

```
Benefits/
├── Create/v1/ → CreateBenefitCommand/Handler/Validator/Response
├── Get/v1/ → GetBenefitRequest/Handler/BenefitResponse
├── Search/v1/ → SearchBenefitsRequest/Handler
├── Update/v1/ → UpdateBenefitCommand/Handler/Validator/Response
├── Delete/v1/ → DeleteBenefitCommand/Handler/Response
└── Specifications/ → BenefitsSpecs.cs
```

---

## 📊 Domain Methods

```csharp
// Create
var benefit = Benefit.Create("Health Insurance", "Health", 500, 1000);

// Update
benefit.Update(
    benefitName: "Premium Health Insurance",
    employeeContribution: 600,
    employerContribution: 1200,
    description: "Updated coverage");

// Control Status
benefit.MakeRequired();
benefit.MakeOptional();
benefit.Activate();
benefit.Deactivate();
```

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** BenefitEnrollment & Payroll Integration


