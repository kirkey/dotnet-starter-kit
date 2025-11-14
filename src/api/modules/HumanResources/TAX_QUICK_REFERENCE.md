# 📋 TAX DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 15 complete files

---

## 🚀 Quick Start

### Create Tax Bracket
```csharp
var command = new CreateTaxCommand(
    TaxType: "Federal",
    Year: 2025,
    MinIncome: 0,
    MaxIncome: 11000,
    Rate: 0.10m,
    FilingStatus: "Single",
    Description: "10% bracket for 2025");

var result = await mediator.Send(command);
// Returns: CreateTaxResponse with Id
```

### Search Tax Brackets
```csharp
var request = new SearchTaxesRequest
{
    TaxType = "Federal",
    Year = 2025,
    FilingStatus = "Single",
    MinIncomeFilter = 0,
    MaxIncomeFilter = 100000,
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<TaxResponse>
```

### Get Single Tax Bracket
```csharp
var request = new GetTaxRequest(taxId);
var result = await mediator.Send(request);
// Returns: TaxResponse
```

### Update Tax Bracket
```csharp
var command = new UpdateTaxCommand(
    Id: taxId,
    FilingStatus: "Married",
    Description: "Updated for married filing status");

var result = await mediator.Send(command);
// Returns: UpdateTaxResponse
```

### Delete Tax Bracket
```csharp
var command = new DeleteTaxCommand(taxId);
var result = await mediator.Send(command);
// Returns: DeleteTaxResponse
```

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **TaxType** | string? | "Federal", "State", "FICA" |
| **Year** | int? | 2025 |
| **FilingStatus** | string? | "Single", "Married", "Head of Household" |
| **MinIncomeFilter** | decimal? | 0 |
| **MaxIncomeFilter** | decimal? | 100000 |
| **PageNumber** | int | 1 |
| **PageSize** | int | 10 |

---

## ✅ Validations

### Create Tax
- ✅ TaxType required, max 50 chars
- ✅ Year 2000-2099
- ✅ MinIncome >= 0
- ✅ MaxIncome > MinIncome
- ✅ Rate 0-100% (0.0-1.0)
- ✅ FilingStatus max 50 chars (optional)
- ✅ Description max 500 chars (optional)

### Update Tax
- ✅ Id required
- ✅ FilingStatus max 50 chars (when provided)
- ✅ Description max 500 chars (when provided)

---

## 🎯 TaxResponse Properties

```csharp
TaxResponse
├── Id: DefaultIdType
├── TaxType: string
├── Year: int
├── MinIncome: decimal
├── MaxIncome: decimal
├── Rate: decimal (0.0-1.0)
├── FilingStatus: string?
└── Description: string?
```

---

## 📊 Tax Types Reference

| Type | Code | Usage |
|------|------|-------|
| **Federal** | FED | US Federal Income Tax |
| **FICA-SS** | FICASS | Social Security (6.2%) |
| **FICA-MC** | FICAMC | Medicare (1.45%) |
| **State** | STATE | State Income Tax |
| **Local** | LOCAL | Local Tax |
| **Other** | OTHER | Custom/Special Taxes |

---

## 💰 Rate Reference Examples

```
Standard Rates:
├─ Federal: 10%, 12%, 22%, 24%, 32%, 35%, 37%
├─ Social Security: 6.2% (employee) + 6.2% (employer)
├─ Medicare: 1.45% (employee) + 1.45% (employer)
├─ State: Varies (2-13%)
└─ Local: Varies

Expressed as Decimals:
├─ 10% = 0.10
├─ 6.2% = 0.062
├─ 1.45% = 0.0145
└─ 37% = 0.37
```

---

## 🔧 Configuration

### Register Keyed Services
```csharp
services.AddKeyedScoped<IRepository<TaxBracket>>("hr:taxes");
services.AddKeyedScoped<IReadRepository<TaxBracket>>("hr:taxes");
```

### Register Handlers
```csharp
services.AddMediatR(typeof(CreateTaxHandler));
services.AddMediatR(typeof(SearchTaxesHandler));
services.AddMediatR(typeof(GetTaxHandler));
services.AddMediatR(typeof(UpdateTaxHandler));
services.AddMediatR(typeof(DeleteTaxHandler));
```

### Register Validators
```csharp
services.AddValidatorsFromAssembly(typeof(CreateTaxValidator).Assembly);
```

---

## 📁 Folder Structure

```
Taxes/
├── Create/v1/ → CreateTaxCommand/Handler/Validator/Response
├── Get/v1/ → GetTaxRequest/Handler/TaxResponse
├── Search/v1/ → SearchTaxesRequest/Handler
├── Update/v1/ → UpdateTaxCommand/Handler/Validator/Response
├── Delete/v1/ → DeleteTaxCommand/Handler/Response
└── Specifications/ → TaxesSpecs.cs
```

---

## 📊 Domain Methods

```csharp
// Create
var tax = TaxBracket.Create("Federal", 2025, 0, 11000, 0.10m);

// Update
tax.Update(
    filingStatus: "Married",
    description: "Updated bracket");
```

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** Payroll Engine & Tax Calculations


