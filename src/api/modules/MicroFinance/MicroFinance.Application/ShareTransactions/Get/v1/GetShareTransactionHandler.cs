using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;

public sealed class GetShareTransactionHandler(
    [FromKeyedServices("microfinance:sharetransactions")] IRepository<ShareTransaction> repository)
    : IRequestHandler<GetShareTransactionRequest, ShareTransactionResponse>
{
    public async Task<ShareTransactionResponse> Handle(GetShareTransactionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transaction = await repository.FirstOrDefaultAsync(
            new ShareTransactionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (transaction is null)
        {
            throw new NotFoundException($"Share transaction with ID {request.Id} not found.");
        }

        return new ShareTransactionResponse(
            transaction.Id,
            transaction.ShareAccountId,
            transaction.Reference,
            transaction.TransactionType,
            transaction.NumberOfShares,
            transaction.PricePerShare,
            transaction.TotalAmount,
            transaction.SharesBalanceAfter,
            transaction.TransactionDate,
            transaction.Description,
            transaction.PaymentMethod
        );
    }
}
