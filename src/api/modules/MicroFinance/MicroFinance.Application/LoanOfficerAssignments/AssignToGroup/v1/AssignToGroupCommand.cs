using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToGroup.v1;

public sealed record AssignToGroupCommand(
    Guid StaffId,
    Guid MemberGroupId,
    DateOnly? AssignmentDate = null,
    Guid? PreviousStaffId = null,
    string? Reason = null) : IRequest<AssignToGroupResponse>;
