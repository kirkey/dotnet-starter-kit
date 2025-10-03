using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;

public sealed class DeletePutAwayTaskHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<DeletePutAwayTaskCommand, DeletePutAwayTaskResponse>
{
    public async Task<DeletePutAwayTaskResponse> Handle(DeletePutAwayTaskCommand request, CancellationToken cancellationToken)
    {
        var putAwayTask = await repository.GetByIdAsync(request.PutAwayTaskId, cancellationToken).ConfigureAwait(false)
            ?? throw new PutAwayTaskNotFoundException(request.PutAwayTaskId);

        await repository.DeleteAsync(putAwayTask, cancellationToken).ConfigureAwait(false);

        return new DeletePutAwayTaskResponse(putAwayTask.Id);
    }
}
