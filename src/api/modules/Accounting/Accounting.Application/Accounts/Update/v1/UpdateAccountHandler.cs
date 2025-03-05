using FSH.Framework.Core.Persistence;
using Accounting.Domain;
using Accounting.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Accounts.Update.v1;
public sealed class UpdateAccountHandler(
    ILogger<UpdateAccountHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<Account> repository)
    : IRequestHandler<UpdateAccountCommand, UpdateAccountResponse>
{
    public async Task<UpdateAccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = account ?? throw new AccountNotFoundException(request.Id);

        var updatedAccount = account.Update(request.Category, request.TransactionType,
            request.ParentCode, request.Code,
            request.Name, request.Balance,
            request.Description, request.Notes);

        await repository.UpdateAsync(updatedAccount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account with id : {AccountId} updated.", account.Id);

        return new UpdateAccountResponse(account.Id);
    }
}
