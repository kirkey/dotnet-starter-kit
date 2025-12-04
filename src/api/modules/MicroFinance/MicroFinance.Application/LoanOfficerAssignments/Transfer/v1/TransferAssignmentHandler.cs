using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Transfer.v1;

public sealed class TransferAssignmentHandler(
    [FromKeyedServices("microfinance:loanofficerassignments")] IRepository<LoanOfficerAssignment> repository,
    ILogger<TransferAssignmentHandler> logger)
    : IRequestHandler<TransferAssignmentCommand, TransferAssignmentResponse>
{
    public async Task<TransferAssignmentResponse> Handle(
        TransferAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await repository.FirstOrDefaultAsync(
            new LoanOfficerAssignmentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan officer assignment {request.Id} not found");

        var newAssignment = assignment.Transfer(request.NewStaffId, request.Reason);
        
        await repository.UpdateAsync(assignment, cancellationToken);
        await repository.AddAsync(newAssignment, cancellationToken);

        logger.LogInformation("Assignment transferred: {OldId} -> {NewId}",
            assignment.Id, newAssignment.Id);

        return new TransferAssignmentResponse(assignment.Id, newAssignment.Id, newAssignment.Status);
    }
}
