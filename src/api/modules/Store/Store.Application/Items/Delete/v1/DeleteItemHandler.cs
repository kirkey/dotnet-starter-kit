using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Delete.v1;

public sealed class DeleteItemHandler(
    ILogger<DeleteItemHandler> logger,
    [FromKeyedServices("store:items")] IRepository<Item> repository)
    : IRequestHandler<DeleteItemCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (item is null)
            throw new ItemNotFoundException(request.Id);

        await repository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("item deleted {ItemId}", item.Id);
        return item.Id;
    }
}
