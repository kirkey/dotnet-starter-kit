using Store.Domain.Exceptions.GroceryItem;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Delete.v1;

public sealed class DeleteGroceryItemHandler(
    ILogger<DeleteGroceryItemHandler> logger,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository)
    : IRequestHandler<DeleteGroceryItemCommand>
{
    public async Task Handle(DeleteGroceryItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var groceryItem = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = groceryItem ?? throw new GroceryItemNotFoundException(request.Id);
        await repository.DeleteAsync(groceryItem, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("grocery item with id : {GroceryItemId} successfully deleted", groceryItem.Id);
    }
}
