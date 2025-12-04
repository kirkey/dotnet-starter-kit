using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.DebitFloat.v1;

public sealed record DebitFloatCommand(Guid Id, decimal Amount) : IRequest<DebitFloatResponse>;
