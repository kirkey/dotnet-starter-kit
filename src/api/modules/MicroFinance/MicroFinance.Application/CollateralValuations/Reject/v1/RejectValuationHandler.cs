using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;

public sealed class RejectValuationHandler(
    [FromKeyedServices("microfinance:collateralvaluations")] IRepository<CollateralValuation> repository,
    ILogger<RejectValuationHandler> logger)
    : IRequestHandler<RejectValuationCommand, RejectValuationResponse>
{
    public async Task<RejectValuationResponse> Handle(
        RejectValuationCommand request,
        CancellationToken cancellationToken)
    {
        var valuation = await repository.FirstOrDefaultAsync(
            new CollateralValuationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral valuation {request.Id} not found");

        valuation.Reject(request.Reason);
        await repository.UpdateAsync(valuation, cancellationToken);

        logger.LogInformation("Collateral valuation rejected: {ValuationId}", valuation.Id);

        return new RejectValuationResponse(valuation.Id, valuation.Status, request.Reason);
    }
}
