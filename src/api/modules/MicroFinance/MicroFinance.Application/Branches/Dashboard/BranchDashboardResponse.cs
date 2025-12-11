namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Dashboard;

/// <summary>
/// Response containing comprehensive branch performance metrics and analytics.
/// </summary>
public sealed record BranchDashboardResponse
{
    // Basic Branch Info
    public DefaultIdType BranchId { get; init; }
    public string BranchName { get; init; } = default!;
    public string? Code { get; init; }
    public string BranchType { get; init; } = default!;
    public string? Address { get; init; }
    public string? ManagerName { get; init; }
    public string? Phone { get; init; }
    public DateOnly? OpeningDate { get; init; }
    public string Status { get; init; } = default!;

    // Financial Overview
    public BranchFinancialMetrics Financials { get; init; } = new();

    // Member Metrics
    public BranchMemberMetrics Members { get; init; } = new();

    // Loan Portfolio
    public BranchLoanPortfolioMetrics LoanPortfolio { get; init; } = new();

    // Savings Portfolio
    public BranchSavingsPortfolioMetrics SavingsPortfolio { get; init; } = new();

    // Staff Information
    public BranchStaffMetrics Staff { get; init; } = new();

    // Performance Targets
    public BranchTargetAchievement Targets { get; init; } = new();

    // Trend Data for Charts
    public List<BranchTimeSeriesDataPoint> LoanDisbursementTrend { get; init; } = [];
    public List<BranchTimeSeriesDataPoint> SavingsBalanceTrend { get; init; } = [];
    public List<BranchTimeSeriesDataPoint> MemberGrowthTrend { get; init; } = [];
    public List<BranchTimeSeriesDataPoint> PortfolioAtRiskTrend { get; init; } = [];

    // Product Mix
    public List<BranchProductMixData> LoanProductMix { get; init; } = [];
    public List<BranchProductMixData> SavingsProductMix { get; init; } = [];

    // Top Performers
    public List<TopLoanOfficer> TopLoanOfficers { get; init; } = [];
    public List<TopMember> TopMembers { get; init; } = [];

    // Recent Activities
    public List<RecentLoan> RecentLoans { get; init; } = [];
    public List<MicroFinanceRecentTransaction> RecentTransactions { get; init; } = [];

    // Monthly Comparison
    public List<BranchMonthlyComparison> MonthlyPerformance { get; init; } = [];

    // Branch Ranking
    public BranchRanking Ranking { get; init; } = new();
}

public sealed record BranchFinancialMetrics
{
    public decimal TotalLoanPortfolio { get; init; }
    public decimal TotalSavingsBalance { get; init; }
    public decimal TotalShareCapital { get; init; }
    public decimal LoanDisbursementYTD { get; init; }
    public decimal LoanDisbursementLastYear { get; init; }
    public decimal LoanDisbursementGrowth { get; init; }
    public decimal SavingsGrowthYTD { get; init; }
    public decimal InterestIncomeYTD { get; init; }
    public decimal FeeIncomeYTD { get; init; }
    public decimal TotalIncomeYTD { get; init; }
    public decimal CashOnHand { get; init; }
    public decimal CashVaultLimit { get; init; }
    public decimal CashUtilization { get; init; }
}

public sealed record BranchMemberMetrics
{
    public int TotalMembers { get; init; }
    public int ActiveMembers { get; init; }
    public int NewMembersYTD { get; init; }
    public int NewMembersLastYear { get; init; }
    public int MemberGrowthPercent { get; init; }
    public int InactiveMembers { get; init; }
    public int MembersWithLoans { get; init; }
    public int MembersWithSavings { get; init; }
    public decimal AverageSavingsPerMember { get; init; }
    public decimal AverageLoanSizePerMember { get; init; }
}

public sealed record BranchLoanPortfolioMetrics
{
    public int TotalActiveLoans { get; init; }
    public decimal TotalOutstandingPrincipal { get; init; }
    public decimal TotalOutstandingInterest { get; init; }
    public int LoansDisbursedYTD { get; init; }
    public decimal LoansDisbursedAmountYTD { get; init; }
    public int LoansFullyPaid { get; init; }
    public int LoansOverdue { get; init; }
    public decimal OverdueAmount { get; init; }
    public decimal PortfolioAtRisk30 { get; init; }
    public decimal PortfolioAtRisk90 { get; init; }
    public decimal AverageLoanSize { get; init; }
    public decimal LoanWriteOffAmountYTD { get; init; }
    public int LoanApplicationsPending { get; init; }
    public decimal ApprovalRate { get; init; }
    public decimal CollectionEfficiency { get; init; }
}

public sealed record BranchSavingsPortfolioMetrics
{
    public int TotalSavingsAccounts { get; init; }
    public int ActiveSavingsAccounts { get; init; }
    public decimal TotalSavingsBalance { get; init; }
    public decimal SavingsDepositsYTD { get; init; }
    public decimal SavingsWithdrawalsYTD { get; init; }
    public decimal NetSavingsGrowth { get; init; }
    public decimal AverageSavingsBalance { get; init; }
    public int DormantAccounts { get; init; }
    public decimal TotalShareCapital { get; init; }
    public int ShareAccountsActive { get; init; }
}

public sealed record BranchStaffMetrics
{
    public int TotalStaff { get; init; }
    public int ActiveStaff { get; init; }
    public int LoanOfficers { get; init; }
    public int Tellers { get; init; }
    public string? BranchManager { get; init; }
    public decimal AverageLoansPerOfficer { get; init; }
    public decimal AverageMembersPerOfficer { get; init; }
    public decimal StaffProductivity { get; init; }
}

public sealed record BranchTargetAchievement
{
    public decimal LoanDisbursementTarget { get; init; }
    public decimal LoanDisbursementActual { get; init; }
    public decimal LoanDisbursementAchievement { get; init; }
    public decimal SavingsTarget { get; init; }
    public decimal SavingsActual { get; init; }
    public decimal SavingsAchievement { get; init; }
    public int MemberTargetCount { get; init; }
    public int MemberActualCount { get; init; }
    public decimal MemberAchievement { get; init; }
    public decimal OverallTargetAchievement { get; init; }
}

public sealed record BranchTimeSeriesDataPoint
{
    public string Label { get; init; } = default!;
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
}

public sealed record BranchProductMixData
{
    public string ProductName { get; init; } = default!;
    public int AccountCount { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal Percentage { get; init; }
}

public sealed record TopLoanOfficer
{
    public DefaultIdType StaffId { get; init; }
    public string Name { get; init; } = default!;
    public string? EmployeeNumber { get; init; }
    public int LoansManaged { get; init; }
    public int MembersManaged { get; init; }
    public decimal PortfolioAmount { get; init; }
    public decimal PortfolioQuality { get; init; }
    public decimal CollectionRate { get; init; }
}

public sealed record TopMember
{
    public DefaultIdType MemberId { get; init; }
    public string Name { get; init; } = default!;
    public string? MemberNumber { get; init; }
    public decimal LoanOutstanding { get; init; }
    public decimal SavingsBalance { get; init; }
    public decimal ShareCapital { get; init; }
    public int TotalLoans { get; init; }
    public decimal RepaymentScore { get; init; }
}

public sealed record RecentLoan
{
    public DefaultIdType LoanId { get; init; }
    public string? LoanNumber { get; init; }
    public string MemberName { get; init; } = default!;
    public string ProductName { get; init; } = default!;
    public decimal PrincipalAmount { get; init; }
    public DateTime DisbursementDate { get; init; }
    public string Status { get; init; } = default!;
    public string? LoanOfficerName { get; init; }
}

public sealed record MicroFinanceRecentTransaction
{
    public DefaultIdType TransactionId { get; init; }
    public string? TransactionNumber { get; init; }
    public string TransactionType { get; init; } = default!;
    public string MemberName { get; init; } = default!;
    public decimal Amount { get; init; }
    public DateTime TransactionDate { get; init; }
    public string? ProcessedBy { get; init; }
}

public sealed record BranchMonthlyComparison
{
    public string Month { get; init; } = default!;
    public int Year { get; init; }
    public decimal LoanDisbursement { get; init; }
    public decimal SavingsDeposits { get; init; }
    public int NewMembers { get; init; }
    public decimal PortfolioAtRisk { get; init; }
    public decimal CollectionRate { get; init; }
}

public sealed record BranchRanking
{
    public int OverallRank { get; init; }
    public int TotalBranches { get; init; }
    public int LoanDisbursementRank { get; init; }
    public int SavingsRank { get; init; }
    public int MemberGrowthRank { get; init; }
    public int PortfolioQualityRank { get; init; }
    public string PerformanceTier { get; init; } = default!; // "Excellent", "Good", "Average", "Needs Improvement"
}
