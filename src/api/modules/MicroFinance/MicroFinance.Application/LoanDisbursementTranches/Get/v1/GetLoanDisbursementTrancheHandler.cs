using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;

public sealed class GetLoanDisbursementTrancheHandler(
    [FromKeyedServices("microfinance:loandisbursementtranches")] IReadRepository<LoanDisbursementTranche> repository)
    : IRequestHandler<GetLoanDisbursementTrancheRequest, LoanDisbursementTrancheResponse>
{
    public async Task<LoanDisbursementTrancheResponse> Handle(
        GetLoanDisbursementTrancheRequest request,
        CancellationToken cancellationToken)
    {
        var tranche = await repository.FirstOrDefaultAsync(
            new LoanDisbursementTrancheByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan disbursement tranche {request.Id} not found");

        return new LoanDisbursementTrancheResponse(
            tranche.Id,
            tranche.LoanId,
            tranche.TrancheSequence,
            tranche.TrancheNumber,
            tranche.ScheduledDate,
            tranche.DisbursedDate,
            tranche.Amount,
            tranche.Deductions,
            tranche.NetAmount,
            tranche.DisbursementMethod,
            tranche.BankAccountNumber,
            tranche.BankName,
            tranche.ReferenceNumber,
            tranche.Milestone,
            tranche.MilestoneVerified,
            tranche.Status,
            tranche.ApprovedByUserId,
            tranche.ApprovedAt,
            tranche.DisbursedByUserId);
    }
}
