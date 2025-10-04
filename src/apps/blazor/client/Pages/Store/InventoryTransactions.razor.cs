namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// InventoryTransactions page logic. Provides CRUD and search over InventoryTransaction entities using the generated API client.
/// </summary>
public partial class InventoryTransactions
{
    
    

    protected EntityServerTableContext<InventoryTransactionResponse, DefaultIdType, InventoryTransactionViewModel> Context { get; set; } = default!;
    private EntityTable<InventoryTransactionResponse, DefaultIdType, InventoryTransactionViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<InventoryTransactionResponse, DefaultIdType, InventoryTransactionViewModel>(
            entityName: "Inventory Transaction",
            entityNamePlural: "Inventory Transactions",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<InventoryTransactionResponse>(x => x.TransactionNumber, "Transaction #", "TransactionNumber"),
                new EntityField<InventoryTransactionResponse>(x => x.TransactionType, "Type", "TransactionType"),
                new EntityField<InventoryTransactionResponse>(x => x.ItemName, "Item", "ItemName"),
                new EntityField<InventoryTransactionResponse>(x => x.ItemSku, "SKU", "ItemSku"),
                new EntityField<InventoryTransactionResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<InventoryTransactionResponse>(x => x.Quantity, "Quantity", "Quantity", typeof(double)),
                new EntityField<InventoryTransactionResponse>(x => x.UnitCost, "Unit Cost", "UnitCost", typeof(double?)),
                new EntityField<InventoryTransactionResponse>(x => x.TransactionDate, "Date", "TransactionDate", typeof(DateTime))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetInventoryTransactionEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<InventoryTransactionViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchInventoryTransactionsCommand>();
                var result = await Client.SearchInventoryTransactionsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InventoryTransactionResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateInventoryTransactionEndpointAsync("1", viewModel.Adapt<CreateInventoryTransactionCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteInventoryTransactionEndpointAsync("1", id).ConfigureAwait(false));
        await Task.CompletedTask;
    }

    private async Task ApproveTransaction(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Approval",
            "Are you sure you want to approve this transaction?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new ApproveInventoryTransactionCommand();
            await Client.ApproveInventoryTransactionEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }
}

public class InventoryTransactionViewModel
{
    public string? TransactionNumber { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public double Quantity { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? TransactionType { get; set; }
    public string? ReferenceType { get; set; }
    public DefaultIdType? ReferenceId { get; set; }
    public double? UnitCost { get; set; }
    public string? Notes { get; set; }
}
