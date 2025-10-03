using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Complete.v1;

public sealed class CompletePutAwayHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<CompletePutAwayCommand, CompletePutAwayResponse>
{
    public async Task<CompletePutAwayResponse> Handle(CompletePutAwayCommand request, CancellationToken cancellationToken)
    {
        var putAwayTask = await repository.GetByIdAsync(request.PutAwayTaskId, cancellationToken).ConfigureAwait(false)
            ?? throw new PutAwayTaskNotFoundException(request.PutAwayTaskId);

        putAwayTask.CompletePutAway();

        await repository.UpdateAsync(putAwayTask, cancellationToken).ConfigureAwait(false);

        return new CompletePutAwayResponse(putAwayTask.Id);
    }
}
