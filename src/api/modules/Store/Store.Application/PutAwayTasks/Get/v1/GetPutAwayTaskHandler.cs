using Store.Domain.Exceptions.PutAwayTask;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

public sealed class GetPutAwayTaskHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<GetPutAwayTaskQuery, GetPutAwayTaskResponse>
{
    public async Task<GetPutAwayTaskResponse> Handle(GetPutAwayTaskQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetPutAwayTaskByIdSpec(request.PutAwayTaskId);
        var putAwayTask = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new PutAwayTaskNotFoundException(request.PutAwayTaskId);

        return new GetPutAwayTaskResponse
        {
            Id = putAwayTask.Id,
            TaskNumber = putAwayTask.TaskNumber,
            WarehouseId = putAwayTask.WarehouseId,
            GoodsReceiptId = putAwayTask.GoodsReceiptId,
            Status = putAwayTask.Status,
            Priority = putAwayTask.Priority,
            AssignedTo = putAwayTask.AssignedTo,
            StartDate = putAwayTask.StartDate,
            CompletedDate = putAwayTask.CompletedDate,
            PutAwayStrategy = putAwayTask.PutAwayStrategy,
            Notes = putAwayTask.Notes,
            TotalLines = putAwayTask.TotalLines,
            CompletedLines = putAwayTask.CompletedLines,
            CompletionPercentage = putAwayTask.GetCompletionPercentage(),
            Items = putAwayTask.Items.Select(item => new PutAwayTaskItemDto
            {
                Id = item.Id,
                ItemId = item.ItemId,
                ToBinId = item.ToBinId,
                LotNumberId = item.LotNumberId,
                SerialNumberId = item.SerialNumberId,
                QuantityToPutAway = item.QuantityToPutAway,
                QuantityPutAway = item.QuantityPutAway,
                Status = item.Status,
                SequenceNumber = item.SequenceNumber,
                PutAwayDate = item.PutAwayDate,
                Notes = item.Notes
            }).ToList()
        };
    }
}
