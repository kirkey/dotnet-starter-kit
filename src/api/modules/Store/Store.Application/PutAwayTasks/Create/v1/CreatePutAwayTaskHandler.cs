using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;

public sealed class CreatePutAwayTaskHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<CreatePutAwayTaskCommand, CreatePutAwayTaskResponse>
{
    public async Task<CreatePutAwayTaskResponse> Handle(CreatePutAwayTaskCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate task number
        var existingTask = await repository.FirstOrDefaultAsync(
            new PutAwayTaskByNumberSpec(request.TaskNumber), cancellationToken).ConfigureAwait(false);

        if (existingTask is not null)
        {
            throw new PutAwayTaskAlreadyExistsException(request.TaskNumber);
        }

        var putAwayTask = PutAwayTask.Create(
            request.TaskNumber,
            request.WarehouseId,
            request.GoodsReceiptId,
            request.Priority,
            request.PutAwayStrategy,
            request.Notes);

        await repository.AddAsync(putAwayTask, cancellationToken).ConfigureAwait(false);

        return new CreatePutAwayTaskResponse(putAwayTask.Id);
    }
}
