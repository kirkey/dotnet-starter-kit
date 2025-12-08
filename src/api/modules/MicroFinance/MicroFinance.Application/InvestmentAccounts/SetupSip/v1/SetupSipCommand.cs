using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.SetupSip.v1;

public sealed record SetupSipCommand(
    DefaultIdType Id,
    decimal Amount,
    string Frequency,
    DateOnly NextDate,
    DefaultIdType LinkedSavingsAccountId) : IRequest<SetupSipResponse>;
