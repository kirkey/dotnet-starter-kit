using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database initializer for the MicroFinance module.
/// Handles migrations and seeding of initial data for all domain entities.
/// </summary>
internal sealed class MicroFinanceDbInitializer(
    ILogger<MicroFinanceDbInitializer> logger,
    MicroFinanceDbContext context) : IDbInitializer
{
    /// <inheritdoc/>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for microfinance module", context.TenantInfo!.Identifier);
        }
    }

    /// <inheritdoc/>
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var tenant = context.TenantInfo?.Identifier ?? "default";

        // 1) Seed Loan Products (at least 10)
        await SeedLoanProductsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 2) Seed Savings Products (at least 10)
        await SeedSavingsProductsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 3) Seed Share Products (at least 10)
        await SeedShareProductsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 4) Seed Fee Definitions (at least 10)
        await SeedFeeDefinitionsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 5) Seed Members (at least 10)
        await SeedMembersAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 6) Seed Member Groups (at least 10)
        await SeedMemberGroupsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 7) Seed Group Memberships
        await SeedGroupMembershipsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 8) Seed Savings Accounts
        await SeedSavingsAccountsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 9) Seed Share Accounts
        await SeedShareAccountsAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 10) Seed Loans
        await SeedLoansAsync(tenant, cancellationToken).ConfigureAwait(false);

        // 11) Seed Fixed Deposits
        await SeedFixedDepositsAsync(tenant, cancellationToken).ConfigureAwait(false);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] completed seeding microfinance module with comprehensive sample data", tenant);
    }

    private async Task SeedLoanProductsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var products = new (string Code, string Name, string Desc, decimal MinAmt, decimal MaxAmt, decimal Rate, string Method, int MinTerm, int MaxTerm, string Freq)[]
        {
            ("PERSONAL-LOAN", "Personal Loan", "Standard personal loan for general purposes", 100, 10000, 12, "Declining", 3, 24, "Monthly"),
            ("AGRI-LOAN", "Agricultural Loan", "Seasonal loans for farming activities", 500, 50000, 10, "Flat", 6, 12, "Monthly"),
            ("MICRO-BUSINESS", "Micro Business Loan", "Working capital for small businesses", 200, 25000, 15, "Declining", 3, 36, "Monthly"),
            ("EMERGENCY-LOAN", "Emergency Loan", "Quick disbursement for emergencies", 50, 2000, 18, "Flat", 1, 6, "Weekly"),
            ("EDU-LOAN", "Education Loan", "School fees and education expenses", 100, 15000, 8, "Declining", 6, 48, "Monthly"),
            ("HOUSING-LOAN", "Housing Improvement Loan", "Home repairs and improvements", 1000, 100000, 9, "Declining", 12, 60, "Monthly"),
            ("ASSET-LOAN", "Asset Finance Loan", "Equipment and vehicle purchase", 500, 75000, 11, "Declining", 6, 48, "Monthly"),
            ("GROUP-LOAN", "Group Solidarity Loan", "Joint liability group loans", 50, 5000, 14, "Flat", 3, 12, "Weekly"),
            ("SOLAR-LOAN", "Solar Energy Loan", "Solar panels and renewable energy", 200, 20000, 7, "Declining", 12, 36, "Monthly"),
            ("WATER-SANIT", "Water & Sanitation Loan", "Water tanks and sanitation facilities", 100, 10000, 8, "Flat", 6, 24, "Monthly"),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.LoanProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = LoanProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                currencyCode: "USD",
                minLoanAmount: p.MinAmt,
                maxLoanAmount: p.MaxAmt,
                interestRate: p.Rate,
                interestMethod: p.Method,
                minTermMonths: p.MinTerm,
                maxTermMonths: p.MaxTerm,
                repaymentFrequency: p.Freq);

            await context.LoanProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded loan products", tenant);
    }

    private async Task SeedSavingsProductsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.SavingsProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var products = new (string Code, string Name, string Desc, decimal Rate, string Calc, string Freq, decimal MinOpen, decimal MinBal)[]
        {
            ("REGULAR-SAVINGS", "Regular Savings", "Standard savings account for members", 3.5m, "Daily", "Monthly", 10, 100),
            ("JUNIOR-SAVINGS", "Junior Savings", "Savings account for children under 18", 4.0m, "Daily", "Monthly", 5, 0),
            ("FIXED-SAVINGS", "Fixed Savings", "Higher interest with withdrawal restrictions", 5.5m, "Daily", "Quarterly", 100, 500),
            ("EMERGENCY-FUND", "Emergency Fund", "Quick access emergency savings", 2.5m, "Daily", "Monthly", 20, 0),
            ("GOAL-SAVINGS", "Goal Savings", "Target-based savings for specific goals", 4.5m, "Daily", "Monthly", 25, 0),
            ("BUSINESS-SAVINGS", "Business Savings", "Savings for business members", 3.0m, "Daily", "Monthly", 50, 200),
            ("PREMIUM-SAVINGS", "Premium Savings", "High-value savings with premium rates", 6.0m, "Daily", "Monthly", 1000, 5000),
            ("PENSION-SAVINGS", "Pension Savings", "Retirement savings product", 5.0m, "Daily", "Annually", 100, 0),
            ("FESTIVE-SAVINGS", "Festive Savings", "Seasonal savings for holidays", 4.0m, "Daily", "Monthly", 10, 0),
            ("COOPERATIVE-SAVINGS", "Cooperative Savings", "Mandatory member savings", 3.5m, "Daily", "Monthly", 50, 100),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.SavingsProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = SavingsProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                currencyCode: "USD",
                interestRate: p.Rate,
                interestCalculation: p.Calc,
                interestPostingFrequency: p.Freq,
                minOpeningBalance: p.MinOpen,
                minBalanceForInterest: p.MinBal);

            await context.SavingsProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded savings products", tenant);
    }

    private async Task SeedShareProductsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.ShareProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var products = new (string Code, string Name, string Desc, decimal Nominal, decimal Current, int MinShares, int? MaxShares, bool Transfer, bool Redeem, bool Dividends)[]
        {
            ("ORD-SHARE", "Ordinary Shares", "Standard membership shares", 10, 10, 5, 1000, false, true, true),
            ("PREF-SHARE", "Preference Shares", "Preferred dividend shares", 50, 55, 10, 500, true, true, true),
            ("FOUNDER-SHARE", "Founder Shares", "Special shares for founding members", 100, 120, 1, 100, false, false, true),
            ("STAFF-SHARE", "Staff Shares", "Shares for employees", 25, 25, 5, 200, false, true, true),
            ("YOUTH-SHARE", "Youth Shares", "Affordable shares for young members", 5, 5, 2, 100, false, true, true),
            ("PREMIUM-SHARE", "Premium Shares", "High-value investment shares", 100, 115, 10, 1000, true, true, true),
            ("RESERVE-SHARE", "Reserve Shares", "Institution reserve shares", 500, 500, 1, null, false, false, true),
            ("COMMUNITY-SHARE", "Community Shares", "Community investment shares", 20, 22, 5, 500, true, true, true),
            ("GROWTH-SHARE", "Growth Shares", "Capital appreciation focused", 75, 85, 10, 500, true, true, false),
            ("DIVIDEND-SHARE", "Dividend Shares", "High dividend yield shares", 30, 32, 5, 1000, false, true, true),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.ShareProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = ShareProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                currencyCode: "USD",
                nominalValue: p.Nominal,
                currentPrice: p.Current,
                minSharesForMembership: p.MinShares,
                maxSharesPerMember: p.MaxShares,
                allowTransfer: p.Transfer,
                allowRedemption: p.Redeem,
                paysDividends: p.Dividends);

            await context.ShareProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded share products", tenant);
    }

    private async Task SeedFeeDefinitionsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.FeeDefinitions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var fees = new (string Code, string Name, string Type, string Calc, string Applies, string Freq, decimal Amt, string Desc)[]
        {
            ("LOAN-PROC-FEE", "Loan Processing Fee", FeeDefinition.TypeProcessing, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyOneTime, 2.0m, "One-time fee for processing loan applications"),
            ("LOAN-DISB-FEE", "Loan Disbursement Fee", FeeDefinition.TypeDisbursement, FeeDefinition.CalculationFlat, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyOneTime, 25, "Fixed fee charged at loan disbursement"),
            ("LATE-PAY-FEE", "Late Payment Penalty", FeeDefinition.TypeLateFee, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyPerEvent, 5.0m, "Penalty for late loan repayments"),
            ("ACC-MAINT-FEE", "Account Maintenance Fee", FeeDefinition.TypeAccountMaintenance, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyMonthly, 2, "Monthly account maintenance fee"),
            ("ATM-WITHD-FEE", "ATM Withdrawal Fee", FeeDefinition.TypeWithdrawal, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyPerEvent, 1.5m, "Fee for ATM withdrawals"),
            ("TRANSFER-FEE", "Fund Transfer Fee", FeeDefinition.TypeTransfer, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyPerEvent, 5, "Fee for fund transfers"),
            ("CLOSING-FEE", "Account Closing Fee", FeeDefinition.TypeClosingFee, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyOneTime, 10, "Fee for closing account prematurely"),
            ("INSURANCE-FEE", "Loan Insurance Premium", FeeDefinition.TypeInsurance, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyOneTime, 1.5m, "Insurance premium on loan amount"),
            ("FD-EARLY-FEE", "Early FD Withdrawal Penalty", FeeDefinition.TypeEarlyPayment, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesFixedDeposit, FeeDefinition.FrequencyPerEvent, 3.0m, "Penalty for early fixed deposit withdrawal"),
            ("MEMBER-REG-FEE", "Membership Registration Fee", FeeDefinition.TypeProcessing, FeeDefinition.CalculationFlat, FeeDefinition.AppliesMember, FeeDefinition.FrequencyOneTime, 15, "One-time membership registration fee"),
        };

        for (int i = existingCount; i < fees.Length; i++)
        {
            var f = fees[i];
            if (await context.FeeDefinitions.AnyAsync(x => x.Code == f.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var fee = FeeDefinition.Create(
                code: f.Code,
                name: f.Name,
                feeType: f.Type,
                calculationType: f.Calc,
                appliesTo: f.Applies,
                chargeFrequency: f.Freq,
                amount: f.Amt,
                currencyCode: "USD",
                description: f.Desc);

            await context.FeeDefinitions.AddAsync(fee, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded fee definitions", tenant);
    }

    private async Task SeedMembersAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.Members.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var members = new (string Num, string First, string Last, string? Middle, string Email, string Phone, string Gender, string Occupation, decimal Income, int BirthYear)[]
        {
            ("MBR-001", "John", "Doe", "Michael", "john.doe@email.com", "+1234567890", "Male", "Farmer", 500, 1985),
            ("MBR-002", "Jane", "Smith", null, "jane.smith@email.com", "+1234567891", "Female", "Teacher", 800, 1990),
            ("MBR-003", "Robert", "Johnson", "William", "robert.j@email.com", "+1234567892", "Male", "Trader", 1200, 1982),
            ("MBR-004", "Mary", "Williams", null, "mary.w@email.com", "+1234567893", "Female", "Nurse", 950, 1988),
            ("MBR-005", "James", "Brown", "Edward", "james.b@email.com", "+1234567894", "Male", "Mechanic", 700, 1975),
            ("MBR-006", "Patricia", "Davis", null, "patricia.d@email.com", "+1234567895", "Female", "Shopkeeper", 600, 1992),
            ("MBR-007", "Michael", "Miller", "Joseph", "michael.m@email.com", "+1234567896", "Male", "Driver", 550, 1980),
            ("MBR-008", "Linda", "Wilson", null, "linda.w@email.com", "+1234567897", "Female", "Tailor", 450, 1987),
            ("MBR-009", "David", "Moore", "Thomas", "david.m@email.com", "+1234567898", "Male", "Carpenter", 650, 1978),
            ("MBR-010", "Elizabeth", "Taylor", null, "elizabeth.t@email.com", "+1234567899", "Female", "Baker", 500, 1995),
        };

        for (int i = existingCount; i < members.Length; i++)
        {
            var m = members[i];
            if (await context.Members.AnyAsync(x => x.MemberNumber == m.Num, cancellationToken).ConfigureAwait(false))
                continue;

            var member = Member.Create(
                memberNumber: m.Num,
                firstName: m.First,
                lastName: m.Last,
                middleName: m.Middle,
                email: m.Email,
                phoneNumber: m.Phone,
                dateOfBirth: new DateOnly(m.BirthYear, 1, 15),
                gender: m.Gender,
                address: $"{100 + i} Main Street",
                city: "Metropolis",
                state: "Central State",
                postalCode: $"1000{i}",
                country: "United States",
                nationalId: $"NAT-{m.Num}",
                occupation: m.Occupation,
                monthlyIncome: m.Income,
                joinDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12 + i)));

            await context.Members.AddAsync(member, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded members", tenant);
    }

    private async Task SeedMemberGroupsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.MemberGroups.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var groups = new (string Code, string Name, string Desc, string Location, string Freq, string Day)[]
        {
            ("GRP-001", "Sunrise Women Group", "Women empowerment savings group", "Community Center A", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-002", "Farmers United", "Agricultural cooperative group", "Village Hall", MemberGroup.FrequencyBiweekly, "Wednesday"),
            ("GRP-003", "Youth Entrepreneurs", "Young business owners group", "Youth Center", MemberGroup.FrequencyWeekly, "Friday"),
            ("GRP-004", "Market Traders Association", "Traders savings collective", "Market Square", MemberGroup.FrequencyWeekly, "Saturday"),
            ("GRP-005", "Teachers Cooperative", "Education sector savings group", "School Hall", MemberGroup.FrequencyMonthly, "Tuesday"),
            ("GRP-006", "Healthcare Workers Group", "Medical staff savings", "Hospital Meeting Room", MemberGroup.FrequencyBiweekly, "Thursday"),
            ("GRP-007", "Artisan Collective", "Craftsmen and artisans group", "Craft Center", MemberGroup.FrequencyWeekly, "Wednesday"),
            ("GRP-008", "Transport Workers Union", "Drivers and transport staff", "Transport Hub", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-009", "Village Development Group", "Community development savings", "Village Chief Office", MemberGroup.FrequencyMonthly, "Saturday"),
            ("GRP-010", "Women in Business", "Female entrepreneurs collective", "Business Center", MemberGroup.FrequencyWeekly, "Tuesday"),
        };

        for (int i = existingCount; i < groups.Length; i++)
        {
            var g = groups[i];
            if (await context.MemberGroups.AnyAsync(x => x.Code == g.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var group = MemberGroup.Create(
                code: g.Code,
                name: g.Name,
                description: g.Desc,
                formationDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6 + i)),
                meetingLocation: g.Location,
                meetingFrequency: g.Freq,
                meetingDay: g.Day,
                meetingTime: new TimeOnly(9, 0));

            group.Activate();
            await context.MemberGroups.AddAsync(group, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded member groups", tenant);
    }

    private async Task SeedGroupMembershipsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.GroupMemberships.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var groups = await context.MemberGroups.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 2 || groups.Count < 1) return;

        // Assign first 2 members to first group
        for (int i = 0; i < Math.Min(2, members.Count) && groups.Count > 0; i++)
        {
            var exists = await context.GroupMemberships
                .AnyAsync(gm => gm.MemberId == members[i].Id && gm.GroupId == groups[0].Id, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                var membership = GroupMembership.Create(
                    memberId: members[i].Id,
                    groupId: groups[0].Id,
                    role: i == 0 ? GroupMembership.RoleLeader : GroupMembership.RoleMember);

                await context.GroupMemberships.AddAsync(membership, cancellationToken).ConfigureAwait(false);
            }
        }

        // Assign members 3-4 to second group
        for (int i = 2; i < Math.Min(4, members.Count) && groups.Count > 1; i++)
        {
            var exists = await context.GroupMemberships
                .AnyAsync(gm => gm.MemberId == members[i].Id && gm.GroupId == groups[1].Id, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                var membership = GroupMembership.Create(
                    memberId: members[i].Id,
                    groupId: groups[1].Id,
                    role: i == 2 ? GroupMembership.RoleLeader : GroupMembership.RoleMember);

                await context.GroupMemberships.AddAsync(membership, cancellationToken).ConfigureAwait(false);
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded group memberships", tenant);
    }

    private async Task SeedSavingsAccountsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.SavingsAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.SavingsProducts.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        int accountNumber = 1001;
        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var accNum = $"SAV-{accountNumber + i:D6}";
            var exists = await context.SavingsAccounts.AnyAsync(sa => sa.AccountNumber == accNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var account = SavingsAccount.Create(
                accountNumber: accNum,
                memberId: members[i].Id,
                savingsProductId: products[i % products.Count].Id,
                openingBalance: (i + 1) * 100,
                openedDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)));

            await context.SavingsAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded savings accounts", tenant);
    }

    private async Task SeedShareAccountsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.ShareAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.ShareProducts.Take(2).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        int accountNumber = 2001;
        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var accNum = $"SHR-{accountNumber + i:D6}";
            var exists = await context.ShareAccounts.AnyAsync(sa => sa.AccountNumber == accNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var account = ShareAccount.Create(
                accountNumber: accNum,
                memberId: members[i].Id,
                shareProductId: products[i % products.Count].Id,
                openedDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)));

            // Purchase some initial shares
            account.PurchaseShares((i + 1) * 5, products[i % products.Count].CurrentPrice);

            await context.ShareAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded share accounts", tenant);
    }

    private async Task SeedLoansAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.Loans.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.LoanProducts.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        int loanNumber = 3001;
        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var loanNum = $"LN-{loanNumber + i:D6}";
            var exists = await context.Loans.AnyAsync(l => l.LoanNumber == loanNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var product = products[i % products.Count];
            var loan = Loan.Create(
                memberId: members[i].Id,
                loanProductId: product.Id,
                loanNumber: loanNum,
                principalAmount: product.MinLoanAmount + ((product.MaxLoanAmount - product.MinLoanAmount) / 10 * (i + 1)),
                interestRate: product.InterestRate,
                termMonths: product.MinTermMonths + ((product.MaxTermMonths - product.MinTermMonths) / 5 * (i % 5)),
                repaymentFrequency: product.RepaymentFrequency,
                currency: "USD",
                purpose: $"Loan purpose for member {i + 1}");

            // Approve and disburse some loans
            if (i < 5)
            {
                loan.Approve(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)));
                if (i < 3)
                {
                    loan.Disburse(
                        DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-25)),
                        DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(loan.TermMonths)));
                }
            }

            await context.Loans.AddAsync(loan, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded loans", tenant);
    }

    private async Task SeedFixedDepositsAsync(string tenant, CancellationToken cancellationToken)
    {
        var existingCount = await context.FixedDeposits.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= 10) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1) return;

        int certNumber = 4001;
        var terms = new[] { 6, 12, 18, 24, 36 };
        var rates = new[] { 7.0m, 7.5m, 8.0m, 8.5m, 9.0m };

        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var certNum = $"FD-{certNumber + i:D6}";
            var exists = await context.FixedDeposits.AnyAsync(fd => fd.CertificateNumber == certNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var deposit = FixedDeposit.Create(
                certificateNumber: certNum,
                memberId: members[i].Id,
                principalAmount: (i + 1) * 500,
                interestRate: rates[i % rates.Length],
                termMonths: terms[i % terms.Length],
                depositDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)),
                maturityInstruction: i % 2 == 0 ? FixedDeposit.MaturityTransferToSavings : FixedDeposit.MaturityRenewPrincipalAndInterest);

            await context.FixedDeposits.AddAsync(deposit, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded fixed deposits", tenant);
    }
}

