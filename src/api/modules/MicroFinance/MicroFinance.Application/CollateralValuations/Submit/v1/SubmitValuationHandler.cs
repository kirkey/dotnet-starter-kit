using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Submit.v1;

public sealed class SubmitValuationHandler(
    [FromKeyedServices("microfinance:collateralvaluations")] IRepository<CollateralValuation> repository,
    ILogger<SubmitValuationHandler> logger)
    : IRequestHandler<SubmitValuationCommand, SubmitValuationResponse>
{
    public async Task<SubmitValuationResponse> Handle(
        SubmitValuationCommand request,
        CancellationToken cancellationToken)
    {
        var valuation = await repository.FirstOrDefaultAsync(
            new CollateralValuationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral valuation {request.Id} not found");

        valuation.Submit();
        await repository.UpdateAsync(valuation, cancellationToken);

        logger.LogInformation("Collateral valuation submitted: {ValuationId}", valuation.Id);

        return new SubmitValuationResponse(valuation.Id, valuation.Status);
    }
}
