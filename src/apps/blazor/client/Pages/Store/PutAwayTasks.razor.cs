namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class PutAwayTasks
{
    
    

    protected EntityServerTableContext<PutAwayTaskResponse, DefaultIdType, PutAwayTaskViewModel> Context { get; set; } = default!;
    private EntityTable<PutAwayTaskResponse, DefaultIdType, PutAwayTaskViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<PutAwayTaskResponse, DefaultIdType, PutAwayTaskViewModel>(
            entityName: "Put Away Task",
            entityNamePlural: "Put Away Tasks",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<PutAwayTaskResponse>(x => x.TaskNumber, "Task #", "TaskNumber"),
                new EntityField<PutAwayTaskResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PutAwayTaskResponse>(x => x.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<PutAwayTaskResponse>(x => x.AssignedTo, "Assigned To", "AssignedTo"),
                new EntityField<PutAwayTaskResponse>(x => x.PutAwayStrategy, "Strategy", "PutAwayStrategy"),
                new EntityField<PutAwayTaskResponse>(x => x.TotalLines, "Total Lines", "TotalLines", typeof(int)),
                new EntityField<PutAwayTaskResponse>(x => x.CompletedLines, "Completed", "CompletedLines", typeof(int)),
                new EntityField<PutAwayTaskResponse>(x => x.CompletionPercentage, "Progress %", "CompletionPercentage", typeof(double))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetPutAwayTaskEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<PutAwayTaskViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchPutAwayTasksCommand>();
                var result = await Client.SearchPutAwayTasksEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PutAwayTaskResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreatePutAwayTaskEndpointAsync("1", viewModel.Adapt<CreatePutAwayTaskCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePutAwayTaskEndpointAsync("1", id).ConfigureAwait(false));
        await Task.CompletedTask;
    }

    private async Task AssignTask(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Assign Task",
            "Assign this put-away task to a worker?",
            yesText: "Assign",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new AssignPutAwayTaskCommand();
            await Client.AssignPutAwayTaskEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }
}

public class PutAwayTaskViewModel
{
    public string? TaskNumber { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? GoodsReceiptId { get; set; }
    public int Priority { get; set; }
    public string? PutAwayStrategy { get; set; }
    public string? Notes { get; set; }
}
