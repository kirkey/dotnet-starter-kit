using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Create.v1;

public class CreateInventoryTransactionHandler(
    [FromKeyedServices("store:inventorytransactions")] IRepository<InventoryTransaction> repository,
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> readRepository)
    : IRequestHandler<CreateInventoryTransactionCommand, CreateInventoryTransactionResponse>
{
    public async Task<CreateInventoryTransactionResponse> Handle(CreateInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate transaction number
        var existingTransaction = await readRepository.FirstOrDefaultAsync(
            new InventoryTransactionByNumberSpec(request.TransactionNumber),
            cancellationToken);

        if (existingTransaction is not null)
        {
            throw new ConflictException($"Transaction number '{request.TransactionNumber}' already exists.");
        }

        var transaction = InventoryTransaction.Create(
            request.TransactionNumber,
            request.ItemId,
            request.WarehouseId,
            request.WarehouseLocationId,
            request.PurchaseOrderId,
            request.TransactionType,
            request.Reason,
            request.Quantity,
            request.QuantityBefore,
            request.UnitCost,
            request.TransactionDate,
            request.Reference,
            null, // notes parameter
            request.PerformedBy,
            request.IsApproved);

        await repository.AddAsync(transaction, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateInventoryTransactionResponse(transaction.Id, transaction.TransactionNumber);
    }
}
