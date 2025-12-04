using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Waive.v1;

/// <summary>
/// Command to waive a fee charge.
/// </summary>
public sealed record WaiveFeeChargeCommand(Guid FeeChargeId, string? Reason = null) : IRequest<WaiveFeeChargeResponse>;
