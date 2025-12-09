using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Update.v1;

/// <summary>
/// Command to update a pending fee waiver.
/// </summary>
public sealed record UpdateFeeWaiverCommand(
    DefaultIdType Id,
    decimal? WaivedAmount = null,
    string? WaiverReason = null,
    string? Notes = null) : IRequest<UpdateFeeWaiverResponse>;
