namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Dialog for adding items to a goods receipt.
/// Supports linking to purchase order items for partial receiving workflows.
/// </summary>
public partial class GoodsReceiptItemDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType GoodsReceiptId { get; set; }
    [Parameter] public DefaultIdType? PurchaseOrderId { get; set; }

    private MudForm _form = default!;
    private AddGoodsReceiptItemModel _model = new();
    private bool _hasVariance => _model.QuantityOrdered.HasValue && 
                                  _model.QuantityOrdered != _model.QuantityReceived;

    protected override void OnInitialized()
    {
        _model.GoodsReceiptId = GoodsReceiptId;
        _model.QualityStatus = "Pending";
    }

    /// <summary>
    /// Submits the form to add the item to the receipt.
    /// </summary>
    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        try
        {
            // Map to the simpler API command structure
            var command = new AddGoodsReceiptItemCommand
            {
                GoodsReceiptId = _model.GoodsReceiptId,
                ItemId = _model.ItemId,
                Quantity = (int)_model.QuantityReceived,
                UnitCost = _model.UnitCost ?? 0,
                PurchaseOrderItemId = _model.PurchaseOrderItemId
            };

            await Blazor.Client.AddGoodsReceiptItemEndpointAsync("1", GoodsReceiptId, command).ConfigureAwait(false);
            MudBlazor.Snackbar.Add("Item added successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to add item: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();
}

/// <summary>
/// Model for adding goods receipt items with extended properties beyond the basic API command.
/// </summary>
public class AddGoodsReceiptItemModel
{
    public DefaultIdType GoodsReceiptId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? PurchaseOrderItemId { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal? QuantityOrdered { get; set; }
    public decimal? QuantityRejected { get; set; }
    public decimal? UnitCost { get; set; }
    public string? LotNumber { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? QualityStatus { get; set; }
    public string? InspectedBy { get; set; }
    public DateTime? InspectionDate { get; set; }
    public string? Notes { get; set; }
    public string? VarianceReason { get; set; }
}
