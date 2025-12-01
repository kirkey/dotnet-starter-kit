namespace FSH.Starter.Blazor.Client.Pages.Store.InventoryTransfers;

/// <summary>
/// InventoryTransfers page logic. Provides CRUD and search over InventoryTransfer entities using the generated API client.
/// Supports workflow operations: Approve, Mark In Transit, Complete, and Cancel.
/// </summary>
public partial class InventoryTransfers
{
    

    protected EntityServerTableContext<GetInventoryTransferListResponse, DefaultIdType, InventoryTransferViewModel> Context { get; set; } = null!;
    private EntityTable<GetInventoryTransferListResponse, DefaultIdType, InventoryTransferViewModel> _table = null!;

    private ClientPreference _preference = new();

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

        Context = new EntityServerTableContext<GetInventoryTransferListResponse, DefaultIdType, InventoryTransferViewModel>(
            entityName: "Inventory Transfer",
            entityNamePlural: "Inventory Transfers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<GetInventoryTransferListResponse>(x => x.TransferNumber, "Transfer #", "TransferNumber"),
                new EntityField<GetInventoryTransferListResponse>(x => x.FromWarehouseName, "From", "FromWarehouseName"),
                new EntityField<GetInventoryTransferListResponse>(x => x.ToWarehouseName, "To", "ToWarehouseName"),
                new EntityField<GetInventoryTransferListResponse>(x => x.TransferDate, "Date", "TransferDate", typeof(DateOnly)),
                new EntityField<GetInventoryTransferListResponse>(x => x.Status, "Status", "Status"),
                new EntityField<GetInventoryTransferListResponse>(x => x.TransferType, "Type", "TransferType"),
                new EntityField<GetInventoryTransferListResponse>(x => x.Priority, "Priority", "Priority")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetInventoryTransferEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<InventoryTransferViewModel>();
            // },
            searchFunc: async filter =>
            {
                var command = new SearchInventoryTransfersCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
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

    /// <summary>
    /// Show inventory transfers help dialog.
    /// </summary>
    private async Task ShowInventoryTransfersHelp()
    {
        await DialogService.ShowAsync<InventoryTransfersHelpDialog>("Inventory Transfers Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for InventoryTransfer add/edit operations.
/// Inherits from UpdateInventoryTransferCommand to ensure proper mapping with the API.
/// </summary>
public partial class InventoryTransferViewModel : UpdateInventoryTransferCommand;
