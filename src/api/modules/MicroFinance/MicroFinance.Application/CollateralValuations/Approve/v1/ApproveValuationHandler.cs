using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Approve.v1;

public sealed class ApproveValuationHandler(
    [FromKeyedServices("microfinance:collateralvaluations")] IRepository<CollateralValuation> repository,
    ILogger<ApproveValuationHandler> logger)
    : IRequestHandler<ApproveValuationCommand, ApproveValuationResponse>
{
    public async Task<ApproveValuationResponse> Handle(
        ApproveValuationCommand request,
        CancellationToken cancellationToken)
    {
        var valuation = await repository.FirstOrDefaultAsync(
            new CollateralValuationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral valuation {request.Id} not found");

        valuation.Approve(request.ApprovedById);
        await repository.UpdateAsync(valuation, cancellationToken);

        logger.LogInformation("Collateral valuation approved: {ValuationId}", valuation.Id);

        return new ApproveValuationResponse(valuation.Id, valuation.Status, valuation.ApprovedDate);
    }
}
