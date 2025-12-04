using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Staff, LoanOfficerAssignment, LoanOfficerTarget, and StaffTraining entities.
/// </summary>
/// 
// Staff Events
public sealed record StaffCreated(Staff Staff) : DomainEvent;
public sealed record StaffUpdated(Staff Staff) : DomainEvent;
public sealed record StaffBranchAssigned(Guid StaffId, Guid? PreviousBranchId, Guid NewBranchId) : DomainEvent;
public sealed record StaffRoleChanged(
    Guid StaffId,
    string PreviousJobTitle,
    string NewJobTitle,
    string PreviousRole,
    string NewRole) : DomainEvent;
public sealed record StaffReportingManagerSet(Guid StaffId, Guid ManagerId, string ManagerName) : DomainEvent;
public sealed record StaffLoanApprovalAuthoritySet(Guid StaffId, bool CanApprove, decimal? ApprovalLimit) : DomainEvent;
public sealed record StaffConfirmed(Guid StaffId, DateOnly ConfirmationDate) : DomainEvent;
public sealed record StaffPlacedOnLeave(Guid StaffId) : DomainEvent;
public sealed record StaffSuspended(Guid StaffId, string? Reason) : DomainEvent;
public sealed record StaffReinstated(Guid StaffId) : DomainEvent;
public sealed record StaffTerminated(Guid StaffId, DateOnly TerminationDate, string? Reason) : DomainEvent;
public sealed record StaffResigned(Guid StaffId, DateOnly LastWorkingDate) : DomainEvent;

// LoanOfficerAssignment Events
public sealed record LoanOfficerAssignmentCreated(LoanOfficerAssignment Assignment) : DomainEvent;
public sealed record LoanOfficerAssignmentTransferred(
    Guid AssignmentId,
    Guid PreviousStaffId,
    Guid NewStaffId) : DomainEvent;
public sealed record LoanOfficerAssignmentEnded(Guid AssignmentId, DateOnly EndDate) : DomainEvent;

// LoanOfficerTarget Events
public sealed record LoanOfficerTargetCreated(LoanOfficerTarget Target) : DomainEvent;
public sealed record LoanOfficerTargetUpdated(LoanOfficerTarget Target) : DomainEvent;
public sealed record LoanOfficerTargetProgressRecorded(
    Guid TargetId,
    decimal PreviousValue,
    decimal NewValue,
    decimal AchievementPercentage) : DomainEvent;
public sealed record LoanOfficerTargetAchieved(Guid TargetId, decimal AchievedValue) : DomainEvent;
public sealed record LoanOfficerTargetMissed(Guid TargetId, decimal AchievedValue, decimal TargetValue) : DomainEvent;

// StaffTraining Events
public sealed record StaffTrainingScheduled(StaffTraining Training) : DomainEvent;
public sealed record StaffTrainingUpdated(StaffTraining Training) : DomainEvent;
public sealed record StaffTrainingStarted(Guid TrainingId) : DomainEvent;
public sealed record StaffTrainingCompleted(Guid TrainingId, decimal? Score, DateOnly CompletionDate) : DomainEvent;
public sealed record StaffTrainingFailed(Guid TrainingId, decimal Score, decimal PassingScore) : DomainEvent;
public sealed record StaffTrainingCertificateIssued(
    Guid TrainingId,
    string CertificationNumber,
    DateOnly? ExpiryDate) : DomainEvent;
public sealed record StaffTrainingCertificationExpired(Guid TrainingId, string CertificationNumber) : DomainEvent;
public sealed record StaffTrainingCancelled(Guid TrainingId, string? Reason) : DomainEvent;
