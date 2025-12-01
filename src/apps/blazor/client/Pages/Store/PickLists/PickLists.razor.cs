namespace FSH.Starter.Blazor.Client.Pages.Store.PickLists;

/// <summary>
/// Pick Lists page logic. Provides CRUD and search over PickList entities using the generated API client.
/// Supports workflow operations including assign, start, and complete picking operations.
/// </summary>
public partial class PickLists
{
    

    private EntityServerTableContext<PickListResponse, DefaultIdType, PickListViewModel> Context { get; set; } = null!;
    private EntityTable<PickListResponse, DefaultIdType, PickListViewModel> _table = null!;

    private ClientPreference _preference = new();

    private List<WarehouseResponse> _warehouses = [];

    // Search filter properties - bound to UI controls in the razor file
    private DefaultIdType? SearchWarehouseId { get; set; }
    private string? SearchStatus { get; set; }
    private string? SearchPickingType { get; set; }
    private string? SearchAssignedTo { get; set; }
    private int? SearchMinPriority { get; set; }
    private int? SearchMaxPriority { get; set; }
    private DateTime? SearchStartDateFrom { get; set; }
    private DateTime? SearchStartDateTo { get; set; }

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<PickListResponse, DefaultIdType, PickListViewModel>(
            entityName: "Pick List",
            entityNamePlural: "Pick Lists",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<PickListResponse>(x => x.PickListNumber, "Pick List #", "PickListNumber"),
                new EntityField<PickListResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<PickListResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PickListResponse>(x => x.PickingType, "Type", "PickingType"),
                new EntityField<PickListResponse>(x => x.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<PickListResponse>(x => x.AssignedTo, "Assigned To", "AssignedTo"),
                new EntityField<PickListResponse>(x => x.TotalLines, "Total Items", "TotalLines", typeof(int)),
                new EntityField<PickListResponse>(x => x.CompletedLines, "Picked", "CompletedLines", typeof(int)),
                new EntityField<PickListResponse>(x => x.StartDate, "Started", "StartDate", typeof(DateOnly?))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var command = new SearchPickListsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? ["Priority desc", "CreatedOn desc"],
                    WarehouseId = SearchWarehouseId,
                    Status = SearchStatus,
                    PickingType = SearchPickingType,
                    AssignedTo = SearchAssignedTo,
                    MinPriority = SearchMinPriority,
                    MaxPriority = SearchMaxPriority,
                    StartDateFrom = SearchStartDateFrom,
                    StartDateTo = SearchStartDateTo
                };

                var result = await Client.SearchPickListsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PickListResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreatePickListEndpointAsync("1", viewModel.Adapt<CreatePickListCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePickListEndpointAsync("1", id).ConfigureAwait(false));
    }

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
    /// Views the full details of a pick list including items in a dialog.
    /// </summary>
    private async Task ViewPickListDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<PickListDetailsDialog>
        {
            { x => x.PickListId, id }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large,
        };

        var dialog = await DialogService.ShowAsync<PickListDetailsDialog>("Pick List Details", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Assigns a pick list to a picker.
    /// </summary>
    private async Task AssignPickList(DefaultIdType id)
    {
        var parameters = new DialogParameters<AssignPickListDialog>
        {
            { x => x.PickListId, id }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
        };

        var dialog = await DialogService.ShowAsync<AssignPickListDialog>("Assign Pick List", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Starts picking on an assigned pick list.
    /// </summary>
    private async Task StartPicking(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Start Picking",
            "Are you sure you want to start picking this pick list? This will change the status to In Progress.",
            yesText: "Start Picking",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                // Note: API client needs regeneration to include this endpoint
                // Backend endpoint exists at: POST /store/picklists/{id}/start
                var command = new StartPickingCommand { PickListId = id };
                await Client.StartPickingEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Picking started successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to start picking: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Completes picking on an in-progress pick list.
    /// </summary>
    private async Task CompletePicking(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Complete Picking",
            "Are you sure you want to complete this pick list? This will finalize all picked items.",
            yesText: "Complete Picking",
            cancelText: "Cancel");

        if (confirmed is true)
        {
            try
            {
                // Note: API client needs regeneration to include this endpoint
                // Backend endpoint exists at: POST /store/picklists/{id}/complete
                var command = new CompletePickingCommand { PickListId = id };
                await Client.CompletePickingEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Picking completed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to complete picking: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show pick lists help dialog.
    /// </summary>
    private async Task ShowPickListsHelp()
    {
        await DialogService.ShowAsync<PickListsHelpDialog>("Pick Lists Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Pick List add/edit operations.
/// Inherits from CreatePickListCommand (no update operation exists for pick lists).
/// </summary>
public partial class PickListViewModel : CreatePickListCommand
{
    public DefaultIdType Id { get; set; } = DefaultIdType.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ExpectedCompletionDate { get; set; }
}
