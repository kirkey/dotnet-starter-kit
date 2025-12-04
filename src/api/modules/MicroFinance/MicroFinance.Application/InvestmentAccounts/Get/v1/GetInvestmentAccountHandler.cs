using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;

public sealed class GetInvestmentAccountHandler(
    [FromKeyedServices("microfinance:investmentaccounts")] IReadRepository<InvestmentAccount> repository,
    ILogger<GetInvestmentAccountHandler> logger)
    : IRequestHandler<GetInvestmentAccountRequest, InvestmentAccountResponse>
{
    public async Task<InvestmentAccountResponse> Handle(GetInvestmentAccountRequest request, CancellationToken cancellationToken)
    {
        var account = await repository.FirstOrDefaultAsync(new InvestmentAccountByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Investment account {request.Id} not found");

        logger.LogInformation("Retrieved investment account {Id}", account.Id);

        return new InvestmentAccountResponse(
            account.Id,
            account.MemberId,
            account.AccountNumber,
            account.Status,
            account.RiskProfile,
            account.TotalInvested,
            account.CurrentValue,
            account.TotalGainLoss,
            account.TotalGainLossPercent,
            account.RealizedGains,
            account.UnrealizedGains,
            account.TotalDividends,
            account.HoldingsCount,
            account.FirstInvestmentDate,
            account.LastTransactionDate,
            account.AssignedAdvisorId,
            account.HasSip,
            account.SipAmount,
            account.SipFrequency,
            account.NextSipDate,
            account.InvestmentGoal,
            account.TargetDate,
            account.TargetAmount);
    }
}
