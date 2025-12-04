using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToMember.v1;

public sealed record AssignToMemberCommand(
    Guid StaffId,
    Guid MemberId,
    DateOnly? AssignmentDate = null,
    Guid? PreviousStaffId = null,
    string? Reason = null) : IRequest<AssignToMemberResponse>;
