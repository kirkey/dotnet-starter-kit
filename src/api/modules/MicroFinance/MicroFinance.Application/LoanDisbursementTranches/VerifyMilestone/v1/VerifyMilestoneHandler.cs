using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.VerifyMilestone.v1;

public sealed class VerifyMilestoneHandler(
    [FromKeyedServices("microfinance:loandisbursementtranches")] IRepository<LoanDisbursementTranche> repository,
    ILogger<VerifyMilestoneHandler> logger)
    : IRequestHandler<VerifyMilestoneCommand, VerifyMilestoneResponse>
{
    public async Task<VerifyMilestoneResponse> Handle(
        VerifyMilestoneCommand request,
        CancellationToken cancellationToken)
    {
        var tranche = await repository.FirstOrDefaultAsync(
            new LoanDisbursementTrancheByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan disbursement tranche {request.Id} not found");

        tranche.VerifyMilestone();
        await repository.UpdateAsync(tranche, cancellationToken);

        logger.LogInformation("Tranche milestone verified: {TrancheId}", tranche.Id);

        return new VerifyMilestoneResponse(tranche.Id, tranche.MilestoneVerified);
    }
}
