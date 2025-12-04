using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Create.v1;

/// <summary>
/// Handler for creating a new risk indicator.
/// </summary>
public sealed class CreateRiskIndicatorHandler(
    ILogger<CreateRiskIndicatorHandler> logger,
    [FromKeyedServices("microfinance:riskindicators")] IRepository<RiskIndicator> repository)
    : IRequestHandler<CreateRiskIndicatorCommand, CreateRiskIndicatorResponse>
{
    public async Task<CreateRiskIndicatorResponse> Handle(CreateRiskIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = RiskIndicator.Create(
            request.RiskCategoryId,
            request.Code,
            request.Name,
            request.Direction,
            request.Frequency,
            request.Description,
            request.Formula,
            request.Unit,
            request.DataSource,
            request.TargetValue,
            request.GreenThreshold,
            request.YellowThreshold,
            request.OrangeThreshold,
            request.RedThreshold,
            request.WeightFactor);

        await repository.AddAsync(indicator, cancellationToken);
        logger.LogInformation("Risk indicator {Code} created with ID {Id}", indicator.Code, indicator.Id);

        return new CreateRiskIndicatorResponse(indicator.Id, indicator.Code, indicator.Name);
    }
}
