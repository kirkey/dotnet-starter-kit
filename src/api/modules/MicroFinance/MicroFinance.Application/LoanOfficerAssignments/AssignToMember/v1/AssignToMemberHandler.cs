using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToMember.v1;

public sealed class AssignToMemberHandler(
    [FromKeyedServices("microfinance:loanofficerassignments")] IRepository<LoanOfficerAssignment> repository,
    ILogger<AssignToMemberHandler> logger)
    : IRequestHandler<AssignToMemberCommand, AssignToMemberResponse>
{
    public async Task<AssignToMemberResponse> Handle(
        AssignToMemberCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = LoanOfficerAssignment.AssignToMember(
            request.StaffId,
            request.MemberId,
            request.AssignmentDate,
            request.PreviousStaffId,
            request.Reason);

        await repository.AddAsync(assignment, cancellationToken);

        logger.LogInformation("Loan officer assigned to member: {AssignmentId}", assignment.Id);

        return new AssignToMemberResponse(assignment.Id);
    }
}
