namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Dialog for viewing the receiving history of a purchase order.
/// Shows all goods receipts created against the PO and item-level receiving progress.
/// </summary>
public partial class ReceivingHistoryDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType PurchaseOrderId { get; set; }

    private PurchaseOrderResponse? _purchaseOrder;
    private string? _supplierName;
    private List<GoodsReceiptResponse> _receipts = new();
    private List<PurchaseOrderItemResponse> _poItems = new();
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Loads purchase order, receipts, and items data.
    /// </summary>
    private async Task LoadDataAsync()
    {
        _loading = true;
        try
        {
            // Load purchase order
            _purchaseOrder = await Client.GetPurchaseOrderEndpointAsync("1", PurchaseOrderId).ConfigureAwait(false);

            // Load supplier name
            if (_purchaseOrder?.SupplierId != null)
            {
                try
                {
                    var supplier = await Client.GetSupplierEndpointAsync("1", _purchaseOrder.SupplierId).ConfigureAwait(false);
                    _supplierName = supplier.Name;
                }
                catch
                {
                    // Ignore if supplier not found
                }
            }

            // Load goods receipts for this PO
            var receiptsCommand = new SearchGoodsReceiptsCommand
            {
                PurchaseOrderId = PurchaseOrderId,
                PageNumber = 1,
                PageSize = 100,
                OrderBy = new[] { "ReceivedDate desc" }
            };
            var receiptsResult = await Client.SearchGoodsReceiptsEndpointAsync("1", receiptsCommand).ConfigureAwait(false);
            _receipts = receiptsResult.Items?.ToList() ?? new List<GoodsReceiptResponse>();

            // Load PO items to show receiving progress
            var itemsResult = await Client.GetPurchaseOrderItemsForReceivingEndpointAsync("1", PurchaseOrderId).ConfigureAwait(false);
            
            // Convert to PurchaseOrderItemResponse format for compatibility with existing UI
            _poItems = itemsResult.Items?
                .Select(x => new PurchaseOrderItemResponse
                {
                    Id = x.PurchaseOrderItemId,
                    PurchaseOrderId = itemsResult.PurchaseOrderId,
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ItemSku = x.ItemSku,
                    Quantity = x.OrderedQuantity,
                    UnitPrice = x.UnitPrice,
                    DiscountAmount = 0,
                    TotalPrice = x.UnitPrice * x.OrderedQuantity,
                    ReceivedQuantity = x.ReceivedQuantity,
                    Notes = null
                })
                .ToList() ?? new List<PurchaseOrderItemResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load receiving history: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Opens the goods receipt details dialog.
    /// </summary>
    private async Task ViewReceiptDetails(DefaultIdType receiptId)
    {
        var parameters = new DialogParameters
        {
            { nameof(GoodsReceiptDetailsDialog.GoodsReceiptId), receiptId }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<GoodsReceiptDetailsDialog>("Goods Receipt Details", parameters, options);
        var result = await dialog.Result;

        // Reload data if changes were made
        if (result?.Canceled == false)
        {
            await LoadDataAsync();
        }
    }

    /// <summary>
    /// Gets the appropriate color for PO status.
    /// </summary>
    private static Color GetStatusColor(string? status)
    {
        return status switch
        {
            "Draft" => Color.Default,
            "Submitted" => Color.Info,
            "Approved" => Color.Primary,
            "Sent" => Color.Warning,
            "PartiallyReceived" => Color.Warning,
            "Received" => Color.Success,
            "Cancelled" => Color.Error,
            _ => Color.Default
        };
    }

    /// <summary>
    /// Gets the appropriate color for receipt status.
    /// </summary>
    private static Color GetReceiptStatusColor(string? status)
    {
        return status switch
        {
            "Draft" => Color.Default,
            "Received" => Color.Success,
            "Cancelled" => Color.Error,
            "Posted" => Color.Primary,
            _ => Color.Default
        };
    }

    /// <summary>
    /// Gets the progress color based on percentage.
    /// </summary>
    private static Color GetProgressColor(int percentage)
    {
        return percentage switch
        {
            >= 100 => Color.Success,
            >= 75 => Color.Info,
            >= 50 => Color.Warning,
            _ => Color.Error
        };
    }

    private void Close() => MudDialog.Close(DialogResult.Ok(false));
}
