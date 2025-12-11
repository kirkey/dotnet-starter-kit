namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dashboard;

/// <summary>
/// Comprehensive dashboard response for a member group's operational metrics.
/// </summary>
public sealed record MemberGroupDashboardResponse(
    MemberGroupOverview GroupOverview,
    MemberGroupMembershipStatistics MembershipStats,
    GroupFinancialMetrics FinancialMetrics,
    MemberGroupMeetingInformation MeetingInfo,
    MemberGroupActivitySummary ActivitySummary,
    IReadOnlyList<RecentMemberActivity> RecentMemberActivities,
    MemberGroupAlerts Alerts);

/// <summary>
/// Basic group information and current status.
/// </summary>
public sealed record MemberGroupOverview(
    Guid GroupId,
    string Code,
    string Name,
    string? Description,
    DateOnly FormationDate,
    string Status,
    int GroupAgeInDays,
    string? LeaderName,
    Guid? LeaderMemberId,
    string? LoanOfficerName,
    Guid? LoanOfficerId);

/// <summary>
/// Statistics about group membership composition.
/// </summary>
public sealed record MemberGroupMembershipStatistics(
    int TotalMembers,
    int ActiveMembers,
    int InactiveMembers,
    int SuspendedMembers,
    int WithdrawnMembers,
    int Leaders,
    int Secretaries,
    int Treasurers,
    int RegularMembers,
    double AverageMembershipDurationDays,
    int NewMembersThisMonth,
    int MembersLeftThisMonth);

/// <summary>
/// Financial metrics aggregated from group members.
/// </summary>
public sealed record GroupFinancialMetrics(
    MemberGroupLoanPortfolioSummary LoanPortfolio,
    MemberGroupSavingsPortfolioSummary SavingsPortfolio,
    decimal TotalDisbursedToGroup,
    decimal TotalRepaidByGroup,
    decimal GroupRepaymentRate);

/// <summary>
/// Summary of loans held by group members.
/// </summary>
public sealed record MemberGroupLoanPortfolioSummary(
    int TotalLoans,
    int ActiveLoans,
    int OverdueLoans,
    int ClosedLoans,
    decimal TotalOutstandingPrincipal,
    decimal TotalOutstandingInterest,
    decimal PortfolioAtRisk30Days,
    decimal AverageLoanSize);

/// <summary>
/// Summary of savings held by group members.
/// </summary>
public sealed record MemberGroupSavingsPortfolioSummary(
    int TotalSavingsAccounts,
    int ActiveSavingsAccounts,
    decimal TotalSavingsBalance,
    decimal AverageSavingsBalance,
    decimal TotalDepositsThisMonth,
    decimal TotalWithdrawalsThisMonth);

/// <summary>
/// Meeting schedule and attendance information.
/// </summary>
public sealed record MemberGroupMeetingInformation(
    string? MeetingLocation,
    string? MeetingFrequency,
    string? MeetingDay,
    TimeOnly? MeetingTime,
    DateOnly? NextMeetingDate,
    int TotalMeetingsHeld,
    double AverageAttendanceRate);

/// <summary>
/// Summary of recent group activities.
/// </summary>
public sealed record MemberGroupActivitySummary(
    int LoanApplicationsThisMonth,
    int LoanDisbursementsThisMonth,
    int LoanRepaymentsThisMonth,
    decimal TotalCollectedThisMonth,
    int NewMemberJoinsThisMonth,
    int MemberExitsThisMonth);

/// <summary>
/// Recent activity by a group member.
/// </summary>
public sealed record RecentMemberActivity(
    Guid MemberId,
    string MemberName,
    string MemberCode,
    string ActivityType,
    string ActivityDescription,
    decimal? Amount,
    DateOnly ActivityDate);

/// <summary>
/// Alerts and notifications for the group.
/// </summary>
public sealed record MemberGroupAlerts(
    int MembersWithOverdueLoans,
    int MembersWithDormantSavings,
    int InactiveMemberships,
    int PendingLoanApplications,
    bool HasUpcomingMeeting,
    IReadOnlyList<MemberGroupAlert> AlertsList);

/// <summary>
/// Individual alert item.
/// </summary>
public sealed record MemberGroupAlert(
    string Severity,
    string Title,
    string Description,
    string? ActionUrl);
