using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Redeem.v1;

public sealed class RedeemHandler(
    [FromKeyedServices("microfinance:investmentaccounts")] IRepository<InvestmentAccount> repository,
    ILogger<RedeemHandler> logger)
    : IRequestHandler<RedeemCommand, RedeemResponse>
{
    public async Task<RedeemResponse> Handle(RedeemCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.FirstOrDefaultAsync(new InvestmentAccountByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Investment account {request.Id} not found");

        account.Redeem(request.Amount, request.GainLoss);
        await repository.UpdateAsync(account, cancellationToken);

        logger.LogInformation("Redeemed {Amount} from account {Id}", request.Amount, account.Id);

        return new RedeemResponse(account.Id, account.CurrentValue, account.RealizedGains);
    }
}
