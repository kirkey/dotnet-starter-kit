using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;

/// <summary>
/// Handler for adding an item to a pick list.
/// Follows the aggregate pattern: creates PickListItem as a separate entity and updates parent PickList totals.
/// </summary>
public sealed class AddPickListItemHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> pickListRepository,
    [FromKeyedServices("store:picklistitems")] IRepository<PickListItem> pickListItemRepository)
    : IRequestHandler<AddPickListItemCommand, AddPickListItemResponse>
{
    /// <summary>
    /// Handles the add pick list item command.
    /// </summary>
    /// <param name="request">The add item command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The add item response.</returns>
    /// <exception cref="PickListNotFoundException">Thrown when the pick list is not found.</exception>
    public async Task<AddPickListItemResponse> Handle(AddPickListItemCommand request, CancellationToken cancellationToken)
    {
        // Ensure pick list exists and is modifiable (status = Created)
        var pickList = await pickListRepository.GetByIdAsync(request.PickListId, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        if (pickList.Status != "Created")
            throw new InvalidOperationException($"Cannot add items to pick list in {pickList.Status} status. Only Created pick lists can be modified.");

        // Create the pick list item as a separate aggregate
        var pickListItem = PickListItem.Create(
            request.PickListId,
            request.ItemId,
            request.BinId,
            request.LotNumberId,
            request.SerialNumberId,
            request.QuantityToPick,
            request.Notes);

        await pickListItemRepository.AddAsync(pickListItem, cancellationToken).ConfigureAwait(false);

        // Update pick list totals
        pickList.IncrementTotalLines();
        await pickListRepository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new AddPickListItemResponse
        {
            Success = true
        };
    }
}
