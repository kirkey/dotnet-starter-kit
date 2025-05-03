using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.Delete.v1;
public class ChartOfAccountDeleteHandler(
    ILogger<ChartOfAccountDeleteHandler> logger,
    [FromKeyedServices("accounting:ChartOfAccounts")] IRepository<ChartOfAccount> repository)
    : IRequestHandler<ChartOfAccountDeleteRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ChartOfAccountDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = account ?? throw new ChartOfAccountNotFoundException(request.Id);

        await repository.DeleteAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account with id: {AccountId} successfully deleted", account.Id);

        return account.Id;
    }
}
