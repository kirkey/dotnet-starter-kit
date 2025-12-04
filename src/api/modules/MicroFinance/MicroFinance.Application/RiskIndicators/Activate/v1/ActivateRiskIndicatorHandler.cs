using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Activate.v1;

/// <summary>
/// Handler to activate a risk indicator.
/// </summary>
public sealed class ActivateRiskIndicatorHandler(
    ILogger<ActivateRiskIndicatorHandler> logger,
    [FromKeyedServices("microfinance:riskindicators")] IRepository<RiskIndicator> repository)
    : IRequestHandler<ActivateRiskIndicatorCommand, ActivateRiskIndicatorResponse>
{
    public async Task<ActivateRiskIndicatorResponse> Handle(ActivateRiskIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = await repository.FirstOrDefaultAsync(new RiskIndicatorByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Risk indicator {request.Id} not found");

        indicator.Activate();
        await repository.UpdateAsync(indicator, cancellationToken);

        logger.LogInformation("Risk indicator {Id} activated", indicator.Id);

        return new ActivateRiskIndicatorResponse(indicator.Id, indicator.Status);
    }
}
