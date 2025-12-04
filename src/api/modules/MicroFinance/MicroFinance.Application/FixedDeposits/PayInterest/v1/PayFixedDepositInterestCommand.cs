using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PayInterest.v1;

/// <summary>
/// Command to pay out interest from a fixed deposit.
/// </summary>
public sealed record PayFixedDepositInterestCommand(Guid DepositId, decimal Amount) : IRequest<PayFixedDepositInterestResponse>;
