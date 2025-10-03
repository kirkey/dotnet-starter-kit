namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

public sealed record GetPutAwayTaskResponse
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
    public List<PutAwayTaskItemDto> Items { get; set; } = new();
}

public sealed record PutAwayTaskItemDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType ToBinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int QuantityToPutAway { get; set; }
    public int QuantityPutAway { get; set; }
    public string Status { get; set; } = PutAwayTaskItemStatus.Pending;
    public int SequenceNumber { get; set; }
    public DateTime? PutAwayDate { get; set; }
    public string? Notes { get; set; }
}
