namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

public sealed record PutAwayTaskResponse
{
    public DefaultIdType Id { get; set; }
    public string TaskNumber { get; set; } = default!;
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? GoodsReceiptId { get; set; }
    public string Status { get; set; } = PutAwayTaskStatus.Created;
    public int Priority { get; set; } = PutAwayTaskPriority.Normal;
    public string? AssignedTo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? PutAwayStrategy { get; set; }
    public string? Notes { get; set; }
    public int TotalLines { get; set; }
    public int CompletedLines { get; set; }
    public decimal CompletionPercentage { get; set; }
}
