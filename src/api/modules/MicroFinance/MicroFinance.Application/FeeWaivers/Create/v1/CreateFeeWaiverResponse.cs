namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Create.v1;

/// <summary>
/// Response after creating a fee waiver.
/// </summary>
public sealed record CreateFeeWaiverResponse(
    DefaultIdType Id,
    string Reference,
    string WaiverType,
    decimal WaivedAmount,
    string Status);
