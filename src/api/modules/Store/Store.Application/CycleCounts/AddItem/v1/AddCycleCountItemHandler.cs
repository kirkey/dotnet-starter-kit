using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.v1;

public sealed class AddCycleCountItemHandler(
    ILogger<AddCycleCountItemHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<AddCycleCountItemCommand, AddCycleCountItemResponse>
{
    public async Task<AddCycleCountItemResponse> Handle(AddCycleCountItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var cc = await repository.GetByIdAsync(request.CycleCountId, cancellationToken).ConfigureAwait(false);
        _ = cc ?? throw new CycleCountNotFoundException(request.CycleCountId);
        var beforeCount = cc.Items.Count;
        var added = cc.AddItem(request.GroceryItemId, request.SystemQuantity, request.CountedQuantity);
        await repository.UpdateAsync(cc, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("added item to cycle count {CycleCountId} - total items now {Total}", cc.Id, cc.Items.Count);
        var itemId = cc.Items.Last().Id;
        return new AddCycleCountItemResponse(itemId, cc.Id);
    }
}

