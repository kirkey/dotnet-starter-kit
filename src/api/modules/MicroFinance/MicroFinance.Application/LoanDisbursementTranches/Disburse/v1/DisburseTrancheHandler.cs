using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;

public sealed class DisburseTrancheHandler(
    [FromKeyedServices("microfinance:loandisbursementtranches")] IRepository<LoanDisbursementTranche> repository,
    ILogger<DisburseTrancheHandler> logger)
    : IRequestHandler<DisburseTrancheCommand, DisburseTrancheResponse>
{
    public async Task<DisburseTrancheResponse> Handle(
        DisburseTrancheCommand request,
        CancellationToken cancellationToken)
    {
        var tranche = await repository.FirstOrDefaultAsync(
            new LoanDisbursementTrancheByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan disbursement tranche {request.Id} not found");

        tranche.Disburse(request.UserId, request.ReferenceNumber, request.DisbursedDate);
        await repository.UpdateAsync(tranche, cancellationToken);

        logger.LogInformation("Tranche disbursed: {TrancheId}, Amount: {Amount}",
            tranche.Id, tranche.NetAmount);

        return new DisburseTrancheResponse(
            tranche.Id,
            tranche.Status,
            tranche.NetAmount,
            tranche.DisbursedDate!.Value);
    }
}
