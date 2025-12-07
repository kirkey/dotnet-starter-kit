using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;

/// <summary>
/// Handler to get a risk indicator by ID.
/// </summary>
public sealed class GetRiskIndicatorHandler(
    ILogger<GetRiskIndicatorHandler> logger,
    [FromKeyedServices("microfinance:riskindicators")] IReadRepository<RiskIndicator> repository)
    : IRequestHandler<GetRiskIndicatorRequest, RiskIndicatorResponse>
{
    public async Task<RiskIndicatorResponse> Handle(GetRiskIndicatorRequest request, CancellationToken cancellationToken)
    {
        var indicator = await repository.FirstOrDefaultAsync(new RiskIndicatorByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Risk indicator {request.Id} not found");

        logger.LogInformation("Retrieved risk indicator {Id}", indicator.Id);

        return new RiskIndicatorResponse(
            indicator.Id,
            indicator.RiskCategoryId,
            indicator.Code,
            indicator.Name,
            indicator.Description,
            indicator.Formula,
            indicator.Unit,
            indicator.Direction,
            indicator.Frequency,
            indicator.DataSource,
            indicator.TargetValue,
            indicator.GreenThreshold,
            indicator.YellowThreshold,
            indicator.OrangeThreshold,
            indicator.RedThreshold,
            indicator.CurrentValue,
            indicator.PreviousValue,
            indicator.CurrentHealth,
            indicator.LastMeasuredAt,
            indicator.WeightFactor,
            indicator.Status,
            indicator.Notes);
    }
}
