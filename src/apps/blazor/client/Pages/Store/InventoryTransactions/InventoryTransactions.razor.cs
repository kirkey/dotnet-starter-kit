namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryTransactions;

/// <summary>
/// InventoryTransactions page logic. Provides CRUD and search over InventoryTransaction entities using the generated API client.
/// Supports workflow operations: Approve and Reject for pending transactions.
/// </summary>
public partial class InventoryTransactions
{
    protected EntityServerTableContext<InventoryTransactionResponse, DefaultIdType, InventoryTransactionViewModel> Context { get; set; } = null!;
    private EntityTable<InventoryTransactionResponse, DefaultIdType, InventoryTransactionViewModel> _table = null!;

    // Advanced search filters
    private string? _searchTransactionType;
    private string? SearchTransactionType
    {
        get => _searchTransactionType;
        set
        {
            _searchTransactionType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _searchIsApproved;
    private bool? SearchIsApproved
    {
        get => _searchIsApproved;
        set
        {
            _searchIsApproved = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchDateFrom;
    private DateTime? SearchDateFrom
    {
        get => _searchDateFrom;
        set
        {
            _searchDateFrom = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchDateTo;
    private DateTime? SearchDateTo
    {
        get => _searchDateTo;
        set
        {
            _searchDateTo = value;
            _ = _table.ReloadDataAsync();
        }
    }

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
                new EntityField<InventoryTransactionResponse>(x => x.TransactionDate, "Date", "TransactionDate", typeof(DateOnly)),
                new EntityField<InventoryTransactionResponse>(x => x.IsApproved, "Approved", "IsApproved", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchInventoryTransactionsCommand>();
                command.TransactionType = SearchTransactionType;
                command.TransactionDateFrom = SearchDateFrom;
                command.TransactionDateTo = SearchDateTo;
                command.IsApproved = SearchIsApproved;
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

    /// <summary>
    /// Approves a pending inventory transaction.
    /// </summary>
    private async Task ApproveTransaction(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Approval",
            "Are you sure you want to approve this transaction?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new ApproveInventoryTransactionCommand();
                await Client.ApproveInventoryTransactionEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Transaction approved successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to approve transaction: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Rejects a pending inventory transaction.
    /// </summary>
    private async Task RejectTransaction(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Rejection",
            "Are you sure you want to reject this transaction? This action cannot be undone.",
            yesText: "Reject",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new RejectInventoryTransactionCommand();
                await Client.RejectInventoryTransactionEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Transaction rejected successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reject transaction: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show inventory transactions help dialog.
    /// </summary>
    private async Task ShowInventoryTransactionsHelp()
    {
        await DialogService.ShowAsync<InventoryTransactionsHelpDialog>("Inventory Transactions Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
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
