using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.RecordMeasurement.v1;

/// <summary>
/// Command to record a new measurement for a risk indicator.
/// </summary>
public sealed record RecordMeasurementCommand(DefaultIdType Id, decimal Value) : IRequest<RecordMeasurementResponse>;
