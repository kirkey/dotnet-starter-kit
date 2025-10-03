namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Purchase Orders page logic. Provides CRUD and search over PurchaseOrder entities using the generated API client.
/// Mirrors the structure of Items and Suppliers pages for consistency.
/// </summary>
public partial class PurchaseOrders
{
    [Inject] protected IClient ApiClient { get; set; } = default!;

    private EntityServerTableContext<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel> Context { get; set; } = default!;
    private EntityTable<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel> _table = default!;

    private List<SupplierResponse> _suppliers = [];

    // Advanced Search
    private DefaultIdType? _searchSupplierId;
    private DefaultIdType? SearchSupplierId
    {
        get => _searchSupplierId;
        set
        {
            _searchSupplierId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchStatus;
    private string? SearchStatus
    {
        get => _searchStatus;
        set
        {
            _searchStatus = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchFromDate;
    private DateTime? SearchFromDate
    {
        get => _searchFromDate;
        set
        {
            _searchFromDate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchToDate;
    private DateTime? SearchToDate
    {
        get => _searchToDate;
        set
        {
            _searchToDate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel>(
            entityName: "Purchase Orders",
            entityNamePlural: "Purchase Orders",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<PurchaseOrderResponse>(x => x.OrderNumber, "Order Number", "OrderNumber"),
                new EntityField<PurchaseOrderResponse>(x => x.SupplierId, "Supplier ID", "SupplierId"),
                new EntityField<PurchaseOrderResponse>(x => x.OrderDate, "Order Date", "OrderDate", typeof(DateTime)),
                new EntityField<PurchaseOrderResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PurchaseOrderResponse>(x => x.TotalAmount, "Total Amount", "TotalAmount", typeof(decimal)),
                new EntityField<PurchaseOrderResponse>(x => x.ExpectedDeliveryDate, "Expected Delivery", "ExpectedDeliveryDate", typeof(DateTime?)),
                new EntityField<PurchaseOrderResponse>(x => x.IsUrgent, "Urgent", "IsUrgent", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await ApiClient.GetPurchaseOrderEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<PurchaseOrderViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = new SearchPurchaseOrdersCommand
                {
                    PageNumber = paginationFilter.PageNumber,
                    PageSize = paginationFilter.PageSize,
                    SearchTerm = paginationFilter.Keyword,
                    SupplierId = _searchSupplierId,
                    Status = _searchStatus,
                    FromDate = _searchFromDate,
                    ToDate = _searchToDate
                };
                var result = await ApiClient.SearchPurchaseOrdersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PurchaseOrderResponse>>();
            },
            createFunc: async viewModel =>
            {
                await ApiClient.CreatePurchaseOrderEndpointAsync("1", viewModel.Adapt<CreatePurchaseOrderCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await ApiClient.UpdatePurchaseOrderEndpointAsync("1", id, viewModel.Adapt<UpdatePurchaseOrderCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await ApiClient.DeletePurchaseOrderEndpointAsync("1", id).ConfigureAwait(false));
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadSuppliersAsync();
    }

    private async Task LoadSuppliersAsync()
    {
        try
        {
            var paginationFilter = new PaginationFilter { PageNumber = 1, PageSize = 500 };
            var command = paginationFilter.Adapt<SearchSuppliersCommand>();
            var response = await ApiClient.SearchSuppliersEndpointAsync("1", command).ConfigureAwait(false);
            var pagedResponse = response.Adapt<PaginationResponse<SupplierResponse>>();
            _suppliers = pagedResponse.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Toast.Add($"Failed to load suppliers: {ex.Message}", Severity.Error);
        }
    }

    // Order Action Methods
    // TODO: These methods need to be implemented once the API client is regenerated with the purchase order endpoints
    private async Task SubmitOrder(DefaultIdType? id)
    {
        if (id == null) return;
        
        var confirmed = await DialogService.ShowMessageBox(
            "Submit Purchase Order",
            "Are you sure you want to submit this order for approval?",
            yesText: "Submit", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                // TODO: Uncomment when API client is regenerated
                // await ApiClient.SubmitPurchaseOrderEndpointAsync("1", id.Value).ConfigureAwait(false);
                Toast.Add("Purchase order submitted successfully (TODO: implement API call)", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Toast.Add($"Failed to submit order: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ApproveOrder(DefaultIdType? id)
    {
        if (id == null) return;

        var confirmed = await DialogService.ShowMessageBox(
            "Approve Purchase Order",
            "Are you sure you want to approve this order?",
            yesText: "Approve", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                // TODO: Uncomment when API client is regenerated
                // await ApiClient.ApprovePurchaseOrderEndpointAsync("1", id.Value).ConfigureAwait(false);
                Toast.Add("Purchase order approved successfully (TODO: implement API call)", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Toast.Add($"Failed to approve order: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task SendOrder(DefaultIdType? id)
    {
        if (id == null) return;

        var confirmed = await DialogService.ShowMessageBox(
            "Send Purchase Order",
            "Are you sure you want to send this order to the supplier?",
            yesText: "Send", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                // TODO: Uncomment when API client is regenerated
                // await ApiClient.SendPurchaseOrderEndpointAsync("1", id.Value).ConfigureAwait(false);
                Toast.Add("Purchase order sent successfully (TODO: implement API call)", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Toast.Add($"Failed to send order: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ReceiveOrder(DefaultIdType? id)
    {
        if (id == null) return;

        var confirmed = await DialogService.ShowMessageBox(
            "Receive Purchase Order",
            "Are you sure you want to mark this order as received?",
            yesText: "Receive", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                // TODO: Uncomment when API client is regenerated
                // await ApiClient.ReceivePurchaseOrderEndpointAsync("1", id.Value).ConfigureAwait(false);
                Toast.Add("Purchase order marked as received (TODO: implement API call)", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Toast.Add($"Failed to receive order: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task CancelOrder(DefaultIdType? id)
    {
        if (id == null) return;

        var confirmed = await DialogService.ShowMessageBox(
            "Cancel Purchase Order",
            "Are you sure you want to cancel this order?",
            yesText: "Cancel Order", cancelText: "Keep Order");

        if (confirmed == true)
        {
            try
            {
                // TODO: Uncomment when API client is regenerated
                // await ApiClient.CancelPurchaseOrderEndpointAsync("1", id.Value).ConfigureAwait(false);
                Toast.Add("Purchase order cancelled (TODO: implement API call)", Severity.Warning);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Toast.Add($"Failed to cancel order: {ex.Message}", Severity.Error);
            }
        }
    }

    private void ViewOrderDetails(DefaultIdType? id)
    {
        if (id == null) return;
        // Navigate to details page or open a dialog with full order details
        Navigation.NavigateTo($"/store/purchase-orders/items/{id}");
    }
}

/// <summary>
/// ViewModel used by the Purchase Orders page for add/edit operations.
/// Maps to CreatePurchaseOrderCommand for creation and UpdatePurchaseOrderCommand for updates.
/// </summary>
public class PurchaseOrderViewModel
{
    /// <summary>Unique identifier of the purchase order.</summary>
    public DefaultIdType Id { get; set; }

    /// <summary>Order number (e.g., PO-2025-001).</summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>Supplier identifier.</summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>Order placement date.</summary>
    public DateTime? OrderDate { get; set; } = DateTime.Now;

    /// <summary>Expected delivery date.</summary>
    public DateTime? ExpectedDeliveryDate { get; set; }

    /// <summary>Actual delivery date (read-only, set by system).</summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>Order status (Draft, Submitted, Approved, Sent, Received, Cancelled).</summary>
    public string Status { get; set; } = "Draft";

    /// <summary>Total order amount (calculated from line items).</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>Tax amount.</summary>
    public decimal TaxAmount { get; set; }

    /// <summary>Total discount amount.</summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>Net amount after tax and discounts.</summary>
    public decimal NetAmount { get; set; }

    /// <summary>Delivery address for the order.</summary>
    public string? DeliveryAddress { get; set; }

    /// <summary>Contact person at delivery location.</summary>
    public string? ContactPerson { get; set; }

    /// <summary>Contact phone number.</summary>
    public string? ContactPhone { get; set; }

    /// <summary>Additional notes for the order.</summary>
    public string? Notes { get; set; }

    /// <summary>Flag indicating if this is an urgent order.</summary>
    public bool IsUrgent { get; set; }
}
