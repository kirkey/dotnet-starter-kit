namespace FSH.Starter.Blazor.Client.Pages.Store.CycleCounts.Components;

/// <summary>
/// Mobile counting interface component for hands-on inventory counting.
/// Supports barcode scanning, manual entry, and variance detection.
/// </summary>
public partial class MobileCountingInterface : IAsyncDisposable
{
    

    private DotNetObjectReference<MobileCountingInterface>? _dotNetRef;

    private ClientPreference _preference = new();

    private List<CycleCountItemDetailResponse> _allItems = [];
    private List<CycleCountItemDetailResponse> _recentItems = [];
    private CycleCountItemDetailResponse? _currentItem;
    
    private bool _isScanning = false;
    private string _manualBarcode = string.Empty;
    private decimal _actualQuantity = 0;
    private string _countNotes = string.Empty;
    
    private int _totalItems;
    private int _countedItems;
    private int _varianceCount;
    private double _progressPercentage;
    
    private decimal _varianceAmount;
    private double _variancePercentage;
    private bool _showVarianceAlert;

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

        await LoadCountItemsAsync();
    }

    /// <summary>
    /// Loads all items for the cycle count.
    /// </summary>
    private async Task LoadCountItemsAsync()
    {
        try
        {
            var request = new SearchCycleCountItemsRequest
            {
                CycleCountId = CycleCount.Id,
                PageNumber = 1,
                PageSize = 1000,
                OrderBy = ["ItemSku"]
            };

            var result = await Client.SearchCycleCountItemsEndpointAsync("1", request).ConfigureAwait(false);
            _allItems = result.Items?.ToList() ?? [];
            
            _totalItems = _allItems.Count;
            _countedItems = _allItems.Count(x => x.IsCounted);
            _varianceCount = _allItems.Count(x => x.VarianceAmount != 0);
            
            UpdateProgress();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load items: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Starts the camera barcode scanner.
    /// </summary>
    private async Task StartScanner()
    {
        try
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await Js.InvokeVoidAsync("cycleCounts.startBarcodeScanner", _dotNetRef);
            _isScanning = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to start scanner: {ex.Message}", Severity.Error);
            _isScanning = false;
        }
    }

    /// <summary>
    /// Stops the barcode scanner.
    /// </summary>
    private async Task StopScanner()
    {
        try
        {
            await Js.InvokeVoidAsync("cycleCounts.stopBarcodeScanner");
            _isScanning = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to stop scanner: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Called by JavaScript when a barcode is scanned.
    /// </summary>
    [JSInvokable]
    public async Task OnBarcodeScanned(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode)) return;

        _manualBarcode = barcode;
        await SearchItem();
        await StopScanner();
    }

    /// <summary>
    /// Handles Enter key press in barcode field.
    /// </summary>
    private async Task HandleBarcodeKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(_manualBarcode))
        {
            await SearchItem();
        }
    }

    /// <summary>
    /// Searches for an item by barcode/SKU.
    /// </summary>
    private async Task SearchItem()
    {
        if (string.IsNullOrWhiteSpace(_manualBarcode))
        {
            Snackbar.Add("Please enter a barcode or SKU", Severity.Warning);
            return;
        }

        try
        {
            // Find item in the cycle count items
            var item = _allItems.FirstOrDefault(x => 
                x.ItemSku?.Equals(_manualBarcode, StringComparison.OrdinalIgnoreCase) == true ||
                x.ItemBarcode?.Equals(_manualBarcode, StringComparison.OrdinalIgnoreCase) == true);

            if (item == null)
            {
                Snackbar.Add($"❌ Item '{_manualBarcode}' not found in this count", Severity.Error);
                _manualBarcode = string.Empty;
                return;
            }

            if (item.IsCounted)
            {
                var recount = await DialogService.ShowMessageBox(
                    "Item Already Counted",
                    $"This item was already counted (Qty: {item.ActualQuantity}). Do you want to recount it?",
                    yesText: "Recount",
                    cancelText: "Cancel");

                if (recount != true)
                {
                    _manualBarcode = string.Empty;
                    return;
                }
            }

            // Load item for counting
            _currentItem = item;
            _actualQuantity = item.IsCounted ? item.ActualQuantity : item.ExpectedQuantity;
            _countNotes = item.Notes ?? string.Empty;
            _manualBarcode = string.Empty;
            
            CalculateVariance();
            StateHasChanged();
            
            Snackbar.Add($"✓ Item found: {item.ItemName}", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error searching item: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Adjusts quantity by increment (positive or negative).
    /// </summary>
    private void AdjustQuantity(decimal increment)
    {
        _actualQuantity += increment;
        if (_actualQuantity < 0) _actualQuantity = 0;
        
        CalculateVariance();
        StateHasChanged();
    }

    /// <summary>
    /// Calculates variance between expected and actual quantity.
    /// </summary>
    private void CalculateVariance()
    {
        if (_currentItem == null) return;

        _varianceAmount = _actualQuantity - _currentItem.ExpectedQuantity;
        
        if (_currentItem.ExpectedQuantity > 0)
        {
            _variancePercentage = Math.Round((double)(_varianceAmount / _currentItem.ExpectedQuantity) * 100, 2);
        }
        else
        {
            _variancePercentage = 0;
        }

        _showVarianceAlert = Math.Abs(_variancePercentage) >= 5; // Show alert if variance > 5%
    }

    /// <summary>
    /// Checks if count can be saved.
    /// </summary>
    private bool CanSaveCount()
    {
        if (_currentItem == null) return false;
        
        // Require notes if there's a variance
        if (Math.Abs(_varianceAmount) > 0 && string.IsNullOrWhiteSpace(_countNotes))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Saves the count for the current item.
    /// </summary>
    private async Task SaveCount()
    {
        if (_currentItem == null || !CanSaveCount()) return;

        try
        {
            var command = new UpdateCycleCountItemCommand
            {
                ActualQuantity = _actualQuantity,
                IsCounted = true,
                Notes = _countNotes
            };

            await Client.UpdateCycleCountItemEndpointAsync("1", _currentItem.Id, command).ConfigureAwait(false);

            // Update local item
            _currentItem.ActualQuantity = _actualQuantity;
            _currentItem.IsCounted = true;
            _currentItem.VarianceAmount = _varianceAmount;
            _currentItem.Notes = _countNotes;

            // Add to recent items
            _recentItems.Insert(0, _currentItem);
            if (_recentItems.Count > 10) _recentItems.RemoveAt(_recentItems.Count - 1);

            // Update counts
            _countedItems = _allItems.Count(x => x.IsCounted);
            _varianceCount = _allItems.Count(x => x.VarianceAmount != 0);
            UpdateProgress();

            Snackbar.Add("✅ Count saved!", Severity.Success);

            // Clear current item and prepare for next
            ClearCurrentItem();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to save count: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Clears the current item entry.
    /// </summary>
    private void ClearCurrentItem()
    {
        _currentItem = null;
        _actualQuantity = 0;
        _countNotes = string.Empty;
        _varianceAmount = 0;
        _variancePercentage = 0;
        _showVarianceAlert = false;
        StateHasChanged();
    }

    /// <summary>
    /// Updates progress percentage.
    /// </summary>
    private void UpdateProgress()
    {
        if (_totalItems > 0)
        {
            _progressPercentage = Math.Round((double)_countedItems / _totalItems * 100, 1);
        }
    }

    /// <summary>
    /// Completes the cycle count.
    /// </summary>
    private async Task CompleteCount()
    {
        if (_countedItems < _totalItems)
        {
            Snackbar.Add($"⚠️ Please count all items ({_countedItems}/{_totalItems})", Severity.Warning);
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Complete Cycle Count",
            $"Complete count with {_varianceCount} variances? This will finalize all counts.",
            yesText: "Complete",
            cancelText: "Cancel");

        if (confirmed != true) return;

        try
        {
            var command = new CompleteCycleCountCommand { Id = CycleCount.Id };
            await Client.CompleteCycleCountEndpointAsync("1", CycleCount.Id, command).ConfigureAwait(false);

            await OnCountCompleted.InvokeAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to complete count: {ex.Message}", Severity.Error);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_isScanning)
        {
            await StopScanner();
        }
        
        _dotNetRef?.Dispose();
    }
}
