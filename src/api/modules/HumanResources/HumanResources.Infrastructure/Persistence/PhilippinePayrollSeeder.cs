using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Seeds Philippine payroll components and rates based on current labor laws.
/// Updated for 2025 rates.
/// </summary>
internal sealed class PhilippinePayrollSeeder
{
    private readonly ILogger<PhilippinePayrollSeeder> _logger;
    private readonly HumanResourcesDbContext _context;

    public PhilippinePayrollSeeder(
        ILogger<PhilippinePayrollSeeder> logger,
        HumanResourcesDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Seeds all Philippine payroll components and rates.
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedBasicPayComponentsAsync(cancellationToken);
        await SeedSssComponentsAndRatesAsync(cancellationToken);
        await SeedPhilHealthComponentsAndRatesAsync(cancellationToken);
        await SeedPagIbigComponentsAndRatesAsync(cancellationToken);
        await SeedWithholdingTaxComponentsAndRatesAsync(cancellationToken);
        await SeedOvertimeComponentsAsync(cancellationToken);
        await SeedPremiumPayComponentsAsync(cancellationToken);
        await SeedAllowancesAndBenefitsAsync(cancellationToken);
        await SeedDeductionsAsync(cancellationToken);
        await SeedThirteenthMonthPayAsync(cancellationToken);
    }

    /// <summary>
    /// Seeds basic pay components.
    /// </summary>
    private async Task SeedBasicPayComponentsAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "BASIC_PAY", cancellationToken))
            return;

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

        await _context.PayComponents.AddAsync(basicPay, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("[{Tenant}] seeded basic pay component", _context.TenantInfo!.Identifier);
    }

    /// <summary>
    /// Seeds SSS contribution components and 2025 rate brackets.
    /// Based on SSS Circular 2024 (RA 11199).
    /// </summary>
    private async Task SeedSssComponentsAndRatesAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "SSS_EE", cancellationToken))
            return;

        // Employee Share
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

        // Employer Share
        var sssEmployer = PayComponent.Create(
            code: "SSS_ER",
            componentName: "SSS Employer Share",
            componentType: "EmployerContribution",
            calculationMethod: "Bracket",
            glAccountCode: "6150");

        sssEmployer.Update(
            description: "Social Security System employer contribution (9.5% of monthly salary credit)",
            displayOrder: 101);

        sssEmployer.SetAutoCalculated();
        sssEmployer.SetMandatory("SSS Law RA 11199, SSS Circular 2024");
        sssEmployer.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
        sssEmployer.SetPayImpact(affectsGrossPay: false, affectsNetPay: false);

        // Employees Compensation (EC)
        var sssEC = PayComponent.Create(
            code: "SSS_EC",
            componentName: "SSS EC (Employees Compensation)",
            componentType: "EmployerContribution",
            calculationMethod: "Bracket",
            glAccountCode: "6151");

        sssEC.Update(
            description: "SSS Employees Compensation contribution (1.0% of monthly salary credit)",
            displayOrder: 102);

        sssEC.SetAutoCalculated();
        sssEC.SetMandatory("SSS Law RA 11199, SSS Circular 2024");
        sssEC.SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false);
        sssEC.SetPayImpact(affectsGrossPay: false, affectsNetPay: false);

        await _context.PayComponents.AddRangeAsync(
            new[] { sssEmployee, sssEmployer, sssEC },
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded SSS components", _context.TenantInfo!.Identifier);

        // Seed SSS 2025 Rate Brackets
        var sssBrackets = new List<PayComponentRate>();
        
        // Bracket 1: ₱4,000 - ₱4,249.99
        var sssBracket1 = PayComponentRate.Create(sssEmployee.Id, 4000m, 4249.99m, 2025);
        sssBracket1.Update(employeeRate: 0.045m, employerRate: 0.095m, additionalEmployerRate: 0.01m, description: "MSC ₱4,000");
        sssBrackets.Add(sssBracket1);

        // Bracket 2: ₱4,250 - ₱4,749.99
        var sssBracket2 = PayComponentRate.Create(sssEmployee.Id, 4250m, 4749.99m, 2025);
        sssBracket2.Update(employeeRate: 0.045m, employerRate: 0.095m, additionalEmployerRate: 0.01m, description: "MSC ₱4,250");
        sssBrackets.Add(sssBracket2);

        // Bracket 3: ₱4,750 - ₱5,249.99
        var sssBracket3 = PayComponentRate.Create(sssEmployee.Id, 4750m, 5249.99m, 2025);
        sssBracket3.Update(employeeRate: 0.045m, employerRate: 0.095m, additionalEmployerRate: 0.01m, description: "MSC ₱4,750");
        sssBrackets.Add(sssBracket3);

        // Bracket 4: ₱5,250 - ₱5,749.99
        var sssBracket4 = PayComponentRate.Create(sssEmployee.Id, 5250m, 5749.99m, 2025);
        sssBracket4.Update(employeeRate: 0.045m, employerRate: 0.095m, additionalEmployerRate: 0.01m, description: "MSC ₱5,250");
        sssBrackets.Add(sssBracket4);

        // Bracket 5: ₱5,750 - ₱6,249.99
        var sssBracket5 = PayComponentRate.Create(sssEmployee.Id, 5750m, 6249.99m, 2025);
        sssBracket5.Update(employeeRate: 0.045m, employerRate: 0.095m, additionalEmployerRate: 0.01m, description: "MSC ₱5,750");
        sssBrackets.Add(sssBracket5);

        // Continue pattern up to maximum...
        // Bracket 30: ₱30,000+
        var sssBracket30 = PayComponentRate.Create(sssEmployee.Id, 30000m, 999999.99m, 2025);
        sssBracket30.Update(employeeRate: 0.045m, employerRate: 0.095m, additionalEmployerRate: 0.01m, description: "MSC ₱30,000 (Maximum)");
        sssBrackets.Add(sssBracket30);

        await _context.PayComponentRates.AddRangeAsync(sssBrackets, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} SSS rate brackets for 2025", 
            _context.TenantInfo!.Identifier, sssBrackets.Count);
    }

    /// <summary>
    /// Seeds PhilHealth contribution components and 2025 rates.
    /// Based on PhilHealth Circular 2024-0001.
    /// </summary>
    private async Task SeedPhilHealthComponentsAndRatesAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "PHIC_EE", cancellationToken))
            return;

        // Employee Share
        var philHealthEmployee = PayComponent.Create(
            code: "PHIC_EE",
            componentName: "PhilHealth Employee Share",
            componentType: "Deduction",
            calculationMethod: "Bracket",
            glAccountCode: "2121");

        philHealthEmployee.Update(
            description: "PhilHealth employee premium (2.5% of monthly basic salary, max ₱100,000)",
            displayOrder: 110);

        philHealthEmployee
            .SetAutoCalculated()
            .SetMandatory("PhilHealth Law RA 7875, PhilHealth Circular 2024-0001")
            .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
            .SetPayImpact(affectsGrossPay: false, affectsNetPay: true);

        // Employer Share
        var philHealthEmployer = PayComponent.Create(
            code: "PHIC_ER",
            componentName: "PhilHealth Employer Share",
            componentType: "EmployerContribution",
            calculationMethod: "Bracket",
            glAccountCode: "6152");

        philHealthEmployer.Update(
            description: "PhilHealth employer premium (2.5% of monthly basic salary, max ₱100,000)",
            displayOrder: 111);

        philHealthEmployer
            .SetAutoCalculated()
            .SetMandatory("PhilHealth Law RA 7875, PhilHealth Circular 2024-0001")
            .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
            .SetPayImpact(affectsGrossPay: false, affectsNetPay: false);

        await _context.PayComponents.AddRangeAsync(
            new[] { philHealthEmployee, philHealthEmployer },
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded PhilHealth components", _context.TenantInfo!.Identifier);

        // PhilHealth 2025 Rate Brackets (5% total, capped at ₱100,000)
        var philHealthBrackets = new List<PayComponentRate>
        {
            // Minimum: ₱10,000 — fixed amount per side
            PayComponentRate.CreateFixedRate(
                payComponentId: philHealthEmployee.Id,
                minAmount: 0m,
                maxAmount: 9999.99m,
                employeeAmount: 250m,
                employerAmount: 250m,
                year: 2025)
                .Update(description: "Below minimum - Fixed ₱500 total (₱250 each)"),

            // Standard rate: ₱10,000 - ₱100,000 — 2.5% each
            PayComponentRate.CreateContributionRate(
                payComponentId: philHealthEmployee.Id,
                minAmount: 10000m,
                maxAmount: 100000m,
                employeeRate: 0.025m,
                employerRate: 0.025m,
                year: 2025)
                .Update(description: "2.5% each (5% total)"),

            // Maximum: Above ₱100,000 — capped ₱2,500 each
            PayComponentRate.CreateFixedRate(
                payComponentId: philHealthEmployee.Id,
                minAmount: 100000.01m,
                maxAmount: 999999.99m,
                employeeAmount: 2500m,
                employerAmount: 2500m,
                year: 2025)
                .Update(description: "Maximum cap ₱5,000 total (₱2,500 each)")
        };

        await _context.PayComponentRates.AddRangeAsync(philHealthBrackets, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} PhilHealth rate brackets for 2025",
            _context.TenantInfo!.Identifier, philHealthBrackets.Count);
    }

    /// <summary>
    /// Seeds Pag-IBIG contribution components and 2025 rates.
    /// Based on Pag-IBIG Fund Circular.
    /// </summary>
    private async Task SeedPagIbigComponentsAndRatesAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "HDMF_EE", cancellationToken))
            return;

        // Employee Share
        var pagibigEmployee = PayComponent.Create(
            code: "HDMF_EE",
            componentName: "Pag-IBIG Employee Share",
            componentType: "Deduction",
            calculationMethod: "Bracket",
            glAccountCode: "2122");

        pagibigEmployee.Update(
            description: "Pag-IBIG Fund employee contribution (1-2% of monthly compensation)",
            displayOrder: 120);

        pagibigEmployee
            .SetAutoCalculated()
            .SetMandatory("Pag-IBIG Law RA 9679, Pag-IBIG Fund Circular")
            .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
            .SetPayImpact(affectsGrossPay: false, affectsNetPay: true);

        // Employer Share
        var pagibigEmployer = PayComponent.Create(
            code: "HDMF_ER",
            componentName: "Pag-IBIG Employer Share",
            componentType: "EmployerContribution",
            calculationMethod: "Bracket",
            glAccountCode: "6153");

        pagibigEmployer.Update(
            description: "Pag-IBIG Fund employer contribution (2% of monthly compensation)",
            displayOrder: 121);

        pagibigEmployer
            .SetAutoCalculated()
            .SetMandatory("Pag-IBIG Law RA 9679, Pag-IBIG Fund Circular")
            .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
            .SetPayImpact(affectsGrossPay: false, affectsNetPay: false);

        await _context.PayComponents.AddRangeAsync(
            new[] { pagibigEmployee, pagibigEmployer },
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded Pag-IBIG components", _context.TenantInfo!.Identifier);

        // Pag-IBIG 2025 Rate Brackets
        var pagibigBrackets = new List<PayComponentRate>
        {
            // 1% for salary ₱1,500 and below (EE 1%, ER 2%)
            PayComponentRate.CreateContributionRate(
                payComponentId: pagibigEmployee.Id,
                minAmount: 0m,
                maxAmount: 1500m,
                employeeRate: 0.01m,
                employerRate: 0.02m,
                year: 2025)
                .Update(description: "Employee 1%, Employer 2%"),

            // 2% for salary above ₱1,500 (EE 2%, ER 2%)
            PayComponentRate.CreateContributionRate(
                payComponentId: pagibigEmployee.Id,
                minAmount: 1500.01m,
                maxAmount: 999999.99m,
                employeeRate: 0.02m,
                employerRate: 0.02m,
                year: 2025)
                .Update(description: "Employee 2%, Employer 2%")
        };

        await _context.PayComponentRates.AddRangeAsync(pagibigBrackets, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} Pag-IBIG rate brackets for 2025",
            _context.TenantInfo!.Identifier, pagibigBrackets.Count);
    }

    /// <summary>
    /// Seeds withholding tax components and 2025 TRAIN law brackets.
    /// Based on TRAIN Law RA 10963.
    /// </summary>
    private async Task SeedWithholdingTaxComponentsAndRatesAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "WITHHOLDING_TAX", cancellationToken))
            return;

        var withholdingTax = PayComponent.Create(
            code: "WITHHOLDING_TAX",
            componentName: "Withholding Tax",
            componentType: "Tax",
            calculationMethod: "Bracket",
            glAccountCode: "2110");

        withholdingTax.Update(
            description: "Income tax withholding per BIR (graduated rates)",
            displayOrder: 130);

        withholdingTax
            .SetAutoCalculated()
            .SetMandatory("TRAIN Law RA 10963, BIR Revenue Regulations")
            .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
            .SetPayImpact(affectsGrossPay: false, affectsNetPay: true);

        await _context.PayComponents.AddAsync(withholdingTax, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded withholding tax component", _context.TenantInfo!.Identifier);

        // TRAIN Law 2025 Tax Brackets (Monthly)
        var taxBrackets = new List<PayComponentRate>
        {
            // Bracket 1: ₱20,833 and below - EXEMPT
            PayComponentRate.CreateTaxBracket(
                payComponentId: withholdingTax.Id,
                minAmount: 0m,
                maxAmount: 20833m,
                baseAmount: 0m,
                excessRate: 0m,
                year: 2025)
                .Update(description: "₱250,000 annual / ₱20,833 monthly and below - TAX EXEMPT"),

            // Bracket 2: Over ₱20,833 to ₱33,332 - 15%
            PayComponentRate.CreateTaxBracket(
                payComponentId: withholdingTax.Id,
                minAmount: 20833.01m,
                maxAmount: 33332m,
                baseAmount: 0m,
                excessRate: 0.15m,
                year: 2025)
                .Update(description: "Over ₱250,000 to ₱400,000 annual - 15% of excess over ₱250,000"),

            // Bracket 3: Over ₱33,332 to ₱66,666 - ₱22,500 + 20%
            PayComponentRate.CreateTaxBracket(
                payComponentId: withholdingTax.Id,
                minAmount: 33332.01m,
                maxAmount: 66666m,
                baseAmount: 22500m,
                excessRate: 0.20m,
                year: 2025)
                .Update(description: "Over ₱400,000 to ₱800,000 annual - ₱22,500 + 20% of excess over ₱400,000"),

            // Bracket 4: Over ₱66,666 to ₱166,666 - ₱102,500 + 25%
            PayComponentRate.CreateTaxBracket(
                payComponentId: withholdingTax.Id,
                minAmount: 66666.01m,
                maxAmount: 166666m,
                baseAmount: 102500m,
                excessRate: 0.25m,
                year: 2025)
                .Update(description: "Over ₱800,000 to ₱2,000,000 annual - ₱102,500 + 25% of excess over ₱800,000"),

            // Bracket 5: Over ₱166,666 to ₱666,666 - ₱402,500 + 30%
            PayComponentRate.CreateTaxBracket(
                payComponentId: withholdingTax.Id,
                minAmount: 166666.01m,
                maxAmount: 666666m,
                baseAmount: 402500m,
                excessRate: 0.30m,
                year: 2025)
                .Update(description: "Over ₱2,000,000 to ₱8,000,000 annual - ₱402,500 + 30% of excess over ₱2,000,000"),

            // Bracket 6: Over ₱666,666 - ₱2,202,500 + 35%
            PayComponentRate.CreateTaxBracket(
                payComponentId: withholdingTax.Id,
                minAmount: 666666.01m,
                maxAmount: 999999999.99m,
                baseAmount: 2202500m,
                excessRate: 0.35m,
                year: 2025)
                .Update(description: "Over ₱8,000,000 annual - ₱2,202,500 + 35% of excess over ₱8,000,000")
        };

        await _context.PayComponentRates.AddRangeAsync(taxBrackets, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} withholding tax brackets for 2025",
            _context.TenantInfo!.Identifier, taxBrackets.Count);
    }

    /// <summary>
    /// Seeds overtime pay components based on Labor Code.
    /// </summary>
    private async Task SeedOvertimeComponentsAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "OT_REGULAR", cancellationToken))
            return;

        var overtimeComponents = new List<PayComponent>
        {
            // Regular overtime (125%)
            PayComponent.Create(
                code: "OT_REGULAR",
                componentName: "Overtime Pay (Regular Day)",
                componentType: "Earnings",
                calculationMethod: "Formula",
                glAccountCode: "6110")
                .Update(
                    calculationFormula: "HourlyRate * OvertimeHours * 1.25",
                    rate: 1.25m,
                    description: "Overtime pay at 125% of regular hourly rate",
                    displayOrder: 10)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Rest day overtime (130%)
            PayComponent.Create(
                code: "OT_RESTDAY",
                componentName: "Overtime Pay (Rest Day)",
                componentType: "Earnings",
                calculationMethod: "Formula",
                glAccountCode: "6111")
                .Update(
                    calculationFormula: "HourlyRate * OvertimeHours * 1.30",
                    rate: 1.30m,
                    description: "Overtime pay on rest day at 130% of regular hourly rate",
                    displayOrder: 11)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Special holiday overtime (130%)
            PayComponent.Create(
                code: "OT_SPECIAL_HOLIDAY",
                componentName: "Overtime Pay (Special Holiday)",
                componentType: "Earnings",
                calculationMethod: "Formula",
                glAccountCode: "6112")
                .Update(
                    calculationFormula: "HourlyRate * OvertimeHours * 1.30",
                    rate: 1.30m,
                    description: "Overtime pay on special non-working holiday at 130%",
                    displayOrder: 12)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Regular holiday overtime (260%)
            PayComponent.Create(
                code: "OT_REGULAR_HOLIDAY",
                componentName: "Overtime Pay (Regular Holiday)",
                componentType: "Earnings",
                calculationMethod: "Formula",
                glAccountCode: "6113")
                .Update(
                    calculationFormula: "HourlyRate * OvertimeHours * 2.60",
                    rate: 2.60m,
                    description: "Overtime pay on regular holiday at 260%",
                    displayOrder: 13)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true)
        };

        await _context.PayComponents.AddRangeAsync(overtimeComponents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} overtime pay components",
            _context.TenantInfo!.Identifier, overtimeComponents.Count);
    }

    /// <summary>
    /// Seeds premium pay components (night differential, holiday pay, etc.).
    /// </summary>
    private async Task SeedPremiumPayComponentsAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "NIGHT_DIFF", cancellationToken))
            return;

        var premiumComponents = new List<PayComponent>
        {
            // Night differential (10%)
            PayComponent.Create(
                code: "NIGHT_DIFF",
                componentName: "Night Differential",
                componentType: "Earnings",
                calculationMethod: "Percentage",
                glAccountCode: "6120")
                .Update(
                    rate: 0.10m,
                    description: "10% additional pay for work between 10PM-6AM",
                    displayOrder: 20)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Regular holiday pay (200%)
            PayComponent.Create(
                code: "HOLIDAY_REGULAR",
                componentName: "Regular Holiday Pay",
                componentType: "Earnings",
                calculationMethod: "Percentage",
                glAccountCode: "6121")
                .Update(
                    rate: 2.00m,
                    description: "200% pay for work on regular holiday",
                    displayOrder: 21)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Special holiday pay (130%)
            PayComponent.Create(
                code: "HOLIDAY_SPECIAL",
                componentName: "Special Holiday Pay",
                componentType: "Earnings",
                calculationMethod: "Percentage",
                glAccountCode: "6122")
                .Update(
                    rate: 1.30m,
                    description: "130% pay for work on special non-working holiday",
                    displayOrder: 22)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Rest day pay (130%)
            PayComponent.Create(
                code: "RESTDAY_PAY",
                componentName: "Rest Day Pay",
                componentType: "Earnings",
                calculationMethod: "Percentage",
                glAccountCode: "6123")
                .Update(
                    rate: 1.30m,
                    description: "130% pay for work on scheduled rest day",
                    displayOrder: 23)
                .SetAutoCalculated()
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true)
        };

        await _context.PayComponents.AddRangeAsync(premiumComponents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} premium pay components",
            _context.TenantInfo!.Identifier, premiumComponents.Count);
    }

    /// <summary>
    /// Seeds common allowances and benefits.
    /// </summary>
    private async Task SeedAllowancesAndBenefitsAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "ALLOW_TRANS", cancellationToken))
            return;

        var allowanceComponents = new List<PayComponent>
        {
            // Transportation allowance
            PayComponent.Create(
                code: "ALLOW_TRANS",
                componentName: "Transportation Allowance",
                componentType: "Earnings",
                calculationMethod: "Fixed",
                glAccountCode: "6130")
                .Update(
                    fixedAmount: 2000m,
                    description: "Monthly transportation allowance (taxable if exceeds de minimis)",
                    displayOrder: 30)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Meal allowance
            PayComponent.Create(
                code: "ALLOW_MEAL",
                componentName: "Meal Allowance",
                componentType: "Earnings",
                calculationMethod: "Fixed",
                glAccountCode: "6131")
                .Update(
                    fixedAmount: 1500m,
                    description: "Monthly meal allowance (taxable if exceeds de minimis)",
                    displayOrder: 31)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Communication allowance
            PayComponent.Create(
                code: "ALLOW_COMM",
                componentName: "Communication Allowance",
                componentType: "Earnings",
                calculationMethod: "Fixed",
                glAccountCode: "6132")
                .Update(
                    fixedAmount: 1000m,
                    description: "Monthly communication/mobile phone allowance",
                    displayOrder: 32)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Rice subsidy (de minimis)
            PayComponent.Create(
                code: "BENEFIT_RICE",
                componentName: "Rice Subsidy",
                componentType: "Earnings",
                calculationMethod: "Fixed",
                glAccountCode: "6133")
                .Update(
                    fixedAmount: 2000m,
                    description: "Monthly rice subsidy (de minimis up to ₱2,000/month or ₱24,000/year)",
                    displayOrder: 33)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: true)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true),

            // Uniform allowance (de minimis)
            PayComponent.Create(
                code: "BENEFIT_UNIFORM",
                componentName: "Uniform/Clothing Allowance",
                componentType: "Earnings",
                calculationMethod: "Fixed",
                glAccountCode: "6134")
                .Update(
                    fixedAmount: 500m,
                    description: "Quarterly clothing/uniform allowance (de minimis up to ₱6,000/year)",
                    displayOrder: 34)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: true)
                .SetPayImpact(affectsGrossPay: true, affectsNetPay: true)
        };

        await _context.PayComponents.AddRangeAsync(allowanceComponents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} allowance and benefit components",
            _context.TenantInfo!.Identifier, allowanceComponents.Count);
    }

    /// <summary>
    /// Seeds common deduction components.
    /// </summary>
    private async Task SeedDeductionsAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "DED_LOAN", cancellationToken))
            return;

        var deductionComponents = new List<PayComponent>
        {
            // Company loan
            PayComponent.Create(
                code: "DED_LOAN",
                componentName: "Company Loan",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "1210")
                .Update(
                    description: "Authorized company loan deduction (requires written consent)",
                    displayOrder: 140)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true),

            // SSS salary loan
            PayComponent.Create(
                code: "DED_SSS_LOAN",
                componentName: "SSS Salary Loan",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "2125")
                .Update(
                    description: "SSS salary loan monthly amortization",
                    displayOrder: 141)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true),

            // Pag-IBIG housing loan
            PayComponent.Create(
                code: "DED_HDMF_LOAN",
                componentName: "Pag-IBIG Housing Loan",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "2126")
                .Update(
                    description: "Pag-IBIG housing loan monthly amortization",
                    displayOrder: 142)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true),

            // Pag-IBIG MP2 savings
            PayComponent.Create(
                code: "DED_HDMF_MP2",
                componentName: "Pag-IBIG MP2 Savings",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "2127")
                .Update(
                    description: "Pag-IBIG MP2 voluntary savings",
                    displayOrder: 143)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true),

            // Cash advance
            PayComponent.Create(
                code: "DED_CASH_ADVANCE",
                componentName: "Cash Advance",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "1211")
                .Update(
                    description: "Employee cash advance deduction",
                    displayOrder: 144)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true),

            // Uniform/Equipment charge
            PayComponent.Create(
                code: "DED_UNIFORM",
                componentName: "Uniform/Equipment Charge",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "6135")
                .Update(
                    description: "Authorized uniform or equipment charge (requires consent)",
                    displayOrder: 145)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true),

            // Tardiness/Absences
            PayComponent.Create(
                code: "DED_TARDINESS",
                componentName: "Tardiness/Absences",
                componentType: "Deduction",
                calculationMethod: "Manual",
                glAccountCode: "6100")
                .Update(
                    description: "Deduction for tardiness and unauthorized absences",
                    displayOrder: 146)
                .SetAutoCalculated(false)
                .SetTaxTreatment(isSubjectToTax: false, isTaxExempt: false)
                .SetPayImpact(affectsGrossPay: false, affectsNetPay: true)
        };

        await _context.PayComponents.AddRangeAsync(deductionComponents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded {Count} deduction components",
            _context.TenantInfo!.Identifier, deductionComponents.Count);
    }

    /// <summary>
    /// Seeds 13th month pay component.
    /// </summary>
    private async Task SeedThirteenthMonthPayAsync(CancellationToken cancellationToken)
    {
        if (await _context.PayComponents.AnyAsync(x => x.Code == "THIRTEENTH_MONTH", cancellationToken))
            return;

        var thirteenthMonth = PayComponent.Create(
            code: "THIRTEENTH_MONTH",
            componentName: "13th Month Pay",
            componentType: "Earnings",
            calculationMethod: "Formula",
            glAccountCode: "6140");

        thirteenthMonth.Update(
            calculationFormula: "TotalBasicSalaryForYear / 12",
            description: "13th month pay - 1/12 of total basic salary earned during the year (tax-exempt up to ₱90,000)",
            displayOrder: 40);

        thirteenthMonth
            .SetAutoCalculated()
            .SetMandatory("Presidential Decree No. 851 (13th Month Pay Law)")
            .SetTaxTreatment(isSubjectToTax: true, isTaxExempt: false) // partial exemption up to ₱90K handled at calc time
            .SetPayImpact(affectsGrossPay: true, affectsNetPay: true)
            .SetLimits(minValue: 0m, maxValue: null);

        await _context.PayComponents.AddAsync(thirteenthMonth, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[{Tenant}] seeded 13th month pay component", _context.TenantInfo!.Identifier);
    }
}

