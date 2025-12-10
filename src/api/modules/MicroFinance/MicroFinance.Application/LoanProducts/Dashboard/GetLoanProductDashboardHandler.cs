using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Dashboard;

/// <summary>
/// Query to get comprehensive dashboard analytics for a loan product.
/// </summary>
public sealed record GetLoanProductDashboardQuery(DefaultIdType Id) : IRequest<LoanProductDashboardResponse>;

/// <summary>
/// Handler for retrieving loan product dashboard analytics.
/// </summary>
public sealed class GetLoanProductDashboardHandler(
    IReadRepository<LoanProduct> loanProductRepository,
    IReadRepository<Loan> loanRepository,
    IReadRepository<LoanSchedule> scheduleRepository,
    IReadRepository<LoanRepayment> repaymentRepository,
    IReadRepository<Member> memberRepository,
    ICacheService cacheService)
    : IRequestHandler<GetLoanProductDashboardQuery, LoanProductDashboardResponse>
{
    public async Task<LoanProductDashboardResponse> Handle(
        GetLoanProductDashboardQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Try cache first
        string cacheKey = $"loan-product-dashboard:{request.Id}";
        var cached = await cacheService.GetAsync<LoanProductDashboardResponse>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        // Get the loan product
        var product = await loanProductRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Loan Product with ID {request.Id} not found.");

        // Get all loans for this product
        var loansSpec = new LoansByProductSpec(request.Id);
        var loans = await loanRepository.ListAsync(loansSpec, cancellationToken);

        // Get loan IDs for related queries
        var loanIds = loans.Select(l => l.Id).ToList();

        // Get schedules for overdue calculations
        var schedulesSpec = new SchedulesByLoanIdsSpec(loanIds);
        var schedules = loanIds.Count != 0
            ? await scheduleRepository.ListAsync(schedulesSpec, cancellationToken)
            : new List<LoanSchedule>();

        // Get repayments for collection metrics
        var repaymentsSpec = new RepaymentsByLoanIdsSpec(loanIds);
        var repayments = loanIds.Count != 0
            ? await repaymentRepository.ListAsync(repaymentsSpec, cancellationToken)
            : new List<LoanRepayment>();

        // Get unique member IDs
        var memberIds = loans.Select(l => l.MemberId).Distinct().ToList();
        var membersSpec = new MembersByIdsSpec(memberIds);
        var members = memberIds.Count != 0
            ? await memberRepository.ListAsync(membersSpec, cancellationToken)
            : new List<Member>();
        var memberDict = members.ToDictionary(m => m.Id, m => m);

        // Build the response
        var response = new LoanProductDashboardResponse(
            LoanProductId: product.Id,
            ProductCode: product.Code,
            ProductName: product.Name,
            IsActive: product.IsActive,
            Overview: CalculateOverview(product, loans),
            Portfolio: CalculatePortfolioMetrics(loans, repayments, memberIds.Count),
            Repayments: CalculateRepaymentMetrics(repayments, schedules),
            Delinquency: CalculateDelinquencyMetrics(loans, schedules),
            LoansByStatus: CalculateLoanDistribution(loans),
            DisbursementTrends: CalculateDisbursementTrends(loans),
            CollectionTrends: CalculateCollectionTrends(repayments),
            RecentActivity: GetRecentActivity(loans, memberDict),
            Alerts: GenerateAlerts(loans, schedules)
        );

        // Cache for 5 minutes
        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static LoanProductOverview CalculateOverview(LoanProduct product, List<Loan> loans)
    {
        var disbursedLoans = loans.Where(l => l.DisbursementDate.HasValue).ToList();

        return new LoanProductOverview(
            TotalLoansIssued: loans.Count,
            ActiveLoans: loans.Count(l => l.Status == Loan.StatusDisbursed),
            PendingLoans: loans.Count(l => l.Status == Loan.StatusPending),
            ApprovedLoans: loans.Count(l => l.Status == Loan.StatusApproved),
            ClosedLoans: loans.Count(l => l.Status == Loan.StatusClosed),
            RejectedLoans: loans.Count(l => l.Status == Loan.StatusRejected),
            WrittenOffLoans: loans.Count(l => l.Status == Loan.StatusWrittenOff),
            TotalAmountDisbursed: disbursedLoans.Sum(l => l.PrincipalAmount),
            TotalOutstanding: loans.Where(l => l.Status == Loan.StatusDisbursed)
                .Sum(l => l.OutstandingPrincipal + l.OutstandingInterest),
            AverageInterestRate: loans.Count != 0 ? loans.Average(l => l.InterestRate) : 0,
            AverageLoanSize: disbursedLoans.Count != 0 ? disbursedLoans.Average(l => l.PrincipalAmount) : 0,
            AverageTermMonths: loans.Count != 0 ? (int)loans.Average(l => l.TermMonths) : 0,
            ProductInterestRate: product.InterestRate,
            MinLoanAmount: product.MinLoanAmount,
            MaxLoanAmount: product.MaxLoanAmount
        );
    }

    private static LoanProductPortfolioMetrics CalculatePortfolioMetrics(
        List<Loan> loans,
        List<LoanRepayment> repayments,
        int borrowerCount)
    {
        var disbursedLoans = loans.Where(l => l.DisbursementDate.HasValue).ToList();
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var activeRepayments = repayments.Where(r => r.Status == LoanRepayment.StatusActive).ToList();

        // Calculate expected interest (simplified - actual would need amortization calculation)
        var totalInterestExpected = activeLoans.Sum(l =>
            l.PrincipalAmount * (l.InterestRate / 100) * (l.TermMonths / 12m));

        return new LoanProductPortfolioMetrics(
            TotalPrincipalDisbursed: disbursedLoans.Sum(l => l.PrincipalAmount),
            TotalPrincipalOutstanding: activeLoans.Sum(l => l.OutstandingPrincipal),
            TotalPrincipalCollected: activeRepayments.Sum(r => r.PrincipalAmount),
            TotalInterestExpected: totalInterestExpected,
            TotalInterestOutstanding: activeLoans.Sum(l => l.OutstandingInterest),
            TotalInterestCollected: activeRepayments.Sum(r => r.InterestAmount),
            MinLoanIssued: disbursedLoans.Count != 0 ? disbursedLoans.Min(l => l.PrincipalAmount) : 0,
            MaxLoanIssued: disbursedLoans.Count != 0 ? disbursedLoans.Max(l => l.PrincipalAmount) : 0,
            ShortestTermMonths: loans.Count != 0 ? loans.Min(l => l.TermMonths) : 0,
            LongestTermMonths: loans.Count != 0 ? loans.Max(l => l.TermMonths) : 0,
            TotalBorrowers: borrowerCount
        );
    }

    private static RepaymentMetrics CalculateRepaymentMetrics(
        List<LoanRepayment> repayments,
        List<LoanSchedule> schedules)
    {
        var activeRepayments = repayments.Where(r => r.Status == LoanRepayment.StatusActive).ToList();

        // Calculate on-time vs late repayments by comparing to schedule due dates
        var schedulesByLoan = schedules.GroupBy(s => s.LoanId).ToDictionary(g => g.Key, g => g.ToList());
        int onTimeCount = 0;
        int lateCount = 0;

        foreach (var repayment in activeRepayments)
        {
            if (schedulesByLoan.TryGetValue(repayment.LoanId, out var loanSchedules))
            {
                // Find the schedule that this payment was for
                var relevantSchedule = loanSchedules
                    .Where(s => !s.IsPaid || s.PaidDate >= repayment.RepaymentDate)
                    .OrderBy(s => s.DueDate)
                    .FirstOrDefault();

                if (relevantSchedule != null)
                {
                    if (repayment.RepaymentDate <= relevantSchedule.DueDate)
                        onTimeCount++;
                    else
                        lateCount++;
                }
            }
        }

        var totalCollected = activeRepayments.Sum(r => r.TotalAmount);
        var totalExpected = schedules.Sum(s => s.TotalAmount);

        return new RepaymentMetrics(
            TotalCollected: totalCollected,
            TotalPrincipalCollected: activeRepayments.Sum(r => r.PrincipalAmount),
            TotalInterestCollected: activeRepayments.Sum(r => r.InterestAmount),
            TotalPenaltiesCollected: activeRepayments.Sum(r => r.PenaltyAmount),
            TotalRepayments: activeRepayments.Count,
            CollectionRate: totalExpected > 0 ? (totalCollected / totalExpected * 100) : 0,
            OnTimeRepaymentRate: activeRepayments.Count > 0
                ? ((decimal)onTimeCount / activeRepayments.Count * 100)
                : 0,
            OnTimeRepayments: onTimeCount,
            LateRepayments: lateCount,
            AverageRepaymentAmount: activeRepayments.Count > 0
                ? activeRepayments.Average(r => r.TotalAmount)
                : 0
        );
    }

    private static DelinquencyMetrics CalculateDelinquencyMetrics(
        List<Loan> loans,
        List<LoanSchedule> schedules)
    {
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var writtenOffLoans = loans.Where(l => l.Status == Loan.StatusWrittenOff).ToList();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Get unpaid schedules past due date
        var overdueSchedules = schedules
            .Where(s => !s.IsPaid && s.DueDate < today)
            .ToList();

        var overdueByLoan = overdueSchedules.GroupBy(s => s.LoanId).ToDictionary(g => g.Key, g => g.ToList());

        int overdueLoans = 0;
        decimal overdueAmount = 0;
        decimal par1Day = 0;
        decimal par30Days = 0;
        decimal par60Days = 0;
        decimal par90Days = 0;
        int loansInArrears = 0;
        decimal arrearsAmount = 0;

        var totalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal);

        foreach (var loan in activeLoans)
        {
            if (overdueByLoan.TryGetValue(loan.Id, out var loanOverdueSchedules))
            {
                var oldestOverdue = loanOverdueSchedules.Min(s => s.DueDate);
                var daysOverdue = today.DayNumber - oldestOverdue.DayNumber;
                var loanOverdueAmount = loanOverdueSchedules.Sum(s => s.TotalAmount - s.PaidAmount);

                if (daysOverdue > 0)
                {
                    overdueLoans++;
                    overdueAmount += loanOverdueAmount;
                    loansInArrears++;
                    arrearsAmount += loanOverdueAmount;

                    // PAR calculations (outstanding principal of loans with payments X days overdue)
                    if (daysOverdue >= 1) par1Day += loan.OutstandingPrincipal;
                    if (daysOverdue >= 30) par30Days += loan.OutstandingPrincipal;
                    if (daysOverdue >= 60) par60Days += loan.OutstandingPrincipal;
                    if (daysOverdue >= 90) par90Days += loan.OutstandingPrincipal;
                }
            }
        }

        var writtenOffAmount = writtenOffLoans.Sum(l => l.OutstandingPrincipal + l.OutstandingInterest);
        var totalDisbursed = loans.Where(l => l.DisbursementDate.HasValue).Sum(l => l.PrincipalAmount);

        return new DelinquencyMetrics(
            OverdueLoans: overdueLoans,
            OverdueAmount: overdueAmount,
            PortfolioAtRisk1Day: totalOutstanding > 0 ? (par1Day / totalOutstanding * 100) : 0,
            PortfolioAtRisk30Days: totalOutstanding > 0 ? (par30Days / totalOutstanding * 100) : 0,
            PortfolioAtRisk60Days: totalOutstanding > 0 ? (par60Days / totalOutstanding * 100) : 0,
            PortfolioAtRisk90Days: totalOutstanding > 0 ? (par90Days / totalOutstanding * 100) : 0,
            WrittenOffCount: writtenOffLoans.Count,
            WrittenOffAmount: writtenOffAmount,
            WriteOffRate: totalDisbursed > 0 ? (writtenOffAmount / totalDisbursed * 100) : 0,
            LoansInArrears: loansInArrears,
            ArrearsAmount: arrearsAmount
        );
    }

    private static List<LoanDistribution> CalculateLoanDistribution(List<Loan> loans)
    {
        var total = loans.Count;
        var statusGroups = loans
            .GroupBy(l => l.Status)
            .Select(g => new LoanDistribution(
                Status: g.Key,
                Count: g.Count(),
                TotalAmount: g.Sum(l => l.PrincipalAmount),
                Percentage: total > 0 ? ((decimal)g.Count() / total * 100) : 0
            ))
            .OrderByDescending(d => d.Count)
            .ToList();

        return statusGroups;
    }

    private static List<MonthlyTrend> CalculateDisbursementTrends(List<Loan> loans)
    {
        var disbursedLoans = loans.Where(l => l.DisbursementDate.HasValue).ToList();
        var last12Months = Enumerable.Range(0, 12)
            .Select(i => DateTime.UtcNow.AddMonths(-i))
            .Select(d => new { d.Month, d.Year })
            .Reverse()
            .ToList();

        return last12Months.Select(m =>
        {
            var monthLoans = disbursedLoans
                .Where(l => l.DisbursementDate!.Value.Month == m.Month && l.DisbursementDate.Value.Year == m.Year)
                .ToList();

            return new MonthlyTrend(
                Month: new DateTime(m.Year, m.Month, 1).ToString("MMM"),
                Year: m.Year,
                Amount: monthLoans.Sum(l => l.PrincipalAmount),
                Count: monthLoans.Count
            );
        }).ToList();
    }

    private static List<MonthlyTrend> CalculateCollectionTrends(List<LoanRepayment> repayments)
    {
        var activeRepayments = repayments.Where(r => r.Status == LoanRepayment.StatusActive).ToList();
        var last12Months = Enumerable.Range(0, 12)
            .Select(i => DateTime.UtcNow.AddMonths(-i))
            .Select(d => new { d.Month, d.Year })
            .Reverse()
            .ToList();

        return last12Months.Select(m =>
        {
            var monthRepayments = activeRepayments
                .Where(r => r.RepaymentDate.Month == m.Month && r.RepaymentDate.Year == m.Year)
                .ToList();

            return new MonthlyTrend(
                Month: new DateTime(m.Year, m.Month, 1).ToString("MMM"),
                Year: m.Year,
                Amount: monthRepayments.Sum(r => r.TotalAmount),
                Count: monthRepayments.Count
            );
        }).ToList();
    }

    private static List<RecentLoanActivity> GetRecentActivity(
        List<Loan> loans,
        Dictionary<DefaultIdType, Member> memberDict)
    {
        return loans
            .Where(l => l.DisbursementDate.HasValue)
            .OrderByDescending(l => l.DisbursementDate)
            .Take(10)
            .Select(l => new RecentLoanActivity(
                LoanId: l.Id,
                LoanNumber: l.LoanNumber,
                MemberName: memberDict.TryGetValue(l.MemberId, out var member)
                    ? $"{member.FirstName} {member.LastName}"
                    : "Unknown",
                PrincipalAmount: l.PrincipalAmount,
                DisbursementDate: l.DisbursementDate,
                Status: l.Status,
                OutstandingBalance: l.OutstandingPrincipal + l.OutstandingInterest
            ))
            .ToList();
    }

    private static LoanProductAlerts GenerateAlerts(List<Loan> loans, List<LoanSchedule> schedules)
    {
        var alerts = new List<LoanProductAlert>();
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Calculate PAR 30
        var overdueSchedules = schedules.Where(s => !s.IsPaid && s.DueDate < today).ToList();
        var loansWithOverdue30 = overdueSchedules
            .Where(s => (today.DayNumber - s.DueDate.DayNumber) >= 30)
            .Select(s => s.LoanId)
            .Distinct()
            .Count();

        var totalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal);
        var par30Outstanding = activeLoans
            .Where(l => overdueSchedules.Any(s => s.LoanId == l.Id && (today.DayNumber - s.DueDate.DayNumber) >= 30))
            .Sum(l => l.OutstandingPrincipal);
        var par30Rate = totalOutstanding > 0 ? (par30Outstanding / totalOutstanding * 100) : 0;

        // High PAR alert
        if (par30Rate > 10)
        {
            alerts.Add(new LoanProductAlert(
                Severity: "Critical",
                Title: "High Portfolio at Risk",
                Description: $"PAR 30 is at {par30Rate:F1}% ({loansWithOverdue30} loans). Industry standard is below 5%.",
                ActionUrl: "/microfinance/loans?filter=overdue"
            ));
        }
        else if (par30Rate > 5)
        {
            alerts.Add(new LoanProductAlert(
                Severity: "Warning",
                Title: "Elevated Portfolio at Risk",
                Description: $"PAR 30 is at {par30Rate:F1}%. Monitor closely to prevent further deterioration.",
                ActionUrl: "/microfinance/loans?filter=overdue"
            ));
        }

        // Pending approvals
        var pendingLoans = loans.Count(l => l.Status == Loan.StatusPending);
        if (pendingLoans > 5)
        {
            alerts.Add(new LoanProductAlert(
                Severity: "Warning",
                Title: "Pending Loan Applications",
                Description: $"{pendingLoans} loan applications awaiting approval. Process to maintain member satisfaction.",
                ActionUrl: "/microfinance/loans?filter=pending"
            ));
        }

        // Written off loans
        var writtenOffCount = loans.Count(l => l.Status == Loan.StatusWrittenOff);
        if (writtenOffCount > 0)
        {
            var writtenOffRate = loans.Count > 0 ? ((decimal)writtenOffCount / loans.Count * 100) : 0;
            if (writtenOffRate > 5)
            {
                alerts.Add(new LoanProductAlert(
                    Severity: "Critical",
                    Title: "High Write-off Rate",
                    Description: $"{writtenOffCount} loans written off ({writtenOffRate:F1}%). Review credit assessment criteria.",
                    ActionUrl: "/microfinance/loans?filter=written-off"
                ));
            }
        }

        // Low activity alert
        var last30DaysDisbursements = loans.Count(l =>
            l.DisbursementDate.HasValue &&
            l.DisbursementDate.Value >= today.AddDays(-30));

        if (last30DaysDisbursements == 0 && activeLoans.Count > 0)
        {
            alerts.Add(new LoanProductAlert(
                Severity: "Info",
                Title: "No Recent Disbursements",
                Description: "No new loans disbursed in the last 30 days for this product.",
                ActionUrl: null
            ));
        }

        return new LoanProductAlerts(
            Items: alerts,
            TotalAlerts: alerts.Count,
            CriticalCount: alerts.Count(a => a.Severity == "Critical"),
            WarningCount: alerts.Count(a => a.Severity == "Warning"),
            InfoCount: alerts.Count(a => a.Severity == "Info")
        );
    }
}

// Specification classes
public sealed class LoansByProductSpec : Specification<Loan>
{
    public LoansByProductSpec(DefaultIdType productId)
    {
        Query.Where(l => l.LoanProductId == productId);
    }
}

public sealed class SchedulesByLoanIdsSpec : Specification<LoanSchedule>
{
    public SchedulesByLoanIdsSpec(List<DefaultIdType> loanIds)
    {
        Query.Where(s => loanIds.Contains(s.LoanId));
    }
}

public sealed class RepaymentsByLoanIdsSpec : Specification<LoanRepayment>
{
    public RepaymentsByLoanIdsSpec(List<DefaultIdType> loanIds)
    {
        Query.Where(r => loanIds.Contains(r.LoanId));
    }
}

public sealed class MembersByIdsSpec : Specification<Member>
{
    public MembersByIdsSpec(List<DefaultIdType> memberIds)
    {
        Query.Where(m => memberIds.Contains(m.Id));
    }
}
