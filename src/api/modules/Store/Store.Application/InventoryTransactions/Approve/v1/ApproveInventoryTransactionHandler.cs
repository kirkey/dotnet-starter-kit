using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;
using Store.Domain.Exceptions.InventoryTransaction;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Approve.v1;

public class ApproveInventoryTransactionHandler(
    [FromKeyedServices("store:inventorytransactions")] IRepository<InventoryTransaction> repository,
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> readRepository)
    : IRequestHandler<ApproveInventoryTransactionCommand, ApproveInventoryTransactionResponse>
{
    public async Task<ApproveInventoryTransactionResponse> Handle(ApproveInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        if (transaction is null)
        {
            throw new InventoryTransactionNotFoundException(request.Id);
        }

        // Approve the transaction using domain method
        transaction.Approve(request.ApprovedBy);

        await repository.UpdateAsync(transaction, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var updatedTransaction = await readRepository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        return new ApproveInventoryTransactionResponse(updatedTransaction!.Adapt<InventoryTransactionResponse>());
    }
}
