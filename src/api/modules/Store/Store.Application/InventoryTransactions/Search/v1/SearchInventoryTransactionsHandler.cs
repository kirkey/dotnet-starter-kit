using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Search.v1;

public class SearchInventoryTransactionsHandler(
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> repository)
    : IRequestHandler<SearchInventoryTransactionsCommand, PagedList<InventoryTransactionResponse>>
{
    public async Task<PagedList<InventoryTransactionResponse>> Handle(SearchInventoryTransactionsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchInventoryTransactionsSpec(request);

        var inventoryTransactions = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var inventoryTransactionResponses = inventoryTransactions.Select(it => new InventoryTransactionResponse
        {
            Id = it.Id,
            TransactionNumber = it.TransactionNumber,
            ItemId = it.ItemId,
            WarehouseId = it.WarehouseId,
            Quantity = it.Quantity,
            TransactionDate = it.TransactionDate,
            TransactionType = it.TransactionType,
            ReferenceType = it.ReferenceType,
            ReferenceId = it.ReferenceId,
            UnitCost = it.UnitCost,
            Notes = it.Notes
        }).ToList();

        return new PagedList<InventoryTransactionResponse>(inventoryTransactionResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
