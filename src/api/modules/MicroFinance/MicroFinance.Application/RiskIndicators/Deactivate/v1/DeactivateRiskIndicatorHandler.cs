using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Deactivate.v1;

/// <summary>
/// Handler to deactivate a risk indicator.
/// </summary>
public sealed class DeactivateRiskIndicatorHandler(
    ILogger<DeactivateRiskIndicatorHandler> logger,
    [FromKeyedServices("microfinance:riskindicators")] IRepository<RiskIndicator> repository)
    : IRequestHandler<DeactivateRiskIndicatorCommand, DeactivateRiskIndicatorResponse>
{
    public async Task<DeactivateRiskIndicatorResponse> Handle(DeactivateRiskIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = await repository.FirstOrDefaultAsync(new RiskIndicatorByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Risk indicator {request.Id} not found");

        indicator.Deactivate();
        await repository.UpdateAsync(indicator, cancellationToken);

        logger.LogInformation("Risk indicator {Id} deactivated", indicator.Id);

        return new DeactivateRiskIndicatorResponse(indicator.Id, indicator.Status);
    }
}
