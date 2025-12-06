using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.RecordAudit.v1;

public sealed record RecordAuditAgentBankingCommand(Guid Id, DateOnly AuditDate) : IRequest<RecordAuditAgentBankingResponse>;
