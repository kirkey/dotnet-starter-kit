namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Dashboard;

/// <summary>
/// Comprehensive dashboard response for a staff member's performance and portfolio metrics.
/// </summary>
public sealed record StaffDashboardResponse(
    StaffOverview StaffInfo,
    StaffPortfolioSummary Portfolio,
    StaffPerformanceMetrics Performance,
    StaffTargetProgress Targets,
    StaffRecentActivitySummary RecentActivity,
    IReadOnlyList<AssignedMemberInfo> AssignedMembers,
    IReadOnlyList<AssignedGroupInfo> AssignedGroups,
    StaffAlerts Alerts);

/// <summary>
/// Basic staff information and current status.
/// </summary>
public sealed record StaffOverview(
    Guid StaffId,
    string EmployeeNumber,
    string FullName,
    string Email,
    string? Phone,
    string Role,
    string JobTitle,
    string? Department,
    string Status,
    DateOnly JoiningDate,
    int TenureDays,
    Guid? BranchId,
    string? BranchName,
    string? ReportingTo);

/// <summary>
/// Loan officer portfolio summary.
/// </summary>
public sealed record StaffPortfolioSummary(
    int TotalAssignedMembers,
    int ActiveMembers,
    int TotalAssignedGroups,
    int ActiveLoans,
    int OverdueLoans,
    decimal TotalOutstandingPrincipal,
    decimal TotalOutstandingInterest,
    decimal PortfolioAtRisk30Days,
    int SavingsAccountsManaged,
    decimal TotalSavingsBalance);

/// <summary>
/// Staff performance metrics.
/// </summary>
public sealed record StaffPerformanceMetrics(
    int LoansDisbursedThisMonth,
    decimal AmountDisbursedThisMonth,
    int LoansCollectedThisMonth,
    decimal AmountCollectedThisMonth,
    decimal CollectionRate,
    int NewMembersRegisteredThisMonth,
    int LoanApplicationsProcessedThisMonth,
    decimal AverageProcessingTimeDays);

/// <summary>
/// Target achievement progress.
/// </summary>
public sealed record StaffTargetProgress(
    decimal DisbursementTarget,
    decimal DisbursementActual,
    decimal DisbursementProgress,
    decimal CollectionTarget,
    decimal CollectionActual,
    decimal CollectionProgress,
    int NewMemberTarget,
    int NewMemberActual,
    decimal NewMemberProgress,
    int OverdueRecoveryTarget,
    int OverdueRecoveryActual,
    decimal OverdueRecoveryProgress);

/// <summary>
/// Summary of recent activities.
/// </summary>
public sealed record StaffRecentActivitySummary(
    int LoansApprovedToday,
    int LoansDisbursedToday,
    int RepaymentsReceivedToday,
    decimal TotalCollectedToday,
    int MemberVisitsToday,
    int GroupMeetingsThisWeek);

/// <summary>
/// Information about an assigned member.
/// </summary>
public sealed record AssignedMemberInfo(
    Guid MemberId,
    string MemberNumber,
    string MemberName,
    string Status,
    int ActiveLoans,
    decimal TotalOutstanding,
    bool HasOverdue);

/// <summary>
/// Information about an assigned group.
/// </summary>
public sealed record AssignedGroupInfo(
    Guid GroupId,
    string GroupCode,
    string GroupName,
    string Status,
    int MemberCount,
    int ActiveLoans,
    decimal TotalOutstanding);

/// <summary>
/// Alerts and notifications for the staff member.
/// </summary>
public sealed record StaffAlerts(
    int MembersWithOverdueLoans,
    int LoansNeedingFollowUp,
    int PendingApprovals,
    int UpcomingMeetings,
    IReadOnlyList<StaffAlert> AlertsList);

/// <summary>
/// Individual alert item.
/// </summary>
public sealed record StaffAlert(
    string Severity,
    string Title,
    string Description,
    string? ActionUrl);
