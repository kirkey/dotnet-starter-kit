namespace FSH.Starter.Blazor.Client.Pages.Warehouse;

/// <summary>
/// Component for displaying and managing cycle count items within a cycle count.
/// Allows adding items, recording counted quantities, and viewing variances.
/// </summary>
public partial class CycleCountItems
{

    [Parameter] public DefaultIdType CycleCountId { get; set; }
    [Parameter] public string? CycleCountStatus { get; set; }
    [Parameter] public EventCallback OnItemsChanged { get; set; }

    private List<CycleCountItemResponse> _items = [];
    private List<ItemResponse> _itemsCache = [];
    private bool _loading;
    private bool _canEdit => CycleCountStatus is "Scheduled" or "InProgress";

    protected override async Task OnParametersSetAsync()
    {
        if (CycleCountId != default)
        {
            await LoadItemsAsync();
            await LoadItemsCacheAsync();
        }
    }

    /// <summary>
    /// Loads the cycle count items from the API.
    /// Note: There's no dedicated GetCycleCountItems endpoint, so items are loaded via GetCycleCount.
    /// </summary>
    private async Task LoadItemsAsync()
    {
        _loading = true;
        try
        {
            var cycleCount = await Client.GetCycleCountEndpointAsync("1", CycleCountId).ConfigureAwait(false);
            _items = cycleCount?.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load items: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Loads items into cache for displaying names and SKUs.
    /// </summary>
    private async Task LoadItemsCacheAsync()
    {
        try
        {
            var command = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 1000
            };
            var result = await Client.SearchItemsEndpointAsync("1", command).ConfigureAwait(false);
            _itemsCache = result.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load items cache: {ex.Message}", Severity.Error);
        }
    }

    private string GetItemName(DefaultIdType itemId)
    {
        return _itemsCache.FirstOrDefault(i => i.Id == itemId)?.Name ?? "Unknown";
    }

    private string GetItemSKU(DefaultIdType itemId)
    {
        return _itemsCache.FirstOrDefault(i => i.Id == itemId)?.Sku ?? "-";
    }

    /// <summary>
    /// Opens a dialog to add a new item to the cycle count.
    /// </summary>
    private async Task AddItemAsync()
    {
        var parameters = new DialogParameters<CycleCountItemDialog>
        {
            { x => x.CycleCountId, CycleCountId },
            { x => x.Model, new CycleCountItemModel() }
        };

        var dialog = await DialogService.ShowAsync<CycleCountItemDialog>("Add Item to Count", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await LoadItemsAsync();
            await OnItemsChanged.InvokeAsync();
        }
    }

    /// <summary>
    /// Opens a dialog to record the counted quantity for an item.
    /// </summary>
    private async Task RecordCountAsync(CycleCountItemResponse item)
    {
        var model = new CycleCountItemModel
        {
            Id = item.Id ?? default,
            ItemId = item.ItemId,
            SystemQuantity = item.SystemQuantity,
            CountedQuantity = item.CountedQuantity,
            CountedBy = item.CountedBy
        };

        var parameters = new DialogParameters<CycleCountItemDialog>
        {
            { x => x.CycleCountId, CycleCountId },
            { x => x.Model, model },
            { x => x.IsRecording, true }
        };

        var dialog = await DialogService.ShowAsync<CycleCountItemDialog>("Record Count", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await LoadItemsAsync();
            await OnItemsChanged.InvokeAsync();
        }
    }
}

/// <summary>
/// Model for the cycle count item form.
/// </summary>
public class CycleCountItemModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public int SystemQuantity { get; set; }
    public int? CountedQuantity { get; set; }
    public string? CountedBy { get; set; }
    public string? Notes { get; set; }
}
