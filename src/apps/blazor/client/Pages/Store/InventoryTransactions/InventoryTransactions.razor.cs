namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryTransactions;

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
                new EntityField<InventoryTransactionResponse>(x => x.Quantity, "Quantity", "Quantity", typeof(decimal)),
                new EntityField<InventoryTransactionResponse>(x => x.UnitCost, "Unit Cost", "UnitCost", typeof(decimal?)),
                new EntityField<InventoryTransactionResponse>(x => x.TransactionDate, "Date", "TransactionDate", typeof(DateTime))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetInventoryTransactionEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<InventoryTransactionViewModel>();
            // },
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
        bool? result = await MudBlazor.DialogService.ShowMessageBox(
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

/// <summary>
/// ViewModel for Inventory Transaction add/edit operations.
/// Inherits from CreateInventoryTransactionCommand to ensure proper mapping with the API.
/// </summary>
public partial class InventoryTransactionViewModel : CreateInventoryTransactionCommand
{
    public string? ReferenceType { get; set; }
}
