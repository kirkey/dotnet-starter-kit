using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;
using Store.Domain.Exceptions.InventoryTransaction;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

public class GetInventoryTransactionHandler(
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> repository)
    : IRequestHandler<GetInventoryTransactionCommand, InventoryTransactionResponse>
{
    public async Task<InventoryTransactionResponse> Handle(GetInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(
            new GetInventoryTransactionByIdSpec(request.Id),
            cancellationToken);

        if (transaction is null)
        {
            throw new InventoryTransactionNotFoundException(request.Id);
        }

        return transaction.Adapt<InventoryTransactionResponse>();
    }
}
