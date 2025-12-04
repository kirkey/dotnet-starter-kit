using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.SetupSip.v1;

public sealed class SetupSipHandler(
    [FromKeyedServices("microfinance:investmentaccounts")] IRepository<InvestmentAccount> repository,
    ILogger<SetupSipHandler> logger)
    : IRequestHandler<SetupSipCommand, SetupSipResponse>
{
    public async Task<SetupSipResponse> Handle(SetupSipCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.FirstOrDefaultAsync(new InvestmentAccountByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Investment account {request.Id} not found");

        account.SetupSip(request.Amount, request.Frequency, request.NextDate, request.LinkedSavingsAccountId);
        await repository.UpdateAsync(account, cancellationToken);

        logger.LogInformation("Setup SIP for account {Id} with amount {Amount}", account.Id, request.Amount);

        return new SetupSipResponse(account.Id, account.HasSip, account.SipAmount, account.SipFrequency, account.NextSipDate);
    }
}
