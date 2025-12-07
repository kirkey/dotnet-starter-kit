using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.RecordMeasurement.v1;

/// <summary>
/// Handler to record a new measurement for a risk indicator.
/// </summary>
public sealed class RecordMeasurementHandler(
    ILogger<RecordMeasurementHandler> logger,
    [FromKeyedServices("microfinance:riskindicators")] IRepository<RiskIndicator> repository)
    : IRequestHandler<RecordMeasurementCommand, RecordMeasurementResponse>
{
    public async Task<RecordMeasurementResponse> Handle(RecordMeasurementCommand request, CancellationToken cancellationToken)
    {
        var indicator = await repository.FirstOrDefaultAsync(new RiskIndicatorByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Risk indicator {request.Id} not found");

        indicator.RecordMeasurement(request.Value);
        await repository.UpdateAsync(indicator, cancellationToken);

        logger.LogInformation("Recorded measurement {Value} for risk indicator {Id}, health: {Health}",
            request.Value, indicator.Id, indicator.CurrentHealth);

        return new RecordMeasurementResponse(
            indicator.Id,
            indicator.CurrentValue!.Value,
            indicator.PreviousValue,
            indicator.CurrentHealth,
            indicator.LastMeasuredAt!.Value);
    }
}
