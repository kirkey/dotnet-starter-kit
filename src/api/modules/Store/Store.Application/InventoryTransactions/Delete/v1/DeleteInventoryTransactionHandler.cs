using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;
using Store.Domain.Exceptions.InventoryTransaction;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Delete.v1;

public class DeleteInventoryTransactionHandler(
    [FromKeyedServices("store:inventorytransactions")] IRepository<InventoryTransaction> repository,
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> readRepository)
    : IRequestHandler<DeleteInventoryTransactionCommand, DeleteInventoryTransactionResponse>
{
    public async Task<DeleteInventoryTransactionResponse> Handle(DeleteInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        if (transaction is null)
        {
            throw new InventoryTransactionNotFoundException(request.Id);
        }

        await repository.DeleteAsync(transaction, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new DeleteInventoryTransactionResponse(transaction.Id, transaction.TransactionNumber);
    }
}
