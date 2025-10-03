namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;

public sealed record CreatePutAwayTaskCommand : IRequest<CreatePutAwayTaskResponse>
{
    public string TaskNumber { get; set; } = default!;
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? GoodsReceiptId { get; set; }
    public int Priority { get; set; }
    public string? PutAwayStrategy { get; set; }
    public string? Notes { get; set; }
}
