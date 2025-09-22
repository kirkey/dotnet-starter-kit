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
        // Seed Chart of Accounts (expanded)
        if (!await context.ChartOfAccounts.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var accounts = new List<ChartOfAccount>
            {
                // Top-level control accounts
                ChartOfAccount.Create("1000", "Assets", "Asset", "General", null, "1", 0m, true, "Debit", true, null, "Assets control account"),
                ChartOfAccount.Create("2000", "Liabilities", "Liability", "General", null, "2", 0m, true, "Credit", true, null, "Liabilities control account"),
                ChartOfAccount.Create("3000", "Equity", "Equity", "General", null, "3", 0m, true, "Credit", true, null, "Equity control account"),
                ChartOfAccount.Create("4000", "Revenue", "Revenue", "Sales", null, "4", 0m, true, "Credit", true, null, "Revenue control account"),
                ChartOfAccount.Create("5000", "Expenses", "Expense", "Administrative", null, "5", 0m, true, "Debit", true, null, "Expenses control account"),

                // Common asset accounts
                ChartOfAccount.Create("1010", "Cash on Hand", "Asset", "General", null, "1.1", 0m, false, "Debit", true, null, "Cash account"),
                ChartOfAccount.Create("1020", "Bank - Checking", "Asset", "General", null, "1.2", 0m, false, "Debit", true, null, "Bank checking account"),
                ChartOfAccount.Create("1100", "Accounts Receivable", "Asset", "Customer Service", null, "1.3", 0m, false, "Debit", true, null, "Accounts receivable"),

                // Common liability accounts
                ChartOfAccount.Create("2010", "Accounts Payable", "Liability", "General", null, "2.1", 0m, false, "Credit", true, null, "Accounts payable"),
                ChartOfAccount.Create("2020", "Accrued Expenses", "Liability", "General", null, "2.2", 0m, false, "Credit", true, null, "Accrued expenses"),

                // Equity
                ChartOfAccount.Create("3010", "Owner's Equity", "Equity", "General", null, "3.1", 0m, false, "Credit", true, null, "Owner's equity"),

                // Revenue
                ChartOfAccount.Create("4010", "Service Revenue", "Revenue", "Sales", null, "4.1", 0m, false, "Credit", true, null, "Service revenue"),

                // Expenses
                ChartOfAccount.Create("5010", "Salaries Expense", "Expense", "Administrative", null, "5.1", 0m, false, "Debit", true, null, "Salaries and wages"),
                ChartOfAccount.Create("5020", "Rent Expense", "Expense", "Administrative", null, "5.2", 0m, false, "Debit", true, null, "Office rent"),

                // Additional expense / operational accounts for richer testing
                ChartOfAccount.Create("5030", "Utilities Expense", "Expense", "Operations", null, "5.3", 0m, false, "Debit", true, null, "Utilities"),
                ChartOfAccount.Create("5040", "Maintenance Expense", "Expense", "Operations", null, "5.4", 0m, false, "Debit", true, null, "Maintenance"),
                ChartOfAccount.Create("5050", "Insurance Expense", "Expense", "Administrative", null, "5.5", 0m, false, "Debit", true, null, "Insurance"),

                // Cost of goods sold / inventory
                ChartOfAccount.Create("5100", "Cost of Goods Sold", "Expense", "COGS", null, "5.10", 0m, false, "Debit", true, null, "COGS"),
                ChartOfAccount.Create("1200", "Inventory Asset", "Asset", "Inventory", null, "1.4", 0m, false, "Debit", true, null, "Inventory asset"),
            };

            await context.ChartOfAccounts.AddRangeAsync(accounts, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Chart of Accounts with {Count} records", context.TenantInfo!.Identifier, accounts.Count);
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

            // Seed multiple budgets for richer test data
            if (!await context.Budgets.AnyAsync(cancellationToken).ConfigureAwait(false))
            {
                var budgetsToSeed = new List<Budget>
                {
                    Budget.Create("Default Operating Budget", period.Id, "Operating Budget", fiscalYear, "Operating", "Auto-created default operating budget"),
                    Budget.Create("Capital Budget", period.Id, "Capital Budget", fiscalYear, "Capital", "Auto-created capital budget"),
                    Budget.Create("Cash Flow Budget", period.Id, "Cash Flow Budget", fiscalYear, "Cash Flow", "Auto-created cash flow budget"),
                };

                await context.Budgets.AddRangeAsync(budgetsToSeed, cancellationToken).ConfigureAwait(false);

                // Create details across budgets using available accounts
                var acctDict = await context.ChartOfAccounts.ToDictionaryAsync(a => a.AccountCode, a => a.Id, cancellationToken).ConfigureAwait(false);

                var details = new List<BudgetDetail>();

                // Operating budget details
                var op = budgetsToSeed[0];
                if (acctDict.TryGetValue("1010", out var id1010)) details.Add(BudgetDetail.Create(op.Id, id1010, 150000m, "Cash reserve"));
                if (acctDict.TryGetValue("5010", out var id5010)) details.Add(BudgetDetail.Create(op.Id, id5010, 80000m, "Salaries"));
                if (acctDict.TryGetValue("5030", out var id5030)) details.Add(BudgetDetail.Create(op.Id, id5030, 20000m, "Utilities"));

                // Capital budget details
                var cap = budgetsToSeed[1];
                if (acctDict.TryGetValue("1200", out var id1200)) details.Add(BudgetDetail.Create(cap.Id, id1200, 250000m, "New equipment"));
                if (acctDict.TryGetValue("5020", out var id5020)) details.Add(BudgetDetail.Create(cap.Id, id5020, 50000m, "Project rent"));

                // Cash flow budget details
                var cf = budgetsToSeed[2];
                if (acctDict.TryGetValue("1020", out var id1020)) details.Add(BudgetDetail.Create(cf.Id, id1020, 100000m, "Bank cash planned"));
                if (acctDict.TryGetValue("4010", out var id4010)) details.Add(BudgetDetail.Create(cf.Id, id4010, 200000m, "Projected revenue"));

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

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} Budgets with details", context.TenantInfo!.Identifier, budgetsToSeed.Count);
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

        // Seed Customers (increase count)
        if (!await context.Customers.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var customers = new List<Customer>();
            for (int i = 1; i <= 10; i++)
            {
                var num = $"CUST-{1000 + i}";
                customers.Add(Customer.Create(num, $"Customer {i}", $"{i} Example St", null, $"Contact {i}", $"cust{i}@example.com", "Net 30", "4010", "Service Revenue", null, $"+1555000{i:D4}", 10000m + i * 1000m, $"Seeded customer {i}"));
            }

            await context.Customers.AddRangeAsync(customers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Customers", context.TenantInfo!.Identifier, customers.Count);
        }

        // Seed Vendors (increase count)
        if (!await context.Vendors.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var vendors = new List<Vendor>();
            for (int i = 1; i <= 5; i++)
            {
                var num = $"VEND-{1000 + i}";
                vendors.Add(Vendor.Create(num, $"Vendor {i}", $"{i} Supplier Rd", null, $"Vendor Contact {i}", $"vendor{i}@example.com", "Net 30", "5010", "Salaries Expense", null, $"+15557{654321 + i}", $"Seeded vendor {i}"));
            }

            await context.Vendors.AddRangeAsync(vendors, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Vendors", context.TenantInfo!.Identifier, vendors.Count);
        }

        // Seed Payees (increase count)
        if (!await context.Payees.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var payees = new List<Payee>();
            for (int i = 1; i <= 5; i++)
            {
                payees.Add(Payee.Create($"PAY-{1000 + i}", $"Payee {i}", $"{i} Payee Ave", "5010", "Salaries Expense", null, $"Seeded payee {i}", null));
            }

            await context.Payees.AddRangeAsync(payees, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} Payees", context.TenantInfo!.Identifier, payees.Count);
        }

        // Seed Projects (increase count)
        if (!await context.Projects.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var projects = new List<Project>();
            for (int i = 1; i <= 5; i++)
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
            // Local collections not needed; entities are added directly to the DbContext
 
             for (int i = 1; i <= 10; i++)
             {
                 var memberNumber = $"MEM-{1000 + i}";
                 var member = Member.Create(memberNumber, $"Member {i}", $"{i} Electric Ave", DateTime.UtcNow.Date.AddYears(-1), $"PO Box {i}", $"+1555000{i:D4}", "Active", null, $"member{i}@example.com", $"+1555000{i:D4}", $"Contact {i}", "Residential", null, $"Seeded member {i}");
                 members.Add(member);
                 await context.Members.AddAsync(member, cancellationToken).ConfigureAwait(false);
                 await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
 
                 // create meter
                 var meter = Meter.Create($"MTR-{1000 + i}", "Smart Meter", "AcmeMeters", $"SM-{1000 + i}", DateTime.UtcNow.Date, 1m, $"SN-{1000 + i}", "Location", null, member.Id, true, "AMR", null, null, $"Seeded meter {i}");
                 await context.Meters.AddAsync(meter, cancellationToken).ConfigureAwait(false);
                 await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
 
                 // consumption
                 var consumption = Consumption.Create(meter.Id, DateTime.UtcNow.Date, 1000m + i * 100m, 900m + i * 90m, DateTime.UtcNow.ToString("yyyy-MM"), "Actual", 1m, "AMR", $"Seeded consumption {i}");
                 await context.Consumption.AddAsync(consumption, cancellationToken).ConfigureAwait(false);
                 await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
 
                 // invoice
                 var invoice = Invoice.Create($"INV-{1000 + i}", member.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(30), consumption.Id, 100m + i, 10m, 5m, 0m, 100m + i, DateTime.UtcNow.ToString("MMMM yyyy"), null, null, null, null, null, $"Seeded invoice {i}");
                 await context.Invoices.AddAsync(invoice, cancellationToken).ConfigureAwait(false);
                 await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
 
                 // payment
                 var payment = Payment.Create($"PAY-{2000 + i}", member.Id, DateTime.UtcNow.Date, invoice.GetOutstandingAmount(), "Cash", null, "1020", $"Seeded payment {i}");
                 payment = payment.AllocateToInvoice(invoice.Id, invoice.GetOutstandingAmount());
                 await context.Payments.AddAsync(payment, cancellationToken).ConfigureAwait(false);
                 await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
             }

             logger.LogInformation("[{Tenant}] seeded {Count} Members with meters, consumptions, invoices and payments", context.TenantInfo!.Identifier, members.Count);
        }

        // Seed Fixed Assets (multiple)
        if (!await context.FixedAssets.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var firstDepr = await context.DepreciationMethods.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1010", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "5010", cancellationToken).ConfigureAwait(false);
            if (firstDepr != null && cashAccount != null && expenseAccount != null)
            {
                var assets = new List<FixedAsset>
                {
                    FixedAsset.Create("Office Laptop A", DateTime.UtcNow.Date, 2000m, firstDepr.Id, 5, 200m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-LAP-001", "HQ", "IT", null, null, null, null, null, null, "AcmeCorp", "Model-LAP-1", true, "Office laptop"),
                    FixedAsset.Create("Office Laptop B", DateTime.UtcNow.Date, 2200m, firstDepr.Id, 5, 220m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-LAP-002", "HQ", "IT", null, null, null, null, null, null, "AcmeCorp", "Model-LAP-2", true, "Office laptop B"),
                    FixedAsset.Create("Forklift", DateTime.UtcNow.Date, 15000m, firstDepr.Id, 7, 1500m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-FLK-001", "Warehouse", "Operations", null, null, null, null, null, null, "LiftCo", "FL-1", true, "Seeded forklift")
                };

                await context.FixedAssets.AddRangeAsync(assets, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} FixedAssets", context.TenantInfo!.Identifier, assets.Count);
            }
        }

        // Seed a simple JournalEntry + GeneralLedger and PostingBatch (unchanged)
        if (!await context.JournalEntries.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var period = await context.AccountingPeriods.OrderByDescending(p => p.FiscalYear).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1010", cancellationToken).ConfigureAwait(false);
            var revenueAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "4010", cancellationToken).ConfigureAwait(false);
            if (period != null && cashAccount != null && revenueAccount != null)
            {
                var je = JournalEntry.Create(DateTime.UtcNow, "JE-1000", "Seed sales journal entry", "Seeding", period.Id);
                je = je.AddLine(cashAccount.Id, 110m, 0m, "Cash sale");
                je = je.AddLine(revenueAccount.Id, 0m, 110m, "Sales revenue");
                je = je.Post();
                await context.JournalEntries.AddAsync(je, cancellationToken).ConfigureAwait(false);

                // Create corresponding GeneralLedger entries
                var gl1 = GeneralLedger.Create(je.Id, cashAccount.Id, 110m, 0m, "General", DateTime.UtcNow, "Seeding", "JE-1000", period.Id, "Seeded GL debit");
                var gl2 = GeneralLedger.Create(je.Id, revenueAccount.Id, 0m, 110m, "General", DateTime.UtcNow, "Seeding", "JE-1000", period.Id, "Seeded GL credit");
                await context.GeneralLedgers.AddRangeAsync(new[] { gl1, gl2 }, cancellationToken).ConfigureAwait(false);

                // Add a draft PostingBatch containing the journal entry (batch remains draft)
                var batch = PostingBatch.Create("BATCH-1000", DateTime.UtcNow, "Seed batch", period.Id);
                batch.AddJournalEntry(je);
                await context.PostingBatches.AddAsync(batch, cancellationToken).ConfigureAwait(false);

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded JournalEntry {Ref} and PostingBatch {Batch}", context.TenantInfo!.Identifier, je.ReferenceNumber, batch.BatchNumber);
            }
        }

        // Seed simple accrual and deferred revenue (unchanged)
        if (!await context.Accruals.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var accrual = Accrual.Create("ACCR-1000", DateTime.UtcNow.Date, 2500m, "Seeded accrual for utilities");
            await context.Accruals.AddAsync(accrual, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Accrual {Number}", context.TenantInfo!.Identifier, accrual.AccrualNumber);
        }

        if (!await context.DeferredRevenues.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var defRev = DeferredRevenue.Create("DEF-1000", DateTime.UtcNow.Date.AddMonths(1), 1200m, "Seeded deferred revenue for subscription");
            await context.DeferredRevenues.AddAsync(defRev, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded DeferredRevenue {Number}", context.TenantInfo!.Identifier, defRev.DeferredRevenueNumber);
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

            if (pcList.Any())
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

            if (sdList.Any())
            {
                await context.SecurityDeposits.AddRangeAsync(sdList, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded {Count} SecurityDeposits", context.TenantInfo!.Identifier, sdList.Count);
            }
        }

        // ...existing code beyond seeding sections...
    }
}
