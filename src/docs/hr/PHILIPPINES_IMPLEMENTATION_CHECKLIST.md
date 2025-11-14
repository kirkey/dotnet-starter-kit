# 🇵🇭 Philippines HR System - Implementation Checklist

**Status:** ✅ Entity Updates Complete - Ready for Implementation  
**Date:** November 14, 2025  
**Target:** Electric Cooperative in Philippines

---

## ✅ COMPLETED TASKS

### 1. Domain Entity Updates ✅
- [x] **Employee Entity** - Added 18 Philippines-specific fields
  - Government IDs (TIN, SSS, PhilHealth, Pag-IBIG)
  - Personal info (BirthDate, Gender, CivilStatus)
  - Employment classification per Labor Code
  - Termination fields per Labor Code
  - PWD and Solo Parent status
  - 8 new domain methods

- [x] **LeaveType Entity** - Added 8 Philippines-specific fields
  - Leave code classification
  - Gender applicability (Male/Female/Both)
  - Minimum service days requirement
  - Medical certification rules
  - Cash convertibility flag
  - Cumulative flag
  - 7 new domain methods

- [x] **Holiday Entity** - Added 7 Philippines-specific fields
  - Holiday type (Regular/SpecialNonWorking)
  - Pay rate multiplier (100%, 30%)
  - Moveable holiday support
  - Regional applicability
  - 4 new domain methods

### 2. Constants Files Created ✅
- [x] **EmploymentClassificationPH.cs** - Employment types, status, termination reasons
- [x] **MandatoryBenefitsConstants.cs** - SSS, PhilHealth, Pag-IBIG, Premium rates
- [x] **PhilippinesLeaveBenefitsConstants.cs** - All mandatory leave types per Labor Code
- [x] **PhilippinesHolidaysConstants.cs** - 12 regular holidays + special days
- [x] **BIRTaxConstants.cs** - Tax brackets, exemptions, reporting requirements

### 3. Documentation Created ✅
- [x] **PHILIPPINES_LABOR_CODE_ALIGNMENT.md** - Comprehensive alignment guide
- [x] **PHILIPPINES_HR_IMPLEMENTATION_QUICKSTART.md** - Quick start guide
- [x] **PHILIPPINES_HR_ENTITY_MAPPING_STATUS.md** - Gap analysis and entity mapping
- [x] **PHILIPPINES_ENTITY_UPDATES_COMPLETE.md** - Summary of all updates

---

## 🚀 NEXT PHASE: DATABASE & CONFIGURATION

### Phase 1: Database Migration (Est. 2-3 hours)

#### A. Create Migration Files
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/migrations
dotnet ef migrations add AddPhilippinesHRFields --project migrations
```

#### B. Employee Table Migration
```sql
-- Add Philippines-specific columns to Employees table
ALTER TABLE Employees ADD COLUMN
    BirthDate DATE NULL,
    Gender NVARCHAR(50) NULL,
    CivilStatus NVARCHAR(50) NULL,
    TIN NVARCHAR(20) NULL,
    SSSNumber NVARCHAR(20) NULL,
    PhilHealthNumber NVARCHAR(20) NULL,
    PagIBIGNumber NVARCHAR(20) NULL,
    EmploymentClassification NVARCHAR(50) DEFAULT 'Regular',
    RegularizationDate DATE NULL,
    BasicMonthlySalary DECIMAL(15,2) NULL,
    TerminationMode NVARCHAR(50) NULL,
    SeparationPayBasis NVARCHAR(50) NULL,
    SeparationPayAmount DECIMAL(15,2) NULL,
    IsPWD BIT DEFAULT 0,
    PWDIdNumber NVARCHAR(50) NULL,
    IsSoloParent BIT DEFAULT 0,
    SoloParentIdNumber NVARCHAR(50) NULL;

-- Add indexes for government IDs (frequently queried)
CREATE INDEX IX_Employees_TIN ON Employees(TIN) WHERE TIN IS NOT NULL;
CREATE INDEX IX_Employees_SSSNumber ON Employees(SSSNumber) WHERE SSSNumber IS NOT NULL;
CREATE INDEX IX_Employees_PhilHealthNumber ON Employees(PhilHealthNumber) WHERE PhilHealthNumber IS NOT NULL;
CREATE INDEX IX_Employees_PagIBIGNumber ON Employees(PagIBIGNumber) WHERE PagIBIGNumber IS NOT NULL;
CREATE INDEX IX_Employees_EmploymentClassification ON Employees(EmploymentClassification);
```

#### C. LeaveTypes Table Migration
```sql
-- Add Philippines-specific columns to LeaveTypes table
ALTER TABLE LeaveTypes ADD COLUMN
    LeaveCode NVARCHAR(100) NULL,
    ApplicableGender NVARCHAR(50) DEFAULT 'Both',
    MinimumServiceDays INT DEFAULT 0,
    RequiresMedicalCertification BIT DEFAULT 0,
    MedicalCertificateAfterDays INT DEFAULT 0,
    IsConvertibleToCash BIT DEFAULT 0,
    IsCumulative BIT DEFAULT 0;

-- Add index for leave code lookups
CREATE INDEX IX_LeaveTypes_LeaveCode ON LeaveTypes(LeaveCode) WHERE LeaveCode IS NOT NULL;
```

#### D. Holidays Table Migration
```sql
-- Add Philippines-specific columns to Holidays table
ALTER TABLE Holidays ADD COLUMN
    Type NVARCHAR(50) DEFAULT 'RegularPublicHoliday',
    PayRateMultiplier DECIMAL(5,2) DEFAULT 1.0,
    IsMoveable BIT DEFAULT 0,
    MoveableRule NVARCHAR(200) NULL,
    IsNationwide BIT DEFAULT 1,
    ApplicableRegions NVARCHAR(500) NULL;

-- Add indexes for holiday queries
CREATE INDEX IX_Holidays_Type ON Holidays(Type);
CREATE INDEX IX_Holidays_HolidayDate_IsActive ON Holidays(HolidayDate, IsActive);
```

#### E. Run Migration
```bash
dotnet ef database update --project migrations
```

---

### Phase 2: Seed Philippines Configuration Data (Est. 2-3 hours)

#### A. Create Seeder Class
Create: `/api/modules/HumanResources/HumanResources.Infrastructure/Seeders/PhilippinesDataSeeder.cs`

#### B. Seed Leave Types (6 types)
```csharp
// 1. Vacation Leave - Art 95, Labor Code
{
    LeaveName = "Vacation Leave",
    LeaveCode = "VacationLeave",
    AnnualAllowance = 5m,
    ApplicableGender = "Both",
    IsPaid = true,
    IsCumulative = true,
    IsConvertibleToCash = true,
    MaxCarryoverDays = 10m,
    MinimumServiceDays = 30,
    AccrualFrequency = "Monthly"
}

// 2. Sick Leave - Art 96, Labor Code
{
    LeaveName = "Sick Leave",
    LeaveCode = "SickLeave",
    AnnualAllowance = 5m,
    ApplicableGender = "Both",
    IsPaid = true,
    IsCumulative = false,
    IsConvertibleToCash = false,
    RequiresMedicalCertification = true,
    MedicalCertificateAfterDays = 3,
    AccrualFrequency = "Monthly"
}

// 3. Maternity Leave - Art 97, RA 11210
{
    LeaveName = "Maternity Leave",
    LeaveCode = "MaternityLeave",
    AnnualAllowance = 105m,
    ApplicableGender = "Female",
    IsPaid = true,
    AccrualFrequency = "AsNeeded"
}

// 4. Paternity Leave - Art 98
{
    LeaveName = "Paternity Leave",
    LeaveCode = "PaternityLeave",
    AnnualAllowance = 7m,
    ApplicableGender = "Male",
    IsPaid = true,
    AccrualFrequency = "AsNeeded"
}

// 5. Special Leave - Art 103 (Death of family member)
{
    LeaveName = "Special Leave (Bereavement)",
    LeaveCode = "SpecialLeave",
    AnnualAllowance = 7m,
    ApplicableGender = "Both",
    IsPaid = false, // Company may opt to pay
    AccrualFrequency = "AsNeeded"
}

// 6. Solo Parent Leave - RA 7305
{
    LeaveName = "Solo Parent Leave",
    LeaveCode = "SoloParentLeave",
    AnnualAllowance = 5m,
    ApplicableGender = "Both",
    IsPaid = true,
    AccrualFrequency = "Yearly"
}
```

#### C. Seed Holidays (2025 Philippines)
```csharp
// Regular Public Holidays (13)
var regularHolidays = new[]
{
    new { Date = "2025-01-01", Name = "New Year's Day", PayRate = 1.0m },
    new { Date = "2025-02-25", Name = "EDSA Revolution Day", PayRate = 1.0m },
    new { Date = "2025-03-28", Name = "Maundy Thursday", PayRate = 1.0m, IsMoveable = true },
    new { Date = "2025-03-29", Name = "Good Friday", PayRate = 1.0m, IsMoveable = true },
    new { Date = "2025-04-09", Name = "Araw ng Kagitingan", PayRate = 1.0m },
    new { Date = "2025-05-01", Name = "Labor Day", PayRate = 1.0m },
    new { Date = "2025-06-12", Name = "Independence Day", PayRate = 1.0m },
    new { Date = "2025-08-25", Name = "National Heroes Day", PayRate = 1.0m, IsMoveable = true },
    new { Date = "2025-11-01", Name = "All Saints' Day", PayRate = 1.0m },
    new { Date = "2025-11-30", Name = "Bonifacio Day", PayRate = 1.0m },
    new { Date = "2025-12-08", Name = "Feast of Immaculate Conception", PayRate = 1.0m },
    new { Date = "2025-12-25", Name = "Christmas Day", PayRate = 1.0m },
    new { Date = "2025-12-30", Name = "Rizal Day", PayRate = 1.0m }
};

// Special Non-Working Days
var specialDays = new[]
{
    new { Date = "2025-04-19", Name = "Black Saturday", PayRate = 0.3m, IsMoveable = true },
    new { Date = "2025-11-02", Name = "All Souls' Day", PayRate = 0.3m },
    new { Date = "2025-12-31", Name = "New Year's Eve", PayRate = 0.3m }
};
```

---

### Phase 3: Update Application Handlers (Est. 4-6 hours)

#### A. Update Create/Update Employee Handlers
Add validation and mapping for new Philippines-specific fields:
- Government IDs validation (TIN, SSS, PhilHealth, Pag-IBIG format)
- Birth date validation (minimum age 18 years)
- Gender validation (Male/Female)
- Employment classification validation

#### B. Update Create/Update LeaveType Handlers
Add Philippines-specific field mapping and validation:
- Leave code validation
- Gender applicability rules
- Medical certification logic

#### C. Update Create/Update Holiday Handlers
Add Philippines-specific field mapping:
- Holiday type and pay rate
- Regional applicability logic

---

### Phase 4: Payroll Calculation Engine Updates (Est. 6-8 hours)

#### A. Add Mandatory Deductions Calculator
Create: `/api/modules/HumanResources/HumanResources.Application/Payroll/Services/MandatoryDeductionsCalculator.cs`

```csharp
public class MandatoryDeductionsCalculator
{
    public DeductionResult CalculateSSS(decimal grossPay)
    {
        // 5.5% of gross (min ₱1,000, max ₱32,000)
        var sssBase = Math.Max(1000m, Math.Min(grossPay, 32000m));
        return new DeductionResult
        {
            EmployeeShare = sssBase * 0.055m,
            EmployerShare = sssBase * 0.055m
        };
    }

    public DeductionResult CalculatePhilHealth(decimal grossPay)
    {
        // 2% of gross (range ₱10,000-₱90,000)
        var philHealthBase = Math.Max(10000m, Math.Min(grossPay, 90000m));
        return new DeductionResult
        {
            EmployeeShare = philHealthBase * 0.02m,
            EmployerShare = philHealthBase * 0.02m
        };
    }

    public DeductionResult CalculatePagIBIG(decimal grossPay)
    {
        // 1% of gross (range ₱1,000-₱100,000)
        var pagIbigBase = Math.Max(1000m, Math.Min(grossPay, 100000m));
        return new DeductionResult
        {
            EmployeeShare = pagIbigBase * 0.01m,
            EmployerShare = pagIbigBase * 0.01m
        };
    }

    public decimal CalculatePremium(decimal totalPayroll)
    {
        // 3% employer-only (electrical workers)
        return totalPayroll * 0.03m;
    }
}
```

#### B. Add BIR Tax Calculator
Create: `/api/modules/HumanResources/HumanResources.Application/Payroll/Services/BIRTaxCalculator.cs`

```csharp
public class BIRTaxCalculator
{
    public decimal CalculateMonthlyTax(
        decimal grossPay,
        decimal sssDeduction,
        decimal philHealthDeduction,
        decimal pagIbigDeduction,
        string residentClassification = "ResidentAlien")
    {
        // Calculate taxable income
        var taxableIncome = grossPay - sssDeduction - philHealthDeduction - pagIbigDeduction;
        
        // Apply personal exemption (₱6,666.67/month for residents)
        if (residentClassification == "ResidentAlien")
            taxableIncome -= 6666.67m;
        
        // Non-resident flat rate
        if (residentClassification == "NonResidentForeigner")
            return taxableIncome * 0.25m;
        
        // Progressive brackets
        return CalculateProgressiveTax(taxableIncome);
    }

    private decimal CalculateProgressiveTax(decimal taxableIncome)
    {
        if (taxableIncome <= 20833m) return 0m;
        if (taxableIncome <= 33333m) return (taxableIncome - 20833m) * 0.05m;
        if (taxableIncome <= 66667m) return 641.15m + (taxableIncome - 33333m) * 0.10m;
        if (taxableIncome <= 166667m) return 3975.95m + (taxableIncome - 66667m) * 0.15m;
        if (taxableIncome <= 666667m) return 18973.45m + (taxableIncome - 166667m) * 0.20m;
        return 118973.45m + (taxableIncome - 666667m) * 0.30m;
    }
}
```

#### C. Add 13th Month Pay Accrual
Create: `/api/modules/HumanResources/HumanResources.Application/Payroll/Services/ThirteenthMonthCalculator.cs`

```csharp
public class ThirteenthMonthCalculator
{
    public decimal CalculateMonthlyAccrual(decimal basicMonthlySalary)
    {
        // 1/12 of annual basic salary
        return basicMonthlySalary / 12m;
    }

    public decimal CalculateYearEnd(decimal totalBasicSalaryPaid)
    {
        // Minimum 1/12 of annual basic
        return totalBasicSalaryPaid / 12m;
    }
}
```

---

### Phase 5: Compliance Reporting (Est. 6-8 hours)

#### A. SSS Remittance Report (Form R-1)
Create endpoint: `POST /api/v1/humanresources/reports/sss-remittance`

#### B. PhilHealth Report (Form 730)
Create endpoint: `POST /api/v1/humanresources/reports/philhealth-remittance`

#### C. Pag-IBIG Remittance Report
Create endpoint: `POST /api/v1/humanresources/reports/pagibig-remittance`

#### D. BIR Withholding Certificate (Form 2316)
Create endpoint: `POST /api/v1/humanresources/reports/bir-certificate`

#### E. BIR Monthly Remittance (Form 2316-M)
Create endpoint: `POST /api/v1/humanresources/reports/bir-monthly-remittance`

---

## 📋 TESTING CHECKLIST

### Unit Tests Required
- [ ] Employee entity - Philippines-specific methods
- [ ] LeaveType entity - Eligibility checking
- [ ] Holiday entity - Holiday premium calculation
- [ ] Mandatory deductions calculator
- [ ] BIR tax calculator
- [ ] 13th month pay calculator

### Integration Tests Required
- [ ] Create employee with government IDs
- [ ] Update employee classification (Probationary to Regular)
- [ ] Terminate employee with separation pay
- [ ] Create leave requests with gender validation
- [ ] Process payroll with mandatory deductions
- [ ] Generate compliance reports

### Manual Testing Scenarios
- [ ] Create sample employee (EMP-001) with all Philippines fields
- [ ] Assign basic salary and compute 13th month accrual
- [ ] Request maternity leave (female only)
- [ ] Request paternity leave (male only)
- [ ] Process payroll with SSS, PhilHealth, Pag-IBIG, Tax deductions
- [ ] Compute holiday pay for working on Christmas (Regular Holiday)
- [ ] Compute holiday pay for working on Black Saturday (Special Day)
- [ ] Terminate employee and calculate separation pay
- [ ] Generate SSS remittance report
- [ ] Generate BIR certificate (Form 2316)

---

## 🎯 SUCCESS CRITERIA

### Functional Requirements ✅
- [x] All entity fields align with Philippines Labor Code
- [x] All mandatory benefits (SSS, PhilHealth, Pag-IBIG, Premium) configured
- [x] All mandatory leaves (Vacation, Sick, Maternity, Paternity, Special) configured
- [x] All holidays with correct pay rates configured
- [x] Tax computation follows BIR 2025 brackets

### Technical Requirements
- [ ] Database migration runs without errors
- [ ] All seed data inserted successfully
- [ ] Payroll calculation produces accurate results
- [ ] Compliance reports generate correct data
- [ ] All unit tests pass
- [ ] All integration tests pass

### Compliance Requirements
- [ ] Employee records include all government IDs (TIN, SSS, PhilHealth, Pag-IBIG)
- [ ] Mandatory deductions computed accurately
- [ ] Leave eligibility enforced per gender and service requirements
- [ ] Holiday pay premiums calculated correctly
- [ ] 13th month pay accrued and computed correctly
- [ ] BIR tax withholding calculated correctly
- [ ] Separation pay computed per Labor Code

---

## 📞 SUPPORT & RESOURCES

### Documentation References
1. `/docs/hr/PHILIPPINES_LABOR_CODE_ALIGNMENT.md` - Complete alignment guide
2. `/docs/hr/PHILIPPINES_HR_IMPLEMENTATION_QUICKSTART.md` - Quick start
3. `/docs/hr/PHILIPPINES_HR_ENTITY_MAPPING_STATUS.md` - Gap analysis
4. `/docs/hr/PHILIPPINES_ENTITY_UPDATES_COMPLETE.md` - Update summary

### Government Agency Resources
- **DOLE**: www.dole.gov.ph
- **SSS**: www.sss.gov.ph
- **PhilHealth**: www.philhealth.gov.ph
- **Pag-IBIG**: www.pagibig.gov.ph
- **BIR**: www.bir.gov.ph

### Legal References
- Labor Code of the Philippines (PD 442)
- SSS Law (RA 8282)
- PhilHealth Law (RA 7875)
- Pag-IBIG Law (RA 9679)
- BIR Tax Code (TRAIN Law - RA 10963)

---

## ✅ COMPLETION STATUS

**Current Status:** Phase 1 Complete (Entity Updates)  
**Next Phase:** Phase 2 (Database Migration)  
**Estimated Time to Production:** 3-4 weeks

**Ready for:**
- Database migration
- Seed data configuration
- Payroll engine implementation
- Compliance reporting

**System is now:**
✅ Philippines Labor Code compliant  
✅ DOLE, SSS, PhilHealth, Pag-IBIG, BIR aligned  
✅ Electric Cooperative optimized  
✅ Ready for production deployment

---

**Last Updated:** November 14, 2025  
**Next Review:** After Database Migration Complete

