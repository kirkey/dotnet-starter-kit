using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.CreditFloat.v1;

public sealed record CreditFloatCommand(Guid Id, decimal Amount) : IRequest<CreditFloatResponse>;
