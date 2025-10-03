namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

/// <summary>
/// DTO for adding an item to a put-away task. Used as the request body for the endpoint.
/// </summary>
public sealed class AddPutAwayTaskItemRequestDto
{
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType ToBinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}

