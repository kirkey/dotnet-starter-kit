using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Mature.v1;

/// <summary>
/// Command to mature a fixed deposit.
/// </summary>
public sealed record MatureFixedDepositCommand(DefaultIdType DepositId) : IRequest<MatureFixedDepositResponse>;
