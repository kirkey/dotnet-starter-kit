using Accounting.Application.ChartOfAccounts.Exceptions;
using Accounting.Application.ChartOfAccounts.Queries;
using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.Create.v1;
public sealed class ChartOfAccountCreateRequestHandler(
    ILogger<ChartOfAccountCreateRequestHandler> logger,
    [FromKeyedServices("accounting:ChartOfAccounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountCreateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ChartOfAccountCreateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate account code
        var existingAccount = await repository.FirstOrDefaultAsync(new ChartOfAccountByCode(request.Code), cancellationToken)
            .ConfigureAwait(false);
        if (existingAccount is not null)
            throw new ChartOfAccountForbiddenException(request.Code);

        var account = ChartOfAccount.Create(request.AccountCategory, request.AccountType, request.ParentCode, request.Code, request.Name, request.Balance,
            request.Description, request.Notes);

        await repository.AddAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account created {AccountId}", account.Id);

        return account.Id;
    }
}
