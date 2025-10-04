namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class PickLists
{
    
    

    protected EntityServerTableContext<PickListResponse, DefaultIdType, PickListViewModel> Context { get; set; } = default!;
    private EntityTable<PickListResponse, DefaultIdType, PickListViewModel> _table = default!;

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<PickListResponse, DefaultIdType, PickListViewModel>(
            entityName: "Pick List",
            entityNamePlural: "Pick Lists",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<PickListResponse>(x => x.PickListNumber, "Pick List #", "PickListNumber"),
                new EntityField<PickListResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PickListResponse>(x => x.PickingType, "Type", "PickingType"),
                new EntityField<PickListResponse>(x => x.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<PickListResponse>(x => x.AssignedTo, "Assigned To", "AssignedTo"),
                new EntityField<PickListResponse>(x => x.TotalLines, "Total Lines", "TotalLines", typeof(int)),
                new EntityField<PickListResponse>(x => x.CompletedLines, "Completed", "CompletedLines", typeof(int))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetPickListEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<PickListViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchPickListsCommand>();
                var result = await Client.SearchPickListsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PickListResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreatePickListEndpointAsync("1", viewModel.Adapt<CreatePickListCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePickListEndpointAsync("1", id).ConfigureAwait(false));
    }

    private async Task AssignPickList(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Assign Pick List",
            "Assign this pick list to a worker?",
            yesText: "Assign",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new AssignPickListCommand();
            await Client.AssignPickListEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }
}

public class PickListViewModel
{
    public string? PickListNumber { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public string? PickingType { get; set; }
    public int Priority { get; set; }
    public string? ReferenceNumber { get; set; }
}
