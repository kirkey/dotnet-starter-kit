namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Dashboard;

/// <summary>
/// Comprehensive member dashboard response containing all financial relationship metrics.
/// </summary>
public sealed record MemberDashboardResponse(
    DefaultIdType MemberId,
    string MemberNumber,
    string MemberName,
    DateOnly MemberSince,
    bool IsActive,
    MemberOverview Overview,
    LoanPortfolioSummary LoanPortfolio,
    SavingsPortfolioSummary SavingsPortfolio,
    SharePortfolioSummary SharePortfolio,
    FixedDepositSummary FixedDeposits,
    FeesSummary Fees,
    RepaymentPerformance RepaymentPerformance,
    List<RecentTransactionItem> RecentTransactions,
    List<UpcomingPayment> UpcomingPayments,
    List<ProductHolding> ProductHoldings,
    CreditIndicators CreditIndicators
);

/// <summary>
/// High-level overview of member's financial standing.
/// </summary>
public sealed record MemberOverview(
    decimal TotalNetWorth,
    decimal TotalAssets,
    decimal TotalLiabilities,
    int TotalProducts,
    int ActiveLoans,
    int ActiveSavingsAccounts,
    int TotalShares,
    int ActiveFixedDeposits
);

/// <summary>
/// Summary of member's loan portfolio.
/// </summary>
public sealed record LoanPortfolioSummary(
    int TotalLoans,
    int ActiveLoans,
    int CompletedLoans,
    decimal TotalBorrowed,
    decimal TotalOutstanding,
    decimal TotalPrincipalOutstanding,
    decimal TotalInterestOutstanding,
    decimal TotalRepaid,
    decimal AverageInterestRate,
    List<LoanDetail> ActiveLoanDetails
);

/// <summary>
/// Individual loan details for active loans.
/// </summary>
public sealed record LoanDetail(
    DefaultIdType LoanId,
    string LoanNumber,
    string ProductName,
    decimal PrincipalAmount,
    decimal OutstandingPrincipal,
    decimal OutstandingInterest,
    decimal TotalOutstanding,
    decimal InterestRate,
    DateOnly DisbursementDate,
    DateOnly ExpectedEndDate,
    int DaysUntilNextPayment,
    int DaysOverdue,
    string Status
);

/// <summary>
/// Summary of member's savings portfolio.
/// </summary>
public sealed record SavingsPortfolioSummary(
    int TotalAccounts,
    int ActiveAccounts,
    int DormantAccounts,
    decimal TotalBalance,
    decimal TotalDeposits,
    decimal TotalWithdrawals,
    decimal TotalInterestEarned,
    decimal AverageBalance,
    List<SavingsAccountDetail> AccountDetails
);

/// <summary>
/// Individual savings account details.
/// </summary>
public sealed record SavingsAccountDetail(
    DefaultIdType AccountId,
    string AccountNumber,
    string ProductName,
    decimal CurrentBalance,
    decimal InterestRate,
    decimal InterestEarned,
    DateOnly OpenedDate,
    DateOnly? LastTransactionDate,
    string Status
);

/// <summary>
/// Summary of member's share portfolio.
/// </summary>
public sealed record SharePortfolioSummary(
    int TotalShareAccounts,
    int TotalShares,
    decimal TotalShareValue,
    decimal TotalDividendsEarned,
    decimal CurrentSharePrice,
    List<ShareAccountDetail> AccountDetails
);

/// <summary>
/// Individual share account details.
/// </summary>
public sealed record ShareAccountDetail(
    DefaultIdType AccountId,
    string AccountNumber,
    int NumberOfShares,
    decimal ShareValue,
    decimal TotalValue,
    DateOnly PurchaseDate,
    string Status
);

/// <summary>
/// Summary of member's fixed deposits.
/// </summary>
public sealed record FixedDepositSummary(
    int TotalDeposits,
    int ActiveDeposits,
    int MaturedDeposits,
    decimal TotalPrincipal,
    decimal TotalInterestEarned,
    decimal TotalMaturityValue,
    List<FixedDepositDetail> DepositDetails
);

/// <summary>
/// Individual fixed deposit details.
/// </summary>
public sealed record FixedDepositDetail(
    DefaultIdType DepositId,
    string CertificateNumber,
    decimal PrincipalAmount,
    decimal InterestRate,
    decimal AccruedInterest,
    decimal MaturityAmount,
    DateOnly StartDate,
    DateOnly MaturityDate,
    int DaysToMaturity,
    string Status
);

/// <summary>
/// Summary of fees charged to member.
/// </summary>
public sealed record FeesSummary(
    decimal TotalFeesCharged,
    decimal TotalFeesPaid,
    decimal TotalFeesOutstanding,
    int TotalFeeTransactions,
    List<FeeBreakdown> FeesByType
);

/// <summary>
/// Fee breakdown by type.
/// </summary>
public sealed record FeeBreakdown(
    string FeeType,
    decimal TotalAmount,
    int Count
);

/// <summary>
/// Member's loan repayment performance metrics.
/// </summary>
public sealed record RepaymentPerformance(
    int TotalPaymentsMade,
    int OnTimePayments,
    int LatePayments,
    decimal OnTimePaymentPercentage,
    decimal TotalPrincipalRepaid,
    decimal TotalInterestRepaid,
    int CurrentDaysOverdue,
    int MaxDaysOverdue,
    List<MonthlyRepaymentSummary> Last12MonthsPerformance
);

/// <summary>
/// Monthly repayment summary.
/// </summary>
public sealed record MonthlyRepaymentSummary(
    int Year,
    int Month,
    decimal ExpectedAmount,
    decimal ActualAmount,
    decimal Variance,
    bool OnTime
);

/// <summary>
/// Recent transaction item.
/// </summary>
public sealed record RecentTransactionItem(
    DefaultIdType TransactionId,
    DateOnly TransactionDate,
    string TransactionType,
    string Description,
    string AccountNumber,
    decimal Amount,
    string Direction
);

/// <summary>
/// Upcoming payment obligation.
/// </summary>
public sealed record UpcomingPayment(
    DateOnly DueDate,
    string LoanNumber,
    decimal PrincipalDue,
    decimal InterestDue,
    decimal TotalDue,
    int DaysUntilDue
);

/// <summary>
/// Product holding summary.
/// </summary>
public sealed record ProductHolding(
    string ProductType,
    string ProductName,
    int Count,
    decimal TotalValue,
    string Status
);

/// <summary>
/// Credit worthiness indicators.
/// </summary>
public sealed record CreditIndicators(
    decimal RepaymentScore,
    string RiskCategory,
    int MembershipTenureDays,
    decimal TotalLifetimeBorrowing,
    decimal TotalLifetimeRepaid,
    int TotalLoansCompleted,
    int TotalDefaultedLoans,
    decimal DebtToIncomeRatio,
    bool IsEligibleForNewLoan,
    decimal MaxEligibleLoanAmount
);
