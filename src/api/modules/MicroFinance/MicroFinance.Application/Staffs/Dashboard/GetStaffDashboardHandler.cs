using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific staff member.
/// </summary>
public sealed record GetStaffDashboardQuery(Guid StaffId) : IRequest<StaffDashboardResponse>;

/// <summary>
/// Handler for retrieving staff dashboard data with caching support.
/// </summary>
public sealed class GetStaffDashboardHandler : IRequestHandler<GetStaffDashboardQuery, StaffDashboardResponse>
{
    private readonly IReadRepository<Domain.Staff> _staffRepository;
    private readonly IReadRepository<LoanOfficerAssignment> _assignmentRepository;
    private readonly IReadRepository<Member> _memberRepository;
    private readonly IReadRepository<MemberGroup> _groupRepository;
    private readonly IReadRepository<Loan> _loanRepository;
    private readonly IReadRepository<LoanRepayment> _repaymentRepository;
    private readonly IReadRepository<Branch> _branchRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetStaffDashboardHandler> _logger;

    private const string CacheKeyPrefix = "staff-dashboard:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public GetStaffDashboardHandler(
        IReadRepository<Domain.Staff> staffRepository,
        IReadRepository<LoanOfficerAssignment> assignmentRepository,
        IReadRepository<Member> memberRepository,
        IReadRepository<MemberGroup> groupRepository,
        IReadRepository<Loan> loanRepository,
        IReadRepository<LoanRepayment> repaymentRepository,
        IReadRepository<Branch> branchRepository,
        ICacheService cacheService,
        ILogger<GetStaffDashboardHandler> logger)
    {
        _staffRepository = staffRepository;
        _assignmentRepository = assignmentRepository;
        _memberRepository = memberRepository;
        _groupRepository = groupRepository;
        _loanRepository = loanRepository;
        _repaymentRepository = repaymentRepository;
        _branchRepository = branchRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<StaffDashboardResponse> Handle(GetStaffDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeyPrefix}{request.StaffId}";

        var cachedResponse = await _cacheService.GetAsync<StaffDashboardResponse>(cacheKey, cancellationToken).ConfigureAwait(false);
        if (cachedResponse is not null)
        {
            _logger.LogDebug("Returning cached staff dashboard for {StaffId}", request.StaffId);
            return cachedResponse;
        }

        // Get staff member
        var staff = await _staffRepository.GetByIdAsync(request.StaffId, cancellationToken).ConfigureAwait(false);
        if (staff is null)
        {
            throw new KeyNotFoundException($"Staff member with ID {request.StaffId} not found.");
        }

        // Get branch name
        string? branchName = null;
        if (staff.BranchId.HasValue)
        {
            var branch = await _branchRepository.GetByIdAsync(staff.BranchId.Value, cancellationToken).ConfigureAwait(false);
            branchName = branch?.Name;
        }

        // Get loan officer assignments
        var assignmentsSpec = new LoanOfficerAssignmentsByStaffIdSpec(request.StaffId);
        var assignments = await _assignmentRepository.ListAsync(assignmentsSpec, cancellationToken).ConfigureAwait(false);

        // Get assigned member IDs
        var memberIds = assignments
            .Where(a => a.MemberId.HasValue)
            .Select(a => a.MemberId!.Value)
            .Distinct()
            .ToList();

        // Get assigned group IDs
        var groupIds = assignments
            .Where(a => a.MemberGroupId.HasValue)
            .Select(a => a.MemberGroupId!.Value)
            .Distinct()
            .ToList();

        // Get members
        var membersSpec = new MembersByIdsForStaffSpec(memberIds);
        var members = memberIds.Count > 0
            ? await _memberRepository.ListAsync(membersSpec, cancellationToken).ConfigureAwait(false)
            : new List<Member>();

        // Get groups
        var groupsSpec = new GroupsByIdsForStaffSpec(groupIds);
        var groups = groupIds.Count > 0
            ? await _groupRepository.ListAsync(groupsSpec, cancellationToken).ConfigureAwait(false)
            : new List<MemberGroup>();

        // Get loans for assigned members
        var loansSpec = new LoansByMemberIdsForStaffSpec(memberIds);
        var loans = memberIds.Count > 0
            ? await _loanRepository.ListAsync(loansSpec, cancellationToken).ConfigureAwait(false)
            : new List<Loan>();

        // Get repayments this month
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var monthStart = new DateOnly(today.Year, today.Month, 1);
        var loanIds = loans.Select(l => l.Id).ToList();
        var repaymentsSpec = new LoanRepaymentsForStaffThisMonthSpec(loanIds, monthStart);
        var repayments = loanIds.Count > 0
            ? await _repaymentRepository.ListAsync(repaymentsSpec, cancellationToken).ConfigureAwait(false)
            : new List<LoanRepayment>();

        // Build dashboard response
        var response = BuildDashboardResponse(
            staff,
            branchName,
            members,
            groups,
            loans,
            repayments,
            today,
            monthStart);

        await _cacheService.SetAsync(cacheKey, response, CacheDuration, cancellationToken).ConfigureAwait(false);

        return response;
    }

    private static StaffDashboardResponse BuildDashboardResponse(
        Domain.Staff staff,
        string? branchName,
        IReadOnlyList<Member> members,
        IReadOnlyList<MemberGroup> groups,
        IReadOnlyList<Loan> loans,
        IReadOnlyList<LoanRepayment> repayments,
        DateOnly today,
        DateOnly monthStart)
    {
        // Staff Overview
        var staffInfo = new StaffOverview(
            StaffId: staff.Id,
            EmployeeNumber: staff.EmployeeNumber,
            FullName: staff.FullName,
            Email: staff.Email,
            Phone: staff.Phone,
            Role: staff.Role,
            JobTitle: staff.JobTitle,
            Department: staff.Department,
            Status: staff.Status,
            JoiningDate: staff.JoiningDate,
            TenureDays: today.DayNumber - staff.JoiningDate.DayNumber,
            BranchId: staff.BranchId,
            BranchName: branchName,
            ReportingTo: staff.ReportingTo);

        // Portfolio Summary
        var portfolio = BuildPortfolioSummary(members, groups, loans, today);

        // Performance Metrics
        var performance = BuildPerformanceMetrics(loans, repayments, members, monthStart);

        // Target Progress (mock - would need LoanOfficerTarget entity)
        var targets = BuildTargetProgress(loans, repayments, members, monthStart);

        // Recent Activity Summary
        var recentActivity = BuildRecentActivity(loans, repayments, today);

        // Assigned Members
        var assignedMembers = BuildAssignedMembers(members, loans, today);

        // Assigned Groups
        var assignedGroups = BuildAssignedGroups(groups, loans);

        // Alerts
        var alerts = BuildAlerts(members, loans, today);

        return new StaffDashboardResponse(
            staffInfo,
            portfolio,
            performance,
            targets,
            recentActivity,
            assignedMembers,
            assignedGroups,
            alerts);
    }

    private static StaffPortfolioSummary BuildPortfolioSummary(
        IReadOnlyList<Member> members,
        IReadOnlyList<MemberGroup> groups,
        IReadOnlyList<Loan> loans,
        DateOnly today)
    {
        var activeMembers = members.Count(m => m.IsActive);
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var overdueLoans = activeLoans.Where(l => 
            l.OutstandingPrincipal > 0 && 
            l.ExpectedEndDate.HasValue && 
            l.ExpectedEndDate.Value < today).ToList();

        var totalOutstandingPrincipal = activeLoans.Sum(l => l.OutstandingPrincipal);
        var totalOutstandingInterest = activeLoans.Sum(l => l.OutstandingInterest);

        // PAR > 30 days
        var par30Date = today.AddDays(-30);
        var par30Amount = activeLoans
            .Where(l => l.ExpectedEndDate.HasValue && l.ExpectedEndDate.Value < par30Date)
            .Sum(l => l.OutstandingPrincipal);
        var par30Rate = totalOutstandingPrincipal > 0 ? par30Amount / totalOutstandingPrincipal * 100 : 0;

        return new StaffPortfolioSummary(
            TotalAssignedMembers: members.Count,
            ActiveMembers: activeMembers,
            TotalAssignedGroups: groups.Count,
            ActiveLoans: activeLoans.Count,
            OverdueLoans: overdueLoans.Count,
            TotalOutstandingPrincipal: totalOutstandingPrincipal,
            TotalOutstandingInterest: totalOutstandingInterest,
            PortfolioAtRisk30Days: par30Rate,
            SavingsAccountsManaged: 0, // Would need SavingsAccount data
            TotalSavingsBalance: 0);
    }

    private static StaffPerformanceMetrics BuildPerformanceMetrics(
        IReadOnlyList<Loan> loans,
        IReadOnlyList<LoanRepayment> repayments,
        IReadOnlyList<Member> members,
        DateOnly monthStart)
    {
        var disbursedThisMonth = loans.Where(l => 
            l.DisbursementDate.HasValue && 
            l.DisbursementDate.Value >= monthStart).ToList();

        var repaymentsThisMonth = repayments.Where(r => r.RepaymentDate >= monthStart).ToList();
        var totalCollected = repaymentsThisMonth.Sum(r => r.TotalAmount);
        var totalDisbursed = disbursedThisMonth.Sum(l => l.PrincipalAmount);

        // Collection rate = collected / (disbursed this month + outstanding expected)
        var collectionRate = totalDisbursed > 0 ? (totalCollected / totalDisbursed) * 100 : 0;

        return new StaffPerformanceMetrics(
            LoansDisbursedThisMonth: disbursedThisMonth.Count,
            AmountDisbursedThisMonth: totalDisbursed,
            LoansCollectedThisMonth: repaymentsThisMonth.Select(r => r.LoanId).Distinct().Count(),
            AmountCollectedThisMonth: totalCollected,
            CollectionRate: collectionRate,
            NewMembersRegisteredThisMonth: 0, // Would need member registration date
            LoanApplicationsProcessedThisMonth: loans.Count(l => l.ApplicationDate >= monthStart),
            AverageProcessingTimeDays: 0); // Would need approval date tracking
    }

    private static StaffTargetProgress BuildTargetProgress(
        IReadOnlyList<Loan> loans,
        IReadOnlyList<LoanRepayment> repayments,
        IReadOnlyList<Member> members,
        DateOnly monthStart)
    {
        // Mock targets - in real scenario, would fetch from LoanOfficerTarget entity
        var disbursementTarget = 100000m;
        var collectionTarget = 80000m;
        var newMemberTarget = 10;
        var overdueRecoveryTarget = 5;

        var disbursementActual = loans
            .Where(l => l.DisbursementDate.HasValue && l.DisbursementDate.Value >= monthStart)
            .Sum(l => l.PrincipalAmount);

        var collectionActual = repayments
            .Where(r => r.RepaymentDate >= monthStart)
            .Sum(r => r.TotalAmount);

        return new StaffTargetProgress(
            DisbursementTarget: disbursementTarget,
            DisbursementActual: disbursementActual,
            DisbursementProgress: disbursementTarget > 0 ? (disbursementActual / disbursementTarget) * 100 : 0,
            CollectionTarget: collectionTarget,
            CollectionActual: collectionActual,
            CollectionProgress: collectionTarget > 0 ? (collectionActual / collectionTarget) * 100 : 0,
            NewMemberTarget: newMemberTarget,
            NewMemberActual: 0, // Would need member registration date
            NewMemberProgress: 0,
            OverdueRecoveryTarget: overdueRecoveryTarget,
            OverdueRecoveryActual: 0, // Would need overdue tracking
            OverdueRecoveryProgress: 0);
    }

    private static StaffRecentActivitySummary BuildRecentActivity(
        IReadOnlyList<Loan> loans,
        IReadOnlyList<LoanRepayment> repayments,
        DateOnly today)
    {
        var weekStart = today.AddDays(-(int)today.DayOfWeek);

        return new StaffRecentActivitySummary(
            LoansApprovedToday: loans.Count(l => l.ApprovalDate == today),
            LoansDisbursedToday: loans.Count(l => l.DisbursementDate == today),
            RepaymentsReceivedToday: repayments.Count(r => r.RepaymentDate == today),
            TotalCollectedToday: repayments.Where(r => r.RepaymentDate == today).Sum(r => r.TotalAmount),
            MemberVisitsToday: 0, // Would need visit tracking
            GroupMeetingsThisWeek: 0); // Would need meeting tracking
    }

    private static List<AssignedMemberInfo> BuildAssignedMembers(
        IReadOnlyList<Member> members,
        IReadOnlyList<Loan> loans,
        DateOnly today)
    {
        var memberLoans = loans.GroupBy(l => l.MemberId)
            .ToDictionary(g => g.Key, g => g.ToList());

        return members.Take(10).Select(m =>
        {
            var memberLoanList = memberLoans.TryGetValue(m.Id, out var loanList) ? loanList : new List<Loan>();
            var activeLoans = memberLoanList.Where(l => l.Status == Loan.StatusDisbursed).ToList();
            var totalOutstanding = activeLoans.Sum(l => l.OutstandingPrincipal + l.OutstandingInterest);
            var hasOverdue = activeLoans.Any(l => 
                l.ExpectedEndDate.HasValue && l.ExpectedEndDate.Value < today);

            return new AssignedMemberInfo(
                MemberId: m.Id,
                MemberNumber: m.MemberNumber,
                MemberName: $"{m.FirstName} {m.LastName}",
                Status: m.IsActive ? "Active" : "Inactive",
                ActiveLoans: activeLoans.Count,
                TotalOutstanding: totalOutstanding,
                HasOverdue: hasOverdue);
        }).ToList();
    }

    private static List<AssignedGroupInfo> BuildAssignedGroups(
        IReadOnlyList<MemberGroup> groups,
        IReadOnlyList<Loan> loans)
    {
        return groups.Take(10).Select(g => new AssignedGroupInfo(
            GroupId: g.Id,
            GroupCode: g.Code,
            GroupName: g.Name,
            Status: g.Status,
            MemberCount: 0, // Would need group membership data
            ActiveLoans: 0, // Would need to join with members
            TotalOutstanding: 0)).ToList();
    }

    private static StaffAlerts BuildAlerts(
        IReadOnlyList<Member> members,
        IReadOnlyList<Loan> loans,
        DateOnly today)
    {
        var alertsList = new List<StaffAlert>();

        // Members with overdue loans
        var activeLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).ToList();
        var overdueLoans = activeLoans.Where(l =>
            l.OutstandingPrincipal > 0 &&
            l.ExpectedEndDate.HasValue &&
            l.ExpectedEndDate.Value < today).ToList();
        var membersWithOverdue = overdueLoans.Select(l => l.MemberId).Distinct().Count();

        if (membersWithOverdue > 0)
        {
            alertsList.Add(new StaffAlert(
                Severity: "Error",
                Title: "Overdue Loans",
                Description: $"{membersWithOverdue} member(s) have overdue loan payments",
                ActionUrl: null));
        }

        // Pending loan applications
        var pendingLoans = loans.Count(l => l.Status == Loan.StatusPending);
        if (pendingLoans > 0)
        {
            alertsList.Add(new StaffAlert(
                Severity: "Warning",
                Title: "Pending Applications",
                Description: $"{pendingLoans} loan application(s) awaiting review",
                ActionUrl: null));
        }

        // Loans needing follow-up (approaching due date)
        var weekFromNow = today.AddDays(7);
        var loansNeedingFollowUp = activeLoans.Count(l =>
            l.ExpectedEndDate.HasValue &&
            l.ExpectedEndDate.Value <= weekFromNow &&
            l.ExpectedEndDate.Value >= today);

        if (loansNeedingFollowUp > 0)
        {
            alertsList.Add(new StaffAlert(
                Severity: "Info",
                Title: "Upcoming Due Dates",
                Description: $"{loansNeedingFollowUp} loan(s) due within the next week",
                ActionUrl: null));
        }

        return new StaffAlerts(
            MembersWithOverdueLoans: membersWithOverdue,
            LoansNeedingFollowUp: loansNeedingFollowUp,
            PendingApprovals: pendingLoans,
            UpcomingMeetings: 0,
            AlertsList: alertsList);
    }
}

#region Specifications

/// <summary>
/// Specification to get loan officer assignments by staff ID.
/// </summary>
public sealed class LoanOfficerAssignmentsByStaffIdSpec : Specification<LoanOfficerAssignment>
{
    public LoanOfficerAssignmentsByStaffIdSpec(Guid staffId)
    {
        Query.Where(a => a.StaffId == staffId && a.Status == LoanOfficerAssignment.StatusActive);
    }
}

/// <summary>
/// Specification to get members by their IDs for staff dashboard.
/// </summary>
public sealed class MembersByIdsForStaffSpec : Specification<Member>
{
    public MembersByIdsForStaffSpec(IEnumerable<Guid> memberIds)
    {
        Query.Where(m => memberIds.Contains(m.Id));
    }
}

/// <summary>
/// Specification to get groups by their IDs for staff dashboard.
/// </summary>
public sealed class GroupsByIdsForStaffSpec : Specification<MemberGroup>
{
    public GroupsByIdsForStaffSpec(IEnumerable<Guid> groupIds)
    {
        Query.Where(g => groupIds.Contains(g.Id));
    }
}

/// <summary>
/// Specification to get loans by member IDs for staff dashboard.
/// </summary>
public sealed class LoansByMemberIdsForStaffSpec : Specification<Loan>
{
    public LoansByMemberIdsForStaffSpec(IEnumerable<Guid> memberIds)
    {
        Query.Where(l => memberIds.Contains(l.MemberId));
    }
}

/// <summary>
/// Specification to get loan repayments for specific loans this month.
/// </summary>
public sealed class LoanRepaymentsForStaffThisMonthSpec : Specification<LoanRepayment>
{
    public LoanRepaymentsForStaffThisMonthSpec(IEnumerable<Guid> loanIds, DateOnly monthStart)
    {
        Query.Where(r => loanIds.Contains(r.LoanId) && r.RepaymentDate >= monthStart);
    }
}

#endregion
