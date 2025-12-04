using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToGroup.v1;

public sealed class AssignToGroupHandler(
    [FromKeyedServices("microfinance:loanofficerassignments")] IRepository<LoanOfficerAssignment> repository,
    ILogger<AssignToGroupHandler> logger)
    : IRequestHandler<AssignToGroupCommand, AssignToGroupResponse>
{
    public async Task<AssignToGroupResponse> Handle(
        AssignToGroupCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = LoanOfficerAssignment.AssignToGroup(
            request.StaffId,
            request.MemberGroupId,
            request.AssignmentDate,
            request.PreviousStaffId,
            request.Reason);

        await repository.AddAsync(assignment, cancellationToken);

        logger.LogInformation("Loan officer assigned to group: {AssignmentId}", assignment.Id);

        return new AssignToGroupResponse(assignment.Id);
    }
}
