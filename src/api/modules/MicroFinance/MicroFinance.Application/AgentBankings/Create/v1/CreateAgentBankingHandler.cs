using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Create.v1;

public sealed class CreateAgentBankingHandler(
    ILogger<CreateAgentBankingHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<CreateAgentBankingCommand, CreateAgentBankingResponse>
{
    public async Task<CreateAgentBankingResponse> Handle(CreateAgentBankingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = AgentBanking.Create(
            request.AgentCode,
            request.BusinessName,
            request.ContactName,
            request.PhoneNumber,
            request.Address,
            request.CommissionRate,
            request.DailyTransactionLimit,
            request.MonthlyTransactionLimit,
            request.ContractStartDate,
            request.BranchId);

        await repository.AddAsync(agent, cancellationToken);
        logger.LogInformation("Agent banking {AgentCode} created with ID {Id}", agent.AgentCode, agent.Id);

        return new CreateAgentBankingResponse(agent.Id, agent.AgentCode);
    }
}
