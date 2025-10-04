namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// InventoryTransfers page logic. Provides CRUD and search over InventoryTransfer entities using the generated API client.
/// Supports workflow operations: Approve, Mark In Transit, Complete, and Cancel.
/// </summary>
public partial class InventoryTransfers
{
    
    

    protected EntityServerTableContext<GetInventoryTransferListResponse, DefaultIdType, InventoryTransferViewModel> Context { get; set; } = default!;
    private EntityTable<GetInventoryTransferListResponse, DefaultIdType, InventoryTransferViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<GetInventoryTransferListResponse, DefaultIdType, InventoryTransferViewModel>(
            entityName: "Inventory Transfer",
            entityNamePlural: "Inventory Transfers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<GetInventoryTransferListResponse>(x => x.TransferNumber, "Transfer #", "TransferNumber"),
                new EntityField<GetInventoryTransferListResponse>(x => x.FromWarehouseName, "From", "FromWarehouseName"),
                new EntityField<GetInventoryTransferListResponse>(x => x.ToWarehouseName, "To", "ToWarehouseName"),
                new EntityField<GetInventoryTransferListResponse>(x => x.TransferDate, "Date", "TransferDate", typeof(DateTime)),
                new EntityField<GetInventoryTransferListResponse>(x => x.Status, "Status", "Status"),
                new EntityField<GetInventoryTransferListResponse>(x => x.TransferType, "Type", "TransferType"),
                new EntityField<GetInventoryTransferListResponse>(x => x.Priority, "Priority", "Priority")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetInventoryTransferEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<InventoryTransferViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchInventoryTransfersCommand>();
                var result = await Client.SearchInventoryTransfersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GetInventoryTransferListResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateInventoryTransferEndpointAsync("1", viewModel.Adapt<CreateInventoryTransferCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateInventoryTransferEndpointAsync("1", id, viewModel.Adapt<UpdateInventoryTransferCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteInventoryTransferEndpointAsync("1", id).ConfigureAwait(false));
        await Task.CompletedTask;
    }

    private async Task ApproveTransfer(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Approval",
            "Are you sure you want to approve this transfer?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new ApproveInventoryTransferCommand();
            await Client.ApproveInventoryTransferEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }

    private async Task MarkInTransit(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Mark In Transit",
            "Mark this transfer as in transit?",
            yesText: "Confirm",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new MarkInTransitInventoryTransferCommand();
            await Client.MarkInTransitInventoryTransferEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }

    private async Task CompleteTransfer(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Complete Transfer",
            "Mark this transfer as completed?",
            yesText: "Complete",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new CompleteInventoryTransferCommand();
            await Client.CompleteInventoryTransferEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }

    private async Task CancelTransfer(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Cancel Transfer",
            "Are you sure you want to cancel this transfer?",
            yesText: "Cancel Transfer",
            cancelText: "Keep Transfer",
            options: new DialogOptions { MaxWidth = MaxWidth.Small });

        if (result == true)
        {
            var command = new CancelInventoryTransferCommand();
            await Client.CancelInventoryTransferEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }
}

/// <summary>
/// ViewModel for InventoryTransfer add/edit operations.
/// </summary>
public class InventoryTransferViewModel
{
    public string? TransferNumber { get; set; }
    public DefaultIdType FromWarehouseId { get; set; }
    public DefaultIdType ToWarehouseId { get; set; }
    public DateTime? TransferDate { get; set; }
    public DateTime? ExpectedArrivalDate { get; set; }
    public DateTime? ActualArrivalDate { get; set; }
    public string? TransferType { get; set; }
    public string? Priority { get; set; }
    public string? TransportMethod { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Notes { get; set; }
}
