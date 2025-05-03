using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.Update.v1;
public sealed class ChartOfAccountUpdateRequestHandler(
    ILogger<ChartOfAccountUpdateRequestHandler> logger,
    [FromKeyedServices("accounting:ChartOfAccounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountUpdateRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ChartOfAccountUpdateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = account ?? throw new ChartOfAccountNotFoundException(request.Id);

        var updatedAccount = account.Update(
            request.AccountCategory, request.AccountType,
            request.ParentCode, request.Code,
            request.Name, request.Balance,
            request.Description, request.Notes);

        await repository.UpdateAsync(updatedAccount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account with id : {AccountId} updated.", account.Id);

        return account.Id;
    }
}
