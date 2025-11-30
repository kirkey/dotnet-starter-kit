namespace FSH.Starter.Blazor.Client.Pages.Store.PutAwayTasks;

/// <summary>
/// Dialog component for viewing detailed information about a put-away task.
/// Displays all put-away task properties including items, progress tracking, and status.
/// </summary>
public partial class PutAwayTaskDetailsDialog
{
    [Parameter] public DefaultIdType PutAwayTaskId { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private GetPutAwayTaskResponse? _putAwayTask;
    private bool _loading = true;
    private string? _errorMessage;
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        await LoadPutAwayTaskAsync();
    }

    /// <summary>
    /// Loads the put-away task details from the API.
    /// </summary>
    private async Task LoadPutAwayTaskAsync()
    {
        try
        {
            _loading = true;
            _putAwayTask = await Client.GetPutAwayTaskEndpointAsync("1", PutAwayTaskId).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load put-away task details: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Gets the appropriate color for displaying put-away task status.
    /// </summary>
    private Color GetStatusColor(string status) => status switch
    {
        "Created" => Color.Default,
        "Assigned" => Color.Info,
        "InProgress" => Color.Warning,
        "Completed" => Color.Success,
        "Cancelled" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Gets the appropriate color for displaying put-away task item status.
    /// </summary>
    private Color GetItemStatusColor(string status) => status switch
    {
        "Pending" => Color.Default,
        "PutAway" => Color.Success,
        "Partial" => Color.Warning,
        _ => Color.Default
    };

    /// <summary>
    /// Calculates the completion percentage as a formatted string.
    /// </summary>
    private string GetCompletionPercentage()
    {
        if (_putAwayTask == null || _putAwayTask.TotalLines == 0) return "0%";
        var percentage = (_putAwayTask.CompletedLines * 100.0 / _putAwayTask.TotalLines);
        return $"{percentage:F1}%";
    }

    /// <summary>
    /// Calculates the completion percentage as a numeric value for progress bar.
    /// </summary>
    private double GetCompletionValue()
    {
        if (_putAwayTask == null || _putAwayTask.TotalLines == 0) return 0;
        return (_putAwayTask.CompletedLines * 100.0 / _putAwayTask.TotalLines);
    }

    /// <summary>
    /// Opens dialog to add an item to the put-away task.
    /// </summary>
    private async Task AddItem()
    {
        var parameters = new DialogParameters<AddPutAwayTaskItemDialog>
        {
            { x => x.PutAwayTaskId, PutAwayTaskId },
            { x => x.TaskNumber, _putAwayTask?.TaskNumber ?? string.Empty }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
        };

        var dialog = await DialogService.ShowAsync<AddPutAwayTaskItemDialog>("Add Item to Put Away Task", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadPutAwayTaskAsync();
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

