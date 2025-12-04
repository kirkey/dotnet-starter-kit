namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;

public sealed record LoanOfficerAssignmentResponse(
    Guid Id,
    Guid StaffId,
    string AssignmentType,
    Guid? MemberId,
    Guid? MemberGroupId,
    Guid? LoanId,
    Guid? BranchId,
    DateOnly AssignmentDate,
    DateOnly? EndDate,
    Guid? PreviousStaffId,
    string? Reason,
    bool IsPrimary,
    string Status);
