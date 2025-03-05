using FSH.Framework.Core.Persistence;
using Accounting.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Accounts.Create.v1;
public sealed class CreateAccountHandler(
    ILogger<CreateAccountHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<Account> repository)
    : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = Account.Create(request.Category, request.TransactionType, request.ParentCode, request.Code, request.Name, request.Balance ?? 0,
            request.Description, request.Notes);

        await repository.AddAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("account created {AccountId}", account.Id);

        return new CreateAccountResponse(account.Id);
    }
}
