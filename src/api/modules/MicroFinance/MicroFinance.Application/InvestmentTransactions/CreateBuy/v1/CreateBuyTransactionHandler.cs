using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateBuy.v1;

public sealed class CreateBuyTransactionHandler(
    [FromKeyedServices("microfinance:investmenttransactions")] IRepository<InvestmentTransaction> repository,
    ILogger<CreateBuyTransactionHandler> logger)
    : IRequestHandler<CreateBuyTransactionCommand, CreateBuyTransactionResponse>
{
    public async Task<CreateBuyTransactionResponse> Handle(CreateBuyTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = InvestmentTransaction.CreateBuy(
            request.InvestmentAccountId,
            request.ProductId,
            request.TransactionReference,
            request.Amount,
            request.EntryLoad,
            request.PaymentMode,
            request.PaymentReference);

        await repository.AddAsync(transaction, cancellationToken);
        logger.LogInformation("Buy transaction {Reference} created with ID {Id}", transaction.TransactionReference, transaction.Id);

        return new CreateBuyTransactionResponse(
            transaction.Id,
            transaction.TransactionReference,
            transaction.Status,
            transaction.Amount,
            transaction.NetAmount);
    }
}
