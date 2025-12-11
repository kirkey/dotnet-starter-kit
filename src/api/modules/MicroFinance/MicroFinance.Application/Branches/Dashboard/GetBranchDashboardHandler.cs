using FSH.Framework.Core.Exceptions;
using StaffEntity = FSH.Starter.WebApi.MicroFinance.Domain.Staff;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific branch.
/// </summary>
public sealed record GetBranchDashboardQuery(DefaultIdType BranchId) : IRequest<BranchDashboardResponse>;

public sealed class GetBranchDashboardHandler(
    IReadRepository<Branch> branchRepository,
    IReadRepository<Member> memberRepository,
    IReadRepository<Loan> loanRepository,
    IReadRepository<LoanRepayment> loanRepaymentRepository,
    IReadRepository<SavingsAccount> savingsAccountRepository,
    IReadRepository<SavingsTransaction> savingsTransactionRepository,
    IReadRepository<ShareAccount> shareAccountRepository,
    IReadRepository<Staff> staffRepository,
    IReadRepository<BranchTarget> branchTargetRepository,
    IReadRepository<CashVault> cashVaultRepository,
    ICacheService cacheService,
    ILogger<GetBranchDashboardHandler> logger)
    : IRequestHandler<GetBranchDashboardQuery, BranchDashboardResponse>
{
    public async Task<BranchDashboardResponse> Handle(GetBranchDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"branch-dashboard:{request.BranchId}";

        var cachedResult = await cacheService.GetAsync<BranchDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for branch {BranchId}", request.BranchId);

        var branch = await branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException($"Branch {request.BranchId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var lastYearStart = startOfYear.AddYears(-1);
        var lastYearEnd = startOfYear;

        // Get all members for this branch
        var allMembers = await memberRepository
            .ListAsync(new MembersByBranchSpec(request.BranchId), cancellationToken);

        var membersYTD = allMembers.Where(m => m.CreatedOn >= startOfYear).ToList();
        var membersLastYear = allMembers.Where(m => m.CreatedOn >= lastYearStart && m.CreatedOn < lastYearEnd).ToList();

        // Get loans for this branch
        var memberIds = allMembers.Select(m => m.Id).ToList();
        var allLoans = await loanRepository
            .ListAsync(new LoansByMemberIdsSpec(memberIds), cancellationToken);

        // Get savings accounts
        var allSavingsAccounts = await savingsAccountRepository
            .ListAsync(new SavingsAccountsByMemberIdsSpec(memberIds), cancellationToken);

        // Get share accounts
        var allShareAccounts = await shareAccountRepository
            .ListAsync(new ShareAccountsByMemberIdsSpec(memberIds), cancellationToken);

        // Get staff
        var allStaff = await staffRepository
            .ListAsync(new StaffByBranchSpec(request.BranchId), cancellationToken);

        // Get branch targets
        var branchTargets = await branchTargetRepository
            .ListAsync(new BranchTargetsByBranchSpec(request.BranchId), cancellationToken);

        // Get cash vault
        var cashVaults = await cashVaultRepository
            .ListAsync(new CashVaultsByBranchSpec(request.BranchId), cancellationToken);

        // Calculate metrics
        var financials = CalculateFinancialMetrics(
            allLoans, allSavingsAccounts, allShareAccounts, cashVaults, branch, startOfYear, lastYearStart, lastYearEnd);

        var memberMetrics = CalculateMemberMetrics(
            allMembers, membersYTD, membersLastYear, allLoans, allSavingsAccounts);

        var loanPortfolio = await CalculateLoanPortfolioMetrics(
            allLoans, startOfYear, cancellationToken);

        var savingsPortfolio = CalculateSavingsPortfolioMetrics(
            allSavingsAccounts, allShareAccounts, startOfYear);

        var staffMetrics = CalculateStaffMetrics(
            allStaff, allLoans, allMembers);

        var targetAchievement = CalculateTargetAchievement(
            branchTargets, allLoans, allSavingsAccounts, allMembers, startOfYear);

        // Generate trend data
        var loanDisbursementTrend = GenerateLoanDisbursementTrend(allLoans, 12);
        var savingsBalanceTrend = GenerateSavingsBalanceTrend(allSavingsAccounts, 12);
        var memberGrowthTrend = GenerateMemberGrowthTrend(allMembers, 12);
        var portfolioAtRiskTrend = GeneratePortfolioAtRiskTrend(allLoans, 12);

        // Product mix
        var loanProductMix = CalculateLoanProductMix(allLoans);
        var savingsProductMix = CalculateSavingsProductMix(allSavingsAccounts);

        // Top performers
        // TODO: Implement TopLoanOfficers when LoanOfficerAssignment tracking is needed
        var topLoanOfficers = new List<TopLoanOfficer>(); // CalculateTopLoanOfficers(allStaff, allLoans, allMembers);
        var topMembers = CalculateTopMembers(allMembers, allLoans, allSavingsAccounts, allShareAccounts);

        // Recent activities
        var recentLoans = allLoans
            .Where(l => l.DisbursementDate.HasValue)
            .OrderByDescending(l => l.DisbursementDate)
            .Take(10)
            .Select(l => new RecentLoan
            {
                LoanId = l.Id,
                LoanNumber = l.LoanNumber,
                MemberName = GetMemberName(l.MemberId, allMembers),
                ProductName = l.LoanProduct?.Name ?? "Unknown",
                PrincipalAmount = l.PrincipalAmount,
                DisbursementDate = l.DisbursementDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
                Status = l.Status ?? "Unknown",
                LoanOfficerName = "N/A" // Requires LoanOfficerAssignment query
            }).ToList();

        var recentTransactions = new List<MicroFinanceRecentTransaction>(); // Would need SavingsTransaction data

        // Monthly comparison
        var monthlyPerformance = GenerateMonthlyComparison(allLoans, allSavingsAccounts, allMembers, 6);

        // Branch ranking (simplified - would need all branches data)
        var ranking = new BranchRanking
        {
            OverallRank = 1,
            TotalBranches = 1,
            LoanDisbursementRank = 1,
            SavingsRank = 1,
            MemberGrowthRank = 1,
            PortfolioQualityRank = 1,
            PerformanceTier = "Excellent"
        };

        var response = new BranchDashboardResponse
        {
            BranchId = branch.Id,
            BranchName = branch.Name,
            Code = branch.Code,
            BranchType = branch.BranchType,
            Address = branch.Address,
            ManagerName = branch.ManagerName,
            Phone = branch.Phone,
            OpeningDate = branch.OpeningDate,
            Status = branch.Status,
            Financials = financials,
            Members = memberMetrics,
            LoanPortfolio = loanPortfolio,
            SavingsPortfolio = savingsPortfolio,
            Staff = staffMetrics,
            Targets = targetAchievement,
            LoanDisbursementTrend = loanDisbursementTrend,
            SavingsBalanceTrend = savingsBalanceTrend,
            MemberGrowthTrend = memberGrowthTrend,
            PortfolioAtRiskTrend = portfolioAtRiskTrend,
            LoanProductMix = loanProductMix,
            SavingsProductMix = savingsProductMix,
            TopLoanOfficers = topLoanOfficers,
            TopMembers = topMembers,
            RecentLoans = recentLoans,
            RecentTransactions = recentTransactions,
            MonthlyPerformance = monthlyPerformance,
            Ranking = ranking
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static BranchFinancialMetrics CalculateFinancialMetrics(
        List<Loan> allLoans,
        List<SavingsAccount> savingsAccounts,
        List<ShareAccount> shareAccounts,
        List<CashVault> cashVaults,
        Branch branch,
        DateTime startOfYear,
        DateTime lastYearStart,
        DateTime lastYearEnd)
    {
        var totalLoanPortfolio = allLoans
            .Where(l => l.Status != "Closed" && l.Status != "Written Off")
            .Sum(l => l.OutstandingPrincipal);

        var totalSavingsBalance = savingsAccounts
            .Where(s => s.Status == "Active")
            .Sum(s => s.Balance);

        var totalShareCapital = shareAccounts
            .Where(s => s.Status == "Active")
            .Sum(s => s.TotalShareValue);

        var loansYTD = allLoans.Where(l => l.DisbursementDate.HasValue && 
            l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) >= startOfYear).ToList();
        
        var loansLastYear = allLoans.Where(l => l.DisbursementDate.HasValue && 
            l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) >= lastYearStart && 
            l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) < lastYearEnd).ToList();

        var loanDisbursementYTD = loansYTD.Sum(l => l.PrincipalAmount);
        var loanDisbursementLastYear = loansLastYear.Sum(l => l.PrincipalAmount);
        var growth = loanDisbursementLastYear > 0 
            ? (loanDisbursementYTD - loanDisbursementLastYear) / loanDisbursementLastYear * 100 
            : 0;

        var cashOnHand = cashVaults.Sum(c => c.CurrentBalance);
        var cashVaultLimit = branch.CashHoldingLimit ?? 0;
        var cashUtilization = cashVaultLimit > 0 ? (cashOnHand / cashVaultLimit * 100) : 0;

        return new BranchFinancialMetrics
        {
            TotalLoanPortfolio = totalLoanPortfolio,
            TotalSavingsBalance = totalSavingsBalance,
            TotalShareCapital = totalShareCapital,
            LoanDisbursementYTD = loanDisbursementYTD,
            LoanDisbursementLastYear = loanDisbursementLastYear,
            LoanDisbursementGrowth = growth,
            SavingsGrowthYTD = 0, // Would calculate from transactions
            InterestIncomeYTD = 0, // Would calculate from repayments
            FeeIncomeYTD = 0, // Would calculate from fee charges
            TotalIncomeYTD = 0,
            CashOnHand = cashOnHand,
            CashVaultLimit = cashVaultLimit,
            CashUtilization = cashUtilization
        };
    }

    private static BranchMemberMetrics CalculateMemberMetrics(
        List<Member> allMembers,
        List<Member> membersYTD,
        List<Member> membersLastYear,
        List<Loan> loans,
        List<SavingsAccount> savingsAccounts)
    {
        var totalMembers = allMembers.Count;
        var activeMembers = allMembers.Count(m => m.IsActive);
        var newMembersYTD = membersYTD.Count;
        var newMembersLastYear = membersLastYear.Count;
        var growth = newMembersLastYear > 0 
            ? (int)((newMembersYTD - newMembersLastYear) / (decimal)newMembersLastYear * 100)
            : 0;

        var memberIdsWithLoans = loans.Select(l => l.MemberId).Distinct().ToHashSet();
        var memberIdsWithSavings = savingsAccounts.Select(s => s.MemberId).Distinct().ToHashSet();

        var avgSavingsBalance = savingsAccounts.Count > 0 
            ? savingsAccounts.Average(s => s.Balance)
            : 0;

        var activeLoanAmount = loans
            .Where(l => l.Status != "Closed" && l.Status != "Written Off")
            .Sum(l => l.OutstandingPrincipal);
        var membersWithActiveLoans = loans
            .Where(l => l.Status != "Closed" && l.Status != "Written Off")
            .Select(l => l.MemberId)
            .Distinct()
            .Count();
        var avgLoanSize = membersWithActiveLoans > 0 ? activeLoanAmount / membersWithActiveLoans : 0;

        return new BranchMemberMetrics
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            NewMembersYTD = newMembersYTD,
            NewMembersLastYear = newMembersLastYear,
            MemberGrowthPercent = growth,
            InactiveMembers = totalMembers - activeMembers,
            MembersWithLoans = memberIdsWithLoans.Count,
            MembersWithSavings = memberIdsWithSavings.Count,
            AverageSavingsPerMember = avgSavingsBalance,
            AverageLoanSizePerMember = avgLoanSize
        };
    }

    private static async Task<BranchLoanPortfolioMetrics> CalculateLoanPortfolioMetrics(
        List<Loan> allLoans,
        DateTime startOfYear,
        CancellationToken cancellationToken)
    {
        var activeLoans = allLoans.Where(l => l.Status != "Closed" && l.Status != "Written Off").ToList();
        var totalOutstandingPrincipal = activeLoans.Sum(l => l.OutstandingPrincipal);
        var totalOutstandingInterest = activeLoans.Sum(l => l.OutstandingInterest);

        var loansYTD = allLoans.Where(l => l.DisbursementDate.HasValue && 
            l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) >= startOfYear).ToList();

        var overdueLoans = activeLoans.Where(l => 
            l.ExpectedEndDate.HasValue && 
            l.ExpectedEndDate.Value < DateOnly.FromDateTime(DateTime.UtcNow) &&
            (l.OutstandingPrincipal) > 0).ToList();

        var overdueAmount = overdueLoans.Sum(l => l.OutstandingPrincipal);

        // Portfolio at Risk calculation (loans with payments overdue by 30+ or 90+ days)
        var par30Amount = activeLoans
            .Where(l => CalculateDaysOverdue(l) >= 30)
            .Sum(l => l.OutstandingPrincipal);
        
        var par90Amount = activeLoans
            .Where(l => CalculateDaysOverdue(l) >= 90)
            .Sum(l => l.OutstandingPrincipal);

        var par30 = totalOutstandingPrincipal > 0 ? (par30Amount / totalOutstandingPrincipal * 100) : 0;
        var par90 = totalOutstandingPrincipal > 0 ? (par90Amount / totalOutstandingPrincipal * 100) : 0;

        var avgLoanSize = activeLoans.Count > 0 ? activeLoans.Average(l => l.PrincipalAmount) : 0;

        // Approval rate (simplified - would need application data)
        var approvalRate = 85.0m;

        return new BranchLoanPortfolioMetrics
        {
            TotalActiveLoans = activeLoans.Count,
            TotalOutstandingPrincipal = totalOutstandingPrincipal,
            TotalOutstandingInterest = totalOutstandingInterest,
            LoansDisbursedYTD = loansYTD.Count,
            LoansDisbursedAmountYTD = loansYTD.Sum(l => l.PrincipalAmount),
            LoansFullyPaid = allLoans.Count(l => l.Status == "Closed"),
            LoansOverdue = overdueLoans.Count,
            OverdueAmount = overdueAmount,
            PortfolioAtRisk30 = par30,
            PortfolioAtRisk90 = par90,
            AverageLoanSize = avgLoanSize,
            LoanWriteOffAmountYTD = allLoans
                .Where(l => l.Status == "Written Off" && l.CreatedOn >= startOfYear)
                .Sum(l => l.OutstandingPrincipal),
            LoanApplicationsPending = 0, // Would need application data
            ApprovalRate = approvalRate,
            CollectionEfficiency = totalOutstandingPrincipal > 0 
                ? ((totalOutstandingPrincipal - overdueAmount) / totalOutstandingPrincipal * 100) 
                : 100
        };
    }

    private static int CalculateDaysOverdue(Loan loan)
    {
        if (!loan.ExpectedEndDate.HasValue || !loan.DisbursementDate.HasValue) return 0;
        
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        // Calculate days overdue based on next expected payment due date
        // Simplified: check if past ExpectedEndDate
        if (loan.ExpectedEndDate.Value >= today) return 0;

        var daysOverdue = (today.DayNumber - loan.ExpectedEndDate.Value.DayNumber);
        return daysOverdue;
    }

    private static BranchSavingsPortfolioMetrics CalculateSavingsPortfolioMetrics(
        List<SavingsAccount> savingsAccounts,
        List<ShareAccount> shareAccounts,
        DateTime startOfYear)
    {
        var activeAccounts = savingsAccounts.Where(s => s.Status == "Active").ToList();
        var totalBalance = activeAccounts.Sum(s => s.Balance);
        var avgBalance = activeAccounts.Count > 0 ? activeAccounts.Average(s => s.Balance) : 0;

        var dormantAccounts = savingsAccounts.Count(s => s.Status == "Dormant");

        var totalShareCapital = shareAccounts
            .Where(s => s.Status == "Active")
            .Sum(s => s.TotalShareValue);

        return new BranchSavingsPortfolioMetrics
        {
            TotalSavingsAccounts = savingsAccounts.Count,
            ActiveSavingsAccounts = activeAccounts.Count,
            TotalSavingsBalance = totalBalance,
            SavingsDepositsYTD = 0, // Would calculate from transactions
            SavingsWithdrawalsYTD = 0, // Would calculate from transactions
            NetSavingsGrowth = 0,
            AverageSavingsBalance = avgBalance,
            DormantAccounts = dormantAccounts,
            TotalShareCapital = totalShareCapital,
            ShareAccountsActive = shareAccounts.Count(s => s.Status == "Active")
        };
    }

    private static BranchStaffMetrics CalculateStaffMetrics(
        List<Staff> staff,
        List<Loan> loans,
        List<Member> members)
    {
        var activeStaff = staff.Where(s => s.Status == "Active").ToList();
        var loanOfficers = activeStaff.Where(s => s.Role == "Loan Officer").ToList();
        var tellers = activeStaff.Where(s => s.Role == "Teller").ToList();
        var manager = activeStaff.FirstOrDefault(s => s.Role == "Branch Manager");

        var activeLoans = loans.Where(l => l.Status != "Closed" && l.Status != "Written Off").ToList();
        var avgLoansPerOfficer = loanOfficers.Count > 0 
            ? (decimal)activeLoans.Count / loanOfficers.Count 
            : 0;

        var avgMembersPerOfficer = loanOfficers.Count > 0 
            ? (decimal)members.Count / loanOfficers.Count 
            : 0;

        return new BranchStaffMetrics
        {
            TotalStaff = staff.Count,
            ActiveStaff = activeStaff.Count,
            LoanOfficers = loanOfficers.Count,
            Tellers = tellers.Count,
            BranchManager = manager?.FirstName + " " + manager?.LastName,
            AverageLoansPerOfficer = avgLoansPerOfficer,
            AverageMembersPerOfficer = avgMembersPerOfficer,
            StaffProductivity = avgLoansPerOfficer * avgMembersPerOfficer
        };
    }

    private static BranchTargetAchievement CalculateTargetAchievement(
        List<BranchTarget> targets,
        List<Loan> loans,
        List<SavingsAccount> savingsAccounts,
        List<Member> members,
        DateTime startOfYear)
    {
        var currentQuarter = (DateTime.UtcNow.Month - 1) / 3;
        var quarterTargets = targets
            .Where(t => t.PeriodStart <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                       t.PeriodEnd >= DateOnly.FromDateTime(DateTime.UtcNow))
            .ToList();

        var loanTarget = quarterTargets
            .Where(t => t.TargetType == "LoanDisbursement")
            .Sum(t => t.TargetValue);
        
        var savingsTarget = quarterTargets
            .Where(t => t.TargetType == "SavingsBalance")
            .Sum(t => t.TargetValue);
        
        var memberTarget = (int)quarterTargets
            .Where(t => t.TargetType == "MemberAcquisition")
            .Sum(t => t.TargetValue);

        var loansYTD = loans.Where(l => l.DisbursementDate.HasValue && 
            l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) >= startOfYear).ToList();
        
        var loanActual = loansYTD.Sum(l => l.PrincipalAmount);
        var savingsActual = savingsAccounts.Where(s => s.Status == "Active").Sum(s => s.Balance);
        var memberActual = members.Count(m => m.CreatedOn >= startOfYear);

        var loanAchievement = loanTarget > 0 ? (loanActual / loanTarget * 100) : 0;
        var savingsAchievement = savingsTarget > 0 ? (savingsActual / savingsTarget * 100) : 0;
        var memberAchievement = memberTarget > 0 ? ((decimal)memberActual / memberTarget * 100) : 0;

        var overallAchievement = (loanAchievement + savingsAchievement + memberAchievement) / 3;

        return new BranchTargetAchievement
        {
            LoanDisbursementTarget = loanTarget,
            LoanDisbursementActual = loanActual,
            LoanDisbursementAchievement = loanAchievement,
            SavingsTarget = savingsTarget,
            SavingsActual = savingsActual,
            SavingsAchievement = savingsAchievement,
            MemberTargetCount = memberTarget,
            MemberActualCount = memberActual,
            MemberAchievement = memberAchievement,
            OverallTargetAchievement = overallAchievement
        };
    }

    private static List<BranchTimeSeriesDataPoint> GenerateLoanDisbursementTrend(List<Loan> loans, int months)
    {
        var result = new List<BranchTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthDate = today.AddMonths(-i);
            var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var monthLoans = loans.Where(l => 
                l.DisbursementDate.HasValue &&
                l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) >= monthStart &&
                l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) < monthEnd).ToList();

            result.Add(new BranchTimeSeriesDataPoint
            {
                Label = monthStart.ToString("MMM yyyy"),
                Date = monthStart,
                Value = monthLoans.Sum(l => l.PrincipalAmount)
            });
        }

        return result;
    }

    private static List<BranchTimeSeriesDataPoint> GenerateSavingsBalanceTrend(
        List<SavingsAccount> accounts, int months)
    {
        var result = new List<BranchTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthDate = today.AddMonths(-i);
            var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);

            // Simplified - just showing current balance for all months
            result.Add(new BranchTimeSeriesDataPoint
            {
                Label = monthStart.ToString("MMM yyyy"),
                Date = monthStart,
                Value = accounts.Where(a => a.Status == "Active").Sum(a => a.Balance)
            });
        }

        return result;
    }

    private static List<BranchTimeSeriesDataPoint> GenerateMemberGrowthTrend(
        List<Member> members, int months)
    {
        var result = new List<BranchTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthDate = today.AddMonths(-i);
            var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var monthMembers = members.Where(m => 
                m.CreatedOn >= monthStart && m.CreatedOn < monthEnd).Count();

            result.Add(new BranchTimeSeriesDataPoint
            {
                Label = monthStart.ToString("MMM yyyy"),
                Date = monthStart,
                Value = monthMembers
            });
        }

        return result;
    }

    private static List<BranchTimeSeriesDataPoint> GeneratePortfolioAtRiskTrend(
        List<Loan> loans, int months)
    {
        var result = new List<BranchTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthDate = today.AddMonths(-i);
            var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);

            var activeLoans = loans.Where(l => l.Status != "Closed" && l.Status != "Written Off").ToList();
            var totalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal);
            var par30 = activeLoans.Where(l => CalculateDaysOverdue(l) >= 30)
                .Sum(l => l.OutstandingPrincipal);

            var parRate = totalOutstanding > 0 ? (par30 / totalOutstanding * 100) : 0;

            result.Add(new BranchTimeSeriesDataPoint
            {
                Label = monthStart.ToString("MMM yyyy"),
                Date = monthStart,
                Value = parRate
            });
        }

        return result;
    }

    private static List<BranchProductMixData> CalculateLoanProductMix(List<Loan> loans)
    {
        var activeLoans = loans.Where(l => l.Status != "Closed" && l.Status != "Written Off").ToList();
        var total = activeLoans.Sum(l => l.OutstandingPrincipal);

        return activeLoans
            .GroupBy(l => l.LoanProduct?.Name ?? "Unknown")
            .Select(g => new BranchProductMixData
            {
                ProductName = g.Key,
                AccountCount = g.Count(),
                TotalAmount = g.Sum(l => l.OutstandingPrincipal),
                Percentage = total > 0 ? (g.Sum(l => l.OutstandingPrincipal) / total * 100) : 0
            })
            .OrderByDescending(p => p.TotalAmount)
            .ToList();
    }

    private static List<BranchProductMixData> CalculateSavingsProductMix(List<SavingsAccount> accounts)
    {
        var activeAccounts = accounts.Where(a => a.Status == "Active").ToList();
        var total = activeAccounts.Sum(a => a.Balance);

        return activeAccounts
            .GroupBy(a => a.SavingsProduct?.Name ?? "Unknown")
            .Select(g => new BranchProductMixData
            {
                ProductName = g.Key,
                AccountCount = g.Count(),
                TotalAmount = g.Sum(a => a.Balance),
                Percentage = total > 0 ? (g.Sum(a => a.Balance) / total * 100) : 0
            })
            .OrderByDescending(p => p.TotalAmount)
            .ToList();
    }

    private static List<TopLoanOfficer> CalculateTopLoanOfficers(
        List<StaffEntity> staff, List<Loan> loans, List<Member> members)
    {
        // Simplified: return empty list until LoanOfficerAssignment entity is properly queried
        // TODO: Query LoanOfficerAssignment table to map officers to their loans
        return new List<TopLoanOfficer>();
    }

    private static List<TopMember> CalculateTopMembers(
        List<Member> members,
        List<Loan> loans,
        List<SavingsAccount> savingsAccounts,
        List<ShareAccount> shareAccounts)
    {
        return members.Select(member =>
        {
            var memberLoans = loans.Where(l => l.MemberId == member.Id).ToList();
            var activeLoans = memberLoans.Where(l => l.Status != "Closed" && l.Status != "Written Off").ToList();
            var loanOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal);

            var memberSavings = savingsAccounts
                .Where(s => s.MemberId == member.Id && s.Status == "Active")
                .Sum(s => s.Balance);

            var memberShares = shareAccounts
                .Where(s => s.MemberId == member.Id && s.Status == "Active")
                .Sum(s => s.TotalShareValue);

            // Calculate repayment score (simplified)
            var totalPrincipalDue = memberLoans.Sum(l => l.PrincipalAmount);
            var totalPrincipalPaid = memberLoans.Sum(l => l.PrincipalAmount - l.OutstandingPrincipal);
            var repaymentScore = totalPrincipalDue > 0 ? (totalPrincipalPaid / totalPrincipalDue * 100) : 100;

            return new TopMember
            {
                MemberId = member.Id,
                Name = $"{member.FirstName} {member.LastName}",
                MemberNumber = member.MemberNumber,
                LoanOutstanding = loanOutstanding,
                SavingsBalance = memberSavings,
                ShareCapital = memberShares,
                TotalLoans = memberLoans.Count,
                RepaymentScore = repaymentScore
            };
        })
        .OrderByDescending(m => m.LoanOutstanding + m.SavingsBalance + m.ShareCapital)
        .Take(10)
        .ToList();
    }

    private static List<BranchMonthlyComparison> GenerateMonthlyComparison(
        List<Loan> loans,
        List<SavingsAccount> savingsAccounts,
        List<Member> members,
        int months)
    {
        var result = new List<BranchMonthlyComparison>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthDate = today.AddMonths(-i);
            var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var monthLoans = loans.Where(l => 
                l.DisbursementDate.HasValue &&
                l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) >= monthStart &&
                l.DisbursementDate.Value.ToDateTime(TimeOnly.MinValue) < monthEnd).ToList();

            var monthMembers = members.Where(m => 
                m.CreatedOn >= monthStart && m.CreatedOn < monthEnd).Count();

            var activeLoans = loans.Where(l => l.Status != "Closed" && l.Status != "Written Off").ToList();
            var totalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal);
            var par30 = activeLoans.Where(l => CalculateDaysOverdue(l) >= 30)
                .Sum(l => l.OutstandingPrincipal);

            result.Add(new BranchMonthlyComparison
            {
                Month = monthStart.ToString("MMM"),
                Year = monthStart.Year,
                LoanDisbursement = monthLoans.Sum(l => l.PrincipalAmount),
                SavingsDeposits = 0, // Would calculate from transactions
                NewMembers = monthMembers,
                PortfolioAtRisk = totalOutstanding > 0 ? (par30 / totalOutstanding * 100) : 0,
                CollectionRate = 95.0m // Simplified
            });
        }

        return result;
    }

    private static string GetMemberName(DefaultIdType memberId, List<Member> members)
    {
        var member = members.FirstOrDefault(m => m.Id == memberId);
        return member != null ? $"{member.FirstName} {member.LastName}" : "Unknown";
    }

    private static string? GetLoanOfficerName(DefaultIdType? officerId, List<StaffEntity> staff)
    {
        if (!officerId.HasValue) return null;
        var officer = staff.FirstOrDefault(s => s.Id == officerId.Value);
        return officer != null ? $"{officer.FirstName} {officer.LastName}" : null;
    }
}

// Specification classes
internal sealed class MembersByBranchSpec : Specification<Member>
{
    public MembersByBranchSpec(DefaultIdType branchId)
    {
        Query.Where(m => m.Id == branchId);
    }
}

internal sealed class LoansByMemberIdsSpec : Specification<Loan>
{
    public LoansByMemberIdsSpec(List<DefaultIdType> memberIds)
    {
        Query.Where(l => memberIds.Contains(l.MemberId))
             .Include(l => l.LoanProduct);
    }
}

internal sealed class SavingsAccountsByMemberIdsSpec : Specification<SavingsAccount>
{
    public SavingsAccountsByMemberIdsSpec(List<DefaultIdType> memberIds)
    {
        Query.Where(s => memberIds.Contains(s.MemberId))
             .Include(s => s.SavingsProduct);
    }
}

internal sealed class ShareAccountsByMemberIdsSpec : Specification<ShareAccount>
{
    public ShareAccountsByMemberIdsSpec(List<DefaultIdType> memberIds)
    {
        Query.Where(s => memberIds.Contains(s.MemberId));
    }
}

internal sealed class StaffByBranchSpec : Specification<StaffEntity>
{
    public StaffByBranchSpec(DefaultIdType branchId)
    {
        Query.Where(s => s.BranchId == branchId);
    }
}

internal sealed class BranchTargetsByBranchSpec : Specification<BranchTarget>
{
    public BranchTargetsByBranchSpec(DefaultIdType branchId)
    {
        Query.Where(t => t.BranchId == branchId);
    }
}

internal sealed class CashVaultsByBranchSpec : Specification<CashVault>
{
    public CashVaultsByBranchSpec(DefaultIdType branchId)
    {
        Query.Where(c => c.BranchId == branchId);
    }
}
