using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Complete.v1;

public sealed class CompleteTransactionHandler(
    [FromKeyedServices("microfinance:investmenttransactions")] IRepository<InvestmentTransaction> repository,
    ILogger<CompleteTransactionHandler> logger)
    : IRequestHandler<CompleteTransactionCommand, CompleteTransactionResponse>
{
    public async Task<CompleteTransactionResponse> Handle(CompleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(new InvestmentTransactionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Investment transaction {request.Id} not found");

        transaction.Complete(request.GainLoss);
        await repository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation("Completed investment transaction {Id}", transaction.Id);

        return new CompleteTransactionResponse(transaction.Id, transaction.Status, transaction.NetAmount, transaction.GainLoss);
    }
}
