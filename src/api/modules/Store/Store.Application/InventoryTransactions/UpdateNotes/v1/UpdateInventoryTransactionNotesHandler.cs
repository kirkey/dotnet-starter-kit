using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;
using Store.Domain.Exceptions.InventoryTransaction;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.UpdateNotes.v1;

/// <summary>
/// Handler for updating notes on an inventory transaction.
/// </summary>
/// <remarks>
/// This handler updates the notes field on an existing transaction by calling the domain method.
/// Only updates if the notes value has changed.
/// </remarks>
public class UpdateInventoryTransactionNotesHandler(
    [FromKeyedServices("store:inventorytransactions")] IRepository<InventoryTransaction> repository,
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> readRepository)
    : IRequestHandler<UpdateInventoryTransactionNotesCommand, UpdateInventoryTransactionNotesResponse>
{
    /// <summary>
    /// Handles the update of transaction notes.
    /// </summary>
    /// <param name="request">The update command containing transaction ID and new notes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response containing the updated transaction details.</returns>
    /// <exception cref="InventoryTransactionNotFoundException">Thrown when the transaction is not found.</exception>
    public async Task<UpdateInventoryTransactionNotesResponse> Handle(UpdateInventoryTransactionNotesCommand request, CancellationToken cancellationToken)
    {
        var transaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        if (transaction is null)
        {
            throw new InventoryTransactionNotFoundException(request.Id);
        }

        // Update notes using domain method
        transaction.UpdateNotes(request.Notes);

        await repository.UpdateAsync(transaction, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var updatedTransaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        return new UpdateInventoryTransactionNotesResponse(updatedTransaction!.Adapt<InventoryTransactionResponse>());
    }
}

