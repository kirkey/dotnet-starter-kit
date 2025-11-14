# 📋 PAYROLL DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 15 complete files

---

## 🚀 Quick Start

### Create a Payroll Period
```csharp
var command = new CreatePayrollCommand(
    StartDate: new DateTime(2025, 11, 1),
    EndDate: new DateTime(2025, 11, 30),
    PayFrequency: "Monthly");

var result = await mediator.Send(command);
// Returns: CreatePayrollResponse with Id
```

### Search Payrolls
```csharp
var request = new SearchPayrollsRequest
{
    Status = "Draft",
    PayFrequency = "Monthly",
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<PayrollResponse>
```

### Get Single Payroll
```csharp
var request = new GetPayrollRequest(payrollId);
var result = await mediator.Send(request);
// Returns: PayrollResponse
```

### Process Payroll
```csharp
var command = new UpdatePayrollCommand(
    Id: payrollId,
    Status: "Processing");

var result = await mediator.Send(command);
// Transitions: Draft → Processing
```

### Post to GL
```csharp
var command = new UpdatePayrollCommand(
    Id: payrollId,
    Status: "Posted",
    JournalEntryId: "JE-2025-001");

var result = await mediator.Send(command);
// Transitions: Processed → Posted (locks payroll)
```

### Mark as Paid
```csharp
var command = new UpdatePayrollCommand(
    Id: payrollId,
    Status: "Paid");

var result = await mediator.Send(command);
// Transitions: Posted → Paid
```

### Delete Payroll
```csharp
var command = new DeletePayrollCommand(payrollId);
var result = await mediator.Send(command);
// Returns: DeletePayrollResponse
```

---

## 📊 Payroll Status Workflow

```
Draft → Processing → Processed → Posted → Paid
```

| Status | Allowed Operations | Locked |
|--------|------------------|--------|
| **Draft** | Edit lines, Process | No |
| **Processing** | CompleteProcessing | No |
| **Processed** | Post to GL | No |
| **Posted** | Mark as Paid | YES |
| **Paid** | View only | YES |

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **StartDate** | DateTime? | 2025-11-01 |
| **EndDate** | DateTime? | 2025-11-30 |
| **PayFrequency** | string | "Monthly" |
| **Status** | string | "Processed" |

---

## ✅ Validations

### Create Payroll
- ✅ StartDate required
- ✅ EndDate > StartDate
- ✅ PayFrequency required (Weekly, BiWeekly, SemiMonthly, Monthly)
- ✅ Notes max 500 chars

### Update Payroll
- ✅ Status must be valid transition
- ✅ JournalEntryId required when posting
- ✅ JournalEntryId max 50 chars
- ✅ Notes max 500 chars

---

## 🎯 Payroll Response Properties

```csharp
PayrollResponse
├── Id: DefaultIdType
├── StartDate: DateTime
├── EndDate: DateTime
├── PayFrequency: string
├── Status: string
├── TotalGrossPay: decimal
├── TotalTaxes: decimal
├── TotalDeductions: decimal
├── TotalNetPay: decimal
├── EmployeeCount: int
├── ProcessedDate: DateTime?
├── PostedDate: DateTime?
├── PaidDate: DateTime?
├── JournalEntryId: string?
├── IsLocked: bool
└── Notes: string?
```

---

## 🔧 Configuration

### Register Keyed Services
```csharp
// In your service configuration
services.AddKeyedScoped<IRepository<Payroll>>("hr:payrolls");
services.AddKeyedScoped<IReadRepository<Payroll>>("hr:payrolls");
```

### Register Handlers
```csharp
// Automatically registered via MediatR
services.AddMediatR(typeof(CreatePayrollHandler));
services.AddMediatR(typeof(SearchPayrollsHandler));
services.AddMediatR(typeof(GetPayrollHandler));
services.AddMediatR(typeof(UpdatePayrollHandler));
services.AddMediatR(typeof(DeletePayrollHandler));
```

### Register Validators
```csharp
// Automatically registered via FluentValidation
services.AddValidatorsFromAssembly(typeof(CreatePayrollValidator).Assembly);
```

---

## 📁 Folder Structure

```
Payrolls/
├── Create/v1/ → CreatePayrollCommand/Handler/Validator/Response
├── Get/v1/ → GetPayrollRequest/Handler/PayrollResponse
├── Search/v1/ → SearchPayrollsRequest/Handler
├── Update/v1/ → UpdatePayrollCommand/Handler/Validator/Response
├── Delete/v1/ → DeletePayrollCommand/Handler/Response
└── Specifications/ → PayrollsSpecs.cs
```

---

## 🚀 Next Steps

### PayrollLines Implementation
- Create PayrollLine CRUD operations
- Implement employee pay calculations
- Handle deductions and taxes

### API Endpoints
- MapCreatePayrollEndpoint
- MapGetPayrollEndpoint
- MapSearchPayrollsEndpoint
- MapUpdatePayrollEndpoint
- MapDeletePayrollEndpoint

### Database Configuration
- EF Core DbContext configuration
- Payroll table structure
- PayrollLine table structure
- Foreign key relationships

### Integration
- TimeSheet → Payroll hours
- LeaveBalance → Deductions
- Accounting → GL posting

---

## 📞 Support

**Questions about Payroll domain?**

1. See: `PAYROLL_IMPLEMENTATION_COMPLETE.md` for detailed docs
2. Check: Domain entity methods in `Payroll.cs`
3. Review: Specification patterns in `PayrollsSpecs.cs`

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** Infrastructure & PayrollLines


