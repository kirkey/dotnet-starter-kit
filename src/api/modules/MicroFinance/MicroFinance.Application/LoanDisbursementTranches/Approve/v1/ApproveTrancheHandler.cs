using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Approve.v1;

public sealed class ApproveTrancheHandler(
    [FromKeyedServices("microfinance:loandisbursementtranches")] IRepository<LoanDisbursementTranche> repository,
    ILogger<ApproveTrancheHandler> logger)
    : IRequestHandler<ApproveTrancheCommand, ApproveTrancheResponse>
{
    public async Task<ApproveTrancheResponse> Handle(
        ApproveTrancheCommand request,
        CancellationToken cancellationToken)
    {
        var tranche = await repository.FirstOrDefaultAsync(
            new LoanDisbursementTrancheByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan disbursement tranche {request.Id} not found");

        tranche.Approve(request.UserId);
        await repository.UpdateAsync(tranche, cancellationToken);

        logger.LogInformation("Tranche approved: {TrancheId}", tranche.Id);

        return new ApproveTrancheResponse(tranche.Id, tranche.Status, tranche.ApprovedAt!.Value);
    }
}
