using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.SetupSip.v1;

public sealed record SetupSipCommand(
    Guid Id,
    decimal Amount,
    string Frequency,
    DateOnly NextDate,
    Guid LinkedSavingsAccountId) : IRequest<SetupSipResponse>;
