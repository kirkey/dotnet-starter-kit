using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

public sealed class AddPutAwayTaskItemHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<AddPutAwayTaskItemCommand, AddPutAwayTaskItemResponse>
{
    public async Task<AddPutAwayTaskItemResponse> Handle(AddPutAwayTaskItemCommand request, CancellationToken cancellationToken)
    {
        var putAwayTask = await repository.GetByIdAsync(request.PutAwayTaskId, cancellationToken).ConfigureAwait(false)
            ?? throw new PutAwayTaskNotFoundException(request.PutAwayTaskId);

        putAwayTask.AddItem(
            request.ItemId,
            request.ToBinId,
            request.LotNumberId,
            request.SerialNumberId,
            request.QuantityToPutAway,
            request.SequenceNumber,
            request.Notes);

        await repository.UpdateAsync(putAwayTask, cancellationToken).ConfigureAwait(false);

        return new AddPutAwayTaskItemResponse(putAwayTask.Id);
    }
}
