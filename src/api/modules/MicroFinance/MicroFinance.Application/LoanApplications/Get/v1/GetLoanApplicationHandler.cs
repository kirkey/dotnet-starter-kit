using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Get.v1;

/// <summary>
/// Handler for retrieving a loan application by ID.
/// </summary>
public sealed class GetLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IReadRepository<LoanApplication> repository)
    : IRequestHandler<GetLoanApplicationRequest, LoanApplicationResponse>
{
    public async Task<LoanApplicationResponse> Handle(GetLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = await repository.FirstOrDefaultAsync(new LoanApplicationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan application with ID {request.Id} not found.");

        return new LoanApplicationResponse(
            application.Id,
            application.ApplicationNumber,
            application.MemberId,
            application.LoanProductId,
            application.MemberGroupId,
            application.RequestedAmount,
            application.ApprovedAmount,
            application.RequestedTermMonths,
            application.ApprovedTermMonths,
            application.Purpose,
            application.Status,
            application.ApplicationDate,
            application.AssignedOfficerId,
            application.AssignedAt,
            application.DecisionAt,
            application.DecisionByUserId,
            application.RejectionReason,
            application.LoanId);
    }
}
