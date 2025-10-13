using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Search.v1;

/// <summary>
/// Handler for searching inventory transactions with filtering, sorting, and pagination support.
/// </summary>
/// <remarks>
/// Processes SearchInventoryTransactionsCommand to retrieve a paginated list of inventory transactions
/// based on search criteria such as transaction number, item ID, warehouse ID, transaction type, and date ranges.
/// Uses SearchInventoryTransactionsSpec for building the query specification.
/// </remarks>
public sealed class SearchInventoryTransactionsHandler(
    [FromKeyedServices("store:inventorytransactions")] IReadRepository<InventoryTransaction> repository)
    : IRequestHandler<SearchInventoryTransactionsCommand, PagedList<InventoryTransactionResponse>>
{
    /// <summary>
    /// Handles the search request for inventory transactions.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of inventory transaction responses.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<InventoryTransactionResponse>> Handle(SearchInventoryTransactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInventoryTransactionsSpec(request);

        var inventoryTransactions = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var inventoryTransactionResponses = inventoryTransactions.Select(it => new InventoryTransactionResponse
        {
            Id = it.Id,
            TransactionNumber = it.TransactionNumber,
            ItemId = it.ItemId,
            WarehouseId = it.WarehouseId ?? DefaultIdType.Empty, // Handle nullable WarehouseId by providing default value
            Quantity = it.Quantity,
            TransactionDate = it.TransactionDate,
            TransactionType = it.TransactionType,
            ReferenceType = it.TransactionType, // Using TransactionType as ReferenceType since ReferenceType doesn't exist in domain model
            ReferenceId = null, // ReferenceId property doesn't exist in domain model, setting to null
            UnitCost = it.UnitCost,
            Notes = it.Reason // Domain model uses Reason instead of Description
        }).ToList();

        return new PagedList<InventoryTransactionResponse>(inventoryTransactionResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
