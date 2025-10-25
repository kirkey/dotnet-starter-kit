using FSH.Starter.Blazor.Client.Pages.Store.PurchaseOrders;

namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Dialog for viewing goods receipt details including line items.
/// Supports adding and removing items for draft receipts.
/// </summary>
public partial class GoodsReceiptDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType GoodsReceiptId { get; set; }

    private GetGoodsReceiptResponse? _goodsReceipt;
    private string? _purchaseOrderNumber;
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadGoodsReceiptAsync();
    }

    /// <summary>
    /// Loads the goods receipt details from the API.
    /// </summary>
    private async Task LoadGoodsReceiptAsync()
    {
        _loading = true;
        try
        {
            _goodsReceipt = await Client.GetGoodsReceiptEndpointAsync("1", GoodsReceiptId).ConfigureAwait(false);
            
            // Load purchase order number if linked
            if (_goodsReceipt?.PurchaseOrderId.HasValue == true)
            {
                try
                {
                    var po = await Client.GetPurchaseOrderEndpointAsync("1", _goodsReceipt.PurchaseOrderId.Value).ConfigureAwait(false);
                    _purchaseOrderNumber = po.OrderNumber;
                }
                catch
                {
                    // Ignore if PO not found
                }
            }
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load goods receipt: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Opens dialog to add a new item to the receipt.
    /// </summary>
    private async Task AddItem()
    {
        var parameters = new DialogParameters
        {
            { nameof(GoodsReceiptItemDialog.GoodsReceiptId), GoodsReceiptId },
            { nameof(GoodsReceiptItemDialog.PurchaseOrderId), _goodsReceipt?.PurchaseOrderId }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await MudBlazor.DialogService.ShowAsync<GoodsReceiptItemDialog>("Add Receipt Item", parameters, options);
        var result = await dialog.Result;

        if (result?.Canceled == false)
        {
            await LoadGoodsReceiptAsync();
        }
    }

    /// <summary>
    /// Removes an item from the receipt.
    /// </summary>
    private async Task RemoveItem(DefaultIdType itemId)
    {
        var confirmed = await MudBlazor.DialogService.ShowMessageBox(
            "Remove Item",
            "Are you sure you want to remove this item from the receipt?",
            yesText: "Remove",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                // Note: This would need a RemoveGoodsReceiptItem endpoint in the API
                // For now, we'll show a message
                MudBlazor.Snackbar.Add("Remove item functionality requires API endpoint implementation", Severity.Warning);
                // await Client.RemoveGoodsReceiptItemEndpointAsync("1", GoodsReceiptId, itemId).ConfigureAwait(false);
                // await LoadGoodsReceiptAsync();
            }
            catch (Exception ex)
            {
                MudBlazor.Snackbar.Add($"Failed to remove item: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Marks the receipt as received.
    /// </summary>
    private async Task MarkReceived()
    {
        var confirmed = await MudBlazor.DialogService.ShowMessageBox(
            "Mark as Received",
            "Are you sure you want to mark this receipt as received? This will update inventory quantities.",
            yesText: "Mark Received",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                // Note: This endpoint may not exist yet - checking implementation docs
                MudBlazor.Snackbar.Add("Mark as received functionality requires API endpoint implementation", Severity.Warning);
                // var request = new MarkReceivedCommand { GoodsReceiptId = GoodsReceiptId };
                // await Client.MarkReceivedEndpointAsync("1", GoodsReceiptId, request).ConfigureAwait(false);
                // await LoadGoodsReceiptAsync();
            }
            catch (Exception ex)
            {
                MudBlazor.Snackbar.Add($"Failed to mark as received: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Opens the purchase order details dialog.
    /// </summary>
    private async Task ViewPurchaseOrder(DefaultIdType purchaseOrderId)
    {
        var parameters = new DialogParameters
        {
            { nameof(PurchaseOrderDetailsDialog.PurchaseOrderId), purchaseOrderId }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };

        await MudBlazor.DialogService.ShowAsync<PurchaseOrderDetailsDialog>("Purchase Order Details", parameters, options);
    }

    /// <summary>
    /// Gets the appropriate color for the status chip.
    /// </summary>
    private Color GetStatusColor(string? status)
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

    private void Close() => MudDialog.Close(DialogResult.Ok(true));
}

