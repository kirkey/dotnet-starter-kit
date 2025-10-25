namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Dialog for creating a goods receipt from a purchase order.
/// Supports selecting PO, viewing available items, and creating receipt with selected quantities.
/// </summary>
public partial class CreateReceiptFromPODialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    private List<PurchaseOrderResponse> _purchaseOrders = new();
    private List<SupplierResponse> _suppliers = new();
    private List<PurchaseOrderItemResponse> _poItems = new();
    private PurchaseOrderResponse? _selectedPurchaseOrder;
    private Dictionary<DefaultIdType, int> _receiveQuantities = new();
    private HashSet<DefaultIdType> _selectedItemIds = new();
    private bool _selectAll = false;
    private bool _loading = true;
    private string _searchString = "";

    // Receipt fields
    private string _receiptNumber = $"GR-{DateTime.UtcNow:yyyy-MM-ddTHH-mm-ss}";
    private DateTime? _receivedDate = DateTime.UtcNow;
    private DefaultIdType _warehouseId;
    private string _notes = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Loads purchase orders and suppliers.
    /// </summary>
    private async Task LoadDataAsync()
    {
        _loading = true;
        try
        {
            // Load purchase orders that are sent or partially received
            var poCommand = new SearchPurchaseOrdersCommand
            {
                PageNumber = 1,
                PageSize = 100,
                OrderBy = new[] { "OrderDate desc" }
            };
            var poResult = await Client.SearchPurchaseOrdersEndpointAsync("1", poCommand).ConfigureAwait(false);
            _purchaseOrders = poResult.Items?
                .Where(x => x.Status == "Sent" || x.Status == "PartiallyReceived")
                .ToList() ?? new List<PurchaseOrderResponse>();

            // Load suppliers for display
            var supplierCommand = new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = new[] { "Name" }
            };
            var supplierResult = await Client.SearchSuppliersEndpointAsync("1", supplierCommand).ConfigureAwait(false);
            _suppliers = supplierResult.Items?.ToList() ?? new List<SupplierResponse>();
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load data: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Selects a purchase order and loads its items.
    /// </summary>
    private async Task SelectPurchaseOrder(PurchaseOrderResponse po)
    {
        _selectedPurchaseOrder = po;
        _loading = true;
        try
        {
            // Load PO items available for receiving
            var result = await Client.GetPurchaseOrderItemsForReceivingEndpointAsync("1", po.Id).ConfigureAwait(false);
            
            // Convert to PurchaseOrderItemResponse format for compatibility with existing UI
            _poItems = result.Items?
                .Where(x => !x.IsFullyReceived)
                .Select(x => new PurchaseOrderItemResponse
                {
                    Id = x.PurchaseOrderItemId,
                    PurchaseOrderId = result.PurchaseOrderId,
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

            // Initialize receive quantities with remaining quantities
            _receiveQuantities.Clear();
            foreach (var item in result.Items ?? new List<PurchaseOrderItemForReceiving>())
            {
                if (!item.IsFullyReceived)
                {
                    _receiveQuantities[item.PurchaseOrderItemId] = item.RemainingQuantity;
                }
            }
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load purchase order items: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Returns to purchase order selection.
    /// </summary>
    private void BackToPOSelection()
    {
        _selectedPurchaseOrder = null;
        _poItems.Clear();
        _receiveQuantities.Clear();
        _selectedItemIds.Clear();
        _selectAll = false;
    }

    /// <summary>
    /// Filters purchase orders based on search string.
    /// </summary>
    private bool FilterPurchaseOrders(PurchaseOrderResponse po)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;
        if (po.OrderNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;
        if (GetSupplierName(po.SupplierId).Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    /// <summary>
    /// Gets supplier name by ID.
    /// </summary>
    private string GetSupplierName(DefaultIdType supplierId)
    {
        return _suppliers.FirstOrDefault(x => x.Id == supplierId)?.Name ?? "Unknown";
    }

    /// <summary>
    /// Gets status color.
    /// </summary>
    private static Color GetStatusColor(string? status)
    {
        return status switch
        {
            "Sent" => Color.Info,
            "PartiallyReceived" => Color.Warning,
            _ => Color.Default
        };
    }

    /// <summary>
    /// Toggles selection of all items.
    /// </summary>
    private void ToggleSelectAll(bool value)
    {
        _selectAll = value;
        if (value)
        {
            _selectedItemIds = _poItems.Select(x => x.Id).ToHashSet();
        }
        else
        {
            _selectedItemIds.Clear();
        }
    }

    /// <summary>
    /// Checks if an item is selected.
    /// </summary>
    private bool IsItemSelected(DefaultIdType itemId) => _selectedItemIds.Contains(itemId);

    /// <summary>
    /// Toggles selection of an item.
    /// </summary>
    private void ToggleItemSelection(DefaultIdType itemId, bool value)
    {
        if (value)
        {
            _selectedItemIds.Add(itemId);
        }
        else
        {
            _selectedItemIds.Remove(itemId);
        }
    }

    /// <summary>
    /// Checks if receipt can be created.
    /// </summary>
    private bool CanCreateReceipt()
    {
        return !string.IsNullOrWhiteSpace(_receiptNumber) &&
               _receivedDate.HasValue &&
               _warehouseId != DefaultIdType.Empty &&
               _selectedItemIds.Any();
    }

    /// <summary>
    /// Creates the goods receipt.
    /// </summary>
    private async Task CreateReceipt()
    {
        if (!CanCreateReceipt())
        {
            MudBlazor.Snackbar.Add("Please fill in all required fields and select at least one item", Severity.Warning);
            return;
        }

        try
        {
            // Create the goods receipt
            var createCommand = new CreateGoodsReceiptCommand
            {
                ReceiptNumber = _receiptNumber,
                PurchaseOrderId = _selectedPurchaseOrder!.Id,
                WarehouseId = _warehouseId,
                ReceivedDate = _receivedDate!.Value,
                Notes = _notes
            };

            var createResponse = await Client.CreateGoodsReceiptEndpointAsync("1", createCommand).ConfigureAwait(false);

            // Add selected items to the receipt
            foreach (var itemId in _selectedItemIds)
            {
                var poItem = _poItems.First(x => x.Id == itemId);
                var quantity = _receiveQuantities[itemId];

                if (quantity > 0)
                {
                    var addItemCommand = new AddGoodsReceiptItemCommand
                    {
                        GoodsReceiptId = createResponse.Id,
                        ItemId = poItem.ItemId,
                        Quantity = quantity,
                        UnitCost = poItem.UnitPrice,
                        PurchaseOrderItemId = itemId
                    };

                    await Client.AddGoodsReceiptItemEndpointAsync("1", createResponse.Id, addItemCommand).ConfigureAwait(false);
                }
            }

            MudBlazor.Snackbar.Add($"Goods receipt {_receiptNumber} created successfully with {_selectedItemIds.Count} items", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to create goods receipt: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();
}
