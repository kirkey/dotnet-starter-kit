using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Fail.v1;

public sealed class FailMobileTransactionHandler(
    [FromKeyedServices("microfinance:mobiletransactions")] IRepository<MobileTransaction> repository,
    ILogger<FailMobileTransactionHandler> logger)
    : IRequestHandler<FailMobileTransactionCommand, FailMobileTransactionResponse>
{
    public async Task<FailMobileTransactionResponse> Handle(
        FailMobileTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(
            new MobileTransactionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Mobile transaction {request.Id} not found");

        transaction.Fail(request.FailureReason);
        await repository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation("Mobile transaction failed: {TransactionId}, Reason: {Reason}",
            transaction.Id, request.FailureReason);

        return new FailMobileTransactionResponse(
            transaction.Id,
            transaction.Status,
            transaction.FailureReason!);
    }
}
