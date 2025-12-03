namespace FSH.Starter.Blazor.Client.Pages.Store.PurchaseOrders;

/// <summary>
/// Purchase Orders page logic. Provides CRUD and search over PurchaseOrder entities using the generated API client.
/// Supports workflow operations like submit, approve, send, receive, and cancel.
/// </summary>
public partial class PurchaseOrders
{
    /// <summary>
    /// The entity table context for managing purchase orders with server-side operations.
    /// </summary>
    protected EntityServerTableContext<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for purchase orders.
    /// </summary>
    private EntityTable<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel> _table = null!;

    private List<SupplierResponse> _suppliers = [];
    private DefaultIdType? SearchSupplierId { get; set; }
    private string? SearchStatus { get; set; }
    private DateTime? SearchFromDate { get; set; }
    private DateTime? SearchToDate { get; set; }

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
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

        SetupPurchaseOrdersContext();
        
        // Load suppliers after context is set
        await LoadSuppliersAsync();
    }

    /// <summary>
    /// Sets up the entity table context for purchase orders.
    /// </summary>
    private void SetupPurchaseOrdersContext() =>
        Context = new EntityServerTableContext<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel>(
            entityName: "Purchase Order",
            entityNamePlural: "Purchase Orders",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<PurchaseOrderResponse>(x => x.OrderNumber, "Order Number", "OrderNumber"),
                new EntityField<PurchaseOrderResponse>(x => x.SupplierId, "Supplier ID", "SupplierId"),
                new EntityField<PurchaseOrderResponse>(x => x.OrderDate, "Order Date", "OrderDate", typeof(DateOnly)),
                new EntityField<PurchaseOrderResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PurchaseOrderResponse>(x => x.TotalAmount, "Total Amount", "TotalAmount", typeof(decimal)),
                new EntityField<PurchaseOrderResponse>(x => x.ExpectedDeliveryDate, "Expected Delivery", "ExpectedDeliveryDate", typeof(DateOnly?)),
                new EntityField<PurchaseOrderResponse>(x => x.IsUrgent, "Urgent", "IsUrgent", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = new SearchPurchaseOrdersCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    SupplierId = SearchSupplierId,
                    Status = SearchStatus,
                    FromDate = SearchFromDate,
                    ToDate = SearchToDate
                };
                var result = await Client.SearchPurchaseOrdersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PurchaseOrderResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreatePurchaseOrderEndpointAsync("1", viewModel.Adapt<CreatePurchaseOrderCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdatePurchaseOrderEndpointAsync("1", id, viewModel.Adapt<UpdatePurchaseOrderCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePurchaseOrderEndpointAsync("1", id).ConfigureAwait(false));

    private async Task LoadSuppliersAsync()
    {
        try
        {
            var command = new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 500,
                OrderBy = ["Name"]
            };
            var result = await Client.SearchSuppliersEndpointAsync("1", command).ConfigureAwait(false);
            _suppliers = result.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load suppliers: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Views the full details of a purchase order in a dialog.
    /// </summary>
    private async Task ViewOrderDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<PurchaseOrderDetailsDialog>
        {
            { x => x.PurchaseOrderId, id }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = false,
            MaxWidth = MaxWidth.ExtraLarge, 
        };

        var dialog = await DialogService.ShowAsync<PurchaseOrderDetailsDialog>("Purchase Order Details", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Submits a draft purchase order for approval.
    /// </summary>
    private async Task SubmitOrder(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Submit Purchase Order",
            "Are you sure you want to submit this purchase order for approval?",
            yesText: "Submit",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new SubmitPurchaseOrderCommand();
                await Client.SubmitPurchaseOrderEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Purchase order submitted successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to submit purchase order: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Approves a submitted purchase order.
    /// </summary>
    private async Task ApproveOrder(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Approve Purchase Order",
            "Are you sure you want to approve this purchase order?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var request = new ApprovePurchaseOrderRequest();
                await Client.ApprovePurchaseOrderEndpointAsync("1", id, request).ConfigureAwait(false);
                Snackbar.Add("Purchase order approved successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to approve purchase order: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Sends an approved purchase order to the supplier.
    /// </summary>
    private async Task SendOrder(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Send Purchase Order",
            "Are you sure you want to send this purchase order to the supplier?",
            yesText: "Send",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var request = new SendPurchaseOrderRequest();
                await Client.SendPurchaseOrderEndpointAsync("1", id, request).ConfigureAwait(false);
                Snackbar.Add("Purchase order sent successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to send purchase order: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Marks a purchase order as received.
    /// </summary>
    private async Task ReceiveOrder(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Receive Purchase Order",
            "Are you sure you want to mark this purchase order as received?",
            yesText: "Receive",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var request = new ReceivePurchaseOrderRequest();
                await Client.ReceivePurchaseOrderEndpointAsync("1", id, request).ConfigureAwait(false);
                Snackbar.Add("Purchase order marked as received", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to receive purchase order: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Cancels a purchase order.
    /// </summary>
    private async Task CancelOrder(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Cancel Purchase Order",
            "Are you sure you want to cancel this purchase order?",
            yesText: "Cancel",
            cancelText: "No");

        if (confirmed == true)
        {
            try
            {
                var request = new CancelPurchaseOrderRequest();
                await Client.CancelPurchaseOrderEndpointAsync("1", id, request).ConfigureAwait(false);
                Snackbar.Add("Purchase order cancelled", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to cancel purchase order: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Downloads a PDF report for the purchase order.
    /// </summary>
    private async Task DownloadPdf(DefaultIdType id)
    {
        try
        {
            Snackbar.Add("Generating PDF report...", Severity.Info);
            var fileResponse = await Client.GeneratePurchaseOrderPdfEndpointAsync("1", id).ConfigureAwait(false);

            if (fileResponse is not null && fileResponse.Stream is not null)
            {
                var fileName = $"PurchaseOrder_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                using var ms = new MemoryStream();
                await fileResponse.Stream.CopyToAsync(ms);
                var fileData = ms.ToArray();
                var base64 = Convert.ToBase64String(fileData);
                await Js.InvokeVoidAsync("fshDownload.saveFile", fileName, base64);
                Snackbar.Add("PDF report downloaded successfully", Severity.Success);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error downloading PDF: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Show purchase orders help dialog.
    /// </summary>
    private async Task ShowPurchaseOrdersHelp()
    {
        await DialogService.ShowAsync<PurchaseOrdersHelpDialog>("Purchase Orders Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel used by the PurchaseOrders page for add/edit operations.
/// Inherits from UpdatePurchaseOrderCommand to ensure proper mapping with the API.
/// </summary>
public partial class PurchaseOrderViewModel : UpdatePurchaseOrderCommand;

