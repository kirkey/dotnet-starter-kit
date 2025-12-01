namespace FSH.Starter.Blazor.Client.Pages.Store.GoodsReceipts;

/// <summary>
/// Goods Receipts page logic. Provides CRUD and search over GoodsReceipt entities using the generated API client.
/// Supports partial receiving workflows including viewing details, marking received, canceling, and creating from purchase orders.
/// </summary>
public partial class GoodsReceipts
{
    

    private EntityServerTableContext<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel> Context = null!;
    private EntityTable<GoodsReceiptResponse, DefaultIdType, GoodsReceiptViewModel> _table = null!;

    private ClientPreference _preference = new();

    private List<WarehouseResponse> _warehouses = [];
    private List<PurchaseOrderResponse> _purchaseOrders = [];
    
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
                new EntityField<GoodsReceiptResponse>(x => x.ReceivedDate, "Received Date", "ReceivedDate", typeof(DateOnly)),
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
                var command = new SearchGoodsReceiptsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    PurchaseOrderId = SearchPurchaseOrderId,
                    Status = SearchStatus,
                    ReceivedDateFrom = SearchReceivedDateFrom,
                    ReceivedDateTo = SearchReceivedDateTo
                };
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
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

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
            var command = new SearchWarehousesRequest
            {
                PageNumber = 1,
                PageSize = 100,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchWarehousesEndpointAsync("1", command).ConfigureAwait(false);
            _warehouses = result.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load warehouses: {ex.Message}", Severity.Error);
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
            _purchaseOrders = result.Items?.Where(x => x.Status is "Sent" or "PartiallyReceived").ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load purchase orders: {ex.Message}", Severity.Error);
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

        var dialog = await DialogService.ShowAsync<GoodsReceiptDetailsDialog>("Goods Receipt Details", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Marks a draft goods receipt as received.
    /// </summary>
    private async Task MarkReceived(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Mark as Received",
            "Are you sure you want to mark this goods receipt as received? This will update inventory quantities.",
            yesText: "Mark Received",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new MarkReceivedCommand { GoodsReceiptId = id };
                await Client.MarkReceivedEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Goods receipt marked as received successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to mark receipt as received: {ex.Message}", Severity.Error);
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

        var dialog = await DialogService.ShowAsync<CreateReceiptFromPODialog>("Create Receipt from Purchase Order", options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
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

        await DialogService.ShowAsync<ReceivingHistoryDialog>("Receiving History", parameters, options);
    }

    /// <summary>
    /// Show goods receipts help dialog.
    /// </summary>
    private async Task ShowGoodsReceiptsHelp()
    {
        await DialogService.ShowAsync<GoodsReceiptsHelpDialog>("Goods Receipts Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
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

