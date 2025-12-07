using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Invest.v1;

public sealed class InvestHandler(
    [FromKeyedServices("microfinance:investmentaccounts")] IRepository<InvestmentAccount> repository,
    ILogger<InvestHandler> logger)
    : IRequestHandler<InvestCommand, InvestResponse>
{
    public async Task<InvestResponse> Handle(InvestCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.FirstOrDefaultAsync(new InvestmentAccountByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Investment account {request.Id} not found");

        account.Invest(request.Amount);
        await repository.UpdateAsync(account, cancellationToken);

        logger.LogInformation("Invested {Amount} in account {Id}", request.Amount, account.Id);

        return new InvestResponse(account.Id, account.TotalInvested, account.CurrentValue);
    }
}
