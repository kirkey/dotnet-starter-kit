using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Start.v1;

public sealed class StartPutAwayHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<StartPutAwayCommand, StartPutAwayResponse>
{
    public async Task<StartPutAwayResponse> Handle(StartPutAwayCommand request, CancellationToken cancellationToken)
    {
        var putAwayTask = await repository.GetByIdAsync(request.PutAwayTaskId, cancellationToken).ConfigureAwait(false)
            ?? throw new PutAwayTaskNotFoundException(request.PutAwayTaskId);

        putAwayTask.StartPutAway();

        await repository.UpdateAsync(putAwayTask, cancellationToken).ConfigureAwait(false);

        return new StartPutAwayResponse(putAwayTask.Id);
    }
}
