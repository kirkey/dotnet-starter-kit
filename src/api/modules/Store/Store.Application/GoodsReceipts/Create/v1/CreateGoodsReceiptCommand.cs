namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

/// <summary>
/// Command to create a new goods receipt.
/// </summary>
public sealed class CreateGoodsReceiptCommand : IRequest<CreateGoodsReceiptResponse>
{
    /// <summary>
    /// Unique receipt number for tracking deliveries.
    /// </summary>
    public string ReceiptNumber { get; set; } = default!;
    
    /// <summary>
    /// Optional reference to the purchase order being fulfilled.
    /// </summary>
    public DefaultIdType? PurchaseOrderId { get; set; }
    
    /// <summary>
    /// Date when goods were received.
    /// </summary>
    public DateTime ReceivedDate { get; set; }
    
    /// <summary>
    /// Optional notes or comments about the goods receipt.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Optional name for the goods receipt.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Optional description for the goods receipt.
    /// </summary>
    public string? Description { get; set; }
}
