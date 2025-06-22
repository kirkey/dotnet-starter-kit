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
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountCreateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ChartOfAccountCreateRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
        
            // Check for duplicate account code or name
            if (await repository.FirstOrDefaultAsync(new ChartOfAccountByCode(request.AccountCode), cancellationToken) != null ||
                await repository.FirstOrDefaultAsync(new ChartOfAccountByName(request.Name), cancellationToken) != null)
            {
                throw new ChartOfAccountForbiddenException($"{request.AccountCode} or {request.Name}");
            }
        
            var account = ChartOfAccount.Create(
                request.AccountCategory, request.AccountType, request.ParentCode,
                request.AccountCode, request.Name, request.Balance,
                request.Description, request.Notes);
        
            await repository.AddAsync(account, cancellationToken).ConfigureAwait(false);
        
            logger.LogInformation("account created {AccountId}", account.Id);
        
            return account.Id;
        }
}
