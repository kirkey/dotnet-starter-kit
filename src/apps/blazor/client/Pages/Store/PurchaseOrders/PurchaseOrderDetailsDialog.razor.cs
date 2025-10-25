namespace FSH.Starter.Blazor.Client.Pages.Store.PurchaseOrders;

/// <summary>
/// Dialog component for viewing purchase order details and managing order items.
/// Provides comprehensive view of purchase order information with inline item management.
/// </summary>
public partial class PurchaseOrderDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    /// <summary>
    /// The purchase order ID to display details for.
    /// </summary>
    [Parameter] 
    public DefaultIdType PurchaseOrderId { get; set; }

    private PurchaseOrderResponse? _purchaseOrder;
    private string _supplierName = string.Empty;
    private bool _loading;

    /// <summary>
    /// Loads the purchase order details when the component initializes.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadPurchaseOrderAsync();
    }

    /// <summary>
    /// Loads the purchase order details from the API.
    /// </summary>
    private async Task LoadPurchaseOrderAsync()
    {
        _loading = true;
        try
        {
            _purchaseOrder = await Client.GetPurchaseOrderEndpointAsync("1", PurchaseOrderId).ConfigureAwait(false);
            
            // Load supplier name
            if (_purchaseOrder?.SupplierId != null && _purchaseOrder.SupplierId != DefaultIdType.Empty)
            {
                try
                {
                    var supplier = await Client.GetSupplierEndpointAsync("1", _purchaseOrder.SupplierId).ConfigureAwait(false);
                    _supplierName = supplier?.Name ?? "Unknown";
                }
                catch
                {
                    _supplierName = "Unknown";
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load purchase order: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Handles the event when items are changed in the PurchaseOrderItems component.
    /// Reloads the purchase order to update totals.
    /// </summary>
    private async Task HandleItemsChanged()
    {
        await LoadPurchaseOrderAsync();
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }
}

