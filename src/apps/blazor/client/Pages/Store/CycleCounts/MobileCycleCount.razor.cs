namespace FSH.Starter.Blazor.Client.Pages.Store.CycleCounts;

/// <summary>
/// Mobile-optimized cycle count interface for warehouse/store staff using mobile devices.
/// Features touch-friendly UI, barcode scanning, and offline support.
/// </summary>
public partial class MobileCycleCount
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    private bool _isLoading = true;
    private bool _isCountingMode = false;
    private CycleCountResponse? _selectedCount;
    
    private ClientPreference _preference = new();
    
    private List<CycleCountResponse> _todayCounts = [];
    private List<CycleCountResponse> _upcomingCounts = [];
    private List<CycleCountResponse> _completedCounts = [];

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

        await LoadCountsAsync();
    }

    /// <summary>
    /// Loads cycle counts and categorizes them by status and date.
    /// </summary>
    private async Task LoadCountsAsync()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var request = new SearchCycleCountsRequest
            {
                PageNumber = 1,
                PageSize = 100,
                OrderBy = ["CountDate"]
            };

            var result = await Client.SearchCycleCountsEndpointAsync("1", request).ConfigureAwait(false);
            var allCounts = result.Items?.ToList() ?? [];

            var today = DateOnly.FromDateTime(DateTime.Today);

            _todayCounts = allCounts
                .Where(c => DateOnly.FromDateTime(c.CountDate) == today && c.Status is "Scheduled" or "InProgress")
                .OrderBy(c => c.Status == "InProgress" ? 0 : 1)
                .ThenBy(c => c.CountDate)
                .ToList();

            _upcomingCounts = allCounts
                .Where(c => DateOnly.FromDateTime(c.CountDate) > today && c.Status == "Scheduled")
                .OrderBy(c => c.CountDate)
                .Take(10)
                .ToList();

            _completedCounts = allCounts
                .Where(c => c.Status == "Completed")
                .OrderByDescending(c => c.CountDate)
                .Take(10)
                .ToList();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load counts: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Starts a scheduled cycle count and enters counting mode.
    /// </summary>
    private async Task StartCount(CycleCountResponse count)
    {
        try
        {
            var command = new StartCycleCountCommand { Id = count.Id };
            await Client.StartCycleCountEndpointAsync("1", count.Id, command).ConfigureAwait(false);
            
            _selectedCount = count;
            _selectedCount.Status = "InProgress";
            _isCountingMode = true;
            
            Snackbar.Add("Count started! Begin scanning items.", Severity.Success);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to start count: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Continues an in-progress cycle count.
    /// </summary>
    private void ContinueCount(CycleCountResponse count)
    {
        _selectedCount = count;
        _isCountingMode = true;
        StateHasChanged();
    }

    /// <summary>
    /// Views count details in a dialog.
    /// </summary>
    private async Task ViewCount(CycleCountResponse count)
    {
        var parameters = new DialogParameters<CycleCountDetailsDialog>
        {
            { x => x.CycleCountId, count.Id }
        };

        var options = new DialogOptions 
        { 
            CloseButton = true,
            FullScreen = true,
            MaxWidth = MaxWidth.Small
        };

        await DialogService.ShowAsync<CycleCountDetailsDialog>("Count Details", parameters, options);
    }

    /// <summary>
    /// Exits counting mode and returns to count list.
    /// </summary>
    private async Task ExitCountingMode()
    {
        _isCountingMode = false;
        _selectedCount = null;
        await LoadCountsAsync();
    }

    /// <summary>
    /// Handles count completion event.
    /// </summary>
    private async Task HandleCountCompleted()
    {
        _isCountingMode = false;
        _selectedCount = null;
        await LoadCountsAsync();
        Snackbar.Add("✅ Count completed successfully!", Severity.Success);
    }

    /// <summary>
    /// Switches to web desktop view.
    /// </summary>
    private void SwitchToWebView()
    {
        Navigation.NavigateTo("/store/cycle-counts");
    }

    /// <summary>
    /// Gets the progress percentage for a count.
    /// </summary>
    private double GetProgressPercentage(CycleCountResponse count)
    {
        if (count.TotalItems == 0) return 0;
        return (double)count.CountedItems / count.TotalItems * 100;
    }

    /// <summary>
    /// Gets the color for a status chip.
    /// </summary>
    private Color GetStatusColor(string status) => status switch
    {
        "Scheduled" => Color.Default,
        "InProgress" => Color.Info,
        "Completed" => Color.Success,
        "Cancelled" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Gets the icon for a status.
    /// </summary>
    private string GetStatusIcon(string status) => status switch
    {
        "Scheduled" => "📅",
        "InProgress" => "⏳",
        "Completed" => "✅",
        "Cancelled" => "❌",
        _ => ""
    };
}

