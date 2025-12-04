using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Renew.v1;

/// <summary>
/// Command to renew a matured fixed deposit.
/// </summary>
public sealed record RenewFixedDepositCommand(
    Guid DepositId,
    int? NewTermMonths = null,
    decimal? NewInterestRate = null) : IRequest<RenewFixedDepositResponse>;
