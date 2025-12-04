using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Complete.v1;

public sealed class CompleteMobileTransactionHandler(
    [FromKeyedServices("microfinance:mobiletransactions")] IRepository<MobileTransaction> repository,
    ILogger<CompleteMobileTransactionHandler> logger)
    : IRequestHandler<CompleteMobileTransactionCommand, CompleteMobileTransactionResponse>
{
    public async Task<CompleteMobileTransactionResponse> Handle(
        CompleteMobileTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(
            new MobileTransactionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Mobile transaction {request.Id} not found");

        transaction.Complete(request.ProviderResponse);
        await repository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation("Mobile transaction completed: {TransactionId}", transaction.Id);

        return new CompleteMobileTransactionResponse(
            transaction.Id,
            transaction.Status,
            transaction.CompletedAt!.Value);
    }
}
