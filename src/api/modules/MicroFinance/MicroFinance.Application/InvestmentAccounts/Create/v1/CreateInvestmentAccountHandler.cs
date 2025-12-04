using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Create.v1;

public sealed class CreateInvestmentAccountHandler(
    [FromKeyedServices("microfinance:investmentaccounts")] IRepository<InvestmentAccount> repository,
    ILogger<CreateInvestmentAccountHandler> logger)
    : IRequestHandler<CreateInvestmentAccountCommand, CreateInvestmentAccountResponse>
{
    public async Task<CreateInvestmentAccountResponse> Handle(CreateInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        var account = InvestmentAccount.Create(
            request.MemberId,
            request.AccountNumber,
            request.RiskProfile,
            request.AssignedAdvisorId,
            request.InvestmentGoal);

        await repository.AddAsync(account, cancellationToken);
        logger.LogInformation("Investment account {AccountNumber} created with ID {Id}", account.AccountNumber, account.Id);

        return new CreateInvestmentAccountResponse(account.Id, account.AccountNumber, account.RiskProfile);
    }
}
