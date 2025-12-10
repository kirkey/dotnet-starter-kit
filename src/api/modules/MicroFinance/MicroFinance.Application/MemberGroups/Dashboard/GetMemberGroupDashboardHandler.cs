using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific member group.
/// </summary>
public sealed record GetMemberGroupDashboardQuery(Guid GroupId) : IRequest<MemberGroupDashboardResponse>;

/// <summary>
/// Handler for retrieving member group dashboard data with caching support.
/// </summary>
public sealed class GetMemberGroupDashboardHandler : IRequestHandler<GetMemberGroupDashboardQuery, MemberGroupDashboardResponse>
{
    private readonly IReadRepository<MemberGroup> _memberGroupRepository;
    private readonly IReadRepository<GroupMembership> _membershipRepository;
    private readonly IReadRepository<Member> _memberRepository;
    private readonly IReadRepository<Loan> _loanRepository;
    private readonly IReadRepository<SavingsAccount> _savingsAccountRepository;
    private readonly IReadRepository<SavingsTransaction> _savingsTransactionRepository;
    private readonly IReadRepository<LoanRepayment> _loanRepaymentRepository;
    private readonly IReadRepository<Staff> _staffRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetMemberGroupDashboardHandler> _logger;

    private const string CacheKeyPrefix = "member-group-dashboard:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public GetMemberGroupDashboardHandler(
        IReadRepository<MemberGroup> memberGroupRepository,
        IReadRepository<GroupMembership> membershipRepository,
        IReadRepository<Member> memberRepository,
        IReadRepository<Loan> loanRepository,
        IReadRepository<SavingsAccount> savingsAccountRepository,
        IReadRepository<SavingsTransaction> savingsTransactionRepository,
        IReadRepository<LoanRepayment> loanRepaymentRepository,
        IReadRepository<Staff> staffRepository,
        ICacheService cacheService,
        ILogger<GetMemberGroupDashboardHandler> logger)
    {
        _memberGroupRepository = memberGroupRepository;
        _membershipRepository = membershipRepository;
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
        _savingsAccountRepository = savingsAccountRepository;
        _savingsTransactionRepository = savingsTransactionRepository;
        _loanRepaymentRepository = loanRepaymentRepository;
        _staffRepository = staffRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<MemberGroupDashboardResponse> Handle(GetMemberGroupDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeyPrefix}{request.GroupId}";

        var cachedResponse = await _cacheService.GetAsync<MemberGroupDashboardResponse>(cacheKey, cancellationToken).ConfigureAwait(false);
        if (cachedResponse is not null)
        {
            _logger.LogDebug("Returning cached member group dashboard for {GroupId}", request.GroupId);
            return cachedResponse;
        }

        // Get member group
        var group = await _memberGroupRepository.GetByIdAsync(request.GroupId, cancellationToken).ConfigureAwait(false);
        if (group is null)
        {
            throw new KeyNotFoundException($"Member group with ID {request.GroupId} not found.");
        }

        // Get all memberships for this group
        var membershipsSpec = new GroupMembershipsByGroupIdSpec(request.GroupId);
        var memberships = await _membershipRepository.ListAsync(membershipsSpec, cancellationToken).ConfigureAwait(false);

        // Get member IDs from active memberships
        var memberIds = memberships
            .Where(m => m.Status == GroupMembership.StatusActive)
            .Select(m => m.MemberId)
            .Distinct()
            .ToList();

        // Get members
        var membersSpec = new MembersByIdsForGroupSpec(memberIds);
        var members = memberIds.Count > 0
            ? await _memberRepository.ListAsync(membersSpec, cancellationToken).ConfigureAwait(false)
            : new List<Member>();

        // Get leader name
        string? leaderName = null;
        if (group.LeaderMemberId.HasValue)
        {
            var leader = members.FirstOrDefault(m => m.Id == group.LeaderMemberId.Value);
            if (leader is null && group.LeaderMemberId.HasValue)
            {
                leader = await _memberRepository.GetByIdAsync(group.LeaderMemberId.Value, cancellationToken).ConfigureAwait(false);
            }
            leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
        }

        // Get loan officer name
        string? loanOfficerName = null;
        if (group.LoanOfficerId.HasValue)
        {
            var loanOfficer = await _staffRepository.GetByIdAsync(group.LoanOfficerId.Value, cancellationToken).ConfigureAwait(false);
            loanOfficerName = loanOfficer is not null ? $"{loanOfficer.FirstName} {loanOfficer.LastName}" : null;
        }

        // Get loans for group members
        var loansSpec = new LoansByMemberIdsSpec(memberIds);
        var loans = memberIds.Count > 0
            ? await _loanRepository.ListAsync(loansSpec, cancellationToken).ConfigureAwait(false)
            : new List<Loan>();

        // Get savings accounts for group members
        var savingsSpec = new SavingsAccountsByMemberIdsSpec(memberIds);
        var savingsAccounts = memberIds.Count > 0
            ? await _savingsAccountRepository.ListAsync(savingsSpec, cancellationToken).ConfigureAwait(false)
            : new List<SavingsAccount>();

        // Get savings transactions for this month
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var monthStart = new DateOnly(today.Year, today.Month, 1);
        var savingsAccountIds = savingsAccounts.Select(s => s.Id).ToList();
        var savingsTransactionsSpec = new SavingsTransactionsThisMonthSpec(savingsAccountIds, monthStart);
        var savingsTransactions = savingsAccountIds.Count > 0
            ? await _savingsTransactionRepository.ListAsync(savingsTransactionsSpec, cancellationToken).ConfigureAwait(false)
            : new List<SavingsTransaction>();

        // Get loan repayments for this month
        var loanIds = loans.Select(l => l.Id).ToList();
        var repaymentsSpec = new LoanRepaymentsThisMonthSpec(loanIds, monthStart);
        var repayments = loanIds.Count > 0
            ? await _loanRepaymentRepository.ListAsync(repaymentsSpec, cancellationToken).ConfigureAwait(false)
            : new List<LoanRepayment>();

        // Build dashboard response
        var response = BuildDashboardResponse(
            group,
            memberships,
            members,
            loans,
            savingsAccounts,
            savingsTransactions,
            repayments,
            leaderName,
            loanOfficerName,
            today,
            monthStart);

        await _cacheService.SetAsync(cacheKey, response, CacheDuration, cancellationToken).ConfigureAwait(false);

        return response;
    }

    private static MemberGroupDashboardResponse BuildDashboardResponse(
        MemberGroup group,
        IReadOnlyList<GroupMembership> memberships,
        IReadOnlyList<Member> members,
        IReadOnlyList<Loan> loans,
        IReadOnlyList<SavingsAccount> savingsAccounts,
        IReadOnlyList<SavingsTransaction> savingsTransactions,
        IReadOnlyList<LoanRepayment> repayments,
        string? leaderName,
        string? loanOfficerName,
        DateOnly today,
        DateOnly monthStart)
    {
        var memberDict = members.ToDictionary(m => m.Id, m => m);

        // Group Overview
        var groupOverview = new MemberGroupOverview(
            GroupId: group.Id,
            Code: group.Code,
            Name: group.Name,
            Description: group.Description,
            FormationDate: group.FormationDate,
            Status: group.Status,
            GroupAgeInDays: today.DayNumber - group.FormationDate.DayNumber,
            LeaderName: leaderName,
            LeaderMemberId: group.LeaderMemberId,
            LoanOfficerName: loanOfficerName,
            LoanOfficerId: group.LoanOfficerId);

        // Membership Statistics
        var membershipStats = BuildMembershipStatistics(memberships, today, monthStart);

        // Financial Metrics
        var financialMetrics = BuildFinancialMetrics(loans, savingsAccounts, savingsTransactions, repayments, today, monthStart);

        // Meeting Information
        var meetingInfo = BuildMeetingInformation(group, today);

        // Activity Summary
        var activitySummary = BuildActivitySummary(loans, repayments, memberships, monthStart);

        // Recent Member Activities
        var recentActivities = BuildRecentMemberActivities(members, loans, savingsTransactions, repayments, today);

        // Alerts
        var alerts = BuildAlerts(memberships, loans, savingsAccounts, meetingInfo, today);

        return new MemberGroupDashboardResponse(
            groupOverview,
            membershipStats,
            financialMetrics,
            meetingInfo,
            activitySummary,
            recentActivities,
            alerts);
    }

    private static MembershipStatistics BuildMembershipStatistics(
        IReadOnlyList<GroupMembership> memberships,
        DateOnly today,
        DateOnly monthStart)
    {
        var activeMemberships = memberships.Where(m => m.Status == GroupMembership.StatusActive).ToList();
        var inactiveMemberships = memberships.Where(m => m.Status == GroupMembership.StatusInactive).ToList();
        var suspendedMemberships = memberships.Where(m => m.Status == GroupMembership.StatusSuspended).ToList();
        var withdrawnMemberships = memberships.Where(m => m.Status == GroupMembership.StatusWithdrawn).ToList();

        var avgDuration = activeMemberships.Count > 0
            ? activeMemberships.Average(m => today.DayNumber - m.JoinDate.DayNumber)
            : 0;

        var newThisMonth = memberships.Count(m => m.JoinDate >= monthStart);
        var leftThisMonth = memberships.Count(m => m.LeaveDate.HasValue && m.LeaveDate.Value >= monthStart);

        return new MembershipStatistics(
            TotalMembers: memberships.Count,
            ActiveMembers: activeMemberships.Count,
            InactiveMembers: inactiveMemberships.Count,
            SuspendedMembers: suspendedMemberships.Count,
            WithdrawnMembers: withdrawnMemberships.Count,
            Leaders: memberships.Count(m => m.Role == GroupMembership.RoleLeader && m.Status == GroupMembership.StatusActive),
            Secretaries: memberships.Count(m => m.Role == GroupMembership.RoleSecretary && m.Status == GroupMembership.StatusActive),
            Treasurers: memberships.Count(m => m.Role == GroupMembership.RoleTreasurer && m.Status == GroupMembership.StatusActive),
            RegularMembers: memberships.Count(m => m.Role == GroupMembership.RoleMember && m.Status == GroupMembership.StatusActive),
            AverageMembershipDurationDays: avgDuration,
            NewMembersThisMonth: newThisMonth,
            MembersLeftThisMonth: leftThisMonth);
    }

    private static GroupFinancialMetrics BuildFinancialMetrics(
        IReadOnlyList<Loan> loans,
        IReadOnlyList<SavingsAccount> savingsAccounts,
        IReadOnlyList<SavingsTransaction> savingsTransactions,
        IReadOnlyList<LoanRepayment> repayments,
        DateOnly today,
        DateOnly monthStart)
    {
        // Loan Portfolio
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        // Loans are overdue if they have outstanding balance and expected end date has passed
        var overdueLoans = activeLoans.Where(l => l.OutstandingPrincipal > 0 && l.ExpectedEndDate.HasValue && l.ExpectedEndDate.Value < today).ToList();
        var closedLoans = loans.Where(l => l.Status == Loan.StatusClosed).ToList();

        var totalOutstandingPrincipal = activeLoans.Sum(l => l.OutstandingPrincipal);
        var totalOutstandingInterest = activeLoans.Sum(l => l.OutstandingInterest);

        // PAR > 30 days - loans past expected end date by more than 30 days
        var par30Date = today.AddDays(-30);
        var par30Amount = activeLoans
            .Where(l => l.ExpectedEndDate.HasValue && l.ExpectedEndDate.Value < par30Date)
            .Sum(l => l.OutstandingPrincipal);
        var par30Rate = totalOutstandingPrincipal > 0 ? par30Amount / totalOutstandingPrincipal * 100 : 0;

        var avgLoanSize = activeLoans.Count > 0 ? activeLoans.Average(l => l.PrincipalAmount) : 0;

        var loanPortfolio = new LoanPortfolioSummary(
            TotalLoans: loans.Count,
            ActiveLoans: activeLoans.Count,
            OverdueLoans: overdueLoans.Count,
            ClosedLoans: closedLoans.Count,
            TotalOutstandingPrincipal: totalOutstandingPrincipal,
            TotalOutstandingInterest: totalOutstandingInterest,
            PortfolioAtRisk30Days: par30Rate,
            AverageLoanSize: avgLoanSize);

        // Savings Portfolio
        var activeSavings = savingsAccounts.Where(s => s.Status == SavingsAccount.StatusActive).ToList();
        var totalSavingsBalance = activeSavings.Sum(s => s.Balance);
        var avgSavingsBalance = activeSavings.Count > 0 ? activeSavings.Average(s => s.Balance) : 0;

        var depositsThisMonth = savingsTransactions
            .Where(t => t.TransactionType == SavingsTransaction.TypeDeposit && t.TransactionDate >= monthStart)
            .Sum(t => t.Amount);
        var withdrawalsThisMonth = savingsTransactions
            .Where(t => t.TransactionType == SavingsTransaction.TypeWithdrawal && t.TransactionDate >= monthStart)
            .Sum(t => t.Amount);

        var savingsPortfolio = new SavingsPortfolioSummary(
            TotalSavingsAccounts: savingsAccounts.Count,
            ActiveSavingsAccounts: activeSavings.Count,
            TotalSavingsBalance: totalSavingsBalance,
            AverageSavingsBalance: avgSavingsBalance,
            TotalDepositsThisMonth: depositsThisMonth,
            TotalWithdrawalsThisMonth: withdrawalsThisMonth);

        // Total disbursed and repaid
        var totalDisbursed = loans.Where(l => l.Status != Loan.StatusPending && l.Status != Loan.StatusRejected).Sum(l => l.PrincipalAmount);
        var totalRepaid = repayments.Sum(r => r.TotalAmount);
        var repaymentRate = totalDisbursed > 0 ? totalRepaid / totalDisbursed * 100 : 0;

        return new GroupFinancialMetrics(
            LoanPortfolio: loanPortfolio,
            SavingsPortfolio: savingsPortfolio,
            TotalDisbursedToGroup: totalDisbursed,
            TotalRepaidByGroup: totalRepaid,
            GroupRepaymentRate: repaymentRate);
    }

    private static MeetingInformation BuildMeetingInformation(MemberGroup group, DateOnly today)
    {
        // Calculate next meeting date based on meeting day and frequency
        DateOnly? nextMeetingDate = null;
        if (!string.IsNullOrEmpty(group.MeetingDay))
        {
            if (Enum.TryParse<DayOfWeek>(group.MeetingDay, true, out var meetingDayOfWeek))
            {
                var daysUntilMeeting = ((int)meetingDayOfWeek - (int)today.DayOfWeek + 7) % 7;
                if (daysUntilMeeting == 0) daysUntilMeeting = 7; // Next week if today is the meeting day
                nextMeetingDate = today.AddDays(daysUntilMeeting);
            }
        }

        return new MeetingInformation(
            MeetingLocation: group.MeetingLocation,
            MeetingFrequency: group.MeetingFrequency,
            MeetingDay: group.MeetingDay,
            MeetingTime: group.MeetingTime,
            NextMeetingDate: nextMeetingDate,
            TotalMeetingsHeld: 0, // Would need meeting attendance tracking
            AverageAttendanceRate: 0); // Would need meeting attendance tracking
    }

    private static GroupActivitySummary BuildActivitySummary(
        IReadOnlyList<Loan> loans,
        IReadOnlyList<LoanRepayment> repayments,
        IReadOnlyList<GroupMembership> memberships,
        DateOnly monthStart)
    {
        var loanAppsThisMonth = loans.Count(l => l.ApplicationDate >= monthStart);
        var disbursementsThisMonth = loans.Count(l => l.DisbursementDate.HasValue && l.DisbursementDate.Value >= monthStart);
        var repaymentsThisMonth = repayments.Count(r => r.RepaymentDate >= monthStart);
        var totalCollected = repayments.Where(r => r.RepaymentDate >= monthStart).Sum(r => r.TotalAmount);

        var newJoins = memberships.Count(m => m.JoinDate >= monthStart);
        var exits = memberships.Count(m => m.LeaveDate.HasValue && m.LeaveDate.Value >= monthStart);

        return new GroupActivitySummary(
            LoanApplicationsThisMonth: loanAppsThisMonth,
            LoanDisbursementsThisMonth: disbursementsThisMonth,
            LoanRepaymentsThisMonth: repaymentsThisMonth,
            TotalCollectedThisMonth: totalCollected,
            NewMemberJoinsThisMonth: newJoins,
            MemberExitsThisMonth: exits);
    }

    private static List<RecentMemberActivity> BuildRecentMemberActivities(
        IReadOnlyList<Member> members,
        IReadOnlyList<Loan> loans,
        IReadOnlyList<SavingsTransaction> savingsTransactions,
        IReadOnlyList<LoanRepayment> repayments,
        DateOnly today)
    {
        var activities = new List<RecentMemberActivity>();
        var memberDict = members.ToDictionary(m => m.Id, m => m);
        var loanMemberDict = loans.ToDictionary(l => l.Id, l => l.MemberId);

        // Recent loan repayments
        foreach (var repayment in repayments.OrderByDescending(r => r.RepaymentDate).Take(5))
        {
            if (loanMemberDict.TryGetValue(repayment.LoanId, out var memberId) &&
                memberDict.TryGetValue(memberId, out var member))
            {
                activities.Add(new RecentMemberActivity(
                    MemberId: member.Id,
                    MemberName: $"{member.FirstName} {member.LastName}",
                    MemberCode: member.MemberNumber,
                    ActivityType: "Loan Repayment",
                    ActivityDescription: $"Made loan repayment",
                    Amount: repayment.TotalAmount,
                    ActivityDate: repayment.RepaymentDate));
            }
        }

        // Recent savings transactions
        foreach (var transaction in savingsTransactions.OrderByDescending(t => t.TransactionDate).Take(5))
        {
            activities.Add(new RecentMemberActivity(
                MemberId: Guid.Empty, // Would need to join with SavingsAccount
                MemberName: "Member",
                MemberCode: transaction.Reference ?? "",
                ActivityType: transaction.TransactionType,
                ActivityDescription: transaction.Description ?? $"{transaction.TransactionType} transaction",
                Amount: transaction.Amount,
                ActivityDate: transaction.TransactionDate));
        }

        return activities
            .OrderByDescending(a => a.ActivityDate)
            .Take(10)
            .ToList();
    }

    private static MemberGroupAlerts BuildAlerts(
        IReadOnlyList<GroupMembership> memberships,
        IReadOnlyList<Loan> loans,
        IReadOnlyList<SavingsAccount> savingsAccounts,
        MeetingInformation meetingInfo,
        DateOnly today)
    {
        var alertsList = new List<MemberGroupAlert>();

        // Members with overdue loans (past expected end date)
        var overdueLoans = loans.Where(l =>
            l.Status == Loan.StatusDisbursed &&
            l.OutstandingPrincipal > 0 &&
            l.ExpectedEndDate.HasValue &&
            l.ExpectedEndDate.Value < today).ToList();
        var membersWithOverdue = overdueLoans.Select(l => l.MemberId).Distinct().Count();

        if (membersWithOverdue > 0)
        {
            alertsList.Add(new MemberGroupAlert(
                Severity: "Error",
                Title: "Overdue Loans",
                Description: $"{membersWithOverdue} member(s) have overdue loan payments",
                ActionUrl: null));
        }

        // Dormant savings accounts (no activity in 6 months)
        var sixMonthsAgo = today.AddMonths(-6);
        var dormantSavings = savingsAccounts.Count(s =>
            s.Status == SavingsAccount.StatusDormant ||
            (s.LastInterestPostingDate.HasValue && s.LastInterestPostingDate.Value < sixMonthsAgo));

        if (dormantSavings > 0)
        {
            alertsList.Add(new MemberGroupAlert(
                Severity: "Warning",
                Title: "Dormant Savings Accounts",
                Description: $"{dormantSavings} savings account(s) show no recent activity",
                ActionUrl: null));
        }

        // Inactive memberships
        var inactiveMemberships = memberships.Count(m =>
            m.Status == GroupMembership.StatusInactive ||
            m.Status == GroupMembership.StatusSuspended);

        if (inactiveMemberships > 0)
        {
            alertsList.Add(new MemberGroupAlert(
                Severity: "Info",
                Title: "Inactive Members",
                Description: $"{inactiveMemberships} member(s) are inactive or suspended",
                ActionUrl: null));
        }

        // Pending loan applications
        var pendingLoans = loans.Count(l => l.Status == Loan.StatusPending);
        if (pendingLoans > 0)
        {
            alertsList.Add(new MemberGroupAlert(
                Severity: "Info",
                Title: "Pending Loan Applications",
                Description: $"{pendingLoans} loan application(s) awaiting review",
                ActionUrl: null));
        }

        // Upcoming meeting
        var hasUpcomingMeeting = meetingInfo.NextMeetingDate.HasValue &&
                                  meetingInfo.NextMeetingDate.Value <= today.AddDays(7);

        if (hasUpcomingMeeting)
        {
            alertsList.Add(new MemberGroupAlert(
                Severity: "Info",
                Title: "Upcoming Meeting",
                Description: $"Next meeting scheduled for {meetingInfo.NextMeetingDate:MMM dd, yyyy}",
                ActionUrl: null));
        }

        return new MemberGroupAlerts(
            MembersWithOverdueLoans: membersWithOverdue,
            MembersWithDormantSavings: dormantSavings,
            InactiveMemberships: inactiveMemberships,
            PendingLoanApplications: pendingLoans,
            HasUpcomingMeeting: hasUpcomingMeeting,
            AlertsList: alertsList);
    }
}

#region Specifications

/// <summary>
/// Specification to get group memberships by group ID.
/// </summary>
public sealed class GroupMembershipsByGroupIdSpec : Specification<GroupMembership>
{
    public GroupMembershipsByGroupIdSpec(Guid groupId)
    {
        Query.Where(m => m.GroupId == groupId);
    }
}

/// <summary>
/// Specification to get members by their IDs.
/// </summary>
public sealed class MembersByIdsForGroupSpec : Specification<Member>
{
    public MembersByIdsForGroupSpec(IEnumerable<Guid> memberIds)
    {
        Query.Where(m => memberIds.Contains(m.Id));
    }
}

/// <summary>
/// Specification to get loans by member IDs.
/// </summary>
public sealed class LoansByMemberIdsSpec : Specification<Loan>
{
    public LoansByMemberIdsSpec(IEnumerable<Guid> memberIds)
    {
        Query.Where(l => memberIds.Contains(l.MemberId));
    }
}

/// <summary>
/// Specification to get savings accounts by member IDs.
/// </summary>
public sealed class SavingsAccountsByMemberIdsSpec : Specification<SavingsAccount>
{
    public SavingsAccountsByMemberIdsSpec(IEnumerable<Guid> memberIds)
    {
        Query.Where(s => memberIds.Contains(s.MemberId));
    }
}

/// <summary>
/// Specification to get savings transactions for specific accounts this month.
/// </summary>
public sealed class SavingsTransactionsThisMonthSpec : Specification<SavingsTransaction>
{
    public SavingsTransactionsThisMonthSpec(IEnumerable<Guid> accountIds, DateOnly monthStart)
    {
        Query.Where(t => accountIds.Contains(t.SavingsAccountId) && t.TransactionDate >= monthStart);
    }
}

/// <summary>
/// Specification to get loan repayments for specific loans this month.
/// </summary>
public sealed class LoanRepaymentsThisMonthSpec : Specification<LoanRepayment>
{
    public LoanRepaymentsThisMonthSpec(IEnumerable<Guid> loanIds, DateOnly monthStart)
    {
        Query.Where(r => loanIds.Contains(r.LoanId) && r.RepaymentDate >= monthStart);
    }
}

#endregion
