using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToMember.v1;

public sealed record AssignToMemberCommand(
    DefaultIdType StaffId,
    DefaultIdType MemberId,
    DateOnly? AssignmentDate = null,
    DefaultIdType? PreviousStaffId = null,
    string? Reason = null) : IRequest<AssignToMemberResponse>;
