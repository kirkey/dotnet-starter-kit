# 👥 HR & Payroll Module - Complete Implementation Plan

**Module Name:** HumanResources  
**Date Created:** November 13, 2025  
**Status:** 📋 Planning Phase  
**Priority:** 🔴 Critical for SAAS Readiness  
**Timeline:** 8-10 weeks  

---

## 📋 Executive Summary

This plan outlines the implementation of a complete **HR & Payroll module** that includes:
- ✅ Company Management (multi-entity support)
- ✅ Department & Organizational Structure
- ✅ Employee Management (full lifecycle)
- ✅ Attendance & Time Tracking
- ✅ Leave Management
- ✅ Payroll Processing
- ✅ Tax & Benefits Management
- ✅ Performance & Appraisal (basic)

**Business Impact:**
- Enables SAAS offering to 100% of businesses
- Unlocks enterprise customers (multi-company)
- Provides complete workforce management
- Integrates with Accounting module for labor distribution

---

## 🎯 Module Objectives

### Primary Goals
1. **Multi-Company Support** - Enable multiple legal entities in single tenant
2. **Complete Payroll** - Process payroll with tax calculations
3. **Time Tracking** - Track employee time for payroll and projects
4. **Leave Management** - Manage vacation, sick leave, and other leave types
5. **Organizational Structure** - Department hierarchy and reporting structure
6. **Compliance** - Support tax withholding, labor laws, and reporting

### Integration Points
- **Accounting Module** - Payroll journal entries, labor distribution
- **Project Module** - Time tracking for project costing
- **Todo Module** - Employee tasks and assignments
- **Identity/Auth** - User accounts linked to employees

---

## 📊 Module Structure

```
HumanResources/
├── HumanResources.Domain/
│   ├── Entities/
│   │   ├── Company.cs
│   │   ├── Department.cs
│   │   ├── Employee.cs
│   │   ├── EmployeeContact.cs
│   │   ├── EmployeeDependent.cs
│   │   ├── EmployeeDocument.cs
│   │   ├── Position.cs
│   │   ├── Attendance.cs
│   │   ├── Timesheet.cs
│   │   ├── TimesheetLine.cs
│   │   ├── LeaveType.cs
│   │   ├── LeaveBalance.cs
│   │   ├── LeaveRequest.cs
│   │   ├── Holiday.cs
│   │   ├── Shift.cs
│   │   ├── ShiftAssignment.cs
│   │   ├── Payroll.cs
│   │   ├── PayrollLine.cs
│   │   ├── PayrollDeduction.cs
│   │   ├── PayComponent.cs (Earnings/Deductions)
│   │   ├── TaxBracket.cs
│   │   ├── Benefit.cs
│   │   ├── BenefitEnrollment.cs
│   │   ├── PerformanceReview.cs
│   │   └── GlobalUsings.cs
│   ├── Events/
│   ├── Exceptions/
│   ├── Constants/
│   └── README.md
│
├── HumanResources.Application/
│   ├── Companies/ (CRUD + Activate/Deactivate)
│   ├── Departments/ (CRUD + Hierarchy)
│   ├── Employees/ (CRUD + Hire/Terminate/Transfer)
│   ├── Positions/ (CRUD)
│   ├── Attendance/ (ClockIn/ClockOut/Approve)
│   ├── Timesheets/ (Submit/Approve/Reject)
│   ├── LeaveRequests/ (Submit/Approve/Reject/Cancel)
│   ├── Holidays/ (CRUD)
│   ├── Shifts/ (CRUD + Assign)
│   ├── Payroll/ (Process/Approve/Post/Void)
│   ├── Benefits/ (CRUD + Enroll/Terminate)
│   ├── PerformanceReviews/ (CRUD + Submit/Approve)
│   ├── Common/
│   │   ├── Specifications/
│   │   ├── DTOs/
│   │   └── Responses/
│   └── GlobalUsings.cs
│
├── HumanResources.Infrastructure/
│   ├── Persistence/
│   │   ├── Configurations/
│   │   │   ├── CompanyConfiguration.cs
│   │   │   ├── DepartmentConfiguration.cs
│   │   │   ├── EmployeeConfiguration.cs
│   │   │   ├── AttendanceConfiguration.cs
│   │   │   ├── TimesheetConfiguration.cs
│   │   │   ├── LeaveConfiguration.cs
│   │   │   ├── PayrollConfiguration.cs
│   │   │   └── [22 more configurations]
│   │   └── HRContext.cs
│   ├── Endpoints/
│   │   ├── Companies/
│   │   ├── Departments/
│   │   ├── Employees/
│   │   ├── Attendance/
│   │   ├── Timesheets/
│   │   ├── Leaves/
│   │   ├── Payroll/
│   │   └── [7 more endpoint groups]
│   └── HumanResourcesModule.cs
│
└── HumanResourcesModule.cs (root registration)
```

---

## 🗂️ Entity Details (25 Entities)

### 1️⃣ Company Management (1 entity)

#### **Company**
Multi-entity/multi-company support for enterprises
```csharp
Properties:
- CompanyCode: string (unique, e.g., "COMP-001")
- LegalName: string (official registered name)
- TradeName: string (doing business as name)
- TaxId: string (EIN/Tax ID)
- RegistrationNumber: string (business registration)
- CompanyType: string (Corporation, LLC, Partnership, Sole Proprietor)
- BaseCurrency: string (USD, EUR, etc.)
- FiscalYearEnd: int (month 1-12)
- Address: string
- City: string
- State: string
- ZipCode: string
- Country: string
- Phone: string
- Email: string
- Website: string
- LogoUrl: string
- IsActive: bool
- ParentCompanyId: DefaultIdType? (for holding companies)
- TenantId: string (multi-tenant support)

Methods:
- Create()
- Update()
- Activate()
- Deactivate()
- SetParentCompany()

Events:
- CompanyCreated
- CompanyUpdated
- CompanyActivated
- CompanyDeactivated

Use Cases:
- Multi-entity accounting
- Consolidation reporting
- Separate payroll by legal entity
- Enterprise customer support
```

---

### 2️⃣ Organizational Structure (2 entities)

#### **Department**
Organizational units for employee grouping and reporting
```csharp
Properties:
- DepartmentCode: string (unique, e.g., "DEPT-IT")
- DepartmentName: string
- CompanyId: DefaultIdType (which company owns this dept)
- ParentDepartmentId: DefaultIdType? (for hierarchy)
- ManagerId: DefaultIdType? (Department Head/Manager)
- CostCenterId: DefaultIdType? (link to accounting cost center)
- DepartmentType: string (Operations, Sales, IT, HR, Finance, etc.)
- BudgetAmount: decimal
- HeadCount: int (current employee count)
- Location: string
- IsActive: bool

Methods:
- Create()
- Update()
- Activate()
- Deactivate()
- AssignManager()
- SetParentDepartment()

Events:
- DepartmentCreated
- DepartmentUpdated
- DepartmentManagerAssigned
- DepartmentDeactivated

Use Cases:
- Organizational hierarchy
- Departmental reporting
- Budget allocation
- Manager approval workflows
- Cost center integration
```

#### **Position**
Job positions/titles with salary ranges
```csharp
Properties:
- PositionCode: string (e.g., "POS-DEV-SR")
- PositionTitle: string (e.g., "Senior Developer")
- DepartmentId: DefaultIdType
- PositionLevel: string (Entry, Junior, Mid, Senior, Lead, Manager, Director)
- PositionType: string (Full-Time, Part-Time, Contract, Temporary)
- MinSalary: decimal
- MaxSalary: decimal
- Currency: string
- RequiredQualifications: string
- Responsibilities: string
- IsActive: bool

Methods:
- Create()
- Update()
- Activate()
- Deactivate()

Events:
- PositionCreated
- PositionUpdated

Use Cases:
- Job posting
- Salary range management
- Career progression
- Compensation planning
```

---

### 3️⃣ Employee Management (4 entities)

#### **Employee**
Core employee master data
```csharp
Properties:
- EmployeeNumber: string (unique, e.g., "EMP-2025-001")
- FirstName: string
- MiddleName: string?
- LastName: string
- PreferredName: string?
- Email: string (work email)
- PersonalEmail: string?
- Phone: string
- MobilePhone: string?
- DateOfBirth: DateTime
- Gender: string (Male, Female, Non-Binary, Prefer Not to Say)
- MaritalStatus: string (Single, Married, Divorced, Widowed)
- Nationality: string
- IdentificationType: string (SSN, Passport, National ID)
- IdentificationNumber: string (encrypted)
- Address: string
- City: string
- State: string
- ZipCode: string
- Country: string
- CompanyId: DefaultIdType
- DepartmentId: DefaultIdType
- PositionId: DefaultIdType
- ManagerId: DefaultIdType? (reports to)
- HireDate: DateTime
- TerminationDate: DateTime?
- EmploymentType: string (Full-Time, Part-Time, Contract, Intern)
- EmploymentStatus: string (Active, On Leave, Terminated, Suspended)
- PayType: string (Hourly, Salaried, Commission)
- PayRate: decimal (hourly rate or annual salary)
- PayFrequency: string (Weekly, Bi-Weekly, Semi-Monthly, Monthly)
- PaymentMethod: string (Direct Deposit, Check, Cash)
- BankAccountNumber: string? (encrypted)
- BankRoutingNumber: string?
- TaxStatus: string (W-2, 1099, etc.)
- TaxExemptions: int (number of exemptions)
- EmergencyContactName: string?
- EmergencyContactPhone: string?
- EmergencyContactRelation: string?
- ProfilePhotoUrl: string?
- UserId: string? (link to Identity user)
- IsActive: bool

Methods:
- Create() / Hire()
- Update()
- Transfer(newDepartmentId, newPositionId)
- Promote(newPositionId, newPayRate)
- Terminate(terminationDate, reason)
- Suspend()
- Reactivate()
- UpdatePayRate(newRate, effectiveDate)
- AssignManager(managerId)

Events:
- EmployeeHired
- EmployeeUpdated
- EmployeeTransferred
- EmployeePromoted
- EmployeeTerminated
- EmployeeSuspended
- EmployeePayRateChanged

Use Cases:
- Employee lifecycle management
- Payroll processing
- Organizational chart
- Performance management
- Compliance reporting
```

#### **EmployeeContact**
Additional contact information
```csharp
Properties:
- EmployeeId: DefaultIdType
- ContactType: string (Emergency, Reference, Next of Kin)
- Name: string
- Relationship: string
- Phone: string
- Email: string?
- Address: string?
- IsPrimary: bool

Methods:
- Create()
- Update()
- SetAsPrimary()
- Delete()
```

#### **EmployeeDependent**
Employee dependents for benefits and tax
```csharp
Properties:
- EmployeeId: DefaultIdType
- FirstName: string
- LastName: string
- DateOfBirth: DateTime
- Relationship: string (Spouse, Child, Parent, Other)
- Gender: string
- IdentificationNumber: string?
- IsStudent: bool
- IsDisabled: bool
- IsBeneficiary: bool
- BeneficiaryPercentage: decimal?

Methods:
- Create()
- Update()
- Delete()

Use Cases:
- Health insurance enrollment
- Tax exemptions
- Life insurance beneficiaries
```

#### **EmployeeDocument**
Document management (contracts, certifications, etc.)
```csharp
Properties:
- EmployeeId: DefaultIdType
- DocumentType: string (Contract, Certification, License, Resume, ID, etc.)
- DocumentName: string
- DocumentNumber: string?
- IssueDate: DateTime?
- ExpiryDate: DateTime?
- FileUrl: string
- UploadedBy: string
- IsVerified: bool
- Notes: string?

Methods:
- Upload()
- Update()
- Verify()
- Delete()
- CheckExpiry()

Events:
- DocumentUploaded
- DocumentExpiring (30 days before)
- DocumentExpired

Use Cases:
- Compliance tracking
- Certification management
- Contract storage
- Background check documents
```

---

### 4️⃣ Attendance & Time Tracking (6 entities)

#### **Attendance**
Daily attendance tracking (clock in/out)
```csharp
Properties:
- EmployeeId: DefaultIdType
- AttendanceDate: DateTime (date only)
- ClockIn: DateTime?
- ClockOut: DateTime?
- TotalHours: decimal (calculated)
- Status: string (Present, Absent, Late, Half-Day, On Leave)
- AttendanceType: string (Regular, Overtime, Weekend, Holiday)
- Location: string? (for geo-fencing)
- ClockInLatitude: double?
- ClockInLongitude: double?
- ClockOutLatitude: double?
- ClockOutLongitude: double?
- Notes: string?
- ApprovedBy: DefaultIdType?
- ApprovalStatus: string (Pending, Approved, Rejected)

Methods:
- ClockIn(location?)
- ClockOut(location?)
- MarkAbsent(reason)
- MarkLate()
- Approve(approverId)
- Reject(approverId, reason)

Events:
- EmployeeClockedIn
- EmployeeClockedOut
- AttendanceMarkedAbsent
- AttendanceApproved

Use Cases:
- Time tracking
- Attendance reports
- Late tracking
- Geo-location verification
- Overtime calculation
```

#### **Timesheet**
Weekly/bi-weekly timesheet for payroll
```csharp
Properties:
- EmployeeId: DefaultIdType
- PeriodStart: DateTime
- PeriodEnd: DateTime
- TotalRegularHours: decimal
- TotalOvertimeHours: decimal
- TotalHours: decimal
- Status: string (Draft, Submitted, Approved, Rejected, Paid)
- SubmittedDate: DateTime?
- ApprovedBy: DefaultIdType?
- ApprovedDate: DateTime?
- PayrollId: DefaultIdType? (linked when processed)
- Notes: string?

Methods:
- Create()
- AddLine()
- Submit()
- Approve(approverId)
- Reject(approverId, reason)
- Recall()

Events:
- TimesheetCreated
- TimesheetSubmitted
- TimesheetApproved
- TimesheetRejected

Use Cases:
- Payroll time entry
- Project billing
- Labor cost tracking
- Approval workflow
```

#### **TimesheetLine**
Daily timesheet breakdown
```csharp
Properties:
- TimesheetId: DefaultIdType
- Date: DateTime
- ProjectId: DefaultIdType? (if project billing)
- TaskId: DefaultIdType? (specific task)
- CustomerId: DefaultIdType? (if billable)
- RegularHours: decimal
- OvertimeHours: decimal
- DoubleTimeHours: decimal
- IsBillable: bool
- BillingRate: decimal?
- CostRate: decimal?
- Description: string?
- Notes: string?

Methods:
- Create()
- Update()
- Delete()

Use Cases:
- Detailed time tracking
- Project costing
- Client billing
- Labor distribution
```

#### **Shift**
Shift templates (e.g., Morning, Evening, Night)
```csharp
Properties:
- ShiftCode: string (e.g., "SHIFT-MORNING")
- ShiftName: string (e.g., "Morning Shift")
- StartTime: TimeSpan (e.g., 08:00)
- EndTime: TimeSpan (e.g., 17:00)
- BreakDuration: int (minutes)
- TotalHours: decimal
- ShiftType: string (Regular, Night, Weekend, Rotating)
- IsActive: bool

Methods:
- Create()
- Update()
- Activate()
- Deactivate()

Use Cases:
- Shift scheduling
- Roster management
- Overtime calculation
```

#### **ShiftAssignment**
Assign shifts to employees
```csharp
Properties:
- EmployeeId: DefaultIdType
- ShiftId: DefaultIdType
- AssignmentDate: DateTime
- EffectiveFrom: DateTime
- EffectiveTo: DateTime?
- IsRecurring: bool
- RecurrencePattern: string? (Daily, Weekly, etc.)
- Status: string (Active, Completed, Cancelled)

Methods:
- Assign()
- Update()
- Cancel()

Events:
- ShiftAssigned
- ShiftCancelled

Use Cases:
- Employee scheduling
- Roster planning
- Shift swapping
```

#### **Holiday**
Company holidays calendar
```csharp
Properties:
- CompanyId: DefaultIdType
- HolidayName: string
- HolidayDate: DateTime
- HolidayType: string (National, Company, Religious, Optional)
- IsPaid: bool
- IsRecurring: bool
- Description: string?

Methods:
- Create()
- Update()
- Delete()

Use Cases:
- Holiday calendar
- Payroll processing
- Leave calculation
- Attendance tracking
```

---

### 5️⃣ Leave Management (3 entities)

#### **LeaveType**
Leave type definition
```csharp
Properties:
- LeaveCode: string (e.g., "VAC", "SICK", "MAT")
- LeaveName: string (e.g., "Vacation", "Sick Leave")
- CompanyId: DefaultIdType
- IsPaid: bool
- AccrualRate: decimal? (days per month)
- MaxAccrual: decimal? (max days can accumulate)
- MaxConsecutiveDays: int?
- RequiresApproval: bool
- RequiresMedicalCertificate: bool (for sick leave)
- IsCarryForward: bool (unused balance to next year)
- GenderSpecific: string? (for maternity/paternity)
- IsActive: bool

Methods:
- Create()
- Update()
- Activate()
- Deactivate()

Use Cases:
- Leave policy management
- Accrual calculation
- Leave tracking
```

#### **LeaveBalance**
Employee leave balance tracking
```csharp
Properties:
- EmployeeId: DefaultIdType
- LeaveTypeId: DefaultIdType
- Year: int
- OpeningBalance: decimal
- Accrued: decimal
- Taken: decimal
- Adjusted: decimal
- CarryForward: decimal
- CurrentBalance: decimal (calculated)
- LastAccrualDate: DateTime?

Methods:
- Create()
- Accrue(days)
- Deduct(days)
- Adjust(days, reason)
- CarryForward()

Use Cases:
- Leave balance tracking
- Accrual automation
- Leave reporting
```

#### **LeaveRequest**
Employee leave application
```csharp
Properties:
- EmployeeId: DefaultIdType
- LeaveTypeId: DefaultIdType
- StartDate: DateTime
- EndDate: DateTime
- TotalDays: decimal (working days)
- Reason: string
- ContactDuringLeave: string?
- Status: string (Draft, Submitted, Approved, Rejected, Cancelled)
- SubmittedDate: DateTime?
- ApprovedBy: DefaultIdType?
- ApprovedDate: DateTime?
- RejectedReason: string?
- CancellationReason: string?
- AttachmentUrl: string? (medical certificate, etc.)

Methods:
- Create()
- Submit()
- Approve(approverId)
- Reject(approverId, reason)
- Cancel(reason)

Events:
- LeaveRequested
- LeaveApproved
- LeaveRejected
- LeaveCancelled

Use Cases:
- Leave application
- Approval workflow
- Leave calendar
- Balance deduction
```

---

### 6️⃣ Payroll Management (5 entities)

#### **Payroll**
Payroll processing header
```csharp
Properties:
- PayrollNumber: string (unique, e.g., "PAY-2025-11-001")
- CompanyId: DefaultIdType
- PeriodStart: DateTime
- PeriodEnd: DateTime
- PayDate: DateTime
- PayFrequency: string (Weekly, Bi-Weekly, Semi-Monthly, Monthly)
- TotalEmployees: int
- TotalGrossPay: decimal
- TotalDeductions: decimal
- TotalTaxes: decimal
- TotalNetPay: decimal
- Status: string (Draft, Processing, Approved, Posted, Paid)
- ProcessedBy: DefaultIdType?
- ProcessedDate: DateTime?
- ApprovedBy: DefaultIdType?
- ApprovedDate: DateTime?
- PostedToGL: bool
- GLJournalEntryId: DefaultIdType? (link to accounting)
- PaymentBatchId: string? (for bank file)

Methods:
- Create()
- ProcessPayroll()
- Approve(approverId)
- Post() (to GL)
- Void(reason)
- GeneratePaymentFile()

Events:
- PayrollCreated
- PayrollProcessed
- PayrollApproved
- PayrollPosted
- PayrollVoided

Use Cases:
- Payroll processing
- Tax calculation
- GL posting
- Payment generation
```

#### **PayrollLine**
Individual employee payroll detail
```csharp
Properties:
- PayrollId: DefaultIdType
- EmployeeId: DefaultIdType
- RegularHours: decimal
- OvertimeHours: decimal
- DoubleTimeHours: decimal
- RegularPay: decimal
- OvertimePay: decimal
- DoubleTimePay: decimal
- GrossPay: decimal
- TotalDeductions: decimal
- TotalTaxes: decimal
- NetPay: decimal
- PaymentMethod: string
- BankAccountNumber: string? (masked)
- CheckNumber: string?
- PaymentStatus: string (Pending, Paid, Void)

Methods:
- Calculate()
- UpdatePayment()
- Void()

Use Cases:
- Employee pay calculation
- Payslip generation
- Payment processing
```

#### **PayrollDeduction**
Payroll deduction details (taxes, benefits, garnishments)
```csharp
Properties:
- PayrollLineId: DefaultIdType
- PayComponentId: DefaultIdType
- DeductionType: string (Tax, Benefit, Garnishment, Loan, Other)
- DeductionName: string
- Amount: decimal
- IsPreTax: bool
- CalculationBasis: string (Fixed, Percentage, Tiered)
- IsEmployerContribution: bool (for benefits)
- EmployerAmount: decimal?

Methods:
- Create()
- Calculate()

Use Cases:
- Tax withholding
- Benefits deduction
- Loan repayment
- Garnishments
```

#### **PayComponent**
Payroll component definition (earnings/deductions)
```csharp
Properties:
- ComponentCode: string (e.g., "FED-TAX", "401K")
- ComponentName: string
- CompanyId: DefaultIdType
- ComponentType: string (Earning, Deduction, Tax, Employer Tax)
- CalculationType: string (Fixed, Percentage, Tiered, Formula)
- DefaultAmount: decimal?
- DefaultPercent: decimal?
- IsActive: bool
- GLAccountId: DefaultIdType? (for accounting)
- IsStatutory: bool (required by law)

Methods:
- Create()
- Update()
- Activate()
- Deactivate()

Use Cases:
- Payroll configuration
- Component management
- GL integration
```

#### **TaxBracket**
Tax rate configuration
```csharp
Properties:
- TaxName: string (Federal, State, FICA, Medicare, etc.)
- Jurisdiction: string (US, CA, UK, etc.)
- TaxYear: int
- FilingStatus: string (Single, Married, Head of Household)
- MinIncome: decimal
- MaxIncome: decimal?
- TaxRate: decimal
- FlatAmount: decimal
- IsActive: bool

Methods:
- Create()
- Update()
- CalculateTax(grossPay, filingStatus)

Use Cases:
- Tax calculation
- Withholding automation
- Tax compliance
```

---

### 7️⃣ Benefits Management (2 entities)

#### **Benefit**
Company benefit programs
```csharp
Properties:
- BenefitCode: string (e.g., "HEALTH-PPO")
- BenefitName: string (e.g., "Health Insurance PPO")
- CompanyId: DefaultIdType
- BenefitType: string (Health, Dental, Vision, Life, 401k, HSA, FSA)
- Provider: string
- Description: string
- EmployeeContribution: decimal
- EmployeeContributionType: string (Fixed, Percentage)
- EmployerContribution: decimal
- EmployerContributionType: string
- IsPreTax: bool
- WaitingPeriodDays: int
- IsActive: bool
- EnrollmentStartDate: DateTime?
- EnrollmentEndDate: DateTime?

Methods:
- Create()
- Update()
- Activate()
- Deactivate()

Use Cases:
- Benefit administration
- Open enrollment
- Payroll deduction
```

#### **BenefitEnrollment**
Employee benefit enrollment
```csharp
Properties:
- EmployeeId: DefaultIdType
- BenefitId: DefaultIdType
- EnrollmentDate: DateTime
- EffectiveDate: DateTime
- TerminationDate: DateTime?
- CoverageLevel: string (Employee, Employee+Spouse, Family)
- EmployeeContribution: decimal
- EmployerContribution: decimal
- Status: string (Active, Pending, Terminated)
- Dependents: List<DependentId>

Methods:
- Enroll()
- Update()
- Terminate(date, reason)

Events:
- BenefitEnrolled
- BenefitTerminated

Use Cases:
- Benefit enrollment
- Payroll deduction
- Dependent coverage
```

---

### 8️⃣ Performance Management (1 entity - Basic)

#### **PerformanceReview**
Basic performance review tracking
```csharp
Properties:
- EmployeeId: DefaultIdType
- ReviewerId: DefaultIdType (manager/supervisor)
- ReviewPeriodStart: DateTime
- ReviewPeriodEnd: DateTime
- ReviewDate: DateTime?
- ReviewType: string (Annual, Probation, Quarterly, Mid-Year)
- OverallRating: int (1-5)
- Goals: string
- Achievements: string
- AreasOfImprovement: string
- ManagerComments: string?
- EmployeeComments: string?
- Status: string (Draft, Submitted, Completed)

Methods:
- Create()
- Submit()
- Complete()

Events:
- ReviewCreated
- ReviewSubmitted
- ReviewCompleted

Use Cases:
- Annual reviews
- Performance tracking
- Promotion decisions
- Basic appraisal
```

---

## 🔄 Implementation Phases

### **Phase 1: Foundation (Week 1-2)**
**Goal:** Core structure and company/department setup

**Entities (3):**
1. Company
2. Department
3. Position

**Deliverables:**
- ✅ Domain entities with full documentation
- ✅ Database configurations
- ✅ CRUD operations for all 3 entities
- ✅ API endpoints
- ✅ Unit tests
- ✅ Integration with Accounting (CostCenter link)

**Success Criteria:**
- Can create multiple companies
- Can create department hierarchy
- Can define job positions

---

### **Phase 2: Employee Management (Week 3-4)**
**Goal:** Complete employee lifecycle management

**Entities (4):**
1. Employee
2. EmployeeContact
3. EmployeeDependent
4. EmployeeDocument

**Deliverables:**
- ✅ Employee master data management
- ✅ Hire/Transfer/Promote/Terminate workflows
- ✅ Contact and dependent management
- ✅ Document storage integration
- ✅ Employee search and filtering
- ✅ Organizational chart support

**Success Criteria:**
- Can hire new employees
- Can manage employee lifecycle
- Can store employee documents
- Can view organizational structure

---

### **Phase 3: Attendance & Time (Week 5-6)**
**Goal:** Time tracking and attendance management

**Entities (6):**
1. Attendance
2. Timesheet
3. TimesheetLine
4. Shift
5. ShiftAssignment
6. Holiday

**Deliverables:**
- ✅ Clock in/out functionality
- ✅ Daily attendance tracking
- ✅ Timesheet submission and approval
- ✅ Shift management
- ✅ Holiday calendar
- ✅ Time reports

**Success Criteria:**
- Employees can clock in/out
- Managers can approve timesheets
- System calculates overtime
- Shift scheduling works

---

### **Phase 4: Leave Management (Week 6-7)**
**Goal:** Complete leave/vacation management

**Entities (3):**
1. LeaveType
2. LeaveBalance
3. LeaveRequest

**Deliverables:**
- ✅ Leave type configuration
- ✅ Automatic accrual calculation
- ✅ Leave request submission
- ✅ Approval workflow
- ✅ Leave calendar integration
- ✅ Balance reports

**Success Criteria:**
- Leave accrues automatically
- Employees can request leave
- Managers can approve/reject
- Balance updates correctly

---

### **Phase 5: Payroll Processing (Week 7-8)**
**Goal:** Complete payroll calculation and processing

**Entities (5):**
1. Payroll
2. PayrollLine
3. PayrollDeduction
4. PayComponent
5. TaxBracket

**Deliverables:**
- ✅ Payroll processing engine
- ✅ Tax calculation
- ✅ Deduction management
- ✅ Payslip generation
- ✅ GL posting integration
- ✅ Payment file generation
- ✅ Payroll reports

**Success Criteria:**
- Can process payroll end-to-end
- Taxes calculate correctly
- Posts to GL automatically
- Generates payment files

---

### **Phase 6: Benefits & Performance (Week 9-10)**
**Goal:** Benefits enrollment and basic performance

**Entities (3):**
1. Benefit
2. BenefitEnrollment
3. PerformanceReview

**Deliverables:**
- ✅ Benefit configuration
- ✅ Employee enrollment
- ✅ Payroll integration
- ✅ Basic performance reviews
- ✅ Documentation
- ✅ Final testing

**Success Criteria:**
- Employees can enroll in benefits
- Deductions work in payroll
- Can conduct performance reviews
- Module fully documented

---

## 🔗 Integration Points

### 1. Accounting Module Integration
```csharp
// Payroll Journal Entry Creation
When: Payroll.Post()
Creates: JournalEntry with lines:
  - Debit: Salary Expense (by department/cost center)
  - Debit: Payroll Tax Expense
  - Debit: Benefits Expense
  - Credit: Cash (net pay)
  - Credit: Payroll Liabilities (taxes, garnishments)

// Time Tracking to Project Costing
When: Timesheet.Approve()
Creates: ProjectCost entry for labor
Updates: Project.ActualLaborCost

// Department to Cost Center
Links: Department.CostCenterId → CostCenter.Id
Purpose: Labor cost allocation
```

### 2. Identity/Auth Integration
```csharp
// Employee to User Mapping
Employee.UserId → Identity.Users.Id
Purpose: 
- Employee portal login
- Self-service features
- Manager permissions

// Role-Based Access
Roles:
- HR Administrator (full access)
- Payroll Administrator (payroll only)
- Department Manager (own team only)
- Employee (self-service only)
```

### 3. Todo Module Integration
```csharp
// Employee Tasks
When: LeaveRequest.Submit()
Creates: TodoItem for manager approval

When: Timesheet.Submit()
Creates: TodoItem for manager approval

When: PerformanceReview.Due()
Creates: TodoItem for manager and employee
```

### 4. File Storage Integration
```csharp
// Document Upload
EmployeeDocument.FileUrl → Blob Storage
Company.LogoUrl → Blob Storage
Employee.ProfilePhotoUrl → Blob Storage
LeaveRequest.AttachmentUrl → Blob Storage
```

---

## 📊 Database Schema Highlights

### Key Relationships
```sql
Company (1) ─── (N) Department
Company (1) ─── (N) Employee
Department (1) ─── (N) Employee
Employee (1) ─── (1) Position
Employee (1) ─── (N) Attendance
Employee (1) ─── (N) Timesheet
Employee (1) ─── (N) LeaveBalance
Employee (1) ─── (N) BenefitEnrollment
Department (1) ─── (N) Position
Payroll (1) ─── (N) PayrollLine
PayrollLine (1) ─── (N) PayrollDeduction
```

### Critical Indexes
```sql
-- Performance critical
IX_Employee_CompanyId_Status
IX_Employee_DepartmentId
IX_Employee_ManagerId
IX_Attendance_EmployeeId_Date
IX_Timesheet_EmployeeId_Period
IX_LeaveRequest_EmployeeId_Status
IX_Payroll_CompanyId_PayDate
IX_PayrollLine_PayrollId_EmployeeId

-- Unique constraints
UQ_Company_CompanyCode
UQ_Department_Code_CompanyId
UQ_Employee_EmployeeNumber
UQ_Payroll_PayrollNumber
```

---

## 🧪 Testing Strategy

### Unit Tests
- ✅ Domain logic (30+ tests per entity)
- ✅ Validation rules
- ✅ Business rule enforcement
- ✅ Calculation accuracy (payroll, time, leave)

### Integration Tests
- ✅ API endpoints (CRUD operations)
- ✅ Workflow processes (hire, payroll, leave)
- ✅ Cross-module integration (GL posting)
- ✅ Authorization rules

### End-to-End Tests
- ✅ Complete payroll cycle
- ✅ Employee lifecycle (hire to terminate)
- ✅ Leave request and approval
- ✅ Timesheet submission and payroll

---

## 📈 Success Metrics

### Functionality Metrics
- ✅ 25 entities fully implemented
- ✅ 150+ API endpoints
- ✅ 500+ unit tests
- ✅ 100+ integration tests
- ✅ 20+ workflow processes

### Business Metrics
- ✅ Payroll processing time < 2 hours for 1000 employees
- ✅ Leave request approval < 5 clicks
- ✅ Timesheet submission < 10 minutes/week
- ✅ Attendance tracking < 30 seconds/day
- ✅ 99.9% payroll accuracy

### Code Quality Metrics
- ✅ 100% entity documentation
- ✅ 90%+ test coverage
- ✅ 0 critical security issues
- ✅ All CQRS patterns followed
- ✅ All validation implemented

---

## 🚀 Quick Start Implementation

### Week 1 Deliverables (Nov 13-17, 2025)
**Day 1: Setup**
- Create HumanResources module structure
- Setup Domain project
- Create Company entity + tests

**Day 2: Company Management**
- Company CRUD operations
- Company endpoints
- Integration tests

**Day 3: Department**
- Department entity + hierarchy
- Department CRUD
- Manager assignment

**Day 4: Position**
- Position entity
- Position CRUD
- Salary range validation

**Day 5: Infrastructure**
- All configurations
- Repository registrations
- Endpoint mappings
- Documentation

**Week 1 Demo:** Multi-company with department hierarchy

---

### Week 2 Deliverables (Nov 18-24, 2025)
**Day 1-2: Employee Entity**
- Complete employee master
- Validation rules
- Domain events

**Day 3-4: Employee Operations**
- Hire workflow
- Transfer/Promote
- Terminate process

**Day 5-7: Employee Related**
- EmployeeContact
- EmployeeDependent
- EmployeeDocument
- Search and filtering

**Week 2 Demo:** Complete employee management

---

## 📚 Documentation Requirements

### Entity Documentation
- ✅ XML summary for each entity
- ✅ Property documentation with examples
- ✅ Method documentation
- ✅ Use case documentation
- ✅ Business rule documentation

### API Documentation
- ✅ Swagger/OpenAPI specs
- ✅ Request/response examples
- ✅ Error codes and messages
- ✅ Rate limiting rules
- ✅ Authentication requirements

### User Documentation
- ✅ Administrator guide
- ✅ Manager guide
- ✅ Employee self-service guide
- ✅ Payroll processing guide
- ✅ FAQ and troubleshooting

### Developer Documentation
- ✅ Architecture overview
- ✅ Entity relationship diagrams
- ✅ Integration guides
- ✅ Extension points
- ✅ Testing guidelines

---

## 🔐 Security Considerations

### Data Protection
- ✅ Encrypt sensitive data (SSN, bank accounts)
- ✅ PII compliance (GDPR, CCPA)
- ✅ Audit trail for all changes
- ✅ Row-level security (manager access)
- ✅ Data retention policies

### Access Control
- ✅ Role-based permissions
- ✅ Department-level access
- ✅ Employee self-service restrictions
- ✅ Payroll data segregation
- ✅ Manager approval workflows

---

## 💰 Cost Estimate

### Development Costs
- **Phase 1:** $15K (2 weeks, 2 developers)
- **Phase 2:** $20K (2 weeks, 2 developers)
- **Phase 3:** $20K (2 weeks, 2 developers)
- **Phase 4:** $15K (1 week, 2 developers)
- **Phase 5:** $25K (2 weeks, 2 developers + architect)
- **Phase 6:** $15K (2 weeks, 1 developer)
- **Total:** $110K

### Timeline
- **Start:** November 13, 2025
- **Phase 1 Complete:** November 24, 2025
- **Phase 2 Complete:** December 8, 2025
- **Phase 3 Complete:** December 22, 2025
- **Phase 4 Complete:** December 29, 2025
- **Phase 5 Complete:** January 12, 2026
- **Phase 6 Complete:** January 26, 2026
- **Final Delivery:** January 26, 2026 (10 weeks)

---

## ✅ Next Steps

### Immediate Actions (This Week)
1. ✅ **Approve Plan** - Review and approve this implementation plan
2. ✅ **Create Module Structure** - Setup HumanResources module folders
3. ✅ **Start Phase 1** - Begin Company entity implementation
4. ✅ **Setup CI/CD** - Configure build and test pipelines
5. ✅ **Database Planning** - Design schema and migrations

### Week 2 Actions
1. ✅ Complete Phase 1 (Company, Department, Position)
2. ✅ Demo to stakeholders
3. ✅ Begin Phase 2 (Employee management)
4. ✅ Setup test data and seed scripts

---

## 📞 Support & Questions

**Technical Lead:** [Assign Name]  
**Project Manager:** [Assign Name]  
**Review Cycle:** Weekly demos every Friday  
**Documentation:** Update wiki after each phase  

---

## 🎯 Success Definition

This HR & Payroll module will be considered **COMPLETE** when:

✅ All 25 entities implemented with full CRUD  
✅ All workflows functional (hire, payroll, leave, attendance)  
✅ Integration with Accounting module working  
✅ 90%+ test coverage achieved  
✅ Can process payroll for 1000+ employees  
✅ Can handle multi-company scenarios  
✅ All documentation complete  
✅ Security audit passed  
✅ Performance benchmarks met  
✅ Demo to stakeholders approved  

**Estimated Completion:** January 26, 2026

---

**Ready to start? Let's build this! 🚀**

