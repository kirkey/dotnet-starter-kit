# 📋 PAYROLLLINE DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 15 complete files

---

## 🚀 Quick Start

### Create Payroll Line
```csharp
var command = new CreatePayrollLineCommand(
    PayrollId: payrollId,
    EmployeeId: employeeId,
    RegularHours: 160,
    OvertimeHours: 8);

var result = await mediator.Send(command);
// Returns: CreatePayrollLineResponse with Id
```

### Search Payroll Lines
```csharp
var request = new SearchPayrollLinesRequest
{
    PayrollId = payrollId,
    EmployeeId = employeeId,
    MinNetPay = 1000,
    MaxNetPay = 5000,
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<PayrollLineResponse>
```

### Get Single Payroll Line
```csharp
var request = new GetPayrollLineRequest(payrollLineId);
var result = await mediator.Send(request);
// Returns: PayrollLineResponse
```

### Update Payroll Line
```csharp
var command = new UpdatePayrollLineCommand(
    Id: payrollLineId,
    RegularPay: 3000,
    OvertimePay: 600,
    IncomeTax: 500,
    SocialSecurityTax: 186,
    MedicareTax: 43.50m,
    HealthInsurance: 250,
    RetirementContribution: 400,
    PaymentMethod: "DirectDeposit",
    BankAccountLast4: "1234");

var result = await mediator.Send(command);
// RecalculateTotals() is called automatically
// NetPay = GrossPay - Taxes - Deductions
```

### Delete Payroll Line
```csharp
var command = new DeletePayrollLineCommand(payrollLineId);
var result = await mediator.Send(command);
// Returns: DeletePayrollLineResponse
```

---

## 📊 PayrollLine Calculation

```
EARNINGS:
  RegularPay (hours × rate)
+ OvertimePay (hours × rate × 1.5)
+ BonusPay
+ OtherEarnings
= GrossPay

TAXES:
  IncomeTax
+ SocialSecurityTax (6.2%)
+ MedicareTax (1.45%)
+ OtherTaxes
= TotalTaxes

DEDUCTIONS:
  HealthInsurance
+ RetirementContribution
+ OtherDeductions
= TotalDeductions

NET PAY:
  GrossPay
- TotalTaxes
- TotalDeductions
= NetPay (≥ 0 required)
```

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **PayrollId** | DefaultIdType? | Filter by payroll period |
| **EmployeeId** | DefaultIdType? | Filter by employee |
| **MinNetPay** | decimal? | Minimum net pay amount |
| **MaxNetPay** | decimal? | Maximum net pay amount |
| **PageNumber** | int | Page for pagination (1-based) |
| **PageSize** | int | Records per page (default 10) |

---

## ✅ Validations

### Create PayrollLine
- ✅ PayrollId required & exists
- ✅ EmployeeId required & exists
- ✅ RegularHours: 0-260
- ✅ OvertimeHours: 0-100

### Update PayrollLine
- ✅ Id required
- ✅ RegularHours: 0-260 (when provided)
- ✅ OvertimeHours: 0-100 (when provided)
- ✅ All amounts >= 0
- ✅ PaymentMethod: "DirectDeposit" or "Check"
- ✅ BankAccountLast4: 4 digits only
- ✅ CheckNumber: max 20 chars

---

## 🎯 PayrollLineResponse Properties

```csharp
PayrollLineResponse
├── Identifiers
│   ├── Id: DefaultIdType
│   ├── PayrollId: DefaultIdType
│   └── EmployeeId: DefaultIdType
├── Hours
│   ├── RegularHours: decimal
│   └── OvertimeHours: decimal
├── Earnings
│   ├── RegularPay: decimal
│   ├── OvertimePay: decimal
│   ├── BonusPay: decimal
│   ├── OtherEarnings: decimal
│   └── GrossPay: decimal (sum)
├── Taxes
│   ├── IncomeTax: decimal
│   ├── SocialSecurityTax: decimal
│   ├── MedicareTax: decimal
│   ├── OtherTaxes: decimal
│   └── TotalTaxes: decimal (sum)
├── Deductions
│   ├── HealthInsurance: decimal
│   ├── RetirementContribution: decimal
│   ├── OtherDeductions: decimal
│   └── TotalDeductions: decimal (sum)
├── Net
│   └── NetPay: decimal (GrossPay - Taxes - Deductions)
└── Payment
    ├── PaymentMethod: string?
    ├── BankAccountLast4: string?
    └── CheckNumber: string?
```

---

## 🔧 Configuration

### Register Keyed Services
```csharp
services.AddKeyedScoped<IRepository<PayrollLine>>("hr:payrolllines");
services.AddKeyedScoped<IReadRepository<PayrollLine>>("hr:payrolllines");
```

### Register Handlers
```csharp
services.AddMediatR(typeof(CreatePayrollLineHandler));
services.AddMediatR(typeof(SearchPayrollLinesHandler));
services.AddMediatR(typeof(GetPayrollLineHandler));
services.AddMediatR(typeof(UpdatePayrollLineHandler));
services.AddMediatR(typeof(DeletePayrollLineHandler));
```

### Register Validators
```csharp
services.AddValidatorsFromAssembly(typeof(CreatePayrollLineValidator).Assembly);
```

---

## 📁 Folder Structure

```
PayrollLines/
├── Create/v1/ → CreatePayrollLineCommand/Handler/Validator/Response
├── Get/v1/ → GetPayrollLineRequest/Handler/PayrollLineResponse
├── Search/v1/ → SearchPayrollLinesRequest/Handler
├── Update/v1/ → UpdatePayrollLineCommand/Handler/Validator/Response
├── Delete/v1/ → DeletePayrollLineCommand/Handler/Response
└── Specifications/ → PayrollLinesSpecs.cs
```

---

## 🚀 Next Steps

### Payroll Processing Engine
- Implement tax calculation service
- Implement deduction engine
- Implement overtime calculation

### API Endpoints
- Create REST routes
- Add Swagger documentation
- Map request/response

### Reporting
- Generate payroll summaries
- Generate tax reports
- Generate payment reconciliation

---

## 📊 Domain Methods

```csharp
// Create
var line = PayrollLine.Create(payrollId, employeeId, regularHours, overtimeHours);

// Set Hours
line.SetHours(160, 8); // regularHours, overtimeHours

// Set Earnings
line.SetEarnings(3000, 600, 0, 0); // regular, overtime, bonus, other

// Set Taxes
line.SetTaxes(500, 186, 43.50m, 0); // income, SS, Medicare, other

// Set Deductions
line.SetDeductions(250, 400, 0); // health, retirement, other

// Set Payment
line.SetPaymentMethod("DirectDeposit", "1234", null);

// Recalculate (called automatically in Update handler)
line.RecalculateTotals();
// Calculates: GrossPay, TotalTaxes, TotalDeductions, NetPay
```

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** Payroll Processing Engine


