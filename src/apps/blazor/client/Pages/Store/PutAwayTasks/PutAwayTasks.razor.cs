namespace FSH.Starter.Blazor.Client.Pages.Store.PutAwayTasks;

/// <summary>
/// Put Away Tasks page logic. Provides CRUD and search over PutAwayTask entities using the generated API client.
/// Supports workflow operations including assign, start, and complete put-away operations.
/// </summary>
public partial class PutAwayTasks
{
    private EntityServerTableContext<PutAwayTaskResponse, DefaultIdType, PutAwayTaskViewModel> Context { get; set; } = null!;
    private EntityTable<PutAwayTaskResponse, DefaultIdType, PutAwayTaskViewModel> _table = null!;

    private List<WarehouseResponse> _warehouses = [];

    // Search filter properties - bound to UI controls in the razor file
    private DefaultIdType? SearchWarehouseId { get; set; }
    private string? SearchStatus { get; set; }
    private string? SearchPutAwayStrategy { get; set; }
    private string? SearchAssignedTo { get; set; }
    private int? SearchMinPriority { get; set; }
    private int? SearchMaxPriority { get; set; }
    private DateTime? SearchStartDateFrom { get; set; }
    private DateTime? SearchStartDateTo { get; set; }

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<PutAwayTaskResponse, DefaultIdType, PutAwayTaskViewModel>(
            entityName: "Put Away Task",
            entityNamePlural: "Put Away Tasks",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<PutAwayTaskResponse>(x => x.TaskNumber, "Task #", "TaskNumber"),
                new EntityField<PutAwayTaskResponse>(x => x.WarehouseId, "Warehouse", "WarehouseId", typeof(DefaultIdType)),
                new EntityField<PutAwayTaskResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PutAwayTaskResponse>(x => x.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<PutAwayTaskResponse>(x => x.AssignedTo, "Assigned To", "AssignedTo"),
                new EntityField<PutAwayTaskResponse>(x => x.PutAwayStrategy, "Strategy", "PutAwayStrategy"),
                new EntityField<PutAwayTaskResponse>(x => x.TotalLines, "Total Items", "TotalLines", typeof(int)),
                new EntityField<PutAwayTaskResponse>(x => x.CompletedLines, "Completed", "CompletedLines", typeof(int)),
                new EntityField<PutAwayTaskResponse>(x => x.StartDate, "Started", "StartDate", typeof(DateTime?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = new SearchPutAwayTasksCommand
                {
                    WarehouseId = SearchWarehouseId,
                    Status = SearchStatus,
                    PutAwayStrategy = SearchPutAwayStrategy,
                    AssignedTo = SearchAssignedTo,
                    MinPriority = SearchMinPriority,
                    MaxPriority = SearchMaxPriority,
                    StartDateFrom = SearchStartDateFrom,
                    StartDateTo = SearchStartDateTo
                };

                var result = await Client.SearchPutAwayTasksEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PutAwayTaskResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreatePutAwayTaskEndpointAsync("1", viewModel.Adapt<CreatePutAwayTaskCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePutAwayTaskEndpointAsync("1", id).ConfigureAwait(false));
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadWarehousesAsync();
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
    /// Views the full details of a put-away task including items in a dialog.
    /// </summary>
    private async Task ViewTaskDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<PutAwayTaskDetailsDialog>
        {
            { x => x.PutAwayTaskId, id }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large,
        };

        var dialog = await DialogService.ShowAsync<PutAwayTaskDetailsDialog>("Put Away Task Details", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Assigns a put-away task to a worker.
    /// </summary>
    private async Task AssignTask(DefaultIdType id)
    {
        var parameters = new DialogParameters<AssignPutAwayTaskDialog>
        {
            { x => x.PutAwayTaskId, id }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
        };

        var dialog = await DialogService.ShowAsync<AssignPutAwayTaskDialog>("Assign Put Away Task", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Starts put-away on an assigned put-away task.
    /// </summary>
    private async Task StartPutAway(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Start Put Away",
            "Are you sure you want to start this put-away task? This will change the status to In Progress.",
            yesText: "Start Put Away",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                var command = new StartPutAwayCommand { PutAwayTaskId = id };
                await Client.StartPutAwayEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Put-away started successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to start put-away: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Completes put-away on an in-progress put-away task.
    /// </summary>
    private async Task CompletePutAway(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Complete Put Away",
            "Are you sure you want to complete this put-away task? This will finalize all put-away items.",
            yesText: "Complete Put Away",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                var command = new CompletePutAwayCommand { PutAwayTaskId = id };
                await Client.CompletePutAwayEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Put-away completed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to complete put-away: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show put away tasks help dialog.
    /// </summary>
    private async Task ShowPutAwayTasksHelp()
    {
        await DialogService.ShowAsync<PutAwayTasksHelpDialog>("Put Away Tasks Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Put Away Task add/edit operations.
/// Inherits from CreatePutAwayTaskCommand (no update operation exists for put-away tasks).
/// </summary>
public partial class PutAwayTaskViewModel : CreatePutAwayTaskCommand
{
    public DefaultIdType Id { get; set; } = DefaultIdType.Empty;
    public string Status { get; set; } = string.Empty;
}
