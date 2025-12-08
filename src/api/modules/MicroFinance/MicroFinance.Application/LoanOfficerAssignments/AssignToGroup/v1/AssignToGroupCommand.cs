using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToGroup.v1;

public sealed record AssignToGroupCommand(
    DefaultIdType StaffId,
    DefaultIdType MemberGroupId,
    DateOnly? AssignmentDate = null,
    DefaultIdType? PreviousStaffId = null,
    string? Reason = null) : IRequest<AssignToGroupResponse>;
