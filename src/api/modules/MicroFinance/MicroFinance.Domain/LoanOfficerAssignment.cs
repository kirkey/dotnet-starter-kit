using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents the assignment of a loan officer to members, groups, loans, or geographic areas.
/// Enables portfolio management and performance tracking by officer.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Assign loan officers to manage specific members</description></item>
///   <item><description>Assign officers to entire groups for group lending</description></item>
///   <item><description>Track assignment history when officers change</description></item>
///   <item><description>Support workload balancing across officers</description></item>
///   <item><description>Enable portfolio quality reporting by officer</description></item>
///   <item><description>Route loan applications to assigned officers</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Loan officer assignments are critical for microfinance operations:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Relationship Management</strong>: Officers build trust with assigned members</description></item>
///   <item><description><strong>Accountability</strong>: Clear ownership for portfolio performance</description></item>
///   <item><description><strong>Collections</strong>: Officers responsible for repayment follow-up</description></item>
///   <item><description><strong>Field Visits</strong>: Officers visit assigned members/groups regularly</description></item>
/// </list>
/// <para><strong>Assignment Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Member</strong>: Individual member assignment</description></item>
///   <item><description><strong>Group</strong>: Entire solidarity group</description></item>
///   <item><description><strong>Loan</strong>: Specific loan (may differ from member assignment)</description></item>
///   <item><description><strong>Area</strong>: Geographic coverage area</description></item>
///   <item><description><strong>Branch</strong>: Entire branch oversight</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Staff"/> - Assigned loan officer</description></item>
///   <item><description><see cref="Member"/> - Assigned member (when type=Member)</description></item>
///   <item><description><see cref="MemberGroup"/> - Assigned group (when type=Group)</description></item>
///   <item><description><see cref="Loan"/> - Assigned loan (when type=Loan)</description></item>
///   <item><description><see cref="LoanOfficerTarget"/> - Performance targets for officer</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Assigning a loan officer to a group</strong></para>
/// <code>
/// POST /api/microfinance/loan-officer-assignments
/// {
///   "loanOfficerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "assignmentType": "Group",
///   "memberGroupId": "a1b2c3d4-5e6f-7890-abcd-ef1234567890",
///   "effectiveDate": "2024-01-15",
///   "reason": "New group formation, assigned to senior officer"
/// }
/// </code>
/// </example>
public sealed class LoanOfficerAssignment : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int AssignmentType = 32;
        public const int Reason = 512;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Assignment type classification.
    /// </summary>
    public const string TypeMember = "Member";
    public const string TypeGroup = "Group";
    public const string TypeLoan = "Loan";
    public const string TypeArea = "Area";
    public const string TypeBranch = "Branch";

    /// <summary>
    /// Assignment status.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusTransferred = "Transferred";
    public const string StatusEnded = "Ended";

    /// <summary>
    /// Reference to the staff member (loan officer).
    /// </summary>
    public Guid StaffId { get; private set; }

    /// <summary>
    /// Type of assignment.
    /// </summary>
    public string AssignmentType { get; private set; } = TypeMember;

    /// <summary>
    /// Reference to the member (if member assignment).
    /// </summary>
    public Guid? MemberId { get; private set; }

    /// <summary>
    /// Reference to the member group (if group assignment).
    /// </summary>
    public Guid? MemberGroupId { get; private set; }

    /// <summary>
    /// Reference to a specific loan (if loan assignment).
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>
    /// Reference to a branch (if area/branch assignment).
    /// </summary>
    public Guid? BranchId { get; private set; }

    /// <summary>
    /// Start date of the assignment.
    /// </summary>
    public DateOnly AssignmentDate { get; private set; }

    /// <summary>
    /// End date of the assignment (if ended or transferred).
    /// </summary>
    public DateOnly? EndDate { get; private set; }

    /// <summary>
    /// Previous loan officer (if this is a transfer).
    /// </summary>
    public Guid? PreviousStaffId { get; private set; }

    /// <summary>
    /// Reason for the assignment or transfer.
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Whether this is the primary assignment.
    /// </summary>
    public bool IsPrimary { get; private set; } = true;

    /// <summary>
    /// Current status of the assignment.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Additional notes.

    // Navigation properties
    public Staff Staff { get; private set; } = null!;
    public Member? Member { get; private set; }
    public MemberGroup? MemberGroup { get; private set; }
    public Loan? Loan { get; private set; }
    public Branch? Branch { get; private set; }
    public Staff? PreviousStaff { get; private set; }

    private LoanOfficerAssignment() { }

    /// <summary>
    /// Creates a new loan officer assignment for a member.
    /// </summary>
    public static LoanOfficerAssignment AssignToMember(
        Guid staffId,
        Guid memberId,
        DateOnly? assignmentDate = null,
        Guid? previousStaffId = null,
        string? reason = null)
    {
        var assignment = new LoanOfficerAssignment
        {
            StaffId = staffId,
            MemberId = memberId,
            AssignmentType = TypeMember,
            AssignmentDate = assignmentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            PreviousStaffId = previousStaffId,
            Reason = reason,
            IsPrimary = true,
            Status = StatusActive
        };

        assignment.QueueDomainEvent(new LoanOfficerAssignmentCreated(assignment));
        return assignment;
    }

    /// <summary>
    /// Creates a new loan officer assignment for a group.
    /// </summary>
    public static LoanOfficerAssignment AssignToGroup(
        Guid staffId,
        Guid memberGroupId,
        DateOnly? assignmentDate = null,
        Guid? previousStaffId = null,
        string? reason = null)
    {
        var assignment = new LoanOfficerAssignment
        {
            StaffId = staffId,
            MemberGroupId = memberGroupId,
            AssignmentType = TypeGroup,
            AssignmentDate = assignmentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            PreviousStaffId = previousStaffId,
            Reason = reason,
            IsPrimary = true,
            Status = StatusActive
        };

        assignment.QueueDomainEvent(new LoanOfficerAssignmentCreated(assignment));
        return assignment;
    }

    /// <summary>
    /// Creates a new loan officer assignment for a specific loan.
    /// </summary>
    public static LoanOfficerAssignment AssignToLoan(
        Guid staffId,
        Guid loanId,
        DateOnly? assignmentDate = null,
        Guid? previousStaffId = null,
        string? reason = null)
    {
        var assignment = new LoanOfficerAssignment
        {
            StaffId = staffId,
            LoanId = loanId,
            AssignmentType = TypeLoan,
            AssignmentDate = assignmentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            PreviousStaffId = previousStaffId,
            Reason = reason,
            IsPrimary = true,
            Status = StatusActive
        };

        assignment.QueueDomainEvent(new LoanOfficerAssignmentCreated(assignment));
        return assignment;
    }

    /// <summary>
    /// Transfers the assignment to another loan officer.
    /// </summary>
    public LoanOfficerAssignment Transfer(Guid newStaffId, string? reason = null)
    {
        // End this assignment
        EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusTransferred;
        QueueDomainEvent(new LoanOfficerAssignmentTransferred(Id, StaffId, newStaffId));

        // Create new assignment
        var newAssignment = new LoanOfficerAssignment
        {
            StaffId = newStaffId,
            MemberId = MemberId,
            MemberGroupId = MemberGroupId,
            LoanId = LoanId,
            BranchId = BranchId,
            AssignmentType = AssignmentType,
            AssignmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            PreviousStaffId = StaffId,
            Reason = reason,
            IsPrimary = IsPrimary,
            Status = StatusActive
        };

        newAssignment.QueueDomainEvent(new LoanOfficerAssignmentCreated(newAssignment));
        return newAssignment;
    }

    /// <summary>
    /// Ends the assignment.
    /// </summary>
    public void End(DateOnly? endDate = null, string? reason = null)
    {
        EndDate = endDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusEnded;
        if (reason is not null) Reason = reason;
        QueueDomainEvent(new LoanOfficerAssignmentEnded(Id, EndDate.Value));
    }

    /// <summary>
    /// Marks as secondary assignment.
    /// </summary>
    public void SetAsSecondary()
    {
        IsPrimary = false;
    }
}
