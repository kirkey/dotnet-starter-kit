# 📋 ENROLLMENTS DOMAIN - QUICK REFERENCE

**Status:** ✅ Complete & Compiled  
**Build:** ✅ Success (0 Errors)  
**Files:** 18 complete files

---

## 🚀 Quick Start

### Create Enrollment
```csharp
var command = new CreateEnrollmentCommand(
    EmployeeId: employeeId,
    BenefitId: benefitId,
    EnrollmentDate: DateTime.Today,
    EffectiveDate: DateTime.Today.AddMonths(1),
    CoverageLevel: "Individual",
    EmployeeContributionAmount: 500m,
    EmployerContributionAmount: 1000m);

var result = await mediator.Send(command);
// Returns: CreateEnrollmentResponse with Id
```

### Search Enrollments
```csharp
var request = new SearchEnrollmentsRequest
{
    EmployeeId = employeeId,
    IsActive = true,
    CoverageLevel = "Individual",
    PageNumber = 1,
    PageSize = 10
};

var result = await mediator.Send(request);
// Returns: PagedList<EnrollmentResponse>
```

### Get Single Enrollment
```csharp
var request = new GetEnrollmentRequest(enrollmentId);
var result = await mediator.Send(request);
// Returns: EnrollmentResponse
```

### Update Enrollment
```csharp
var command = new UpdateEnrollmentCommand(
    Id: enrollmentId,
    CoverageLevel: "Family",
    EmployeeContributionAmount: 600m,
    EmployerContributionAmount: 1200m,
    CoveredDependentIds: new[] { dependentId1, dependentId2 });

var result = await mediator.Send(command);
// Returns: UpdateEnrollmentResponse
```

### Terminate Enrollment
```csharp
var command = new TerminateEnrollmentCommand(
    Id: enrollmentId,
    EndDate: DateTime.Today.AddMonths(1));

var result = await mediator.Send(command);
// Returns: TerminateEnrollmentResponse
```

### Delete Enrollment
```csharp
var command = new DeleteEnrollmentCommand(enrollmentId);
var result = await mediator.Send(command);
// Returns: DeleteEnrollmentResponse
```

---

## 🔍 Search Filters

| Filter | Type | Example |
|--------|------|---------|
| **EmployeeId** | DefaultIdType? | Employee identifier |
| **BenefitId** | DefaultIdType? | Benefit identifier |
| **CoverageLevel** | string? | "Individual", "Family" |
| **IsActive** | bool? | true / false |
| **EnrollmentDateFrom** | DateTime? | Start date |
| **EnrollmentDateTo** | DateTime? | End date |
| **PageNumber** | int | 1 |
| **PageSize** | int | 10 |

---

## ✅ Validations

### Create Enrollment
- ✅ EmployeeId required & exists
- ✅ BenefitId required & exists
- ✅ EnrollmentDate required, <= today
- ✅ EffectiveDate >= EnrollmentDate
- ✅ CoverageLevel max 50 chars (optional)
- ✅ Contributions >= 0 (optional)

### Update Enrollment
- ✅ Id required
- ✅ CoverageLevel max 50 chars (when provided)
- ✅ Contributions >= 0 (when provided)

### Terminate Enrollment
- ✅ Id required
- ✅ EndDate required & future date

---

## 🎯 EnrollmentResponse Properties

```csharp
EnrollmentResponse
├── Id: DefaultIdType
├── EmployeeId: DefaultIdType
├── BenefitId: DefaultIdType
├── EnrollmentDate: DateTime
├── EffectiveDate: DateTime
├── CoverageLevel: string?
├── EmployeeContributionAmount: decimal
├── EmployerContributionAmount: decimal
├── AnnualContribution: decimal
├── EndDate: DateTime?
├── IsActive: bool
└── CoveredDependentIds: string?
```

---

## 📊 Coverage Levels

| Level | Employees | Dependents |
|-------|-----------|-----------|
| **Individual** | 1 (employee only) | None |
| **Employee_Plus_Spouse** | 1 | 1 (spouse) |
| **Employee_Plus_Children** | 1 | Multiple children |
| **Family** | 1 | All family members |

---

## 🔧 Configuration

### Register Keyed Services
```csharp
services.AddKeyedScoped<IRepository<BenefitEnrollment>>("hr:enrollments");
services.AddKeyedScoped<IReadRepository<BenefitEnrollment>>("hr:enrollments");
```

### Register Handlers
```csharp
services.AddMediatR(typeof(CreateEnrollmentHandler));
services.AddMediatR(typeof(SearchEnrollmentsHandler));
services.AddMediatR(typeof(GetEnrollmentHandler));
services.AddMediatR(typeof(UpdateEnrollmentHandler));
services.AddMediatR(typeof(TerminateEnrollmentHandler));
services.AddMediatR(typeof(DeleteEnrollmentHandler));
```

### Register Validators
```csharp
services.AddValidatorsFromAssembly(typeof(CreateEnrollmentValidator).Assembly);
```

---

## 📁 Folder Structure

```
Enrollments/
├── Create/v1/ → CreateEnrollmentCommand/Handler/Validator/Response
├── Get/v1/ → GetEnrollmentRequest/Handler/EnrollmentResponse
├── Search/v1/ → SearchEnrollmentsRequest/Handler
├── Update/v1/ → UpdateEnrollmentCommand/Handler/Validator/Response
├── Terminate/v1/ → TerminateEnrollmentCommand/Handler/Validator/Response
├── Delete/v1/ → DeleteEnrollmentCommand/Handler/Response
└── Specifications/ → EnrollmentsSpecs.cs
```

---

## 📊 Domain Methods

```csharp
// Create
var enrollment = BenefitEnrollment.Create(
    employeeId, benefitId, enrollmentDate, effectiveDate);

// Set Coverage
enrollment.SetCoverage("Individual", 500m, 1000m);

// Add Dependents
enrollment.AddDependents(dependentId1, dependentId2);

// Terminate
enrollment.Terminate(endDate);
```

---

**Build Status:** ✅ SUCCESS  
**Compilation Errors:** 0  
**Ready For:** Payroll & Employee Portal


