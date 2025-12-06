using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Update.v1;

public sealed class UpdateAgentBankingHandler(
    ILogger<UpdateAgentBankingHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<UpdateAgentBankingCommand, UpdateAgentBankingResponse>
{
    public async Task<UpdateAgentBankingResponse> Handle(UpdateAgentBankingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.Update(
            businessName: request.BusinessName,
            contactName: request.ContactName,
            phoneNumber: request.PhoneNumber,
            email: request.Email,
            address: request.Address,
            gpsCoordinates: request.GpsCoordinates,
            operatingHours: request.OperatingHours,
            commissionRate: request.CommissionRate,
            dailyTransactionLimit: request.DailyTransactionLimit,
            monthlyTransactionLimit: request.MonthlyTransactionLimit);

        await repository.UpdateAsync(agent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Agent banking {Id} updated", agent.Id);
        return new UpdateAgentBankingResponse(agent.Id);
    }
}
