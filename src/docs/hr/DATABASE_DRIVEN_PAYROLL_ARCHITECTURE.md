# 🗄️ Database-Driven Payroll System - Philippine Labor Law Compliance

**Date:** November 14, 2025  
**Status:** ✅ Architecture Complete  
**Approach:** Fully configurable, database-driven payroll

---

## 🎯 OVERVIEW

This document describes the **database-driven approach** to payroll configuration where:
- ✅ All pay components are stored in the database
- ✅ Philippine labor law rates (SSS, PhilHealth, Pag-IBIG, tax brackets) are configurable
- ✅ Formulas and calculations are data-driven, not hard-coded
- ✅ Administrators can modify rates without code changes
- ✅ Historical rate tracking for audit compliance
- ✅ Employee-specific overrides and custom additions

---

## 📊 NEW ENTITY ARCHITECTURE

### Updated Entities (3)

1. **PayComponent** (ENHANCED) - 24 new fields
   - Configuration master for all earnings, deductions, taxes
   - Supports multiple calculation methods
   - Labor law reference tracking

2. **PayComponentRate** (NEW)
   - Stores brackets and rates (SSS, PhilHealth, Pag-IBIG, tax)
   - Year-specific configurations
   - Historical rate tracking

3. **EmployeePayComponent** (NEW)
   - Per-employee overrides and additions
   - Custom allowances and deductions
   - Loan tracking with installments

---

## 🏗️ ENTITY DETAILS

### 1. PayComponent (Master Configuration)

```csharp
public class PayComponent : AuditableEntity, IAggregateRoot
{
    // Identification
    public string Code { get; set; }              // "SSS_EE", "BASIC_PAY", "OT_REGULAR"
    public string ComponentName { get; set; }     // "SSS Employee Share"
    public string ComponentType { get; set; }     // Earnings, Deduction, Tax, EmployerContribution
    
    // Calculation Configuration
    public string CalculationMethod { get; set; } // Manual, Formula, Percentage, Bracket, Fixed
    public string? CalculationFormula { get; set; } // "HourlyRate * OvertimeHours * 1.25"
    public decimal? Rate { get; set; }            // 0.10 for 10% night differential
    public decimal? FixedAmount { get; set; }     // Fixed allowance amount
    public decimal? MinValue { get; set; }        // Minimum cap
    public decimal? MaxValue { get; set; }        // Maximum cap
    
    // Accounting
    public string GlAccountCode { get; set; }     // "6100" for Salaries Expense
    
    // Flags
    public bool IsActive { get; set; }
    public bool IsCalculated { get; set; }        // Auto-calculated by system
    public bool IsMandatory { get; set; }         // Mandatory per Philippine law
    public bool IsSubjectToTax { get; set; }      // Included in taxable income
    public bool IsTaxExempt { get; set; }         // Tax-exempt (de minimis, etc)
    public bool AffectsGrossPay { get; set; }
    public bool AffectsNetPay { get; set; }
    
    // Compliance
    public string? LaborLawReference { get; set; } // "Labor Code Art 87", "PD 851"
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }         // Order in payslip
    
    // Relationship
    public ICollection<PayComponentRate> Rates { get; set; } // For bracket-based calculations
}
```

**Calculation Methods:**

| Method | Description | Example |
|--------|-------------|---------|
| **Manual** | Entered manually per payroll | Basic Pay |
| **Formula** | Uses formula with variables | `HourlyRate * OvertimeHours * 1.25` |
| **Percentage** | Fixed % of base amount | Night Differential (10% of basic) |
| **Bracket** | Uses PayComponentRate table | SSS, PhilHealth, Pag-IBIG, Tax |
| **Fixed** | Fixed amount per payroll | Transportation Allowance ₱2,000 |

---

### 2. PayComponentRate (Brackets & Rates)

```csharp
public class PayComponentRate : AuditableEntity, IAggregateRoot
{
    // Parent
    public DefaultIdType PayComponentId { get; set; }
    
    // Bracket Range
    public decimal MinAmount { get; set; }        // ₱4,000 (SSS min)
    public decimal MaxAmount { get; set; }        // ₱4,250 (SSS bracket)
    
    // Contribution Rates (for SSS, PhilHealth, Pag-IBIG)
    public decimal? EmployeeRate { get; set; }    // 0.045 = 4.5%
    public decimal? EmployerRate { get; set; }    // 0.095 = 9.5%
    public decimal? AdditionalEmployerRate { get; set; } // 0.01 = 1% (SSS EC)
    
    // Fixed Amounts (alternative to rates)
    public decimal? EmployeeAmount { get; set; }
    public decimal? EmployerAmount { get; set; }
    
    // Tax Brackets (for income tax)
    public decimal? TaxRate { get; set; }         // 0.15 = 15%
    public decimal? BaseAmount { get; set; }      // ₱22,500
    public decimal? ExcessRate { get; set; }      // 0.20 = 20% of excess
    
    // Versioning
    public int Year { get; set; }                 // 2025
    public DateTime? EffectiveStartDate { get; set; }
    public DateTime? EffectiveEndDate { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}
```

---

### 3. EmployeePayComponent (Per-Employee)

```csharp
public class EmployeePayComponent : AuditableEntity, IAggregateRoot
{
    // Relationships
    public DefaultIdType EmployeeId { get; set; }
    public DefaultIdType PayComponentId { get; set; }
    
    // Assignment Type
    public string AssignmentType { get; set; }    // Standard, Override, Addition, OneTime
    
    // Custom Configuration
    public decimal? CustomRate { get; set; }      // Override standard rate
    public decimal? FixedAmount { get; set; }     // Fixed amount for this employee
    public string? CustomFormula { get; set; }    // Custom formula
    
    // Dates
    public DateTime EffectiveStartDate { get; set; }
    public DateTime? EffectiveEndDate { get; set; }
    public bool IsActive { get; set; }
    
    // Recurrence
    public bool IsRecurring { get; set; }         // Every payroll
    public bool IsOneTime { get; set; }           // One-time only
    public DateTime? OneTimeDate { get; set; }
    
    // Loan Tracking
    public int? InstallmentCount { get; set; }    // 12 months
    public int? CurrentInstallment { get; set; }  // Payment #3
    public decimal? TotalAmount { get; set; }     // ₱60,000
    public decimal? RemainingBalance { get; set; } // ₱45,000
    
    // Reference
    public string? ReferenceNumber { get; set; }  // Loan ID, Order ID
    public DefaultIdType? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? Remarks { get; set; }
}
```

---

## 📝 CONFIGURATION EXAMPLES

### Example 1: SSS Employee Contribution

**PayComponent Record:**
```json
{
  "code": "SSS_EE",
  "componentName": "SSS Employee Share",
  "componentType": "Deduction",
  "calculationMethod": "Bracket",
  "glAccountCode": "2120",
  "isCalculated": true,
  "isMandatory": true,
  "isSubjectToTax": false,
  "laborLawReference": "SSS Law RA 11199",
  "description": "Social Security System employee contribution (4.5%)"
}
```

**PayComponentRate Records (Brackets):**
```json
[
  {
    "payComponentId": "{SSS_EE_ID}",
    "minAmount": 4000,
    "maxAmount": 4250,
    "employeeRate": 0.045,
    "employerRate": 0.095,
    "additionalEmployerRate": 0.01,
    "year": 2025
  },
  {
    "payComponentId": "{SSS_EE_ID}",
    "minAmount": 4250,
    "maxAmount": 4750,
    "employeeRate": 0.045,
    "employerRate": 0.095,
    "additionalEmployerRate": 0.01,
    "year": 2025
  },
  // ... more brackets up to ₱35,000
]
```

---

### Example 2: Overtime Regular (125%)

**PayComponent Record:**
```json
{
  "code": "OT_REGULAR",
  "componentName": "Overtime Pay (Regular Day)",
  "componentType": "Earnings",
  "calculationMethod": "Formula",
  "calculationFormula": "HourlyRate * OvertimeHours * 1.25",
  "rate": 1.25,
  "glAccountCode": "6110",
  "isCalculated": true,
  "isMandatory": false,
  "isSubjectToTax": true,
  "affectsGrossPay": true,
  "laborLawReference": "Labor Code Article 87",
  "description": "Overtime pay at 125% rate"
}
```

**No PayComponentRate needed** - uses formula directly.

---

### Example 3: Night Differential (10%)

**PayComponent Record:**
```json
{
  "code": "NIGHT_DIFF",
  "componentName": "Night Differential",
  "componentType": "Earnings",
  "calculationMethod": "Percentage",
  "rate": 0.10,
  "glAccountCode": "6130",
  "isCalculated": true,
  "isMandatory": false,
  "isSubjectToTax": true,
  "affectsGrossPay": true,
  "laborLawReference": "Labor Code Article 86",
  "description": "10% additional pay for 10PM-6AM shift"
}
```

**No PayComponentRate needed** - uses fixed 10% rate.

---

### Example 4: Withholding Tax

**PayComponent Record:**
```json
{
  "code": "WITHHOLDING_TAX",
  "componentName": "Withholding Tax",
  "componentType": "Tax",
  "calculationMethod": "Bracket",
  "glAccountCode": "2110",
  "isCalculated": true,
  "isMandatory": true,
  "isSubjectToTax": false,
  "laborLawReference": "TRAIN Law RA 10963",
  "description": "Income tax withholding per BIR"
}
```

**PayComponentRate Records (Tax Brackets 2025):**
```json
[
  {
    "payComponentId": "{WITHHOLDING_TAX_ID}",
    "minAmount": 0,
    "maxAmount": 20833,
    "baseAmount": 0,
    "excessRate": 0.00,
    "year": 2025,
    "description": "₱20,833 and below - EXEMPT"
  },
  {
    "payComponentId": "{WITHHOLDING_TAX_ID}",
    "minAmount": 20833.01,
    "maxAmount": 33332,
    "baseAmount": 0,
    "excessRate": 0.15,
    "year": 2025,
    "description": "Over ₱20,833 to ₱33,332 - 15%"
  },
  // ... more brackets
]
```

---

### Example 5: Transportation Allowance (Fixed)

**PayComponent Record:**
```json
{
  "code": "TRANS_ALLOW",
  "componentName": "Transportation Allowance",
  "componentType": "Earnings",
  "calculationMethod": "Fixed",
  "fixedAmount": 2000,
  "glAccountCode": "6130",
  "isCalculated": false,
  "isMandatory": false,
  "isSubjectToTax": true,
  "isTaxExempt": false,
  "affectsGrossPay": true,
  "description": "Monthly transportation allowance"
}
```

**EmployeePayComponent Record (Per Employee):**
```json
{
  "employeeId": "{EMPLOYEE_ID}",
  "payComponentId": "{TRANS_ALLOW_ID}",
  "assignmentType": "Addition",
  "fixedAmount": 2000,
  "effectiveStartDate": "2025-01-01",
  "isRecurring": true
}
```

---

### Example 6: Employee Loan Deduction

**PayComponent Record:**
```json
{
  "code": "COOP_LOAN",
  "componentName": "Cooperative Loan",
  "componentType": "Deduction",
  "calculationMethod": "Manual",
  "glAccountCode": "2140",
  "isCalculated": false,
  "isMandatory": false,
  "isSubjectToTax": false,
  "laborLawReference": "Labor Code Article 111",
  "description": "Authorized cooperative loan deduction"
}
```

**EmployeePayComponent Record (Loan):**
```json
{
  "employeeId": "{EMPLOYEE_ID}",
  "payComponentId": "{COOP_LOAN_ID}",
  "assignmentType": "Addition",
  "fixedAmount": 5000,
  "totalAmount": 60000,
  "remainingBalance": 60000,
  "installmentCount": 12,
  "currentInstallment": 0,
  "effectiveStartDate": "2025-01-01",
  "referenceNumber": "LOAN-2025-001",
  "isRecurring": true
}
```

---

## 🔄 PAYROLL CALCULATION WORKFLOW

### Step 1: Load Standard Pay Components
```csharp
var standardComponents = await payComponentRepo.ListAsync(
    new ActivePayComponentsSpec());
// Returns: Basic Pay, SSS, PhilHealth, Pag-IBIG, Withholding Tax, etc.
```

### Step 2: Load Employee-Specific Components
```csharp
var employeeComponents = await employeePayComponentRepo.ListAsync(
    new EmployeePayComponentsByEmployeeIdSpec(employeeId));
// Returns: Transportation Allowance, Cooperative Loan, etc.
```

### Step 3: Calculate Each Component

**For Bracket-Based (SSS):**
```csharp
var sssComponent = standardComponents.First(x => x.Code == "SSS_EE");
var basicSalary = employee.BasicMonthlySalary.Value;

// Find applicable bracket
var bracket = sssComponent.Rates
    .Where(r => r.Year == 2025 && r.IsActive)
    .FirstOrDefault(r => basicSalary >= r.MinAmount && basicSalary <= r.MaxAmount);

var employeeShare = basicSalary * bracket.EmployeeRate;
var employerShare = basicSalary * bracket.EmployerRate;
var ec = basicSalary * bracket.AdditionalEmployerRate;
```

**For Formula-Based (Overtime):**
```csharp
var otComponent = standardComponents.First(x => x.Code == "OT_REGULAR");
var hourlyRate = employee.BasicMonthlySalary.Value / 22 / 8;

// Parse formula: "HourlyRate * OvertimeHours * 1.25"
var overtimePay = hourlyRate * overtimeHours * otComponent.Rate;
```

**For Percentage-Based (Night Differential):**
```csharp
var nightDiffComponent = standardComponents.First(x => x.Code == "NIGHT_DIFF");
var nightDiffPay = hoursWorkedAtNight * hourlyRate * nightDiffComponent.Rate;
```

**For Employee-Specific:**
```csharp
var empComponents = employeeComponents.Where(x => x.IsActive);
foreach (var empComp in empComponents)
{
    if (empComp.IsOneTime && empComp.OneTimeDate != payrollDate)
        continue; // Skip if not for this payroll
    
    var amount = empComp.FixedAmount ?? 0;
    // Add to earnings or deductions based on PayComponent.ComponentType
}
```

---

## 📊 DATABASE SCHEMA

### PayComponents Table
```sql
CREATE TABLE PayComponents (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TenantId NVARCHAR(64) NOT NULL,
    Code NVARCHAR(50) NOT NULL,
    ComponentName NVARCHAR(200) NOT NULL,
    ComponentType NVARCHAR(50) NOT NULL,
    CalculationMethod NVARCHAR(50) NOT NULL,
    CalculationFormula NVARCHAR(500) NULL,
    Rate DECIMAL(18,6) NULL,
    FixedAmount DECIMAL(15,2) NULL,
    MinValue DECIMAL(15,2) NULL,
    MaxValue DECIMAL(15,2) NULL,
    GlAccountCode NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL,
    IsCalculated BIT NOT NULL,
    IsMandatory BIT NOT NULL,
    IsSubjectToTax BIT NOT NULL,
    IsTaxExempt BIT NOT NULL,
    LaborLawReference NVARCHAR(200) NULL,
    Description NVARCHAR(1000) NULL,
    DisplayOrder INT NOT NULL DEFAULT 0,
    AffectsGrossPay BIT NOT NULL DEFAULT 0,
    AffectsNetPay BIT NOT NULL DEFAULT 1,
    CreatedBy NVARCHAR(128) NOT NULL,
    CreatedOn DATETIME2 NOT NULL,
    LastModifiedBy NVARCHAR(128) NULL,
    LastModifiedOn DATETIME2 NULL,
    DeletedOn DATETIME2 NULL,
    DeletedBy NVARCHAR(128) NULL,
    
    CONSTRAINT IX_PayComponents_Code_Tenant UNIQUE (TenantId, Code),
    INDEX IX_PayComponents_Type (ComponentType),
    INDEX IX_PayComponents_Active (IsActive)
);
```

### PayComponentRates Table
```sql
CREATE TABLE PayComponentRates (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TenantId NVARCHAR(64) NOT NULL,
    PayComponentId UNIQUEIDENTIFIER NOT NULL,
    MinAmount DECIMAL(15,2) NOT NULL,
    MaxAmount DECIMAL(15,2) NOT NULL,
    EmployeeRate DECIMAL(18,6) NULL,
    EmployerRate DECIMAL(18,6) NULL,
    AdditionalEmployerRate DECIMAL(18,6) NULL,
    EmployeeAmount DECIMAL(15,2) NULL,
    EmployerAmount DECIMAL(15,2) NULL,
    TaxRate DECIMAL(18,6) NULL,
    BaseAmount DECIMAL(15,2) NULL,
    ExcessRate DECIMAL(18,6) NULL,
    Year INT NOT NULL,
    EffectiveStartDate DATETIME2 NULL,
    EffectiveEndDate DATETIME2 NULL,
    IsActive BIT NOT NULL,
    Description NVARCHAR(500) NULL,
    CreatedBy NVARCHAR(128) NOT NULL,
    CreatedOn DATETIME2 NOT NULL,
    LastModifiedBy NVARCHAR(128) NULL,
    LastModifiedOn DATETIME2 NULL,
    
    FOREIGN KEY (PayComponentId) REFERENCES PayComponents(Id),
    INDEX IX_PayComponentRates_Component (PayComponentId),
    INDEX IX_PayComponentRates_Year (Year),
    INDEX IX_PayComponentRates_Range (MinAmount, MaxAmount)
);
```

### EmployeePayComponents Table
```sql
CREATE TABLE EmployeePayComponents (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TenantId NVARCHAR(64) NOT NULL,
    EmployeeId UNIQUEIDENTIFIER NOT NULL,
    PayComponentId UNIQUEIDENTIFIER NOT NULL,
    AssignmentType NVARCHAR(50) NOT NULL,
    CustomRate DECIMAL(18,6) NULL,
    FixedAmount DECIMAL(15,2) NULL,
    CustomFormula NVARCHAR(500) NULL,
    EffectiveStartDate DATETIME2 NOT NULL,
    EffectiveEndDate DATETIME2 NULL,
    IsActive BIT NOT NULL,
    IsRecurring BIT NOT NULL,
    IsOneTime BIT NOT NULL,
    OneTimeDate DATETIME2 NULL,
    InstallmentCount INT NULL,
    CurrentInstallment INT NULL,
    TotalAmount DECIMAL(15,2) NULL,
    RemainingBalance DECIMAL(15,2) NULL,
    ReferenceNumber NVARCHAR(100) NULL,
    ApprovedBy UNIQUEIDENTIFIER NULL,
    ApprovedDate DATETIME2 NULL,
    Remarks NVARCHAR(1000) NULL,
    CreatedBy NVARCHAR(128) NOT NULL,
    CreatedOn DATETIME2 NOT NULL,
    LastModifiedBy NVARCHAR(128) NULL,
    LastModifiedOn DATETIME2 NULL,
    
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (PayComponentId) REFERENCES PayComponents(Id),
    INDEX IX_EmployeePayComponents_Employee (EmployeeId),
    INDEX IX_EmployeePayComponents_Active (IsActive),
    INDEX IX_EmployeePayComponents_Dates (EffectiveStartDate, EffectiveEndDate)
);
```

---

## 🎯 BENEFITS

### 1. Full Configurability
✅ No code changes to update rates
✅ Administrators manage via UI
✅ Immediate effect on payroll calculations

### 2. Compliance Made Easy
✅ Update SSS/PhilHealth/Pag-IBIG rates annually
✅ Update tax brackets when TRAIN Law changes
✅ Historical tracking for audits

### 3. Flexibility
✅ Add new pay components anytime
✅ Custom formulas without coding
✅ Employee-specific overrides

### 4. Audit Trail
✅ Track who changed what and when
✅ Historical rate preservation
✅ Year-by-year versioning

### 5. Multi-Tenant Support
✅ Different configurations per tenant
✅ Company-specific pay components
✅ Isolated data per cooperative

---

## 🚀 NEXT STEPS

1. **Create database migration** for new fields and tables
2. **Build seeder** to populate Philippine standard components
3. **Create CRUD APIs** for PayComponent, PayComponentRate, EmployeePayComponent
4. **Build payroll calculation engine** that reads from these tables
5. **Create admin UI** for managing pay components
6. **Build employee portal** for viewing assignments

---

## 📚 REFERENCE

- Labor Code of the Philippines
- SSS Law (RA 11199)
- PhilHealth Law (RA 7875)
- Pag-IBIG Law (RA 9679)
- TRAIN Law (RA 10963)
- DOLE Labor Advisories

---

**Document Version:** 1.0  
**Last Updated:** November 14, 2025  
**Next Review:** After implementation

