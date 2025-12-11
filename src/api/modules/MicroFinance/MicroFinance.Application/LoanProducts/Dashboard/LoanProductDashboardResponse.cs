namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Dashboard;

/// <summary>
/// Comprehensive dashboard response for a loan product showing aggregated metrics
/// across all loans using this product.
/// </summary>
public sealed record LoanProductDashboardResponse(
    DefaultIdType LoanProductId,
    string ProductCode,
    string ProductName,
    bool IsActive,
    LoanProductOverview Overview,
    LoanProductPortfolioMetrics Portfolio,
    LoanProductRepaymentMetrics Repayments,
    LoanProductDelinquencyMetrics Delinquency,
    List<LoanProductLoanDistribution> LoansByStatus,
    List<LoanProductMonthlyTrend> DisbursementTrends,
    List<LoanProductMonthlyTrend> CollectionTrends,
    List<RecentLoanActivity> RecentActivity,
    LoanProductAlerts Alerts
);

/// <summary>
/// High-level overview metrics for the loan product.
/// </summary>
public sealed record LoanProductOverview(
    int TotalLoansIssued,
    int ActiveLoans,
    int PendingLoans,
    int ApprovedLoans,
    int ClosedLoans,
    int RejectedLoans,
    int WrittenOffLoans,
    decimal TotalAmountDisbursed,
    decimal TotalOutstanding,
    decimal AverageInterestRate,
    decimal AverageLoanSize,
    int AverageTermMonths,
    decimal ProductInterestRate,
    decimal MinLoanAmount,
    decimal MaxLoanAmount
);

/// <summary>
/// Portfolio-level metrics for the loan product.
/// </summary>
public sealed record LoanProductPortfolioMetrics(
    decimal TotalPrincipalDisbursed,
    decimal TotalPrincipalOutstanding,
    decimal TotalPrincipalCollected,
    decimal TotalInterestExpected,
    decimal TotalInterestOutstanding,
    decimal TotalInterestCollected,
    decimal MinLoanIssued,
    decimal MaxLoanIssued,
    int ShortestTermMonths,
    int LongestTermMonths,
    int TotalBorrowers
);

/// <summary>
/// Repayment performance metrics for the loan product.
/// </summary>
public sealed record LoanProductRepaymentMetrics(
    decimal TotalCollected,
    decimal TotalPrincipalCollected,
    decimal TotalInterestCollected,
    decimal TotalPenaltiesCollected,
    int TotalRepayments,
    decimal CollectionRate,
    decimal OnTimeRepaymentRate,
    int OnTimeRepayments,
    int LateRepayments,
    decimal AverageRepaymentAmount
);

/// <summary>
/// Delinquency and risk metrics for the loan product.
/// </summary>
public sealed record LoanProductDelinquencyMetrics(
    int OverdueLoans,
    decimal OverdueAmount,
    decimal PortfolioAtRisk1Day,
    decimal PortfolioAtRisk30Days,
    decimal PortfolioAtRisk60Days,
    decimal PortfolioAtRisk90Days,
    int WrittenOffCount,
    decimal WrittenOffAmount,
    decimal WriteOffRate,
    int LoansInArrears,
    decimal ArrearsAmount
);

/// <summary>
/// Distribution of loans by status.
/// </summary>
public sealed record LoanProductLoanDistribution(
    string Status,
    int Count,
    decimal TotalAmount,
    decimal Percentage
);

/// <summary>
/// Monthly trend data for charts.
/// </summary>
public sealed record LoanProductMonthlyTrend(
    string Month,
    int Year,
    decimal Amount,
    int Count
);

/// <summary>
/// Recent loan activity for the product.
/// </summary>
public sealed record RecentLoanActivity(
    DefaultIdType LoanId,
    string LoanNumber,
    string MemberName,
    decimal PrincipalAmount,
    DateOnly? DisbursementDate,
    string Status,
    decimal OutstandingBalance
);

/// <summary>
/// Alerts and notifications for the loan product.
/// </summary>
public sealed record LoanProductAlerts(
    List<LoanProductAlert> Items,
    int TotalAlerts,
    int CriticalCount,
    int WarningCount,
    int InfoCount
);

/// <summary>
/// Individual alert item.
/// </summary>
public sealed record LoanProductAlert(
    string Severity,
    string Title,
    string Description,
    string? ActionUrl
);
