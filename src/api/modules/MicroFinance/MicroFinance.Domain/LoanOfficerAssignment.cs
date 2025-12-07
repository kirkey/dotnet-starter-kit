using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents the assignment of a loan officer to members or member groups.
/// Tracks which loans and members are handled by which officer.
/// </summary>
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
