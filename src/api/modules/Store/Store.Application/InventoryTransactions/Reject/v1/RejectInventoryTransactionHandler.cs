using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;
using Store.Domain.Exceptions.InventoryTransaction;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Reject.v1;

/// <summary>
/// Handler for rejecting an inventory transaction.
/// </summary>
/// <remarks>
/// This handler rejects a previously approved transaction by calling the domain method.
/// The rejection can include an optional reason that is appended to the transaction notes.
/// </remarks>
public class RejectInventoryTransactionHandler(
    [FromKeyedServices("store:inventorytransactions")] IRepository<InventoryTransaction> repository,
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> readRepository)
    : IRequestHandler<RejectInventoryTransactionCommand, RejectInventoryTransactionResponse>
{
    /// <summary>
    /// Handles the rejection of an inventory transaction.
    /// </summary>
    /// <param name="request">The rejection command containing transaction ID, rejected by, and reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response containing the rejected transaction details.</returns>
    /// <exception cref="InventoryTransactionNotFoundException">Thrown when the transaction is not found.</exception>
    public async Task<RejectInventoryTransactionResponse> Handle(RejectInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        if (transaction is null)
        {
            throw new InventoryTransactionNotFoundException(request.Id);
        }

        // Reject the transaction using domain method
        transaction.Reject(request.RejectedBy, request.RejectionReason);

        await repository.UpdateAsync(transaction, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var updatedTransaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        return new RejectInventoryTransactionResponse(updatedTransaction!.Adapt<InventoryTransactionResponse>());
    }
}

