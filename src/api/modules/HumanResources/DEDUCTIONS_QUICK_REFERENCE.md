# 📋 DEDUCTIONS DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 15 complete files

---

## 🚀 Quick Start

### Create Deduction
```csharp
var command = new CreateDeductionCommand(
    ComponentName: "Health Insurance",
    ComponentType: "Deduction",
    GLAccountCode: "6501",
    Description: "Employee health insurance premium");

var result = await mediator.Send(command);
// Returns: CreateDeductionResponse with Id
```

### Search Deductions
```csharp
var request = new SearchDeductionsRequest
{
    ComponentType = "Deduction",
    IsActive = true,
    SearchString = "Health",
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<DeductionResponse>
```

### Get Single Deduction
```csharp
var request = new GetDeductionRequest(deductionId);
var result = await mediator.Send(request);
// Returns: DeductionResponse
```

### Update Deduction
```csharp
var command = new UpdateDeductionCommand(
    Id: deductionId,
    ComponentName: "Health Insurance Premium",
    GLAccountCode: "6501",
    Description: "Updated description");

var result = await mediator.Send(command);
// Returns: UpdateDeductionResponse
```

### Delete Deduction
```csharp
var command = new DeleteDeductionCommand(deductionId);
var result = await mediator.Send(command);
// Returns: DeleteDeductionResponse
```

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **SearchString** | string? | "Health" |
| **ComponentType** | string? | "Deduction", "Tax", "Earnings" |
| **IsActive** | bool? | true / false |
| **IsCalculated** | bool? | true / false |
| **PageNumber** | int | 1 |
| **PageSize** | int | 10 |

---

## ✅ Validations

### Create Deduction
- ✅ ComponentName required, max 100 chars
- ✅ ComponentType must be (Earnings, Tax, Deduction)
- ✅ GLAccountCode max 20 chars (optional)
- ✅ Description max 500 chars (optional)

### Update Deduction
- ✅ Id required
- ✅ ComponentName max 100 chars (when provided)
- ✅ GLAccountCode max 20 chars (when provided)
- ✅ Description max 500 chars (when provided)

---

## 🎯 DeductionResponse Properties

```csharp
DeductionResponse
├── Id: DefaultIdType
├── ComponentName: string
├── ComponentType: string (Earnings|Tax|Deduction)
├── GLAccountCode: string
├── IsActive: bool
├── IsCalculated: bool
└── Description: string?
```

---

## 📊 Component Types

| Type | Purpose | Used For |
|------|---------|----------|
| **Earnings** | Earnings configuration | Bonus, commissions, allowances |
| **Tax** | Tax configuration | Income tax, FICA, state tax |
| **Deduction** | Deduction configuration | Health insurance, 401(k), garnishments |

---

## 🔧 Configuration

### Register Keyed Services
```csharp
services.AddKeyedScoped<IRepository<PayComponent>>("hr:deductions");
services.AddKeyedScoped<IReadRepository<PayComponent>>("hr:deductions");
```

### Register Handlers
```csharp
services.AddMediatR(typeof(CreateDeductionHandler));
services.AddMediatR(typeof(SearchDeductionsHandler));
services.AddMediatR(typeof(GetDeductionHandler));
services.AddMediatR(typeof(UpdateDeductionHandler));
services.AddMediatR(typeof(DeleteDeductionHandler));
```

### Register Validators
```csharp
services.AddValidatorsFromAssembly(typeof(CreateDeductionValidator).Assembly);
```

---

## 📁 Folder Structure

```
Deductions/
├── Create/v1/ → CreateDeductionCommand/Handler/Validator/Response
├── Get/v1/ → GetDeductionRequest/Handler/DeductionResponse
├── Search/v1/ → SearchDeductionsRequest/Handler
├── Update/v1/ → UpdateDeductionCommand/Handler/Validator/Response
├── Delete/v1/ → DeleteDeductionCommand/Handler/Response
└── Specifications/ → DeductionsSpecs.cs
```

---

## 🚀 Next Steps

### Payroll Integration
- Link deductions to PayrollLine
- Apply deductions in calculations
- Track deduction usage

### GL Integration
- Create GL posting entries
- Support cost center allocation
- Generate GL reports

### Reporting
- Deduction usage reports
- GL posting reconciliation
- Tax impact analysis

---

## 📊 Domain Methods

```csharp
// Create
var deduction = PayComponent.Create("Health Insurance", "Deduction", "6501");

// Update
deduction.Update(
    componentName: "Health Insurance Premium",
    glAccountCode: "6501",
    description: "Employee health coverage");

// Activate/Deactivate
deduction.Activate();
deduction.Deactivate();
```

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** Tax & Payroll Integration


