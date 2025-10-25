namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Goods Receipts page logic. Provides CRUD and search over GoodsReceipt entities using the generated API client.
/// Supports partial receiving workflows including viewing details, marking received, canceling, and creating from purchase orders.
/// </summary>
public partial class GoodsReceipts
{
    private EntityServerTableContext<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel> Context = default!;
    private EntityTable<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel> _table = default!;

    private List<WarehouseResponse> _warehouses = new();
    private List<PurchaseOrderResponse> _purchaseOrders = new();
    
    private DefaultIdType? SearchWarehouseId { get; set; }
    private DefaultIdType? SearchPurchaseOrderId { get; set; }
    private string? SearchStatus { get; set; }
    private DateTime? SearchReceivedDateFrom { get; set; }
    private DateTime? SearchReceivedDateTo { get; set; }

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel>(
            entityName: "Goods Receipt",
            entityNamePlural: "Goods Receipts",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<GoodsReceiptResponse>(x => x.ReceiptNumber, "Receipt #", "ReceiptNumber"),
                new EntityField<GoodsReceiptResponse>(x => x.ReceivedDate, "Received Date", "ReceivedDate", typeof(DateTime)),
                new EntityField<GoodsReceiptResponse>(x => x.Status, "Status", "Status"),
                new EntityField<GoodsReceiptResponse>(x => x.PurchaseOrderId, "PO", "PurchaseOrderId"),
                new EntityField<GoodsReceiptResponse>(x => x.ItemCount, "Items", "ItemCount", typeof(int)),
                new EntityField<GoodsReceiptResponse>(x => x.TotalLines, "Total Lines", "TotalLines", typeof(int)),
                new EntityField<GoodsReceiptResponse>(x => x.ReceivedLines, "Received Lines", "ReceivedLines", typeof(int))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = filter.Adapt<SearchGoodsReceiptsCommand>();
                command.PurchaseOrderId = SearchPurchaseOrderId;
                command.Status = SearchStatus;
                command.ReceivedDateFrom = SearchReceivedDateFrom;
                command.ReceivedDateTo = SearchReceivedDateTo;
                var result = await Client.SearchGoodsReceiptsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GoodsReceiptResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateGoodsReceiptEndpointAsync("1", viewModel.Adapt<CreateGoodsReceiptCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteGoodsReceiptEndpointAsync("1", id).ConfigureAwait(false));
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadWarehousesAsync();
        await LoadPurchaseOrdersAsync();
    }

    /// <summary>
    /// Loads warehouses for the search filter dropdown.
    /// </summary>
    private async Task LoadWarehousesAsync()
    {
        try
        {
            var command = new SearchWarehousesCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchWarehousesEndpointAsync("1", command).ConfigureAwait(false);
            _warehouses = result.Items?.ToList() ?? new List<WarehouseResponse>();
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load warehouses: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Loads purchase orders for the search filter dropdown (showing only sent/partially received orders).
    /// </summary>
    private async Task LoadPurchaseOrdersAsync()
    {
        try
        {
            var command = new SearchPurchaseOrdersCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = ["OrderNumber"]
            };
            var result = await Client.SearchPurchaseOrdersEndpointAsync("1", command).ConfigureAwait(false);
            _purchaseOrders = result.Items?.Where(x => x.Status == "Sent" || x.Status == "PartiallyReceived").ToList() ?? new List<PurchaseOrderResponse>();
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load purchase orders: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Views the full details of a goods receipt in a dialog.
    /// </summary>
    private async Task ViewReceiptDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<GoodsReceiptDetailsDialog>
        {
            { x => x.GoodsReceiptId, id }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large, 
        };

        var dialog = await MudBlazor.DialogService.ShowAsync<GoodsReceiptDetailsDialog>("Goods Receipt Details", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Marks a draft goods receipt as received.
    /// </summary>
    private async Task MarkReceived(DefaultIdType id)
    {
        var confirmed = await MudBlazor.DialogService.ShowMessageBox(
            "Mark as Received",
            "Are you sure you want to mark this goods receipt as received? This will update inventory quantities.",
            yesText: "Mark Received",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                await Client.MarkReceivedEndpointAsync("1", id).ConfigureAwait(false);
                MudBlazor.Snackbar.Add("Goods receipt marked as received successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                MudBlazor.Snackbar.Add($"Failed to mark receipt as received: {ex.Message}", Severity.Error);
            }
        }
    }


    /// <summary>
    /// Opens dialog to create a goods receipt from a purchase order.
    /// </summary>
    private async Task CreateFromPurchaseOrder()
    {
        var options = new DialogOptions 
        { 
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large, 
        };

        var dialog = await MudBlazor.DialogService.ShowAsync<CreateReceiptFromPODialog>("Create Receipt from Purchase Order", options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Views the receiving history for a purchase order.
    /// </summary>
    private async Task ViewReceivingHistory(DefaultIdType purchaseOrderId)
    {
        var parameters = new DialogParameters<ReceivingHistoryDialog>
        {
            { x => x.PurchaseOrderId, purchaseOrderId }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large, 
        };

        await MudBlazor.DialogService.ShowAsync<ReceivingHistoryDialog>("Receiving History", parameters, options);
    }
}

/// <summary>
/// ViewModel for Goods Receipt add/edit operations.
/// </summary>
public partial class GoodsReceiptViewModel : CreateGoodsReceiptCommand
{
    public DefaultIdType Id { get; set; }
    public string? Status { get; set; }
    public new DateTime? ReceivedDate { get; set; } = DateTime.UtcNow;
}

