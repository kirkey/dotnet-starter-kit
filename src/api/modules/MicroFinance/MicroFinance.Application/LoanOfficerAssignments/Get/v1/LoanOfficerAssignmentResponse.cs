namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;

public sealed record LoanOfficerAssignmentResponse(
    DefaultIdType Id,
    DefaultIdType StaffId,
    string AssignmentType,
    DefaultIdType? MemberId,
    DefaultIdType? MemberGroupId,
    DefaultIdType? LoanId,
    DefaultIdType? BranchId,
    DateOnly AssignmentDate,
    DateOnly? EndDate,
    DefaultIdType? PreviousStaffId,
    string? Reason,
    bool IsPrimary,
    string Status);
