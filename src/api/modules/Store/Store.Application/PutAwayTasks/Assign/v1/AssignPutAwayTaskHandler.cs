using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Assign.v1;

public sealed class AssignPutAwayTaskHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<AssignPutAwayTaskCommand, AssignPutAwayTaskResponse>
{
    public async Task<AssignPutAwayTaskResponse> Handle(AssignPutAwayTaskCommand request, CancellationToken cancellationToken)
    {
        var putAwayTask = await repository.GetByIdAsync(request.PutAwayTaskId, cancellationToken).ConfigureAwait(false)
            ?? throw new PutAwayTaskNotFoundException(request.PutAwayTaskId);

        putAwayTask.AssignToWorker(request.AssignedTo);

        await repository.UpdateAsync(putAwayTask, cancellationToken).ConfigureAwait(false);

        return new AssignPutAwayTaskResponse(putAwayTask.Id);
    }
}
