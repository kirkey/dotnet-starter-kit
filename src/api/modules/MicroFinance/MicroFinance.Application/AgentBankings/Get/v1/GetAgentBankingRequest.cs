using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;

public sealed record GetAgentBankingRequest(DefaultIdType Id) : IRequest<AgentBankingResponse>;
