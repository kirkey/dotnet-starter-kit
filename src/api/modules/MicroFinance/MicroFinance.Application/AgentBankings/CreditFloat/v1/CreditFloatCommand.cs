using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.CreditFloat.v1;

public sealed record CreditFloatCommand(DefaultIdType Id, decimal Amount) : IRequest<CreditFloatResponse>;
