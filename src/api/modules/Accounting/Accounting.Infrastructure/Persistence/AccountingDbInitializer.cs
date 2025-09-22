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
        // Seed Chart of Accounts
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
            };

            await context.ChartOfAccounts.AddRangeAsync(accounts, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Chart of Accounts with {Count} records", context.TenantInfo!.Identifier, accounts.Count);
        }

        // Seed Accounting Periods (create a current fiscal year Annual period if none)
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

            // Seed a default Budget referencing the created period
            if (!await context.Budgets.AnyAsync(cancellationToken).ConfigureAwait(false))
            {
                var budget = Budget.Create("Default Operating Budget", period.Id, "Operating Budget", fiscalYear, "Operating", "Auto-created default budget");

                // Add the budget to the context first so the entity is tracked
                await context.Budgets.AddAsync(budget, cancellationToken).ConfigureAwait(false);

                // Add a few budget details using the existing chart of accounts (if present)
                var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1010", cancellationToken).ConfigureAwait(false);
                var salariesAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "5010", cancellationToken).ConfigureAwait(false);

                var seededDetails = new List<BudgetDetail>();
                if (cashAccount is not null)
                    seededDetails.Add(BudgetDetail.Create(budget.Id, cashAccount.Id, 100000m, "Cash reserve"));
                if (salariesAccount is not null)
                    seededDetails.Add(BudgetDetail.Create(budget.Id, salariesAccount.Id, 50000m, "Salaries budget"));

                if (seededDetails.Count > 0)
                {
                    await context.BudgetDetails.AddRangeAsync(seededDetails, cancellationToken).ConfigureAwait(false);

                    // Update budget totals to reflect seeded details
                    var totalBudgeted = seededDetails.Sum(d => d.BudgetedAmount);
                    var totalActual = seededDetails.Sum(d => d.ActualAmount);
                    budget.SetTotals(totalBudgeted, totalActual);
                }

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded default Budget {BudgetName}", context.TenantInfo!.Identifier, budget.Name);
            }
        }

        // Seed Depreciation Methods
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

        // Seed Customers, Vendors, Payees and Projects with minimal useful defaults
        if (!await context.Customers.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var customers = new List<Customer>
            {
                Customer.Create("CUST-1000", "Default Customer", "123 Main St", null, "John Doe", "customer@example.com", "Net 30", "4010", "Service Revenue", null, "+15551234567", 50000m, "Seeded default customer")
            };
            await context.Customers.AddRangeAsync(customers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Customers", context.TenantInfo!.Identifier);
        }

        if (!await context.Vendors.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var vendors = new List<Vendor>
            {
                Vendor.Create("VEND-1000", "Default Vendor", "456 Supplier Rd", null, "Jane Smith", "vendor@example.com", "Net 30", "5010", "Salaries Expense", null, "+15557654321", "Seeded default vendor")
            };
            await context.Vendors.AddRangeAsync(vendors, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Vendors", context.TenantInfo!.Identifier);
        }

        if (!await context.Payees.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var payees = new List<Payee>
            {
                Payee.Create("PAY-1000", "Default Payee", "789 Payee Ave", "5010", "Salaries Expense", null, "Seeded default payee", null)
            };
            await context.Payees.AddRangeAsync(payees, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Payees", context.TenantInfo!.Identifier);
        }

        if (!await context.Projects.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var projects = new List<Project>
            {
                Project.Create("Default Project", DateTime.UtcNow.Date, 100000m, "ACME Corp", "PM: Seed", "Operations", "Seeded project")
            };
            await context.Projects.AddRangeAsync(projects, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Projects", context.TenantInfo!.Identifier);
        }

        // Seed Members, Meters, Consumption, Invoices and Payments
        if (!await context.Members.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            // Create a default member
            var member = Member.Create("MEM-1000", "Default Member", "1 Electric Ave", DateTime.UtcNow.Date, "PO Box 1", "+15550001111", "Active", null, "member@example.com", "+15550001111", "Jane Doe", "Residential", null, "Default member for seeding");
            await context.Members.AddAsync(member, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Member {MemberNumber}", context.TenantInfo!.Identifier, member.MemberNumber);

            // Create a meter for that member
            var meter = Meter.Create("MTR-1000", "Smart Meter", "AcmeMeters", "SM-1000", DateTime.UtcNow.Date, 1m, "SN-1000", "Head Office", null, member.Id, true, "AMR", null, null, "Seeded meter");
            await context.Meters.AddAsync(meter, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Meter {MeterNumber} for Member {Member}", context.TenantInfo!.Identifier, meter.MeterNumber, member.MemberNumber);

            // Create a consumption record for the meter
            var consumption = Consumption.Create(meter.Id, DateTime.UtcNow.Date, 1500m, 1400m, DateTime.UtcNow.ToString("yyyy-MM"), "Actual", 1m, "AMR", "Seeded consumption");
            await context.Consumption.AddAsync(consumption, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Consumption for Meter {MeterNumber}", context.TenantInfo!.Identifier, meter.MeterNumber);

            // Create an invoice for the member
            var invoice = Invoice.Create("INV-1000", member.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(30), consumption.Id, 100m, 10m, 5m, 0m, 100m, DateTime.UtcNow.ToString("MMMM yyyy"), null, null, null, null, null, "Seeded invoice");
            await context.Invoices.AddAsync(invoice, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Invoice {InvoiceNumber} for Member {Member}", context.TenantInfo!.Identifier, invoice.InvoiceNumber, member.MemberNumber);

            // Create a payment applied to that invoice
            var payment = Payment.Create("PAY-1000", member.Id, DateTime.UtcNow.Date, invoice.GetOutstandingAmount(), "Cash", null, "1020", "Seeded payment");
            // Apply payment to invoice
            payment = payment.AllocateToInvoice(invoice.Id, invoice.GetOutstandingAmount());
            await context.AddAsync(payment, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded Payment {PaymentNumber} for Member {Member}", context.TenantInfo!.Identifier, payment.PaymentNumber, member.MemberNumber);
        }

        // Seed Fixed Assets
        if (!await context.FixedAssets.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var firstDepr = await context.DepreciationMethods.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            var cashAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "1010", cancellationToken).ConfigureAwait(false);
            var expenseAccount = await context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountCode == "5010", cancellationToken).ConfigureAwait(false);
            if (firstDepr != null && cashAccount != null && expenseAccount != null)
            {
                var asset = FixedAsset.Create("Office Laptop", DateTime.UtcNow.Date, 2000m, firstDepr.Id, 5, 200m, cashAccount.Id, expenseAccount.Id, "Equipment", "SN-LAP-001", "HQ", "IT", null, null, null, null, null, null, "AcmeCorp", "Model-LAP-1", true, "Office laptop");
                await context.FixedAssets.AddAsync(asset, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded FixedAsset {AssetName}", context.TenantInfo!.Identifier, asset.AssetName);
            }
        }

        // Seed a simple JournalEntry + GeneralLedger and PostingBatch (draft)
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

        // Seed simple accrual and deferred revenue
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

        // Seed a Regulatory Report
        if (!await context.RegulatoryReports.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var start = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            var report = RegulatoryReport.Create("FERC-Form-1-Seed", "FERC Form 1", "Annual", start, end, end.AddMonths(3), "FERC", false, "Seeded regulatory report");
            await context.RegulatoryReports.AddAsync(report, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded RegulatoryReport {Report}", context.TenantInfo!.Identifier, report.ReportName);
        }

        // --- Additional domain seeds: InventoryItems, RateSchedules, Payments/Allocations, PatronageCapital, SecurityDeposits ---

        // Inventory Items
        if (!await context.InventoryItems.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var inventory = new List<InventoryItem>
            {
                InventoryItem.Create("SKU-TRANS-001", "Transformer Coil A", 5m, 12500m, "Main transformer coil (spare)"),
                InventoryItem.Create("SKU-METER-001", "Meter Type X", 50m, 120m, "Smart meters for residential installs"),
                InventoryItem.Create("SKU-CABLE-001", "Underground Cable 1km", 20m, 800m, "High grade underground cable"),
                InventoryItem.Create("SKU-FUSE-001", "High Voltage Fuse", 200m, 15m, "Replacement fuses"),
                InventoryItem.Create("SKU-SWITCH-001", "Disconnect Switch", 10m, 450m, "Pole-mounted disconnect switch")
            };

            await context.InventoryItems.AddRangeAsync(inventory, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded {Count} InventoryItems", context.TenantInfo!.Identifier, inventory.Count);
        }

        // Rate Schedules
        if (!await context.RateSchedules.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var rs1 = RateSchedule.Create("RS-RES", "Residential Standard", DateTime.UtcNow.Date, 0.12m, 5m, false, null, null, "Default residential rate schedule");
            rs1 = rs1.AddTier(1, 100m, 0.10m)
                     .AddTier(2, 300m, 0.12m)
                     .AddTier(3, 0m, 0.15m); // 0 => unlimited

            var rs2 = RateSchedule.Create("RS-COMM", "Commercial Standard", DateTime.UtcNow.Date, 0.10m, 25m, false, 2.5m, null, "Default commercial rate schedule");
            rs2 = rs2.AddTier(1, 500m, 0.09m)
                     .AddTier(2, 0m, 0.11m);

            await context.RateSchedules.AddAsync(rs1, cancellationToken).ConfigureAwait(false);
            await context.RateSchedules.AddAsync(rs2, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded RateSchedules: {Codes}", context.TenantInfo!.Identifier, string.Join(",", rs1.RateCode, rs2.RateCode));
        }

        // Additional Payments and Allocations
        if (!await context.Payments.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var member = await context.Members.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            var invoice = await context.Invoices.OrderByDescending(i => i.InvoiceDate).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if (member != null)
            {
                var p1 = Payment.Create("PAY-1001", member.Id, DateTime.UtcNow.Date, 75m, "Check", "CHK-1001", "1020", "Seeded payment 2");
                var p2 = Payment.Create("PAY-1002", member.Id, DateTime.UtcNow.Date.AddDays(-10), 120m, "EFT", "EFT-1002", "1020", "Seeded payment 3");

                if (invoice != null)
                {
                    p1 = p1.AllocateToInvoice(invoice.Id, Math.Min(75m, invoice.GetOutstandingAmount()));
                    p2 = p2.AllocateToInvoice(invoice.Id, Math.Min(120m, invoice.GetOutstandingAmount() - (invoice.PaidAmount + 0m)));
                }

                await context.Payments.AddRangeAsync(new[] { p1, p2 }, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded additional Payments for Member {Member}", context.TenantInfo!.Identifier, member.MemberNumber);
            }
        }

        // Patronage Capital allocations
        if (!await context.PatronageCapitals.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var member = await context.Members.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if (member != null)
            {
                var famount = 1500m;
                var pc1 = PatronageCapital.Create(member.Id, DateTime.UtcNow.Year - 1, famount, $"Allocated patronage capital FY{DateTime.UtcNow.Year - 1}", "Seeded patronage allocation");
                var pc2 = PatronageCapital.Create(member.Id, DateTime.UtcNow.Year - 2, 900m, $"Allocated patronage capital FY{DateTime.UtcNow.Year - 2}", "Seeded patronage allocation");
                await context.PatronageCapitals.AddRangeAsync(new[] { pc1, pc2 }, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded PatronageCapital records for Member {Member}", context.TenantInfo!.Identifier, member.MemberNumber);
            }
        }

        // Security Deposits
        if (!await context.SecurityDeposits.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var member = await context.Members.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if (member != null)
            {
                var sd1 = SecurityDeposit.Create(member.Id, 200m, DateTime.UtcNow.Date.AddYears(-1), "Initial security deposit");
                var sd2 = SecurityDeposit.Create(member.Id, 150m, DateTime.UtcNow.Date.AddMonths(-6), "Additional deposit");
                await context.SecurityDeposits.AddRangeAsync(new[] { sd1, sd2 }, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] seeded SecurityDeposits for Member {Member}", context.TenantInfo!.Identifier, member.MemberNumber);
            }
        }
    }
}
