namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.RecordMeasurement.v1;

/// <summary>
/// Response from recording a risk indicator measurement.
/// </summary>
public sealed record RecordMeasurementResponse(
    DefaultIdType Id,
    decimal CurrentValue,
    decimal? PreviousValue,
    string CurrentHealth,
    DateTime MeasuredAt);
