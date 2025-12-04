using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;

public sealed class GetAgentBankingHandler(
    [FromKeyedServices("microfinance:agentbankings")] IReadRepository<AgentBanking> repository)
    : IRequestHandler<GetAgentBankingRequest, AgentBankingResponse>
{
    public async Task<AgentBankingResponse> Handle(GetAgentBankingRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        return new AgentBankingResponse(
            agent.Id,
            agent.AgentCode,
            agent.BusinessName,
            agent.ContactName,
            agent.PhoneNumber,
            agent.Email,
            agent.Address,
            agent.GpsCoordinates,
            agent.Status,
            agent.Tier,
            agent.BranchId,
            agent.LinkedStaffId,
            agent.FloatBalance,
            agent.MinFloatBalance,
            agent.MaxFloatBalance,
            agent.CommissionRate,
            agent.TotalCommissionEarned,
            agent.DailyTransactionLimit,
            agent.MonthlyTransactionLimit,
            agent.DailyVolumeProcessed,
            agent.MonthlyVolumeProcessed,
            agent.TotalTransactionsToday,
            agent.TotalTransactionsMonth,
            agent.ContractStartDate,
            agent.ContractEndDate,
            agent.LastTrainingDate,
            agent.LastAuditDate,
            agent.IsKycVerified,
            agent.DeviceId,
            agent.OperatingHours,
            agent.CreatedOn);
    }
}
