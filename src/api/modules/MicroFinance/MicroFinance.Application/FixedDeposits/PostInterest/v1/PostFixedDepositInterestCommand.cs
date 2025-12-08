using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PostInterest.v1;

/// <summary>
/// Command to post interest to a fixed deposit.
/// </summary>
public sealed record PostFixedDepositInterestCommand(DefaultIdType DepositId, decimal InterestAmount) : IRequest<PostFixedDepositInterestResponse>;
