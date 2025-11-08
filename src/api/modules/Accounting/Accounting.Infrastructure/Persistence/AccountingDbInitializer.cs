using Microsoft.Extensions.Logging;

namespace Accounting.Infrastructure.Persistence;

internal sealed class AccountingDbInitializer(
    ILogger<AccountingDbInitializer> logger,
    AccountingDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for accounting module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Seed Chart of Accounts - Comprehensive Electric Utility Company
        if (!await context.ChartOfAccounts.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var accounts = new List<ChartOfAccount>
            {
                // ============================================
                // ASSETS (1000-1999)
                // ============================================
                
                // Header Account
                ChartOfAccount.Create("1000", "ASSETS", "Asset", "General", null, "1", 0m, true, "Debit", true, null, "Total Assets"),

                // Current Assets (1100-1299)
                ChartOfAccount.Create("1100", "Cash and Cash Equivalents", "Asset", "General", null, "1.1", 0m, false, "Debit", true, null, "Cash and cash equivalents"),
                ChartOfAccount.Create("1110", "Petty Cash", "Asset", "General", null, "1.1.1", 0m, false, "Debit", true, null, "Petty cash fund"),
                ChartOfAccount.Create("1120", "Bank - Operating Account", "Asset", "General", null, "1.1.2", 0m, false, "Debit", true, null, "Primary operating account"),
                ChartOfAccount.Create("1130", "Bank - Payroll Account", "Asset", "General", null, "1.1.3", 0m, false, "Debit", true, null, "Payroll account"),
                ChartOfAccount.Create("1140", "Bank - Money Market", "Asset", "General", null, "1.1.4", 0m, false, "Debit", true, null, "Money market account"),
                ChartOfAccount.Create("1150", "Short-term Investments", "Asset", "General", null, "1.1.5", 0m, false, "Debit", true, null, "Short-term investments"),

                ChartOfAccount.Create("1200", "Accounts Receivable", "Asset", "Customer Service", null, "1.2", 0m, false, "Debit", true, null, "Accounts receivable"),
                ChartOfAccount.Create("1210", "Accounts Receivable - Residential", "Asset", "Customer Service", null, "1.2.1", 0m, false, "Debit", true, null, "Residential customer receivables"),
                ChartOfAccount.Create("1220", "Accounts Receivable - Commercial", "Asset", "Customer Service", null, "1.2.2", 0m, false, "Debit", true, null, "Commercial customer receivables"),
                ChartOfAccount.Create("1230", "Accounts Receivable - Industrial", "Asset", "Customer Service", null, "1.2.3", 0m, false, "Debit", true, null, "Industrial customer receivables"),
                ChartOfAccount.Create("1240", "Allowance for Doubtful Accounts", "Asset", "Customer Service", null, "1.2.4", 0m, false, "Credit", true, null, "Bad debt allowance"),
                ChartOfAccount.Create("1250", "Unbilled Revenue", "Asset", "Customer Service", null, "1.2.5", 0m, false, "Debit", true, null, "Electricity delivered but not yet billed"),

                ChartOfAccount.Create("1300", "Inventory", "Asset", "Inventory", null, "1.3", 0m, false, "Debit", true, null, "Inventory"),
                ChartOfAccount.Create("1310", "Materials and Supplies", "Asset", "Inventory", null, "1.3.1", 0m, false, "Debit", true, null, "Maintenance materials"),
                ChartOfAccount.Create("1320", "Fuel Inventory - Coal", "Asset", "Inventory", null, "1.3.2", 0m, false, "Debit", true, null, "Coal fuel inventory"),
                ChartOfAccount.Create("1330", "Fuel Inventory - Natural Gas", "Asset", "Inventory", null, "1.3.3", 0m, false, "Debit", true, null, "Natural gas fuel inventory"),
                ChartOfAccount.Create("1340", "Spare Parts Inventory", "Asset", "Inventory", null, "1.3.4", 0m, false, "Debit", true, null, "Equipment spare parts"),

                ChartOfAccount.Create("1400", "Prepaid Expenses", "Asset", "General", null, "1.4", 0m, false, "Debit", true, null, "Prepaid expenses"),
                ChartOfAccount.Create("1410", "Prepaid Insurance", "Asset", "General", null, "1.4.1", 0m, false, "Debit", true, null, "Prepaid insurance"),
                ChartOfAccount.Create("1420", "Prepaid Rent", "Asset", "General", null, "1.4.2", 0m, false, "Debit", true, null, "Prepaid rent"),

                // Property, Plant & Equipment (1500-1799)
                ChartOfAccount.Create("1500", "Electric Plant in Service", "Asset", "Operations", null, "1.5", 0m, false, "Debit", true, null, "Utility plant in service"),
                ChartOfAccount.Create("1510", "Generation Plant", "Asset", "Production", null, "1.5.1", 0m, false, "Debit", true, null, "Power generation facilities"),
                ChartOfAccount.Create("1520", "Transmission Plant", "Asset", "Transmission", null, "1.5.2", 0m, false, "Debit", true, null, "Transmission lines and substations"),
                ChartOfAccount.Create("1530", "Distribution Plant", "Asset", "Distribution", null, "1.5.3", 0m, false, "Debit", true, null, "Distribution system"),
                ChartOfAccount.Create("1540", "General Plant", "Asset", "General", null, "1.5.4", 0m, false, "Debit", true, null, "Administrative buildings and equipment"),

                ChartOfAccount.Create("1600", "Accumulated Depreciation", "Asset", "Operations", null, "1.6", 0m, false, "Credit", true, null, "Accumulated depreciation"),
                ChartOfAccount.Create("1610", "Accumulated Depreciation - Generation", "Asset", "Production", null, "1.6.1", 0m, false, "Credit", true, null, "Generation plant depreciation"),
                ChartOfAccount.Create("1620", "Accumulated Depreciation - Transmission", "Asset", "Transmission", null, "1.6.2", 0m, false, "Credit", true, null, "Transmission plant depreciation"),
                ChartOfAccount.Create("1630", "Accumulated Depreciation - Distribution", "Asset", "Distribution", null, "1.6.3", 0m, false, "Credit", true, null, "Distribution plant depreciation"),
                ChartOfAccount.Create("1640", "Accumulated Depreciation - General", "Asset", "General", null, "1.6.4", 0m, false, "Credit", true, null, "General plant depreciation"),

                ChartOfAccount.Create("1700", "Construction Work in Progress", "Asset", "Operations", null, "1.7", 0m, false, "Debit", true, null, "Projects under construction"),

                // Other Assets (1800-1899)
                ChartOfAccount.Create("1800", "Other Assets", "Asset", "General", null, "1.8", 0m, false, "Debit", true, null, "Other long-term assets"),
                ChartOfAccount.Create("1810", "Regulatory Assets", "Asset", "General", null, "1.8.1", 0m, false, "Debit", true, null, "Deferred costs recoverable from customers"),
                ChartOfAccount.Create("1820", "Deferred Tax Assets", "Asset", "General", null, "1.8.2", 0m, false, "Debit", true, null, "Deferred income tax assets"),
                ChartOfAccount.Create("1830", "Long-term Investments", "Asset", "General", null, "1.8.3", 0m, false, "Debit", true, null, "Long-term investments"),

                // ============================================
                // LIABILITIES (2000-2999)
                // ============================================
                
                // Header Account
                ChartOfAccount.Create("2000", "LIABILITIES", "Liability", "General", null, "2", 0m, true, "Credit", true, null, "Total Liabilities"),

                // Current Liabilities (2100-2299)
                ChartOfAccount.Create("2100", "Accounts Payable", "Liability", "General", null, "2.1", 0m, false, "Credit", true, null, "Accounts payable"),
                ChartOfAccount.Create("2110", "Accounts Payable - Trade", "Liability", "General", null, "2.1.1", 0m, false, "Credit", true, null, "Trade payables"),
                ChartOfAccount.Create("2120", "Accounts Payable - Fuel", "Liability", "General", null, "2.1.2", 0m, false, "Credit", true, null, "Fuel purchase payables"),

                ChartOfAccount.Create("2200", "Accrued Liabilities", "Liability", "General", null, "2.2", 0m, false, "Credit", true, null, "Accrued expenses"),
                ChartOfAccount.Create("2210", "Accrued Payroll", "Liability", "General", null, "2.2.1", 0m, false, "Credit", true, null, "Accrued wages and salaries"),
                ChartOfAccount.Create("2220", "Accrued Taxes", "Liability", "General", null, "2.2.2", 0m, false, "Credit", true, null, "Accrued tax liabilities"),
                ChartOfAccount.Create("2230", "Accrued Interest", "Liability", "General", null, "2.2.3", 0m, false, "Credit", true, null, "Accrued interest payable"),

                ChartOfAccount.Create("2300", "Customer Deposits", "Liability", "Customer Service", null, "2.3", 0m, false, "Credit", true, null, "Customer security deposits"),
                ChartOfAccount.Create("2310", "Customer Deposits - Residential", "Liability", "Customer Service", null, "2.3.1", 0m, false, "Credit", true, null, "Residential deposits"),
                ChartOfAccount.Create("2320", "Customer Deposits - Commercial", "Liability", "Customer Service", null, "2.3.2", 0m, false, "Credit", true, null, "Commercial deposits"),

                ChartOfAccount.Create("2400", "Current Portion of Long-term Debt", "Liability", "General", null, "2.4", 0m, false, "Credit", true, null, "Current portion of long-term debt"),

                // Long-term Liabilities (2500-2799)
                ChartOfAccount.Create("2500", "Long-term Debt", "Liability", "General", null, "2.5", 0m, false, "Credit", true, null, "Long-term debt"),
                ChartOfAccount.Create("2510", "Bonds Payable", "Liability", "General", null, "2.5.1", 0m, false, "Credit", true, null, "Revenue bonds"),
                ChartOfAccount.Create("2520", "Notes Payable - Long-term", "Liability", "General", null, "2.5.2", 0m, false, "Credit", true, null, "Long-term notes"),
                ChartOfAccount.Create("2530", "Capital Lease Obligations", "Liability", "General", null, "2.5.3", 0m, false, "Credit", true, null, "Capital lease liabilities"),

                ChartOfAccount.Create("2600", "Deferred Credits", "Liability", "General", null, "2.6", 0m, false, "Credit", true, null, "Deferred credits"),
                ChartOfAccount.Create("2610", "Regulatory Liabilities", "Liability", "General", null, "2.6.1", 0m, false, "Credit", true, null, "Deferred credits refundable to customers"),
                ChartOfAccount.Create("2620", "Deferred Tax Liabilities", "Liability", "General", null, "2.6.2", 0m, false, "Credit", true, null, "Deferred income tax liabilities"),
                ChartOfAccount.Create("2630", "Asset Retirement Obligations", "Liability", "General", null, "2.6.3", 0m, false, "Credit", true, null, "Legal obligations for asset retirement"),

                // ============================================
                // EQUITY (3000-3999)
                // ============================================
                
                // Header Account
                ChartOfAccount.Create("3000", "EQUITY", "Equity", "General", null, "3", 0m, true, "Credit", true, null, "Total Equity"),

                ChartOfAccount.Create("3100", "Member Equity", "Equity", "General", null, "3.1", 0m, false, "Credit", true, null, "Member equity capital"),
                ChartOfAccount.Create("3110", "Common Stock", "Equity", "General", null, "3.1.1", 0m, false, "Credit", true, null, "Common stock"),
                ChartOfAccount.Create("3120", "Patronage Capital", "Equity", "General", null, "3.1.2", 0m, false, "Credit", true, null, "Allocated patronage capital"),
                ChartOfAccount.Create("3130", "Retained Earnings", "Equity", "General", null, "3.1.3", 0m, false, "Credit", true, null, "Retained earnings"),
                ChartOfAccount.Create("3140", "Current Year Earnings", "Equity", "General", null, "3.1.4", 0m, false, "Credit", true, null, "Current year net income"),

                // ============================================
                // REVENUE (4000-4999)
                // ============================================
                
                // Header Account
                ChartOfAccount.Create("4000", "OPERATING REVENUE", "Revenue", "Sales", null, "4", 0m, true, "Credit", true, null, "Total Operating Revenue"),

                // Electric Operating Revenue (4100-4499)
                ChartOfAccount.Create("4100", "Residential Sales", "Revenue", "Sales", null, "4.1", 0m, false, "Credit", true, null, "Residential electricity sales"),
                ChartOfAccount.Create("4110", "Residential - Energy Charges", "Revenue", "Sales", null, "4.1.1", 0m, false, "Credit", true, null, "Residential energy charges"),
                ChartOfAccount.Create("4120", "Residential - Customer Charges", "Revenue", "Sales", null, "4.1.2", 0m, false, "Credit", true, null, "Residential customer charges"),

                ChartOfAccount.Create("4200", "Commercial Sales", "Revenue", "Sales", null, "4.2", 0m, false, "Credit", true, null, "Commercial electricity sales"),
                ChartOfAccount.Create("4210", "Commercial - Energy Charges", "Revenue", "Sales", null, "4.2.1", 0m, false, "Credit", true, null, "Commercial energy charges"),
                ChartOfAccount.Create("4220", "Commercial - Demand Charges", "Revenue", "Sales", null, "4.2.2", 0m, false, "Credit", true, null, "Commercial demand charges"),

                ChartOfAccount.Create("4300", "Industrial Sales", "Revenue", "Sales", null, "4.3", 0m, false, "Credit", true, null, "Industrial electricity sales"),
                ChartOfAccount.Create("4310", "Industrial - Energy Charges", "Revenue", "Sales", null, "4.3.1", 0m, false, "Credit", true, null, "Industrial energy charges"),
                ChartOfAccount.Create("4320", "Industrial - Demand Charges", "Revenue", "Sales", null, "4.3.2", 0m, false, "Credit", true, null, "Industrial demand charges"),

                ChartOfAccount.Create("4400", "Other Electric Revenue", "Revenue", "Sales", null, "4.4", 0m, false, "Credit", true, null, "Other electric revenue"),
                ChartOfAccount.Create("4410", "Late Payment Charges", "Revenue", "Sales", null, "4.4.1", 0m, false, "Credit", true, null, "Late payment fees"),
                ChartOfAccount.Create("4420", "Reconnection Fees", "Revenue", "Sales", null, "4.4.2", 0m, false, "Credit", true, null, "Service reconnection fees"),
                ChartOfAccount.Create("4430", "Meter Testing Fees", "Revenue", "Sales", null, "4.4.3", 0m, false, "Credit", true, null, "Meter testing and service fees"),

                // Non-Operating Revenue (4500-4999)
                ChartOfAccount.Create("4500", "Other Revenue", "Revenue", "Sales", null, "4.5", 0m, false, "Credit", true, null, "Non-operating revenue"),
                ChartOfAccount.Create("4510", "Investment Income", "Revenue", "Sales", null, "4.5.1", 0m, false, "Credit", true, null, "Interest and dividend income"),
                ChartOfAccount.Create("4520", "Gain on Sale of Assets", "Revenue", "Sales", null, "4.5.2", 0m, false, "Credit", true, null, "Gains from asset sales"),
                ChartOfAccount.Create("4530", "Miscellaneous Revenue", "Revenue", "Sales", null, "4.5.3", 0m, false, "Credit", true, null, "Other miscellaneous income"),

                // ============================================
                // EXPENSES (5000-9999)
                // ============================================
                
                // Header Account
                ChartOfAccount.Create("5000", "OPERATING EXPENSES", "Expense", "Operations", null, "5", 0m, true, "Debit", true, null, "Total Operating Expenses"),

                // Power Production Expenses (5100-5299)
                ChartOfAccount.Create("5100", "Power Production - Fuel", "Expense", "Operations", null, "5.1", 0m, false, "Debit", true, null, "Fuel costs for generation"),
                ChartOfAccount.Create("5110", "Fuel - Coal", "Expense", "Operations", null, "5.1.1", 0m, false, "Debit", true, null, "Coal fuel expense"),
                ChartOfAccount.Create("5120", "Fuel - Natural Gas", "Expense", "Operations", null, "5.1.2", 0m, false, "Debit", true, null, "Natural gas fuel expense"),
                ChartOfAccount.Create("5130", "Fuel - Oil", "Expense", "Operations", null, "5.1.3", 0m, false, "Debit", true, null, "Oil fuel expense"),

                ChartOfAccount.Create("5200", "Power Production - Operations", "Expense", "Operations", null, "5.2", 0m, false, "Debit", true, null, "Generation operations"),
                ChartOfAccount.Create("5210", "Generation Labor", "Expense", "Operations", null, "5.2.1", 0m, false, "Debit", true, null, "Generation plant labor"),
                ChartOfAccount.Create("5220", "Generation Maintenance", "Expense", "Operations", null, "5.2.2", 0m, false, "Debit", true, null, "Generation maintenance"),

                // Power Purchased (5300-5399)
                ChartOfAccount.Create("5300", "Purchased Power", "Expense", "Operations", null, "5.3", 0m, false, "Debit", true, null, "Electricity purchased from others"),
                ChartOfAccount.Create("5310", "Purchased Power - Base Load", "Expense", "Operations", null, "5.3.1", 0m, false, "Debit", true, null, "Base load power purchases"),
                ChartOfAccount.Create("5320", "Purchased Power - Peak Load", "Expense", "Operations", null, "5.3.2", 0m, false, "Debit", true, null, "Peak load power purchases"),

                // Transmission Expenses (5400-5499)
                ChartOfAccount.Create("5400", "Transmission Expenses", "Expense", "Operations", null, "5.4", 0m, false, "Debit", true, null, "Transmission system operations"),
                ChartOfAccount.Create("5410", "Transmission Labor", "Expense", "Operations", null, "5.4.1", 0m, false, "Debit", true, null, "Transmission operations labor"),
                ChartOfAccount.Create("5420", "Transmission Maintenance", "Expense", "Operations", null, "5.4.2", 0m, false, "Debit", true, null, "Transmission system maintenance"),

                // Distribution Expenses (5500-5699)
                ChartOfAccount.Create("5500", "Distribution Expenses", "Expense", "Operations", null, "5.5", 0m, false, "Debit", true, null, "Distribution system operations"),
                ChartOfAccount.Create("5510", "Distribution Labor", "Expense", "Operations", null, "5.5.1", 0m, false, "Debit", true, null, "Distribution operations labor"),
                ChartOfAccount.Create("5520", "Distribution Maintenance", "Expense", "Operations", null, "5.5.2", 0m, false, "Debit", true, null, "Distribution system maintenance"),
                ChartOfAccount.Create("5530", "Meter Reading", "Expense", "Operations", null, "5.5.3", 0m, false, "Debit", true, null, "Meter reading expenses"),

                // Customer Service Expenses (5700-5899)
                ChartOfAccount.Create("5700", "Customer Service Expenses", "Expense", "Customer Service", null, "5.7", 0m, false, "Debit", true, null, "Customer service and billing"),
                ChartOfAccount.Create("5710", "Customer Service Labor", "Expense", "Customer Service", null, "5.7.1", 0m, false, "Debit", true, null, "Customer service labor"),
                ChartOfAccount.Create("5720", "Billing Expenses", "Expense", "Customer Service", null, "5.7.2", 0m, false, "Debit", true, null, "Billing and collection"),
                ChartOfAccount.Create("5730", "Uncollectible Accounts", "Expense", "Customer Service", null, "5.7.3", 0m, false, "Debit", true, null, "Bad debt expense"),

                // Administrative & General Expenses (6000-6999)
                ChartOfAccount.Create("6000", "Administrative & General", "Expense", "Administrative", null, "6", 0m, false, "Debit", true, null, "Administrative expenses"),
                ChartOfAccount.Create("6100", "Administrative Salaries", "Expense", "Administrative", null, "6.1", 0m, false, "Debit", true, null, "Administrative and executive salaries"),
                ChartOfAccount.Create("6200", "Office Expenses", "Expense", "Administrative", null, "6.2", 0m, false, "Debit", true, null, "Office supplies and expenses"),
                ChartOfAccount.Create("6300", "Professional Services", "Expense", "Administrative", null, "6.3", 0m, false, "Debit", true, null, "Legal and consulting fees"),
                ChartOfAccount.Create("6400", "Insurance", "Expense", "Administrative", null, "6.4", 0m, false, "Debit", true, null, "Insurance expenses"),
                ChartOfAccount.Create("6500", "Employee Benefits", "Expense", "Administrative", null, "6.5", 0m, false, "Debit", true, null, "Health insurance and benefits"),
                ChartOfAccount.Create("6600", "Regulatory Expenses", "Expense", "Administrative", null, "6.6", 0m, false, "Debit", true, null, "Regulatory compliance costs"),

                // Depreciation & Amortization (7000-7999)
                ChartOfAccount.Create("7000", "Depreciation & Amortization", "Expense", "Administrative", null, "7", 0m, false, "Debit", true, null, "Depreciation expense"),
                ChartOfAccount.Create("7100", "Depreciation - Generation", "Expense", "Production", null, "7.1", 0m, false, "Debit", true, null, "Generation plant depreciation"),
                ChartOfAccount.Create("7200", "Depreciation - Transmission", "Expense", "Transmission", null, "7.2", 0m, false, "Debit", true, null, "Transmission depreciation"),
                ChartOfAccount.Create("7300", "Depreciation - Distribution", "Expense", "Distribution", null, "7.3", 0m, false, "Debit", true, null, "Distribution depreciation"),
                ChartOfAccount.Create("7400", "Depreciation - General", "Expense", "General", null, "7.4", 0m, false, "Debit", true, null, "General plant depreciation"),

                // Taxes (8000-8999)
                ChartOfAccount.Create("8000", "Taxes Other Than Income", "Expense", "Administrative", null, "8", 0m, false, "Debit", true, null, "Property and other taxes"),
                ChartOfAccount.Create("8100", "Property Taxes", "Expense", "Administrative", null, "8.1", 0m, false, "Debit", true, null, "Property tax expense"),
                ChartOfAccount.Create("8200", "Payroll Taxes", "Expense", "Administrative", null, "8.2", 0m, false, "Debit", true, null, "Employer payroll taxes"),

                // Interest Expense (9000-9999)
                ChartOfAccount.Create("9000", "Interest Expense", "Expense", "Administrative", null, "9", 0m, false, "Debit", true, null, "Interest on long-term debt"),
                ChartOfAccount.Create("9100", "Interest on Bonds", "Expense", "Administrative", null, "9.1", 0m, false, "Debit", true, null, "Interest expense on bonds"),
                ChartOfAccount.Create("9200", "Interest on Notes", "Expense", "Administrative", null, "9.2", 0m, false, "Debit", true, null, "Interest expense on notes"),
            };

            await context.ChartOfAccounts.AddRangeAsync(accounts, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded comprehensive Electric Utility Chart of Accounts with {Count} records", context.TenantInfo!.Identifier, accounts.Count);
        }

        // Seed Accounting Periods and multiple Budgets + BudgetDetails
        if (!await context.AccountingPeriods.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var now = DateTime.UtcNow;
            var fiscalYear = now.Year;
            var start = new DateTime(fiscalYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(fiscalYear, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            var period = AccountingPeriod.Create($"FY{fiscalYear}", start, end, fiscalYear, "Annual", false, "Default fiscal year period");
            await context.AccountingPeriods.AddAsync(period, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded AccountingPeriod {PeriodName}", context.TenantInfo!.Identifier, period.Name);

            // Seed budgets with approval workflow for richer test data
            // Seed multiple budgets for richer test data
            if (!await context.Budgets.AnyAsync(cancellationToken).ConfigureAwait(false))
            {
                var budgetsToSeed = new List<Budget>
                {
                    Budget.Create("Default Operating Budget", period.Id, period.Name, fiscalYear, "Operating", "Auto-created default operating budget"),
                    Budget.Create("Capital Budget", period.Id, period.Name, fiscalYear, "Capital", "Auto-created capital budget"),
                    Budget.Create("Cash Flow Budget", period.Id, period.Name, fiscalYear, "Cash Flow", "Auto-created cash flow budget"),
                };

                // Save budgets first before adding details
                await context.Budgets.AddRangeAsync(budgetsToSeed, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                // Now add details after budgets are saved
                var acctDict = await context.ChartOfAccounts.ToDictionaryAsync(a => a.AccountCode, a => a.Id, cancellationToken).ConfigureAwait(false);

                var details = new List<BudgetDetail>();

                // Operating budget details
                var op = budgetsToSeed[0];
                if (acctDict.TryGetValue("1120", out var id1120)) details.Add(BudgetDetail.Create(op.Id, id1120, 500000m, "Operating account reserve"));
                if (acctDict.TryGetValue("5100", out var id5100)) details.Add(BudgetDetail.Create(op.Id, id5100, 2500000m, "Fuel costs"));
                if (acctDict.TryGetValue("5510", out var id5510)) details.Add(BudgetDetail.Create(op.Id, id5510, 800000m, "Distribution labor"));
                if (acctDict.TryGetValue("6100", out var id6100)) details.Add(BudgetDetail.Create(op.Id, id6100, 1200000m, "Administrative salaries"));

                // Capital budget details
                var cap = budgetsToSeed[1];
                if (acctDict.TryGetValue("1530", out var id1530)) details.Add(BudgetDetail.Create(cap.Id, id1530, 5000000m, "Distribution plant upgrades"));
                if (acctDict.TryGetValue("1700", out var id1700)) details.Add(BudgetDetail.Create(cap.Id, id1700, 3000000m, "Construction projects"));

                // Cash flow budget details
                var cf = budgetsToSeed[2];
                if (acctDict.TryGetValue("1120", out var id1120cf)) details.Add(BudgetDetail.Create(cf.Id, id1120cf, 1000000m, "Operating account cash"));
                if (acctDict.TryGetValue("4100", out var id4100)) details.Add(BudgetDetail.Create(cf.Id, id4100, 8000000m, "Residential sales revenue"));

                if (details.Count > 0)
                {
                    await context.BudgetDetails.AddRangeAsync(details, cancellationToken).ConfigureAwait(false);

                    // Update each budget totals
                    foreach (var b in budgetsToSeed)
                    {
                        var bDetails = details.Where(d => d.BudgetId == b.Id).ToList();
                        var totalBudgeted = bDetails.Sum(d => d.BudgetedAmount);
                        var totalActual = bDetails.Sum(d => d.ActualAmount);
                        b.SetTotals(totalBudgeted, totalActual);
                    }
                }

                // Approve first budget after details are added
                budgetsToSeed[0].Approve("cfo");
                context.Budgets.Update(budgetsToSeed[0]);

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} Budgets with details (1 approved, 2 pending)", context.TenantInfo!.Identifier, budgetsToSeed.Count);
            }
        }

        // Seed Depreciation Methods (unchanged)
        if (!await context.DepreciationMethods.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var methods = new List<DepreciationMethod>
            {
                DepreciationMethod.Create("SL", "Straight Line", "(Cost - Salvage) / Life", "Straight line depreciation"),
                DepreciationMethod.Create("DB", "Declining Balance", "(BookValue * Rate)", "Declining balance depreciation"),
                DepreciationMethod.Create("SYD", "Sum Of Years' Digits", "(RemainingLife / SYD) * (Cost - Salvage)", "SYD depreciation")
            };

            await context.DepreciationMethods.AddRangeAsync(methods, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded DepreciationMethods", context.TenantInfo!.Identifier);
        }

        // Seed Vendors (10 records)
        if (!await context.Vendors.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var vendors = new List<Vendor>();
            var expenseAccounts = new[] { "5100", "5200", "5300", "5400", "5500", "6100", "6200", "6300", "6400", "6500" };
            var expenseNames = new[] { "Fuel", "Generation Operations", "Purchased Power", "Transmission", "Distribution", "Admin Salaries", "Office Expenses", "Professional Services", "Insurance", "Employee Benefits" };
            for (int i = 1; i <= 10; i++)
            {
                var num = $"VEND-{1000 + i}";
                var accountCode = expenseAccounts[(i - 1) % expenseAccounts.Length];
                var accountName = expenseNames[(i - 1) % expenseNames.Length];
                vendors.Add(Vendor.Create(num, $"Vendor {i}", $"{i} Supplier Rd", null, $"Vendor Contact {i}", $"vendor{i}@example.com", "Net 30", accountCode, accountName, null, $"+15557{654321 + i}", $"Seeded vendor {i}"));
            }

            await context.Vendors.AddRangeAsync(vendors, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Vendors", context.TenantInfo!.Identifier, vendors.Count);
        }

        // Seed Payees (10 records)
        if (!await context.Payees.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var payees = new List<Payee>();
            var payeeExpenseAccounts = new[] { "5210", "5410", "5510", "5710", "6100", "6200", "6300", "6400", "6500", "8200" };
            var payeeExpenseNames = new[] { "Generation Labor", "Transmission Labor", "Distribution Labor", "Customer Service Labor", "Admin Salaries", "Office Expenses", "Professional Services", "Insurance", "Employee Benefits", "Payroll Taxes" };
            for (int i = 1; i <= 10; i++)
            {
                var accountCode = payeeExpenseAccounts[(i - 1) % payeeExpenseAccounts.Length];
                var accountName = payeeExpenseNames[(i - 1) % payeeExpenseNames.Length];
                payees.Add(Payee.Create($"PAY-{1000 + i}", $"Payee {i}", $"{i} Payee Ave", accountCode, accountName, null, $"Seeded payee {i}", null));
            }

            await context.Payees.AddRangeAsync(payees, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Payees", context.TenantInfo!.Identifier, payees.Count);
        }

        // Seed Banks (5 records) - for check management and bank reconciliation
        if (!await context.Banks.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var banks = new List<Bank>
            {
                Bank.Create(
                    "BNK001",
                    "Chase Bank",
                    "021000021",
                    "CHASUS33",
                    "270 Park Avenue, New York, NY 10017",
                    "John Smith",
                    "+1-212-270-6000",
                    "business@chase.com",
                    "https://www.chase.com",
                    "Primary operating bank account",
                    "Main checking and payroll account"),

                Bank.Create(
                    "BNK002",
                    "Bank of America",
                    "026009593",
                    "BOFAUS3N",
                    "100 North Tryon Street, Charlotte, NC 28255",
                    "Sarah Johnson",
                    "+1-704-386-5681",
                    "business@bankofamerica.com",
                    "https://www.bankofamerica.com",
                    "Secondary operating account",
                    "Backup account for operations"),

                Bank.Create(
                    "BNK003",
                    "Wells Fargo Bank",
                    "121000248",
                    "WFBIUS6S",
                    "420 Montgomery Street, San Francisco, CA 94104",
                    "Michael Davis",
                    "+1-866-249-3302",
                    "business@wellsfargo.com",
                    "https://www.wellsfargo.com",
                    "Investment and savings account",
                    "Used for reserves and investments"),

                Bank.Create(
                    "BNK004",
                    "Citibank",
                    "021000089",
                    "CITIUS33",
                    "388 Greenwich Street, New York, NY 10013",
                    "Emily Chen",
                    "+1-212-559-1000",
                    "business@citi.com",
                    "https://www.citibank.com",
                    "International transactions account",
                    "Used for foreign currency and international payments"),

                Bank.Create(
                    "BNK005",
                    "US Bank",
                    "091000022",
                    "USBKUS44",
                    "425 Walnut Street, Cincinnati, OH 45202",
                    "Robert Wilson",
                    "+1-513-632-4000",
                    "business@usbank.com",
                    "https://www.usbank.com",
                    "Regional operations account",
                    "Used for regional branch operations")
            };

            await context.Banks.AddRangeAsync(banks, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Banks", context.TenantInfo!.Identifier, banks.Count);
        }

        // Seed Projects (10 records)
        if (!await context.Projects.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var projects = new List<Project>();
            for (int i = 1; i <= 10; i++)
            {
                projects.Add(Project.Create($"Project {i}", DateTime.UtcNow.Date, 50000m + i * 10000m, "ACME Corp", $"PM: Seed {i}", "Operations", $"Seeded project {i}"));
            }

            await context.Projects.AddRangeAsync(projects, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Projects", context.TenantInfo!.Identifier, projects.Count);
        }

        // Seed Members, Meters, Consumption, Invoices and Payments (multiple)
        if (!await context.Members.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var members = new List<Member>();
            var meters = new List<Meter>();
            var consumptions = new List<Consumption>();
            var invoices = new List<Invoice>();
            var payments = new List<Payment>();

            for (int i = 1; i <= 10; i++)
            {
                var memberNumber = $"MEM-{1000 + i}";
                var member = Member.Create(
                    memberNumber, 
                    $"Member {i}", 
                    $"{i} Electric Ave", 
                    DateTime.UtcNow.Date.AddYears(-1), 
                    $"PO Box {i}", 
                    $"+1555000{i:D4}", 
                    "Active", 
                    null, 
                    $"member{i}@example.com", 
                    $"+1555000{i:D4}", 
                    $"Contact {i}", 
                    "Residential", 
                    null, 
                    $"Seeded member {i}");
                members.Add(member);
            }

            await context.Members.AddRangeAsync(members, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Create meters, consumptions, invoices, and payments after members are saved
            for (int i = 0; i < members.Count; i++)
            {
                var member = members[i];
                var meterIndex = i + 1;

                // Create meter
                var meter = Meter.Create(
                    $"MTR-{1000 + meterIndex}", 
                    "Smart Meter", 
                    "AcmeMeters", 
                    $"SM-{1000 + meterIndex}", 
                    DateTime.UtcNow.Date, 
                    1m, 
                    $"SN-{1000 + meterIndex}", 
                    "Location", 
                    null, 
                    member.Id, 
                    true, 
                    "AMR", 
                    null, 
                    null, 
                    $"Seeded meter {meterIndex}");
                meters.Add(meter);
            }

            await context.Meters.AddRangeAsync(meters, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Create consumptions and invoices
            for (int i = 0; i < meters.Count; i++)
            {
                var meter = meters[i];
                var index = i + 1;

                // Create consumption
                var consumption = Consumption.Create(
                    meter.Id, 
                    DateTime.UtcNow.Date, 
                    1000m + index * 100m, 
                    900m + index * 90m, 
                    DateTime.UtcNow.ToString("yyyy-MM"), 
                    "Actual", 
                    1m, 
                    "AMR", 
                    $"Seeded consumption {index}");
                consumptions.Add(consumption);
            }

            await context.Consumption.AddRangeAsync(consumptions, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Create invoices
            for (int i = 0; i < consumptions.Count; i++)
            {
                var consumption = consumptions[i];
                var member = members[i];
                var index = i + 1;

                // Create invoice
                var invoice = Invoice.Create(
                    $"INV-{1000 + index}", 
                    member.Id, 
                    DateTime.UtcNow.Date, 
                    DateTime.UtcNow.Date.AddDays(30), 
                    consumption.Id, 
                    100m + index, 
                    10m, 
                    5m, 
                    0m, 
                    100m + index, 
                    DateTime.UtcNow.ToString("MMMM yyyy"), 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    $"Seeded invoice {index}");
                invoices.Add(invoice);
            }

            await context.Invoices.AddRangeAsync(invoices, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Create payments
            for (int i = 0; i < invoices.Count; i++)
            {
                var invoice = invoices[i];
                var member = members[i];
                var index = i + 1;

                // Create payment
                var payment = Payment.Create(
                    $"PAY-{2000 + index}", 
                    member.Id, 
                    DateTime.UtcNow.Date, 
                    invoice.GetOutstandingAmount(), 
                    "Cash", 
                    null, 
                    "1120", 
                    $"Seeded payment {index}");
                payment = payment.AllocateToInvoice(invoice.Id, invoice.GetOutstandingAmount());
                payments.Add(payment);
            }

            await context.Payments.AddRangeAsync(payments, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "[{Tenant}] seeded {Count} Members with {Meters} meters, {Consumptions} consumptions, {Invoices} invoices, and {Payments} payments", 
                context.TenantInfo!.Identifier, 
                members.Count, 
                meters.Count, 
                consumptions.Count, 
                invoices.Count, 
                payments.Count);
        }

        // Seed Fixed Assets (10 records)
        if (!await context.FixedAssets.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var firstDepr = await context.DepreciationMethods.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1120", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "7400", cancellationToken).ConfigureAwait(false);
            if (firstDepr != null && cashAccount != null && expenseAccount != null)
            {
                var assets = new List<FixedAsset>
                {
                    FixedAsset.Create("Office Laptop A", DateTime.UtcNow.Date, 2000m, firstDepr.Id, 5, 200m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-LAP-001", "HQ", "IT", null, null, null, null, null, null, "AcmeCorp", "Model-LAP-1", true, "Office laptop"),
                    FixedAsset.Create("Office Laptop B", DateTime.UtcNow.Date, 2200m, firstDepr.Id, 5, 220m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-LAP-002", "HQ", "IT", null, null, null, null, null, null, "AcmeCorp", "Model-LAP-2", true, "Office laptop B"),
                    FixedAsset.Create("Forklift", DateTime.UtcNow.Date, 15000m, firstDepr.Id, 7, 1500m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-FLK-001", "Warehouse", "Operations", null, null, null, null, null, null, "LiftCo", "FL-1", true, "Seeded forklift"),
                    FixedAsset.Create("Server Rack", DateTime.UtcNow.Date, 8000m, firstDepr.Id, 5, 800m, cashAccount.Id, expenseAccount.Id, "IT Equipment", "SN-SRV-001", "Data Center", "IT", null, null, null, null, null, null, "ServerCo", "Rack-Pro", true, "Data center server rack"),
                    FixedAsset.Create("Office Furniture Set", DateTime.UtcNow.Date, 5000m, firstDepr.Id, 10, 500m, cashAccount.Id, expenseAccount.Id, "Furniture", "SN-FURN-001", "HQ", "Admin", null, null, null, null, null, null, "FurniturePlus", "Desk-Set-A", true, "Office furniture"),
                    FixedAsset.Create("Delivery Van", DateTime.UtcNow.Date, 35000m, firstDepr.Id, 8, 3500m, cashAccount.Id, expenseAccount.Id, "Vehicle", "SN-VAN-001", "Fleet", "Logistics", null, null, null, null, null, null, "AutoMakers", "Van-2024", true, "Delivery vehicle"),
                    FixedAsset.Create("Industrial Printer", DateTime.UtcNow.Date, 12000m, firstDepr.Id, 6, 1200m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-PRT-001", "Production", "Manufacturing", null, null, null, null, null, null, "PrintTech", "IP-5000", true, "Industrial printer"),
                    FixedAsset.Create("Conference Room System", DateTime.UtcNow.Date, 6000m, firstDepr.Id, 5, 600m, cashAccount.Id, expenseAccount.Id, "IT Equipment", "SN-CONF-001", "HQ", "IT", null, null, null, null, null, null, "TechSys", "ConferPro", true, "Conference AV system"),
                    FixedAsset.Create("Warehouse Shelving", DateTime.UtcNow.Date, 10000m, firstDepr.Id, 15, 667m, cashAccount.Id, expenseAccount.Id, "Furniture", "SN-SHLV-001", "Warehouse", "Operations", null, null, null, null, null, null, "ShelvingCo", "Heavy-Duty", true, "Industrial shelving"),
                    FixedAsset.Create("HVAC System", DateTime.UtcNow.Date, 25000m, firstDepr.Id, 12, 2083m, cashAccount.Id, expenseAccount.Id, "Building", "SN-HVAC-001", "HQ", "Facilities", null, null, null, null, null, null, "ClimateControl", "CC-Pro-2000", true, "HVAC system")
                };

                await context.FixedAssets.AddRangeAsync(assets, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} FixedAssets", context.TenantInfo!.Identifier, assets.Count);
            }
        }

        // Seed JournalEntries with approval workflow (mix of Pending, Approved, Posted)
        if (!await context.JournalEntries.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var period = await context.AccountingPeriods.OrderByDescending(p => p.FiscalYear).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1120", cancellationToken).ConfigureAwait(false);
            var revenueAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "4110", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "5200", cancellationToken).ConfigureAwait(false);
            
            if (period != null && cashAccount != null && revenueAccount != null && expenseAccount != null)
            {
                // Create first journal entry - Approved and Posted
                var je1 = JournalEntry.Create(DateTime.UtcNow.AddDays(-10), "JE-1000", "Seed residential energy sales journal entry", "Seeding", period.Id);
                await context.JournalEntries.AddAsync(je1, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                var cashLine1 = JournalEntryLine.Create(je1.Id, cashAccount.Id, 25000m, 0m, "Cash from residential energy sales");
                var revenueLine1 = JournalEntryLine.Create(je1.Id, revenueAccount.Id, 0m, 25000m, "Residential energy revenue");
                await context.JournalEntryLines.AddRangeAsync(new[] { cashLine1, revenueLine1 }, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                je1.Approve("system.admin");
                je1 = je1.Post();
                context.JournalEntries.Update(je1);
                
                var gl1 = GeneralLedger.Create(je1.Id, cashAccount.Id, cashAccount.AccountCode, 25000m, 0m, DateTime.UtcNow.AddDays(-10), "General", null, "JE-1000", "Seeding", je1.Id, period.Id, "Cash receipt from residential energy sales");
                var gl2 = GeneralLedger.Create(je1.Id, revenueAccount.Id, revenueAccount.AccountCode, 0m, 25000m, DateTime.UtcNow.AddDays(-10), "General", null, "JE-1000", "Seeding", je1.Id, period.Id, "Residential energy revenue recognition");
                await context.GeneralLedgers.AddRangeAsync(new[] { gl1, gl2 }, cancellationToken).ConfigureAwait(false);

                // Create second journal entry - Approved but not yet Posted
                var je2 = JournalEntry.Create(DateTime.UtcNow.AddDays(-5), "JE-1001", "Operations expense journal entry", "Seeding", period.Id);
                await context.JournalEntries.AddAsync(je2, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                var expenseLine = JournalEntryLine.Create(je2.Id, expenseAccount.Id, 5000m, 0m, "Operations expense");
                var cashLine2 = JournalEntryLine.Create(je2.Id, cashAccount.Id, 0m, 5000m, "Cash payment");
                await context.JournalEntryLines.AddRangeAsync(new[] { expenseLine, cashLine2 }, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                je2.Approve("finance.manager");
                context.JournalEntries.Update(je2);

                // Create third journal entry - Pending approval
                var je3 = JournalEntry.Create(DateTime.UtcNow.AddDays(-2), "JE-1002", "Pending approval journal entry", "Seeding", period.Id);
                await context.JournalEntries.AddAsync(je3, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                var expenseLine3 = JournalEntryLine.Create(je3.Id, expenseAccount.Id, 3000m, 0m, "Pending expense");
                var cashLine3 = JournalEntryLine.Create(je3.Id, cashAccount.Id, 0m, 3000m, "Cash payment");
                await context.JournalEntryLines.AddRangeAsync(new[] { expenseLine3, cashLine3 }, cancellationToken).ConfigureAwait(false);

                // Create PostingBatch with approved journal entry
                var batch = PostingBatch.Create("BATCH-1000", DateTime.UtcNow, "Seed batch with approved entry", period.Id);
                batch.AddJournalEntry(je1);
                batch.Approve("system.admin");
                await context.PostingBatches.AddAsync(batch, cancellationToken).ConfigureAwait(false);

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded 3 JournalEntries with approval workflow and 1 PostingBatch", context.TenantInfo!.Identifier);
            }
        }

        // Seed Accruals (10 records in pending state)
        if (!await context.Accruals.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var accruals = new List<Accrual>();
            for (int i = 1; i <= 10; i++)
            {
                var accrual = Accrual.Create($"ACCR-{1000 + i}", DateTime.UtcNow.Date.AddDays(-i * 5), 1500m + i * 250m, $"Seeded accrual for expense category {i}");
                accruals.Add(accrual);
            }

            await context.Accruals.AddRangeAsync(accruals, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Accruals", context.TenantInfo!.Identifier, accruals.Count);
        }

        // Seed Deferred Revenues (10 records)
        if (!await context.DeferredRevenues.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var deferredRevenues = new List<DeferredRevenue>();
            for (int i = 1; i <= 10; i++)
            {
                deferredRevenues.Add(DeferredRevenue.Create($"DEF-{1000 + i}", DateTime.UtcNow.Date.AddMonths(i), 1000m + i * 200m, $"Seeded deferred revenue for service {i}"));
            }

            await context.DeferredRevenues.AddRangeAsync(deferredRevenues, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} DeferredRevenues", context.TenantInfo!.Identifier, deferredRevenues.Count);
        }

        // Seed a Regulatory Report (unchanged)
        if (!await context.RegulatoryReports.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var start = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            var report = RegulatoryReport.Create("FERC-Form-1-Seed", "FERC Form 1", "Annual", start, end, end.AddMonths(3), "FERC", false, "Seeded regulatory report");
            await context.RegulatoryReports.AddAsync(report, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded RegulatoryReport {Report}", context.TenantInfo!.Identifier, report.ReportName);
        }

        // Inventory Items (expanded)
        if (!await context.InventoryItems.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var inventory = new List<InventoryItem>
            {
                InventoryItem.Create("SKU-TRANS-001", "Transformer Coil A", 5m, 12500m, "Main transformer coil (spare)"),
                InventoryItem.Create("SKU-METER-001", "Meter Type X", 50m, 120m, "Smart meters for residential installs"),
                InventoryItem.Create("SKU-CABLE-001", "Underground Cable 1km", 20m, 800m, "High grade underground cable"),
                InventoryItem.Create("SKU-FUSE-001", "High Voltage Fuse", 200m, 15m, "Replacement fuses"),
                InventoryItem.Create("SKU-SWITCH-001", "Disconnect Switch", 10m, 450m, "Pole-mounted disconnect switch"),
                InventoryItem.Create("SKU-TOOL-001", "Spanner Set", 100m, 25m, "Tooling set"),
                InventoryItem.Create("SKU-PAINT-001", "Touch-up Paint", 40m, 8m, "Paint for maintenance"),
                InventoryItem.Create("SKU-BATTERY-001", "Battery Pack", 60m, 45m, "Replacement battery pack"),
                InventoryItem.Create("SKU-CABLE-002", "Overhead Cable 100m", 30m, 95m, "Overhead cable"),
                InventoryItem.Create("SKU-FILTER-001", "Air Filter", 500m, 12m, "HVAC filter"),
                InventoryItem.Create("SKU-TRANS-002", "Transformer Coil B", 3m, 15000m, "Secondary transformer coil"),
                InventoryItem.Create("SKU-CONN-001", "Connector Kit", 500m, 5m, "Connector assortment")
            };

            await context.InventoryItems.AddRangeAsync(inventory, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} InventoryItems", context.TenantInfo!.Identifier, inventory.Count);
        }

        // Rate Schedules (keep existing)
        if (!await context.RateSchedules.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var rs1 = RateSchedule.Create("RS-RES", "Residential Standard", DateTime.UtcNow.Date, 0.12m, 5m, false, null, null, "Default residential rate schedule");
            rs1 = rs1.AddTier(1, 100m, 0.10m)
                     .AddTier(2, 300m, 0.12m)
                     .AddTier(3, 0m, 0.15m); // 0 => unlimited

            var rs2 = RateSchedule.Create("RS-COMM", "Commercial Standard", DateTime.UtcNow.Date, 0.10m, 25m, false, 2.5m, null, "Default commercial rate schedule");
            rs2 = rs2.AddTier(1, 500m, 0.09m)
                     .AddTier(2, 0m, 0.11m);

            var rs3 = RateSchedule.Create("RS-IND", "Industrial Standard", DateTime.UtcNow.Date, 0.08m, 50m, false, 5m, null, "Default industrial rate schedule");
            rs3 = rs3.AddTier(1, 1000m, 0.07m)
                     .AddTier(2, 0m, 0.09m);

            await context.RateSchedules.AddAsync(rs1, cancellationToken).ConfigureAwait(false);
            await context.RateSchedules.AddAsync(rs2, cancellationToken).ConfigureAwait(false);
            await context.RateSchedules.AddAsync(rs3, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded RateSchedules: {Codes}", context.TenantInfo!.Identifier, string.Join(",", rs1.RateCode, rs2.RateCode, rs3.RateCode));
        }

        // Additional Payments and Allocations (left as-is; member loop created payments)
        if (!await context.Payments.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            // Payments already seeded per-member in the members block above; keep any additional seeds minimal
        }

        // Patronage Capital allocations (increase per member)
        if (!await context.PatronageCapitals.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var members = await context.Members.ToListAsync(cancellationToken).ConfigureAwait(false);
            var pcList = members
                .SelectMany(m => new[]
                {
                    PatronageCapital.Create(m.Id, DateTime.UtcNow.Year - 1, 1500m, $"Allocated patronage capital FY{DateTime.UtcNow.Year - 1}", "Seeded patronage allocation"),
                    PatronageCapital.Create(m.Id, DateTime.UtcNow.Year - 2, 900m, $"Allocated patronage capital FY{DateTime.UtcNow.Year - 2}", "Seeded patronage allocation")
                })
                .ToList();

            if (pcList.Count > 0)
            {
                await context.PatronageCapitals.AddRangeAsync(pcList, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} PatronageCapital records", context.TenantInfo!.Identifier, pcList.Count);
            }
        }

        // Security Deposits (increase per member)
        if (!await context.SecurityDeposits.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var members = await context.Members.ToListAsync(cancellationToken).ConfigureAwait(false);
            var sdList = members
                .SelectMany(m => new[]
                {
                    SecurityDeposit.Create(m.Id, 200m, DateTime.UtcNow.Date.AddYears(-1), "Initial security deposit"),
                    SecurityDeposit.Create(m.Id, 150m, DateTime.UtcNow.Date.AddMonths(-6), "Additional deposit")
                })
                .ToList();

            if (sdList.Count > 0)
            {
                await context.SecurityDeposits.AddRangeAsync(sdList, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} SecurityDeposits", context.TenantInfo!.Identifier, sdList.Count);
            }
        }

        // Seed TaxCodes (10 records)
        if (!await context.TaxCodes.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var revenueAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "4110", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "8100", cancellationToken).ConfigureAwait(false);
            if (revenueAccount != null && expenseAccount != null)
            {
                var taxCodes = new List<TaxCode>
                {
                    TaxCode.Create("VAT-STD", "Standard VAT", TaxType.VAT, 0.20m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "National", null, expenseAccount.Id, "National Tax Authority", "TX001", "Standard", "Standard VAT rate"),
                    TaxCode.Create("VAT-RED", "Reduced VAT", TaxType.VAT, 0.10m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "National", null, expenseAccount.Id, "National Tax Authority", "TX002", "Reduced", "Reduced VAT rate"),
                    TaxCode.Create("GST-STD", "Standard GST", TaxType.GST, 0.15m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "Federal", null, expenseAccount.Id, "Federal Tax Authority", "TX003", "Standard", "Standard GST rate"),
                    TaxCode.Create("USE-TAX", "Use Tax", TaxType.UseTax, 0.07m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "State", null, expenseAccount.Id, "State Tax Authority", "TX004", "UseTax", "Use tax"),
                    TaxCode.Create("SALES-CA", "California Sales Tax", TaxType.SalesTax, 0.0725m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "California", null, expenseAccount.Id, "CA Tax Authority", "TX005", "State", "CA sales tax"),
                    TaxCode.Create("WHT-STD", "Standard Withholding Tax", TaxType.Withholding, 0.05m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "National", null, expenseAccount.Id, "National Tax Authority", "TX006", "WHT", "Standard withholding tax"),
                    TaxCode.Create("EXCISE-1", "Excise Tax Type 1", TaxType.Excise, 0.12m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "Federal", null, expenseAccount.Id, "Federal Tax Authority", "TX007", "Excise", "Excise tax type 1"),
                    TaxCode.Create("VAT-ZERO", "Zero-Rated VAT", TaxType.VAT, 0.0m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "National", null, expenseAccount.Id, "National Tax Authority", "TX008", "Zero", "Zero-rated VAT"),
                    TaxCode.Create("PROP-TAX", "Property Tax", TaxType.Property, 0.015m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "Municipal", null, expenseAccount.Id, "Municipal Tax Authority", "TX009", "Property", "Property tax"),
                    TaxCode.Create("OTHER-TAX", "Other Tax", TaxType.Other, 0.08m, revenueAccount.Id, DateTime.UtcNow.Date.AddYears(-1), false, "Local", null, expenseAccount.Id, "Local Tax Authority", "TX010", "Other", "Miscellaneous tax")
                };

                await context.TaxCodes.AddRangeAsync(taxCodes, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken). ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} TaxCodes", context.TenantInfo!.Identifier, taxCodes.Count);
            }
        }

        // Seed CostCenters (10 records)
        if (!await context.CostCenters.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var costCenters = new List<CostCenter>
            {
                CostCenter.Create("CC-001", "Sales Department", CostCenterType.Department, null, null, "John Manager", 150000m, "HQ Floor 1", DateTime.UtcNow.Date.AddYears(-1), null, "Sales cost center", "Handles all sales operations"),
                CostCenter.Create("CC-002", "Marketing Department", CostCenterType.Department, null, null, "Jane Director", 120000m, "HQ Floor 2", DateTime.UtcNow.Date.AddYears(-1), null, "Marketing cost center", "Marketing and promotions"),
                CostCenter.Create("CC-003", "IT Department", CostCenterType.Department, null, null, "Bob Tech", 200000m, "HQ Floor 3", DateTime.UtcNow.Date.AddYears(-1), null, "IT cost center", "IT infrastructure and support"),
                CostCenter.Create("CC-004", "HR Department", CostCenterType.Department, null, null, "Alice HR", 80000m, "HQ Floor 1", DateTime.UtcNow.Date.AddYears(-1), null, "HR cost center", "Human resources"),
                CostCenter.Create("CC-005", "Finance Department", CostCenterType.Department, null, null, "Charlie CFO", 100000m, "HQ Floor 2", DateTime.UtcNow.Date.AddYears(-1), null, "Finance cost center", "Financial operations"),
                CostCenter.Create("CC-006", "Operations Division", CostCenterType.Division, null, null, "Dave Ops", 250000m, "Warehouse A", DateTime.UtcNow.Date.AddYears(-1), null, "Operations cost center", "Day-to-day operations"),
                CostCenter.Create("CC-007", "Research Project Alpha", CostCenterType.Project, null, null, "Eve Research", 300000m, "Lab Building", DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(2), "R&D project cost center", "Research project tracking"),
                CostCenter.Create("CC-008", "Customer Service Unit", CostCenterType.BusinessUnit, null, null, "Frank Support", 90000m, "Service Center", DateTime.UtcNow.Date.AddYears(-1), null, "Customer service cost center", "Customer support operations"),
                CostCenter.Create("CC-009", "Production Location 1", CostCenterType.Location, null, null, "Grace Production", 400000m, "Factory Floor A", DateTime.UtcNow.Date.AddYears(-1), null, "Production cost center", "Manufacturing line 1"),
                CostCenter.Create("CC-010", "Administrative Support", CostCenterType.Other, null, null, "Henry Admin", 70000m, "HQ Floor 1", DateTime.UtcNow.Date.AddYears(-1), null, "Admin cost center", "Administrative support services")
            };

            await context.CostCenters.AddRangeAsync(costCenters, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} CostCenters", context.TenantInfo!.Identifier, costCenters.Count);
        }

        // Seed WriteOffs (10 records)
        if (!await context.WriteOffs.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var receivableAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1210", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "5730", cancellationToken).ConfigureAwait(false);
            var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (receivableAccount != null && expenseAccount != null && members.Count > 0)
            {
                var writeOffs = new List<WriteOff>();
                for (int i = 0; i < Math.Min(10, members.Count); i++)
                {
                    var member = members[i];
                    writeOffs.Add(WriteOff.Create($"WO-{1000 + i}", DateTime.UtcNow.Date.AddDays(-i * 10), WriteOffType.BadDebt, 500m + i * 100m, receivableAccount.Id, expenseAccount.Id, member.Id, member.Name, null, null, "Uncollectible debt", $"Seeded write-off {i + 1}"));
                }

                await context.WriteOffs.AddRangeAsync(writeOffs, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} WriteOffs", context.TenantInfo!.Identifier, writeOffs.Count);
            }
        }

        // Seed DebitMemos (10 records)
        if (!await context.DebitMemos.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (members.Count > 0)
            {
                var debitMemos = new List<DebitMemo>();
                for (int i = 0; i < Math.Min(10, members.Count); i++)
                {
                    var member = members[i];
                    debitMemos.Add(DebitMemo.Create($"DM-{1000 + i}", DateTime.UtcNow.Date.AddDays(-i * 7), 250m + i * 50m, "Member", member.Id, null, "Additional charges", $"Seeded debit memo {i + 1}"));
                }

                await context.DebitMemos.AddRangeAsync(debitMemos, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} DebitMemos", context.TenantInfo!.Identifier, debitMemos.Count);
            }
        }

        // Seed CreditMemos (10 records)
        if (!await context.CreditMemos.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (members.Count > 0)
            {
                var creditMemos = new List<CreditMemo>();
                for (int i = 0; i < Math.Min(10, members.Count); i++)
                {
                    var member = members[i];
                    var memo = CreditMemo.Create($"CM-{1000 + i}", DateTime.UtcNow.Date.AddDays(-i * 6), 180m + i * 30m, "Customer", member.Id, null, "Service credit", $"Seeded credit memo {i + 1}");
                    creditMemos.Add(memo);
                }

                await context.CreditMemos.AddRangeAsync(creditMemos, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} CreditMemos", context.TenantInfo!.Identifier, creditMemos.Count);
            }
        }

        // Seed BankReconciliations (10 records)
        if (!await context.BankReconciliations.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var bankAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1120", cancellationToken).ConfigureAwait(false);
            if (bankAccount != null)
            {
                var reconciliations = new List<BankReconciliation>();
                for (int i = 1; i <= 10; i++)
                {
                    var statementBalance = 50000m + i * 5000m;
                    var bookBalance = statementBalance - (i * 100m);
                    var recon = BankReconciliation.Create(bankAccount.Id, DateTime.UtcNow.Date.AddMonths(-i), statementBalance, bookBalance, $"STMT-{1000 + i}", $"Seeded bank reconciliation {i}");
                    reconciliations.Add(recon);
                }

                await context.BankReconciliations.AddRangeAsync(reconciliations, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} BankReconciliations", context.TenantInfo!.Identifier, reconciliations.Count);
            }
        }

        // Seed RecurringJournalEntries (10 records)
        if (!await context.RecurringJournalEntries.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var debitAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "5200", cancellationToken).ConfigureAwait(false);
            var creditAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1120", cancellationToken).ConfigureAwait(false);
            
            if (debitAccount != null && creditAccount != null)
            {
                var recurringEntries = new List<RecurringJournalEntry>
                {
                    RecurringJournalEntry.Create("RJE-001", "Monthly Rent Payment", RecurrenceFrequency.Monthly, 5000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(1), null, "Recurring rent"),
                    RecurringJournalEntry.Create("RJE-002", "Monthly Office Supplies", RecurrenceFrequency.Monthly, 500m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddMonths(6), null, "Office supplies purchase"),
                    RecurringJournalEntry.Create("RJE-003", "Quarterly Insurance", RecurrenceFrequency.Quarterly, 3000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(2), null, "Insurance premium"),
                    RecurringJournalEntry.Create("RJE-004", "Annual License Fee", RecurrenceFrequency.Annually, 12000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(5), null, "Software license"),
                    RecurringJournalEntry.Create("RJE-005", "Quarterly Maintenance", RecurrenceFrequency.Quarterly, 8000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(3), null, "Equipment maintenance"),
                    RecurringJournalEntry.Create("RJE-006", "Monthly Utilities", RecurrenceFrequency.Monthly, 1500m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(1), null, "Utility bills"),
                    RecurringJournalEntry.Create("RJE-007", "Monthly Payroll Processing", RecurrenceFrequency.Monthly, 45000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(1), null, "Payroll processing"),
                    RecurringJournalEntry.Create("RJE-008", "Annually Subscription Fees", RecurrenceFrequency.Annually, 25000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddMonths(3), null, "Annual subscription fees"),
                    RecurringJournalEntry.Create("RJE-009", "Custom Interval Payment", RecurrenceFrequency.Custom, 2000m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(1), 45, "45-day interval payment"),
                    RecurringJournalEntry.Create("RJE-010", "Monthly Marketing Spend", RecurrenceFrequency.Monthly, 7500m, debitAccount.Id, creditAccount.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddYears(1), null, "Marketing campaign")
                };

                await context.RecurringJournalEntries.AddRangeAsync(recurringEntries, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} RecurringJournalEntries", context.TenantInfo!.Identifier, recurringEntries.Count);
            }
        }
        


        // Seed Bills with approval workflow and BillLineItems (10 bills with 2-3 line items each)
        if (!await context.Bills.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var vendors = await context.Vendors.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var expenseAccounts = await context.ChartOfAccounts
                .Where(a => a.AccountType == "Expense" && a.IsActive)
                .Take(10)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            
            if (vendors.Count > 0 && expenseAccounts.Count > 0)
            {
                var bills = new List<Bill>();
                
                for (int i = 0; i < Math.Min(10, vendors.Count); i++)
                {
                    var vendor = vendors[i];
                    var billDate = DateTime.UtcNow.Date.AddDays(-30 + i * 3);
                    var dueDate = billDate.AddDays(30);
                    
                    var bill = Bill.Create(
                        $"BILL-{2000 + i}", 
                        vendor.Id, 
                        billDate, 
                        dueDate, 
                        $"Seeded bill {i + 1} from {vendor.Name}", 
                        null, 
                        "Net 30", 
                        $"PO-{1000 + i}",
                        $"Notes for bill {i + 1}");

                    bills.Add(bill);
                }

                await context.Bills.AddRangeAsync(bills, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                
                // Approve first bill
                if (bills.Count > 0)
                {
                    bills[0].Approve("ap.manager");
                    context.Bills.Update(bills[0]);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
                
                logger.LogInformation("[{Tenant}] seeded {Count} Bills (1 approved, 9 pending)", context.TenantInfo!.Identifier, bills.Count);
            }
        }

        // Seed Checks (10 records)
        if (!await context.Checks.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var banks = await context.Banks.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var payees = await context.Payees.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1120", cancellationToken).ConfigureAwait(false);

            if (banks.Count > 0 && payees.Count > 0 && cashAccount != null)
            {
                var checks = new List<Check>();
                for (int i = 0; i < Math.Min(10, payees.Count); i++)
                {
                    var bank = banks[i % banks.Count];
                    var payee = payees[i];
                    
                    var check = Check.Create(
                        $"CHK-{3000 + i}",
                        cashAccount.AccountCode,
                        cashAccount.AccountName,
                        bank.Id,
                        bank.Name,
                        $"Check for {payee.Name}",
                        $"Seeded check payment {i + 1}");
                    
                    // Issue the check to the payee
                    check = check.Issue(
                        1500m + (i * 250m), // amount
                        payee.Name, // payeeName
                        DateTime.UtcNow.Date.AddDays(-i * 5), // issuedDate
                        payee.Id, // payeeId
                        null, // vendorId
                        null, // paymentId
                        null, // expenseId
                        $"Payment to {payee.Name}"); // memo
                    
                    checks.Add(check);
                }

                await context.Checks.AddRangeAsync(checks, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} Checks", context.TenantInfo!.Identifier, checks.Count);
            }
        }

        // Seed Customers (10 records)
        if (!await context.Customers.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var revenueAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "4110", cancellationToken).ConfigureAwait(false);
            
            if (revenueAccount != null)
            {
                var customers = new List<Customer>();
                var customerTypes = new[] { "Residential", "Commercial", "Industrial" };
                
                for (int i = 1; i <= 10; i++)
                {
                    var customerType = customerTypes[(i - 1) % customerTypes.Length];
                    customers.Add(Customer.Create(
                        $"CUST-{4000 + i}", // customerNumber
                        $"Customer {i} Corp", // customerName
                        customerType, // customerType
                        $"{i} Customer Street", // billingAddress
                        null, // shippingAddress
                        $"customer{i}@example.com", // email
                        $"+1555100{i:D4}", // phone
                        $"Contact Person {i}", // contactName
                        5000m, // creditLimit
                        "Net 30", // paymentTerms
                        false, // taxExempt
                        null, // taxId
                        0m, // discountPercentage
                        null, // defaultRateScheduleId
                        null, // receivableAccountId
                        null, // salesRepresentative
                        $"Seeded customer {i}")); // notes
                }

                await context.Customers.AddRangeAsync(customers, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} Customers", context.TenantInfo!.Identifier, customers.Count);
            }
        }

        // Seed PrepaidExpenses (10 records)
        if (!await context.PrepaidExpenses.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var prepaidAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1410", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "6400", cancellationToken).ConfigureAwait(false);

            if (prepaidAccount != null && expenseAccount != null)
            {
                var prepaidExpenses = new List<PrepaidExpense>();
                var categories = new[] { "Insurance", "Rent", "Software Licenses", "Maintenance Contracts", "Subscriptions" };
                
                for (int i = 1; i <= 10; i++)
                {
                    var category = categories[(i - 1) % categories.Length];
                    var startDate = DateTime.UtcNow.Date.AddDays(-30);
                    var endDate = startDate.AddMonths(12);
                    var totalAmount = 12000m + (i * 1000m);
                    
                    prepaidExpenses.Add(PrepaidExpense.Create(
                        $"PPE-{1000 + i}", // prepaidNumber
                        $"{category} Prepayment {i}", // description
                        totalAmount, // totalAmount
                        startDate, // startDate
                        endDate, // endDate
                        prepaidAccount.Id, // prepaidAssetAccountId
                        expenseAccount.Id, // expenseAccountId
                        startDate, // paymentDate
                        "Monthly", // amortizationSchedule
                        null, // vendorId
                        null, // vendorName
                        null, // paymentId
                        null, // costCenterId
                        null, // periodId
                        $"Seeded prepaid expense for {category}")) ; // notes
                }

                await context.PrepaidExpenses.AddRangeAsync(prepaidExpenses, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} PrepaidExpenses", context.TenantInfo!.Identifier, prepaidExpenses.Count);
            }
        }

        // Seed InterCompanyTransactions (10 records)
        if (!await context.InterCompanyTransactions.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1120", cancellationToken).ConfigureAwait(false);
            var revenueAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "4110", cancellationToken).ConfigureAwait(false);

            if (cashAccount != null && revenueAccount != null)
            {
                var interCompanyTransactions = new List<InterCompanyTransaction>();
                var companies = new[] { "Subsidiary A", "Subsidiary B", "Division C", "Branch D", "Affiliate E" };
                
                for (int i = 1; i <= 10; i++)
                {
                    var fromCompany = companies[(i - 1) % companies.Length];
                    var toCompany = companies[i % companies.Length];
                    
                    interCompanyTransactions.Add(InterCompanyTransaction.Create(
                        $"ICT-{1000 + i}", // transactionNumber
                        DefaultIdType.NewGuid(), // fromEntityId
                        fromCompany, // fromEntityName
                        DefaultIdType.NewGuid(), // toEntityId
                        toCompany, // toEntityName
                        DateTime.UtcNow.Date.AddDays(-i * 10), // transactionDate
                        15000m + (i * 2000m), // amount
                        "Service Transfer", // transactionType
                        cashAccount.Id, // fromAccountId
                        revenueAccount.Id, // toAccountId
                        $"REF-{1000 + i}", // referenceNumber
                        DateTime.UtcNow.Date.AddDays(30 - (i * 10)), // dueDate
                        true, // requiresElimination
                        null, // periodId
                        $"Service transfer {i}", // description
                        $"Intercompany service transfer from {fromCompany} to {toCompany}")); // notes
                }

                await context.InterCompanyTransactions.AddRangeAsync(interCompanyTransactions, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} InterCompanyTransactions", context.TenantInfo!.Identifier, interCompanyTransactions.Count);
            }
        }


        // Seed RetainedEarnings (yearly records for last 5 years)
        if (!await context.RetainedEarnings.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var currentYear = DateTime.UtcNow.Year;
            var retainedEarningsList = new List<RetainedEarnings>();
            
            var openingBalance = 500000m;
            for (int i = 0; i < 5; i++)
            {
                var year = currentYear - i;
                var netIncome = 120000m + (i * 15000m); // Increasing income over years
                var dividends = 50000m + (i * 5000m);
                var adjustments = i * 2000m;
                var closingBalance = openingBalance + netIncome - dividends + adjustments;
                
                var fiscalStart = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var fiscalEnd = new DateTime(year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
                
                var retainedEarning = RetainedEarnings.Create(
                    year, // fiscalYear
                    openingBalance, // openingBalance
                    fiscalStart, // fiscalYearStartDate
                    fiscalEnd, // fiscalYearEndDate
                    null, // retainedEarningsAccountId
                    $"Retained earnings for fiscal year {year}"); // notes
                
                // Update with net income, distributions, and adjustments
                retainedEarning = retainedEarning.UpdateNetIncome(netIncome);
                retainedEarning = retainedEarning.RecordDistribution(dividends, DateTime.UtcNow.Date, "Annual dividend");
                
                retainedEarningsList.Add(retainedEarning);
                
                openingBalance = closingBalance; // Next year's opening is this year's closing
            }

            await context.RetainedEarnings.AddRangeAsync(retainedEarningsList, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} RetainedEarnings records", context.TenantInfo!.Identifier, retainedEarningsList.Count);
        }

        // Seed FiscalPeriodClose records (monthly for current year)
        if (!await context.FiscalPeriodCloses.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var currentYear = DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;
            var fiscalPeriodCloses = new List<FiscalPeriodClose>();
            
            for (int month = 1; month <= currentMonth; month++)
            {
                var periodStart = new DateTime(currentYear, month, 1, 0, 0, 0, DateTimeKind.Utc);
                var periodEnd = periodStart.AddMonths(1).AddDays(-1);
                
                // Get or create period ID for this month
                var periodName = $"{currentYear}-{month:D2}";
                var existingPeriod = await context.AccountingPeriods.FirstOrDefaultAsync(
                    p => p.Name == periodName, cancellationToken).ConfigureAwait(false);
                
                var periodId = existingPeriod?.Id ?? DefaultIdType.NewGuid();
                
                fiscalPeriodCloses.Add(FiscalPeriodClose.Create(
                    $"CLOSE-{currentYear}-{month:D2}", // closeNumber
                    periodId, // periodId
                    "MonthEnd", // closeType
                    periodStart, // periodStartDate
                    periodEnd, // periodEndDate
                    "System Admin", // initiatedBy
                    $"Period close for {periodStart:MMMM yyyy}")); // notes
            }

            await context.FiscalPeriodCloses.AddRangeAsync(fiscalPeriodCloses, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} FiscalPeriodClose records", context.TenantInfo!.Identifier, fiscalPeriodCloses.Count);
        }


        // Seed AccountsPayableAccounts (10 records)
        if (!await context.AccountsPayableAccounts.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var apAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "2110", cancellationToken).ConfigureAwait(false);
            var vendors = await context.Vendors.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (apAccount != null && vendors.Count > 0)
            {
                var apAccounts = new List<AccountsPayableAccount>();
                
                for (int i = 0; i < vendors.Count; i++)
                {
                    var vendor = vendors[i];
                    var balance = 5000m + (i * 1500m);
                    
                    var apAccountEntity = AccountsPayableAccount.Create(
                        $"AP-{5000 + i}", // accountNumber
                        vendor.Name, // accountName
                        apAccount.Id, // generalLedgerAccountId
                        null, // periodId
                        $"AP account for vendor {vendor.Name}"); // notes
                    
                    // Update balance with aging distribution
                    apAccountEntity = apAccountEntity.UpdateBalance(
                        balance * 0.4m, // current0to30
                        balance * 0.3m, // days31to60
                        balance * 0.2m, // days61to90
                        balance * 0.1m); // over90Days
                    
                    apAccounts.Add(apAccountEntity);
                }

                await context.AccountsPayableAccounts.AddRangeAsync(apAccounts, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} AccountsPayableAccounts", context.TenantInfo!.Identifier, apAccounts.Count);
            }
        }

        // Seed AccountsReceivableAccounts (10 records)
        if (!await context.AccountsReceivableAccounts.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var arAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1210", cancellationToken).ConfigureAwait(false);
            var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (arAccount != null && members.Count > 0)
            {
                var arAccounts = new List<AccountsReceivableAccount>();
                
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];
                    var balance = 2000m + (i * 500m);
                    
                    var arAccountEntity = AccountsReceivableAccount.Create(
                        $"AR-{6000 + i}", // accountNumber
                        member.Name, // accountName
                        arAccount.Id, // generalLedgerAccountId
                        null, // periodId
                        $"AR account for member {member.Name}"); // notes
                    
                    // Update balance with aging distribution
                    arAccountEntity = arAccountEntity.UpdateBalance(
                        balance * 0.5m, // current0to30
                        balance * 0.3m, // days31to60
                        balance * 0.15m, // days61to90
                        balance * 0.05m); // over90Days
                    
                    arAccounts.Add(arAccountEntity);
                }

                await context.AccountsReceivableAccounts.AddRangeAsync(arAccounts, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} AccountsReceivableAccounts", context.TenantInfo!.Identifier, arAccounts.Count);
            }
        }

        // Seed PaymentAllocations (link payments to invoices)
        if (!await context.PaymentAllocations.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var payments = await context.Payments.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var invoices = await context.Invoices.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (payments.Count > 0 && invoices.Count > 0)
            {
                var paymentAllocations = new List<PaymentAllocation>();
                
                for (int i = 0; i < Math.Min(payments.Count, invoices.Count); i++)
                {
                    var payment = payments[i];
                    var invoice = invoices[i];
                    
                    var allocation = PaymentAllocation.Create(
                        payment.Id, // paymentId
                        invoice.Id, // invoiceId
                        payment.Amount * 0.8m, // amount (80% of payment)
                        $"Payment allocation {i + 1}"); // notes
                    
                    paymentAllocations.Add(allocation);
                }

                await context.PaymentAllocations.AddRangeAsync(paymentAllocations, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} PaymentAllocations", context.TenantInfo!.Identifier, paymentAllocations.Count);
            }
        }


        logger.LogInformation("[{Tenant}] completed seeding all accounting entities", context.TenantInfo!.Identifier);
    }
}
