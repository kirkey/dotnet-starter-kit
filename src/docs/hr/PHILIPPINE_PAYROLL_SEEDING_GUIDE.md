# 🌱 Philippine Payroll Seeding Guide

**Date:** November 16, 2025  
**Status:** Implementation Guide  
**For:** Database-Driven Payroll System

---

## 📋 OVERVIEW

This document provides comprehensive seed data specifications for the Philippine payroll system components and rates based on current labor laws (2025).

---

## 🎯 IMPLEMENTATION STATUS

✅ **PayComponent Entity** - Complete with fluent methods  
✅ **PayComponentRate Entity** - Complete with fluent methods  
✅ **EmployeePayComponent Entity** - Complete with fluent methods  
✅ **DbContext Configuration** - PayComponentRates and EmployeePayComponents added  
⏳ **PhilippinePayrollSeeder** - In progress (syntax corrections needed)  
⏳ **Integration** - Partially integrated into HumanResourcesDbInitializer

---

## 📊 SEED DATA SUMMARY

### Components to Seed

1. **Basic Pay** (1 component)
2. **SSS Contributions** (3 components + 30 rate brackets)
3. **PhilHealth Contributions** (2 components + 3 rate brackets)
4. **Pag-IBIG Contributions** (2 components + 2 rate brackets)
5. **Withholding Tax** (1 component + 6 tax brackets)
6. **Overtime Pay** (4 components)
7. **Premium Pay** (4 components)
8. **Allowances & Benefits** (5 components)
9. **Deductions** (7 components)
10. **13th Month Pay** (1 component)

**Total:** 30 PayComponents + 41 PayComponentRates

---

## 🔧 CORRECT IMPLEMENTATION PATTERN

### Creating a Pay Component

```csharp
// Step 1: Create the component
var component = PayComponent.Create(
    code: "COMPONENT_CODE",
    componentName: "Display Name",
    componentType: "Earnings|Deduction|Tax|EmployerContribution",
    calculationMethod: "Manual|Formula|Percentage|Bracket|Fixed",
    glAccountCode: "1234");

// Step 2: Update basic properties
component.Update(
    calculationFormula: "optional formula",
    rate: 1.25m,  // optional
    fixedAmount: 2000m,  // optional
    description: "Description text",
    displayOrder: 10);

// Step 3: Set flags using entity methods
component.SetAutoCalculated(true);  // or false
component.SetMandatory("Labor Law Reference");  // if mandatory
component.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);
component.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);

// Step 4: Save
await context.PayComponents.AddAsync(component, cancellationToken);
await context.SaveChangesAsync(cancellationToken);
```

### Creating a Pay Component Rate

```csharp
// Step 1: Create the rate
var rate = PayComponentRate.Create(
    payComponentId: component.Id,
    minAmount: 4000m,
    maxAmount: 4249.99m,
    year: 2025);

// Step 2: Update rate details
rate.Update(
    employeeRate: 0.045m,  // 4.5%
    employerRate: 0.095m,  // 9.5%
    additionalEmployerRate: 0.01m,  // 1% EC
    description: "MSC ₱4,000");

// Step 3: Save
await context.PayComponentRates.AddAsync(rate, cancellationToken);
await context.SaveChangesAsync(cancellationToken);
```

### Alternative: For Tax Brackets

```csharp
var taxBracket = PayComponentRate.CreateTaxBracket(
    payComponentId: withholdingTax.Id,
    minAmount: 20833.01m,
    maxAmount: 33332m,
    baseAmount: 0m,
    excessRate: 0.15m,
    year: 2025);

taxBracket.Update(description: "Over ₱250,000 to ₱400,000 annual - 15%");

await context.PayComponentRates.AddAsync(taxBracket, cancellationToken);
```

### Alternative: For Fixed Amounts

```csharp
var fixedRate = PayComponentRate.CreateFixedRate(
    payComponentId: philHealth.Id,
    minAmount: 100000.01m,
    maxAmount: 999999.99m,
    employeeAmount: 2500m,
    employerAmount: 2500m,
    year: 2025);

fixedRate.Update(description: "Maximum cap ₱5,000 total");

await context.PayComponentRates.AddAsync(fixedRate, cancellationToken);
```

---

## 📝 COMPLETE SEED DATA SPECIFICATIONS

### 1. Basic Pay

```csharp
var basicPay = PayComponent.Create(
    code: "BASIC_PAY",
    componentName: "Basic Pay",
    componentType: "Earnings",
    calculationMethod: "Manual",
    glAccountCode: "6100");

basicPay.Update(
    description: "Regular basic salary or wage",
    displayOrder: 1);

basicPay.SetAutoCalculated(false);
basicPay.SetMandatory();
basicPay.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);
basicPay.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);
```

### 2. SSS Employee Contribution (2025 Rates)

#### Component

```csharp
var sssEmployee = PayComponent.Create(
    code: "SSS_EE",
    componentName: "SSS Employee Share",
    componentType: "Deduction",
    calculationMethod: "Bracket",
    glAccountCode: "2120");

sssEmployee.Update(
    description: "Social Security System employee contribution (4.5% of monthly salary credit)",
    displayOrder: 100);

sssEmployee.SetAutoCalculated();
sssEmployee.SetMandatory("SSS Law RA 11199, SSS Circular 2024");
sssEmployee.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
sssEmployee.SetPayImpact(affectsGrossPay: false, affectsNetPay: true);
```

#### Rate Brackets (Sample - 30 total needed)

```csharp
// Bracket 1: ₱4,000 - ₱4,249.99
var sssBracket1 = PayComponentRate.CreateContributionRate(
    payComponentId: sssEmployee.Id,
    minAmount: 4000m,
    maxAmount: 4249.99m,
    employeeRate: 0.045m,  // 4.5%
    employerRate: 0.095m,  // 9.5%
    year: 2025,
    additionalEmployerRate: 0.01m);  // 1% EC

sssBracket1.Update(description: "MSC ₱4,000");

// ... repeat for all 30 brackets up to ₱30,000
```

### 3. PhilHealth Employee Contribution (2025 Rates)

#### Component

```csharp
var philHealthEmployee = PayComponent.Create(
    code: "PHIC_EE",
    componentName: "PhilHealth Employee Share",
    componentType: "Deduction",
    calculationMethod: "Bracket",
    glAccountCode: "2121");

philHealthEmployee.Update(
    description: "PhilHealth employee premium (2.5% of monthly basic salary, max ₱100,000)",
    displayOrder: 110);

philHealthEmployee.SetAutoCalculated();
philHealthEmployee.SetMandatory("PhilHealth Law RA 7875, PhilHealth Circular 2024-0001");
philHealthEmployee.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
philHealthEmployee.SetPayImpact(affectsGrossPay: false, affectsNetPay: true);
```

#### Rate Brackets

```csharp
// Below minimum: Fixed ₱250 each
var philHealthBracket1 = PayComponentRate.CreateFixedRate(
    payComponentId: philHealthEmployee.Id,
    minAmount: 0m,
    maxAmount: 9999.99m,
    employeeAmount: 250m,
    employerAmount: 250m,
    year: 2025);

philHealthBracket1.Update(description: "Below minimum - Fixed ₱500 total");

// Standard rate: 2.5% each
var philHealthBracket2 = PayComponentRate.CreateContributionRate(
    payComponentId: philHealthEmployee.Id,
    minAmount: 10000m,
    maxAmount: 100000m,
    employeeRate: 0.025m,
    employerRate: 0.025m,
    year: 2025);

philHealthBracket2.Update(description: "2.5% each (5% total)");

// Maximum: Fixed ₱2,500 each
var philHealthBracket3 = PayComponentRate.CreateFixedRate(
    payComponentId: philHealthEmployee.Id,
    minAmount: 100000.01m,
    maxAmount: 999999.99m,
    employeeAmount: 2500m,
    employerAmount: 2500m,
    year: 2025);

philHealthBracket3.Update(description: "Maximum cap ₱5,000 total");
```

### 4. Pag-IBIG Employee Contribution (2025 Rates)

#### Component

```csharp
var pagibigEmployee = PayComponent.Create(
    code: "HDMF_EE",
    componentName: "Pag-IBIG Employee Share",
    componentType: "Deduction",
    calculationMethod: "Bracket",
    glAccountCode: "2122");

pagibigEmployee.Update(
    description: "Pag-IBIG Fund employee contribution (1-2% of monthly compensation)",
    displayOrder: 120);

pagibigEmployee.SetAutoCalculated();
pagibigEmployee.SetMandatory("Pag-IBIG Law RA 9679");
pagibigEmployee.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
pagibigEmployee.SetPayImpact(affectsGrossPay: false, affectsNetPay: true);
```

#### Rate Brackets

```csharp
// 1% for ₱1,500 and below
var pagibigBracket1 = PayComponentRate.CreateContributionRate(
    payComponentId: pagibigEmployee.Id,
    minAmount: 0m,
    maxAmount: 1500m,
    employeeRate: 0.01m,
    employerRate: 0.02m,
    year: 2025);

pagibigBracket1.Update(description: "Employee 1%, Employer 2%");

// 2% for above ₱1,500
var pagibigBracket2 = PayComponentRate.CreateContributionRate(
    payComponentId: pagibigEmployee.Id,
    minAmount: 1500.01m,
    maxAmount: 999999.99m,
    employeeRate: 0.02m,
    employerRate: 0.02m,
    year: 2025);

pagibigBracket2.Update(description: "Employee 2%, Employer 2%");
```

### 5. Withholding Tax (TRAIN Law 2025)

#### Component

```csharp
var withholdingTax = PayComponent.Create(
    code: "WITHHOLDING_TAX",
    componentName: "Withholding Tax",
    componentType: "Tax",
    calculationMethod: "Bracket",
    glAccountCode: "2110");

withholdingTax.Update(
    description: "Income tax withholding per BIR (graduated rates)",
    displayOrder: 130);

withholdingTax.SetAutoCalculated();
withholdingTax.SetMandatory("TRAIN Law RA 10963, BIR Revenue Regulations");
withholdingTax.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
withholdingTax.SetPayImpact(affectsGrossPay: false, affectsNetPay: true);
```

#### Tax Brackets

```csharp
// Bracket 1: ₱20,833 and below - EXEMPT
var taxBracket1 = PayComponentRate.CreateTaxBracket(
    payComponentId: withholdingTax.Id,
    minAmount: 0m,
    maxAmount: 20833m,
    baseAmount: 0m,
    excessRate: 0m,
    year: 2025);

taxBracket1.Update(description: "₱250,000 annual - TAX EXEMPT");

// Bracket 2: Over ₱20,833 to ₱33,332 - 15%
var taxBracket2 = PayComponentRate.CreateTaxBracket(
    payComponentId: withholdingTax.Id,
    minAmount: 20833.01m,
    maxAmount: 33332m,
    baseAmount: 0m,
    excessRate: 0.15m,
    year: 2025);

taxBracket2.Update(description: "Over ₱250K to ₱400K - 15% of excess");

// ... Continue for all 6 tax brackets
```

### 6. Overtime Pay Components

```csharp
// Regular overtime (125%)
var otRegular = PayComponent.Create(
    code: "OT_REGULAR",
    componentName: "Overtime Pay (Regular Day)",
    componentType: "Earnings",
    calculationMethod: "Formula",
    glAccountCode: "6110");

otRegular.Update(
    calculationFormula: "HourlyRate * OvertimeHours * 1.25",
    rate: 1.25m,
    description: "Overtime pay at 125% of regular hourly rate",
    displayOrder: 10);

otRegular.SetAutoCalculated();
otRegular.SetMandatory("Labor Code Article 87");
otRegular.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);
otRegular.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);

// Rest day overtime (130%)
var otRestDay = PayComponent.Create(
    code: "OT_RESTDAY",
    componentName: "Overtime Pay (Rest Day)",
    componentType: "Earnings",
    calculationMethod: "Formula",
    glAccountCode: "6111");

otRestDay.Update(
    calculationFormula: "HourlyRate * OvertimeHours * 1.30",
    rate: 1.30m,
    description: "Overtime pay on rest day at 130%",
    displayOrder: 11);

otRestDay.SetAutoCalculated();
otRestDay.SetMandatory("Labor Code Article 93");
otRestDay.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);
otRestDay.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);

// ... Similar for special holiday and regular holiday overtime
```

### 7. Premium Pay Components

```csharp
// Night differential (10%)
var nightDiff = PayComponent.Create(
    code: "NIGHT_DIFF",
    componentName: "Night Differential",
    componentType: "Earnings",
    calculationMethod: "Percentage",
    glAccountCode: "6120");

nightDiff.Update(
    rate: 0.10m,
    description: "10% additional pay for work between 10PM-6AM",
    displayOrder: 20);

nightDiff.SetAutoCalculated();
nightDiff.SetMandatory("Labor Code Article 86");
nightDiff.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);
nightDiff.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);

// ... Similar for holiday pay and rest day pay
```

### 8. Allowances & Benefits

```csharp
// Transportation allowance
var transAllowance = PayComponent.Create(
    code: "ALLOW_TRANS",
    componentName: "Transportation Allowance",
    componentType: "Earnings",
    calculationMethod: "Fixed",
    glAccountCode: "6130");

transAllowance.Update(
    fixedAmount: 2000m,
    description: "Monthly transportation allowance (taxable if exceeds de minimis)",
    displayOrder: 30);

transAllowance.SetAutoCalculated(false);
transAllowance.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);
transAllowance.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);

// Rice subsidy (de minimis benefit)
var riceSubsidy = PayComponent.Create(
    code: "BENEFIT_RICE",
    componentName: "Rice Subsidy",
    componentType: "Earnings",
    calculationMethod: "Fixed",
    glAccountCode: "6133");

riceSubsidy.Update(
    fixedAmount: 2000m,
    description: "Monthly rice subsidy (de minimis up to ₱2,000/month or ₱24,000/year)",
    displayOrder: 33);

riceSubsidy.SetAutoCalculated(false);
riceSubsidy.SetMandatory("BIR RR 8-2012 (De Minimis Benefits)");
riceSubsidy.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: true);
riceSubsidy.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);

// ... Similar for meal, communication, uniform allowances
```

### 9. Deductions

```csharp
// Company loan
var companyLoan = PayComponent.Create(
    code: "DED_LOAN",
    componentName: "Company Loan",
    componentType: "Deduction",
    calculationMethod: "Manual",
    glAccountCode: "1210");

companyLoan.Update(
    description: "Authorized company loan deduction (requires written consent)",
    displayOrder: 140);

companyLoan.SetAutoCalculated(false);
companyLoan.SetMandatory("Labor Code Article 113 (Wage Deduction)");
companyLoan.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
companyLoan.SetPayImpact(affectsGrossPay: false, affectsNetPay: true);

// ... Similar for SSS loan, Pag-IBIG loan, cash advance, tardiness
```

### 10. 13th Month Pay

```csharp
var thirteenthMonth = PayComponent.Create(
    code: "THIRTEENTH_MONTH",
    componentName: "13th Month Pay",
    componentType: "Earnings",
    calculationMethod: "Formula",
    glAccountCode: "6140");

thirteenthMonth.Update(
    calculationFormula: "TotalBasicSalaryForYear / 12",
    description: "13th month pay - 1/12 of total basic salary (tax-exempt up to ₱90,000)",
    displayOrder: 40);

thirteenthMonth.SetAutoCalculated();
thirteenthMonth.SetMandatory("Presidential Decree No. 851");
thirteenthMonth.SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false);  // Partial exemption
thirteenthMonth.SetPayImpact(affectsGrossPay: true, affectsNetPay: true);
thirteenthMonth.SetLimits(minValue: 0m, maxValue: null);
```

---

## 🚀 NEXT STEPS

1. **Fix PhilippinePayrollSeeder.cs** - Replace all chained extension method calls with proper entity method calls
2. **Complete SSS Brackets** - Add all 30 SSS contribution brackets (₱4,000 to ₱30,000 in ₱500 increments)
3. **Complete Tax Brackets** - Add all 6 TRAIN law tax brackets
4. **Test Seeding** - Run database migration and verify all components and rates are seeded
5. **Create UI** - Build admin interface for managing pay components and rates
6. **Implement Calculation Engine** - Build payroll processor that reads from these tables

---

## 📚 LEGAL REFERENCES

- **SSS:** RA 11199, SSS Circular 2024
- **PhilHealth:** RA 7875, PhilHealth Circular 2024-0001
- **Pag-IBIG:** RA 9679, Pag-IBIG Fund Circular
- **Withholding Tax:** TRAIN Law (RA 10963), BIR Revenue Regulations
- **Labor Code:** Articles 82, 86, 87, 93, 94, 95, 111, 113
- **13th Month Pay:** Presidential Decree No. 851
- **De Minimis Benefits:** BIR RR 8-2012

---

**Document Version:** 1.0  
**Last Updated:** November 16, 2025  
**Status:** Ready for implementation


