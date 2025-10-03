using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;

public sealed class AddPickListItemHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<AddPickListItemCommand, AddPickListItemResponse>
{
    public async Task<AddPickListItemResponse> Handle(AddPickListItemCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.GetByIdAsync(request.PickListId, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.AddItem(
            request.ItemId,
            request.BinId,
            request.LotNumberId,
            request.SerialNumberId,
            request.QuantityToPick,
            request.Notes);

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new AddPickListItemResponse
        {
            Success = true
        };
    }
}
