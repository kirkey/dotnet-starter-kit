using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateSell.v1;

public sealed class CreateSellTransactionHandler(
    [FromKeyedServices("microfinance:investmenttransactions")] IRepository<InvestmentTransaction> repository,
    ILogger<CreateSellTransactionHandler> logger)
    : IRequestHandler<CreateSellTransactionCommand, CreateSellTransactionResponse>
{
    public async Task<CreateSellTransactionResponse> Handle(CreateSellTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = InvestmentTransaction.CreateSell(
            request.InvestmentAccountId,
            request.ProductId,
            request.TransactionReference,
            request.Units,
            request.ExitLoad);

        await repository.AddAsync(transaction, cancellationToken);
        logger.LogInformation("Sell transaction {Reference} created with ID {Id}", transaction.TransactionReference, transaction.Id);

        return new CreateSellTransactionResponse(
            transaction.Id,
            transaction.TransactionReference,
            transaction.Status,
            transaction.Units);
    }
}
