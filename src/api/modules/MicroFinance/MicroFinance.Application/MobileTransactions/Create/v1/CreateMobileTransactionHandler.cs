using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Create.v1;

public sealed class CreateMobileTransactionHandler(
    [FromKeyedServices("microfinance:mobiletransactions")] IRepository<MobileTransaction> repository,
    ILogger<CreateMobileTransactionHandler> logger)
    : IRequestHandler<CreateMobileTransactionCommand, CreateMobileTransactionResponse>
{
    public async Task<CreateMobileTransactionResponse> Handle(
        CreateMobileTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transaction = MobileTransaction.Create(
            request.WalletId,
            request.TransactionReference,
            request.TransactionType,
            request.Amount,
            request.Fee,
            request.SourcePhone,
            request.DestinationPhone);

        await repository.AddAsync(transaction, cancellationToken);

        logger.LogInformation("Mobile transaction created: {TransactionId}", transaction.Id);

        return new CreateMobileTransactionResponse(transaction.Id);
    }
}
