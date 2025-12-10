using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific member.
/// </summary>
public sealed record GetMemberDashboardQuery(DefaultIdType MemberId) : IRequest<MemberDashboardResponse>;

public sealed class GetMemberDashboardHandler(
    IReadRepository<Member> memberRepository,
    IReadRepository<Loan> loanRepository,
    IReadRepository<LoanRepayment> loanRepaymentRepository,
    IReadRepository<LoanSchedule> loanScheduleRepository,
    IReadRepository<SavingsAccount> savingsAccountRepository,
    IReadRepository<SavingsTransaction> savingsTransactionRepository,
    IReadRepository<ShareAccount> shareAccountRepository,
    IReadRepository<FixedDeposit> fixedDepositRepository,
    IReadRepository<FeeCharge> feeChargeRepository,
    ICacheService cacheService,
    ILogger<GetMemberDashboardHandler> logger)
    : IRequestHandler<GetMemberDashboardQuery, MemberDashboardResponse>
{
    public async Task<MemberDashboardResponse> Handle(GetMemberDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"member-dashboard:{request.MemberId}";

        var cachedResult = await cacheService.GetAsync<MemberDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for member {MemberId}", request.MemberId);

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException($"Member {request.MemberId} not found");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Get all loans for this member
        var allLoans = await loanRepository
            .ListAsync(new LoansByMemberSpec(request.MemberId), cancellationToken);

        // Get all loan repayments
        var loanIds = allLoans.Select(l => l.Id).ToList();
        var allRepayments = loanIds.Count > 0
            ? await loanRepaymentRepository.ListAsync(new RepaymentsByLoanIdsSpec(loanIds), cancellationToken)
            : new List<LoanRepayment>();

        // Get all loan schedules (for upcoming payments)
        var allSchedules = loanIds.Count > 0
            ? await loanScheduleRepository.ListAsync(new SchedulesByLoanIdsSpec(loanIds), cancellationToken)
            : new List<LoanSchedule>();

        // Get savings accounts
        var allSavingsAccounts = await savingsAccountRepository
            .ListAsync(new SavingsAccountsByMemberSpec(request.MemberId), cancellationToken);

        // Get savings transactions
        var savingsAccountIds = allSavingsAccounts.Select(s => s.Id).ToList();
        var allSavingsTransactions = savingsAccountIds.Count > 0
            ? await savingsTransactionRepository.ListAsync(new TransactionsBySavingsAccountIdsSpec(savingsAccountIds), cancellationToken)
            : new List<SavingsTransaction>();

        // Get share accounts
        var allShareAccounts = await shareAccountRepository
            .ListAsync(new ShareAccountsByMemberSpec(request.MemberId), cancellationToken);

        // Get fixed deposits
        var allFixedDeposits = await fixedDepositRepository
            .ListAsync(new FixedDepositsByMemberSpec(request.MemberId), cancellationToken);

        // Get fee charges
        var allFeeCharges = await feeChargeRepository
            .ListAsync(new FeeChargesByMemberSpec(request.MemberId), cancellationToken);

        // Calculate all dashboard sections
        var overview = CalculateOverview(allLoans, allSavingsAccounts, allShareAccounts, allFixedDeposits);
        var loanPortfolio = CalculateLoanPortfolio(allLoans, allRepayments, today);
        var savingsPortfolio = CalculateSavingsPortfolio(allSavingsAccounts, allSavingsTransactions);
        var sharePortfolio = CalculateSharePortfolio(allShareAccounts);
        var fixedDepositsSummary = CalculateFixedDepositsSummary(allFixedDeposits, today);
        var feesSummary = CalculateFeesSummary(allFeeCharges);
        var repaymentPerformance = CalculateRepaymentPerformance(allRepayments, allSchedules);
        var recentTransactions = GetRecentTransactions(allRepayments, allSavingsTransactions);
        var upcomingPayments = GetUpcomingPayments(allSchedules, allLoans, today);
        var productHoldings = GetProductHoldings(allLoans, allSavingsAccounts, allShareAccounts, allFixedDeposits);
        var creditIndicators = CalculateCreditIndicators(member, allLoans, allRepayments, allSchedules, today);

        var response = new MemberDashboardResponse(
            MemberId: member.Id,
            MemberNumber: member.MemberNumber,
            MemberName: $"{member.FirstName} {member.LastName}",
            MemberSince: member.JoinDate,
            IsActive: member.IsActive,
            Overview: overview,
            LoanPortfolio: loanPortfolio,
            SavingsPortfolio: savingsPortfolio,
            SharePortfolio: sharePortfolio,
            FixedDeposits: fixedDepositsSummary,
            Fees: feesSummary,
            RepaymentPerformance: repaymentPerformance,
            RecentTransactions: recentTransactions,
            UpcomingPayments: upcomingPayments,
            ProductHoldings: productHoldings,
            CreditIndicators: creditIndicators
        );

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static MemberOverview CalculateOverview(
        List<Loan> loans,
        List<SavingsAccount> savingsAccounts,
        List<ShareAccount> shareAccounts,
        List<FixedDeposit> fixedDeposits)
    {
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var activeSavings = savingsAccounts.Where(s => s.Status == SavingsAccount.StatusActive).ToList();
        var activeShareAccounts = shareAccounts.Where(s => s.Status == ShareAccount.StatusActive).ToList();
        var activeDeposits = fixedDeposits.Where(f => f.Status == FixedDeposit.StatusActive).ToList();

        decimal totalAssets = activeSavings.Sum(s => s.Balance)
            + activeShareAccounts.Sum(s => s.TotalShareValue)
            + activeDeposits.Sum(f => f.PrincipalAmount + f.InterestEarned);

        decimal totalLiabilities = activeLoans.Sum(l => l.OutstandingPrincipal + l.OutstandingInterest);

        return new MemberOverview(
            TotalNetWorth: totalAssets - totalLiabilities,
            TotalAssets: totalAssets,
            TotalLiabilities: totalLiabilities,
            TotalProducts: activeLoans.Count + activeSavings.Count + activeShareAccounts.Count + activeDeposits.Count,
            ActiveLoans: activeLoans.Count,
            ActiveSavingsAccounts: activeSavings.Count,
            TotalShares: activeShareAccounts.Sum(s => s.NumberOfShares),
            ActiveFixedDeposits: activeDeposits.Count
        );
    }

    private static LoanPortfolioSummary CalculateLoanPortfolio(
        List<Loan> loans,
        List<LoanRepayment> repayments,
        DateOnly today)
    {
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var completedLoans = loans.Where(l => l.Status == Loan.StatusClosed).ToList();

        decimal totalBorrowed = loans.Sum(l => l.PrincipalAmount);
        decimal totalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal + l.OutstandingInterest);
        decimal totalPrincipalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal);
        decimal totalInterestOutstanding = activeLoans.Sum(l => l.OutstandingInterest);
        decimal totalRepaid = repayments.Sum(r => r.TotalAmount);
        decimal avgInterestRate = activeLoans.Count > 0 ? activeLoans.Average(l => l.InterestRate) : 0;

        var loanDetails = activeLoans.Select(loan =>
        {
            int daysOverdue = 0;
            int daysUntilNextPayment = 0;

            if (loan.ExpectedEndDate.HasValue && today > loan.ExpectedEndDate.Value)
            {
                daysOverdue = today.DayNumber - loan.ExpectedEndDate.Value.DayNumber;
            }

            return new LoanDetail(
                LoanId: loan.Id,
                LoanNumber: loan.LoanNumber,
                ProductName: loan.LoanProduct?.Name ?? "Unknown",
                PrincipalAmount: loan.PrincipalAmount,
                OutstandingPrincipal: loan.OutstandingPrincipal,
                OutstandingInterest: loan.OutstandingInterest,
                TotalOutstanding: loan.OutstandingPrincipal + loan.OutstandingInterest,
                InterestRate: loan.InterestRate,
                DisbursementDate: loan.DisbursementDate ?? today,
                ExpectedEndDate: loan.ExpectedEndDate ?? today.AddMonths(loan.TermMonths),
                DaysUntilNextPayment: daysUntilNextPayment,
                DaysOverdue: daysOverdue,
                Status: loan.Status
            );
        }).ToList();

        return new LoanPortfolioSummary(
            TotalLoans: loans.Count,
            ActiveLoans: activeLoans.Count,
            CompletedLoans: completedLoans.Count,
            TotalBorrowed: totalBorrowed,
            TotalOutstanding: totalOutstanding,
            TotalPrincipalOutstanding: totalPrincipalOutstanding,
            TotalInterestOutstanding: totalInterestOutstanding,
            TotalRepaid: totalRepaid,
            AverageInterestRate: avgInterestRate,
            ActiveLoanDetails: loanDetails
        );
    }

    private static SavingsPortfolioSummary CalculateSavingsPortfolio(
        List<SavingsAccount> savingsAccounts,
        List<SavingsTransaction> transactions)
    {
        var activeAccounts = savingsAccounts.Where(s => s.Status == SavingsAccount.StatusActive).ToList();
        var dormantAccounts = savingsAccounts.Where(s => s.Status == SavingsAccount.StatusDormant).ToList();

        decimal totalBalance = savingsAccounts.Sum(s => s.Balance);
        decimal totalDeposits = transactions.Where(t => t.TransactionType == SavingsTransaction.TypeDeposit).Sum(t => t.Amount);
        decimal totalWithdrawals = transactions.Where(t => t.TransactionType == SavingsTransaction.TypeWithdrawal).Sum(t => t.Amount);
        decimal totalInterestEarned = transactions.Where(t => t.TransactionType == SavingsTransaction.TypeInterest).Sum(t => t.Amount);
        decimal avgBalance = savingsAccounts.Count > 0 ? totalBalance / savingsAccounts.Count : 0;

        var accountDetails = savingsAccounts.Select(account =>
        {
            var accountTransactions = transactions.Where(t => t.SavingsAccountId == account.Id).ToList();
            decimal interestEarned = accountTransactions.Where(t => t.TransactionType == SavingsTransaction.TypeInterest).Sum(t => t.Amount);
            var lastTransaction = accountTransactions.OrderByDescending(t => t.TransactionDate).FirstOrDefault();

            // Get interest rate from associated product
            decimal productInterestRate = account.SavingsProduct?.InterestRate ?? 0;

            return new SavingsAccountDetail(
                AccountId: account.Id,
                AccountNumber: account.AccountNumber,
                ProductName: account.SavingsProduct?.Name ?? "Unknown",
                CurrentBalance: account.Balance,
                InterestRate: productInterestRate,
                InterestEarned: interestEarned,
                OpenedDate: account.OpenedDate,
                LastTransactionDate: lastTransaction?.TransactionDate,
                Status: account.Status
            );
        }).ToList();

        return new SavingsPortfolioSummary(
            TotalAccounts: savingsAccounts.Count,
            ActiveAccounts: activeAccounts.Count,
            DormantAccounts: dormantAccounts.Count,
            TotalBalance: totalBalance,
            TotalDeposits: totalDeposits,
            TotalWithdrawals: totalWithdrawals,
            TotalInterestEarned: totalInterestEarned,
            AverageBalance: avgBalance,
            AccountDetails: accountDetails
        );
    }

    private static SharePortfolioSummary CalculateSharePortfolio(List<ShareAccount> shareAccounts)
    {
        var activeAccounts = shareAccounts.Where(s => s.Status == ShareAccount.StatusActive).ToList();

        int totalShares = activeAccounts.Sum(s => s.NumberOfShares);
        decimal totalShareValue = activeAccounts.Sum(s => s.TotalShareValue);
        decimal currentSharePrice = activeAccounts.Count > 0 && totalShares > 0 ? totalShareValue / totalShares : 0;

        var accountDetails = shareAccounts.Select(account => new ShareAccountDetail(
            AccountId: account.Id,
            AccountNumber: account.AccountNumber,
            NumberOfShares: account.NumberOfShares,
            ShareValue: account.NumberOfShares > 0 ? account.TotalShareValue / account.NumberOfShares : 0,
            TotalValue: account.TotalShareValue,
            PurchaseDate: account.OpenedDate,
            Status: account.Status
        )).ToList();

        return new SharePortfolioSummary(
            TotalShareAccounts: shareAccounts.Count,
            TotalShares: totalShares,
            TotalShareValue: totalShareValue,
            TotalDividendsEarned: 0, // Would need dividend tracking
            CurrentSharePrice: currentSharePrice,
            AccountDetails: accountDetails
        );
    }

    private static FixedDepositSummary CalculateFixedDepositsSummary(
        List<FixedDeposit> fixedDeposits,
        DateOnly today)
    {
        var activeDeposits = fixedDeposits.Where(f => f.Status == FixedDeposit.StatusActive).ToList();
        var maturedDeposits = fixedDeposits.Where(f => f.Status == FixedDeposit.StatusMatured).ToList();

        decimal totalPrincipal = activeDeposits.Sum(f => f.PrincipalAmount);
        decimal totalInterestEarned = fixedDeposits.Sum(f => f.InterestEarned);
        decimal totalMaturityValue = activeDeposits.Sum(f => f.PrincipalAmount + f.InterestEarned);

        var depositDetails = fixedDeposits.Select(deposit =>
        {
            int daysToMaturity = deposit.MaturityDate > today
                ? deposit.MaturityDate.DayNumber - today.DayNumber
                : 0;

            return new FixedDepositDetail(
                DepositId: deposit.Id,
                CertificateNumber: deposit.CertificateNumber,
                PrincipalAmount: deposit.PrincipalAmount,
                InterestRate: deposit.InterestRate,
                AccruedInterest: deposit.InterestEarned,
                MaturityAmount: deposit.PrincipalAmount + deposit.InterestEarned,
                StartDate: deposit.DepositDate,
                MaturityDate: deposit.MaturityDate,
                DaysToMaturity: daysToMaturity,
                Status: deposit.Status
            );
        }).ToList();

        return new FixedDepositSummary(
            TotalDeposits: fixedDeposits.Count,
            ActiveDeposits: activeDeposits.Count,
            MaturedDeposits: maturedDeposits.Count,
            TotalPrincipal: totalPrincipal,
            TotalInterestEarned: totalInterestEarned,
            TotalMaturityValue: totalMaturityValue,
            DepositDetails: depositDetails
        );
    }

    private static FeesSummary CalculateFeesSummary(List<FeeCharge> feeCharges)
    {
        decimal totalCharged = feeCharges.Sum(f => f.Amount);
        decimal totalPaid = feeCharges.Sum(f => f.AmountPaid);
        decimal totalOutstanding = totalCharged - totalPaid;

        var feesByType = feeCharges
            .GroupBy(f => f.FeeDefinition?.Name ?? "Unknown")
            .Select(g => new FeeBreakdown(
                FeeType: g.Key,
                TotalAmount: g.Sum(f => f.Amount),
                Count: g.Count()
            ))
            .ToList();

        return new FeesSummary(
            TotalFeesCharged: totalCharged,
            TotalFeesPaid: totalPaid,
            TotalFeesOutstanding: totalOutstanding,
            TotalFeeTransactions: feeCharges.Count,
            FeesByType: feesByType
        );
    }

    private static RepaymentPerformance CalculateRepaymentPerformance(
        List<LoanRepayment> repayments,
        List<LoanSchedule> schedules)
    {
        int totalPaymentsMade = repayments.Count;
        int onTimePayments = 0;
        int latePayments = 0;

        // Simple approximation - compare repayment dates to scheduled due dates
        foreach (var repayment in repayments)
        {
            var matchingSchedule = schedules
                .Where(s => s.LoanId == repayment.LoanId)
                .OrderBy(s => Math.Abs(s.DueDate.DayNumber - repayment.RepaymentDate.DayNumber))
                .FirstOrDefault();

            if (matchingSchedule != null && repayment.RepaymentDate <= matchingSchedule.DueDate)
            {
                onTimePayments++;
            }
            else
            {
                latePayments++;
            }
        }

        decimal onTimePercentage = totalPaymentsMade > 0
            ? (decimal)onTimePayments / totalPaymentsMade * 100
            : 100;

        decimal totalPrincipalRepaid = repayments.Sum(r => r.PrincipalAmount);
        decimal totalInterestRepaid = repayments.Sum(r => r.InterestAmount);

        // Get last 12 months performance
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var startOfPeriod = today.AddMonths(-12);

        var monthlyPerformance = Enumerable.Range(0, 12)
            .Select(i =>
            {
                var monthStart = today.AddMonths(-11 + i);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var monthSchedules = schedules
                    .Where(s => s.DueDate >= monthStart && s.DueDate <= monthEnd)
                    .ToList();

                var monthRepayments = repayments
                    .Where(r => r.RepaymentDate >= monthStart && r.RepaymentDate <= monthEnd)
                    .ToList();

                decimal expected = monthSchedules.Sum(s => s.PrincipalAmount + s.InterestAmount);
                decimal actual = monthRepayments.Sum(r => r.TotalAmount);
                bool onTime = monthRepayments.All(r =>
                    monthSchedules.Any(s => s.LoanId == r.LoanId && r.RepaymentDate <= s.DueDate));

                return new MonthlyRepaymentSummary(
                    Year: monthStart.Year,
                    Month: monthStart.Month,
                    ExpectedAmount: expected,
                    ActualAmount: actual,
                    Variance: actual - expected,
                    OnTime: onTime
                );
            })
            .ToList();

        return new RepaymentPerformance(
            TotalPaymentsMade: totalPaymentsMade,
            OnTimePayments: onTimePayments,
            LatePayments: latePayments,
            OnTimePaymentPercentage: onTimePercentage,
            TotalPrincipalRepaid: totalPrincipalRepaid,
            TotalInterestRepaid: totalInterestRepaid,
            CurrentDaysOverdue: 0, // Would need current overdue calculation
            MaxDaysOverdue: 0, // Would need historical tracking
            Last12MonthsPerformance: monthlyPerformance
        );
    }

    private static List<RecentTransactionItem> GetRecentTransactions(
        List<LoanRepayment> repayments,
        List<SavingsTransaction> savingsTransactions)
    {
        var transactions = new List<RecentTransactionItem>();

        // Add loan repayments
        transactions.AddRange(repayments
            .OrderByDescending(r => r.RepaymentDate)
            .Take(10)
            .Select(r => new RecentTransactionItem(
                TransactionId: r.Id,
                TransactionDate: r.RepaymentDate,
                TransactionType: "Loan Repayment",
                Description: $"Payment via {r.PaymentMethod}",
                AccountNumber: r.Loan?.LoanNumber ?? "Unknown",
                Amount: r.TotalAmount,
                Direction: "Debit"
            )));

        // Add savings transactions
        transactions.AddRange(savingsTransactions
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t => new RecentTransactionItem(
                TransactionId: t.Id,
                TransactionDate: t.TransactionDate,
                TransactionType: t.TransactionType,
                Description: t.Description ?? t.TransactionType,
                AccountNumber: t.SavingsAccount?.AccountNumber ?? "Unknown",
                Amount: t.Amount,
                Direction: t.TransactionType == SavingsTransaction.TypeDeposit || t.TransactionType == SavingsTransaction.TypeInterest
                    ? "Credit"
                    : "Debit"
            )));

        return transactions
            .OrderByDescending(t => t.TransactionDate)
            .Take(15)
            .ToList();
    }

    private static List<UpcomingPayment> GetUpcomingPayments(
        List<LoanSchedule> schedules,
        List<Loan> loans,
        DateOnly today)
    {
        return schedules
            .Where(s => s.DueDate >= today && !s.IsPaid)
            .OrderBy(s => s.DueDate)
            .Take(10)
            .Select(s =>
            {
                var loan = loans.FirstOrDefault(l => l.Id == s.LoanId);
                int daysUntilDue = s.DueDate.DayNumber - today.DayNumber;

                return new UpcomingPayment(
                    DueDate: s.DueDate,
                    LoanNumber: loan?.LoanNumber ?? "Unknown",
                    PrincipalDue: s.PrincipalAmount,
                    InterestDue: s.InterestAmount,
                    TotalDue: s.PrincipalAmount + s.InterestAmount,
                    DaysUntilDue: daysUntilDue
                );
            })
            .ToList();
    }

    private static List<ProductHolding> GetProductHoldings(
        List<Loan> loans,
        List<SavingsAccount> savingsAccounts,
        List<ShareAccount> shareAccounts,
        List<FixedDeposit> fixedDeposits)
    {
        var holdings = new List<ProductHolding>();

        // Group loans by product
        var loansByProduct = loans
            .Where(l => l.Status == Loan.StatusDisbursed)
            .GroupBy(l => l.LoanProduct?.Name ?? "Unknown")
            .Select(g => new ProductHolding(
                ProductType: "Loan",
                ProductName: g.Key,
                Count: g.Count(),
                TotalValue: g.Sum(l => l.OutstandingPrincipal + l.OutstandingInterest),
                Status: "Active"
            ));
        holdings.AddRange(loansByProduct);

        // Group savings by product
        var savingsByProduct = savingsAccounts
            .Where(s => s.Status == SavingsAccount.StatusActive)
            .GroupBy(s => s.SavingsProduct?.Name ?? "Unknown")
            .Select(g => new ProductHolding(
                ProductType: "Savings",
                ProductName: g.Key,
                Count: g.Count(),
                TotalValue: g.Sum(s => s.Balance),
                Status: "Active"
            ));
        holdings.AddRange(savingsByProduct);

        // Shares
        var activeShares = shareAccounts.Where(s => s.Status == ShareAccount.StatusActive).ToList();
        if (activeShares.Count > 0)
        {
            holdings.Add(new ProductHolding(
                ProductType: "Shares",
                ProductName: "Share Capital",
                Count: activeShares.Sum(s => s.NumberOfShares),
                TotalValue: activeShares.Sum(s => s.TotalShareValue),
                Status: "Active"
            ));
        }

        // Fixed Deposits
        var activeFds = fixedDeposits.Where(f => f.Status == FixedDeposit.StatusActive).ToList();
        if (activeFds.Count > 0)
        {
            holdings.Add(new ProductHolding(
                ProductType: "Fixed Deposit",
                ProductName: "Term Deposits",
                Count: activeFds.Count,
                TotalValue: activeFds.Sum(f => f.PrincipalAmount),
                Status: "Active"
            ));
        }

        return holdings;
    }

    private static CreditIndicators CalculateCreditIndicators(
        Member member,
        List<Loan> loans,
        List<LoanRepayment> repayments,
        List<LoanSchedule> schedules,
        DateOnly today)
    {
        // Calculate repayment score (simple on-time payment ratio)
        int totalPayments = repayments.Count;
        int onTimePayments = 0;

        foreach (var repayment in repayments)
        {
            var matchingSchedule = schedules
                .Where(s => s.LoanId == repayment.LoanId)
                .OrderBy(s => Math.Abs(s.DueDate.DayNumber - repayment.RepaymentDate.DayNumber))
                .FirstOrDefault();

            if (matchingSchedule != null && repayment.RepaymentDate <= matchingSchedule.DueDate)
            {
                onTimePayments++;
            }
        }

        decimal repaymentScore = totalPayments > 0
            ? (decimal)onTimePayments / totalPayments * 100
            : 100;

        string riskCategory = repaymentScore switch
        {
            >= 90 => "Low Risk",
            >= 70 => "Medium Risk",
            >= 50 => "High Risk",
            _ => "Very High Risk"
        };

        int membershipTenure = today.DayNumber - member.JoinDate.DayNumber;

        decimal totalLifetimeBorrowing = loans.Sum(l => l.PrincipalAmount);
        decimal totalLifetimeRepaid = repayments.Sum(r => r.TotalAmount);
        int loansCompleted = loans.Count(l => l.Status == Loan.StatusClosed);
        int defaultedLoans = loans.Count(l => l.Status == Loan.StatusWrittenOff);

        // Debt to income ratio (if income is available)
        decimal activeDebt = loans.Where(l => l.Status == Loan.StatusDisbursed)
            .Sum(l => l.OutstandingPrincipal);
        decimal monthlyIncome = member.MonthlyIncome ?? 0m;
        decimal debtToIncomeRatio = monthlyIncome > 0
            ? activeDebt / (monthlyIncome * 12) * 100
            : 0;

        // Simple eligibility check
        bool isEligible = member.IsActive
            && repaymentScore >= 70
            && defaultedLoans == 0
            && debtToIncomeRatio < 50;

        // Simple max loan calculation (3x annual income minus current debt)
        decimal maxLoan = monthlyIncome > 0
            ? Math.Max(0, (monthlyIncome * 12 * 3) - activeDebt)
            : 0;

        return new CreditIndicators(
            RepaymentScore: repaymentScore,
            RiskCategory: riskCategory,
            MembershipTenureDays: membershipTenure,
            TotalLifetimeBorrowing: totalLifetimeBorrowing,
            TotalLifetimeRepaid: totalLifetimeRepaid,
            TotalLoansCompleted: loansCompleted,
            TotalDefaultedLoans: defaultedLoans,
            DebtToIncomeRatio: debtToIncomeRatio,
            IsEligibleForNewLoan: isEligible,
            MaxEligibleLoanAmount: maxLoan
        );
    }
}

// Specifications for querying data

internal sealed class LoansByMemberSpec : Specification<Loan>
{
    public LoansByMemberSpec(DefaultIdType memberId)
    {
        Query.Where(l => l.MemberId == memberId);
        Query.Include(l => l.LoanProduct);
    }
}

internal sealed class RepaymentsByLoanIdsSpec : Specification<LoanRepayment>
{
    public RepaymentsByLoanIdsSpec(List<DefaultIdType> loanIds)
    {
        Query.Where(r => loanIds.Contains(r.LoanId));
        Query.Include(r => r.Loan);
    }
}

internal sealed class SchedulesByLoanIdsSpec : Specification<LoanSchedule>
{
    public SchedulesByLoanIdsSpec(List<DefaultIdType> loanIds)
    {
        Query.Where(s => loanIds.Contains(s.LoanId));
    }
}

internal sealed class SavingsAccountsByMemberSpec : Specification<SavingsAccount>
{
    public SavingsAccountsByMemberSpec(DefaultIdType memberId)
    {
        Query.Where(s => s.MemberId == memberId);
        Query.Include(s => s.SavingsProduct);
    }
}

internal sealed class TransactionsBySavingsAccountIdsSpec : Specification<SavingsTransaction>
{
    public TransactionsBySavingsAccountIdsSpec(List<DefaultIdType> savingsAccountIds)
    {
        Query.Where(t => savingsAccountIds.Contains(t.SavingsAccountId));
        Query.Include(t => t.SavingsAccount);
    }
}

internal sealed class ShareAccountsByMemberSpec : Specification<ShareAccount>
{
    public ShareAccountsByMemberSpec(DefaultIdType memberId)
    {
        Query.Where(s => s.MemberId == memberId);
    }
}

internal sealed class FixedDepositsByMemberSpec : Specification<FixedDeposit>
{
    public FixedDepositsByMemberSpec(DefaultIdType memberId)
    {
        Query.Where(f => f.MemberId == memberId);
        Query.Include(f => f.SavingsProduct);
    }
}

internal sealed class FeeChargesByMemberSpec : Specification<FeeCharge>
{
    public FeeChargesByMemberSpec(DefaultIdType memberId)
    {
        Query.Where(f => f.MemberId == memberId);
        Query.Include(f => f.FeeDefinition);
    }
}
