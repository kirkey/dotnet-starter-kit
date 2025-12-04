using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.End.v1;

public sealed class EndAssignmentHandler(
    [FromKeyedServices("microfinance:loanofficerassignments")] IRepository<LoanOfficerAssignment> repository,
    ILogger<EndAssignmentHandler> logger)
    : IRequestHandler<EndAssignmentCommand, EndAssignmentResponse>
{
    public async Task<EndAssignmentResponse> Handle(
        EndAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await repository.FirstOrDefaultAsync(
            new LoanOfficerAssignmentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan officer assignment {request.Id} not found");

        assignment.End(request.EndDate, request.Reason);
        await repository.UpdateAsync(assignment, cancellationToken);

        logger.LogInformation("Assignment ended: {AssignmentId}", assignment.Id);

        return new EndAssignmentResponse(assignment.Id, assignment.Status, assignment.EndDate!.Value);
    }
}
