using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Reverse.v1;

/// <summary>
/// Command to reverse a fee charge.
/// </summary>
public sealed record ReverseFeeChargeCommand(DefaultIdType FeeChargeId, string? Reason = null) : IRequest<ReverseFeeChargeResponse>;
