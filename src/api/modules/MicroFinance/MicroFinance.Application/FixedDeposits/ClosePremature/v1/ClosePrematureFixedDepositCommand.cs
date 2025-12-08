using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.ClosePremature.v1;

/// <summary>
/// Command to close a fixed deposit prematurely.
/// </summary>
public sealed record ClosePrematureFixedDepositCommand(DefaultIdType DepositId, string? Reason = null) : IRequest<ClosePrematureFixedDepositResponse>;
