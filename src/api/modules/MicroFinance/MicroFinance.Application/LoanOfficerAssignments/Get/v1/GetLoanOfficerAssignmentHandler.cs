using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;

public sealed class GetLoanOfficerAssignmentHandler(
    [FromKeyedServices("microfinance:loanofficerassignments")] IReadRepository<LoanOfficerAssignment> repository)
    : IRequestHandler<GetLoanOfficerAssignmentRequest, LoanOfficerAssignmentResponse>
{
    public async Task<LoanOfficerAssignmentResponse> Handle(
        GetLoanOfficerAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var assignment = await repository.FirstOrDefaultAsync(
            new LoanOfficerAssignmentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan officer assignment {request.Id} not found");

        return new LoanOfficerAssignmentResponse(
            assignment.Id,
            assignment.StaffId,
            assignment.AssignmentType,
            assignment.MemberId,
            assignment.MemberGroupId,
            assignment.LoanId,
            assignment.BranchId,
            assignment.AssignmentDate,
            assignment.EndDate,
            assignment.PreviousStaffId,
            assignment.Reason,
            assignment.IsPrimary,
            assignment.Status);
    }
}
