namespace FSH.Starter.Blazor.Client.Pages.Store.CycleCounts;

/// <summary>
/// Dialog component for viewing cycle count details and managing count items.
/// Provides comprehensive view of cycle count information with inline item management and count recording.
/// </summary>
public partial class CycleCountDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    /// <summary>
    /// The cycle count ID to display details for.
    /// </summary>
    [Parameter] 
    public DefaultIdType CycleCountId { get; set; }

    private CycleCountResponse? _cycleCount;
    private Dictionary<DefaultIdType, string> _itemNames = new();
    private bool _loading;

    /// <summary>
    /// Loads the cycle count details when the component initializes.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadCycleCountAsync();
    }

    /// <summary>
    /// Loads the cycle count details from the API.
    /// </summary>
    private async Task LoadCycleCountAsync()
    {
        _loading = true;
        try
        {
            _cycleCount = await Blazor.Client.GetCycleCountEndpointAsync("1", CycleCountId).ConfigureAwait(false);
            
            // Load item names for display
            if (_cycleCount?.Items != null && _cycleCount.Items.Any())
            {
                await LoadItemNamesAsync();
            }
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load cycle count: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Loads item names for display in the items table.
    /// </summary>
    private async Task LoadItemNamesAsync()
    {
        if (_cycleCount?.Items == null) return;

        try
        {
            var itemIds = _cycleCount.Items.Select(x => x.ItemId).Distinct().ToList();
            
            foreach (var itemId in itemIds)
            {
                try
                {
                    var item = await Blazor.Client.GetItemEndpointAsync("1", itemId).ConfigureAwait(false);
                    _itemNames[itemId] = item?.Name ?? "Unknown Item";
                }
                catch
                {
                    _itemNames[itemId] = "Unknown Item";
                }
            }
        }
        catch (Exception ex)
        {
            MudBlazor.Snackbar.Add($"Failed to load item names: {ex.Message}", Severity.Warning);
        }
    }

    /// <summary>
    /// Gets the item name for display.
    /// </summary>
    private string GetItemName(DefaultIdType itemId)
    {
        return _itemNames.TryGetValue(itemId, out var name) ? name : "Loading...";
    }

    /// <summary>
    /// Gets the color for the status chip.
    /// </summary>
    private Color GetStatusColor(string status)
    {
        return status switch
        {
            "Scheduled" => Color.Default,
            "InProgress" => Color.Info,
            "Completed" => Color.Success,
            "Cancelled" => Color.Error,
            _ => Color.Default
        };
    }

    /// <summary>
    /// Gets the color for the progress bar.
    /// </summary>
    private Color GetProgressColor(int percentage)
    {
        return percentage switch
        {
            < 50 => Color.Error,
            < 100 => Color.Warning,
            _ => Color.Success
        };
    }

    /// <summary>
    /// Opens the add item dialog.
    /// </summary>
    private async Task AddItem()
    {
        var parameters = new DialogParameters<CycleCountAddItemDialog>
        {
            { x => x.CycleCountId, CycleCountId }
        };

        var dialog = await MudBlazor.DialogService.ShowAsync<CycleCountAddItemDialog>("Add Item to Count", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadCycleCountAsync();
        }
    }

    /// <summary>
    /// Opens the record count dialog for an item.
    /// </summary>
    private async Task RecordCount(CycleCountItemResponse item)
    {
        var parameters = new DialogParameters<CycleCountRecordDialog>
        {
            { x => x.CycleCountId, CycleCountId },
            { x => x.Item, item }
        };

        var dialog = await MudBlazor.DialogService.ShowAsync<CycleCountRecordDialog>("Record Count", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadCycleCountAsync();
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }
}

