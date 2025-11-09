namespace FSH.Starter.Blazor.Client.Pages.Store.PickLists;

/// <summary>
/// Dialog component for viewing detailed information about a pick list.
/// Displays all pick list properties including items, progress tracking, and status.
/// </summary>
public partial class PickListDetailsDialog
{
    [Parameter] public DefaultIdType PickListId { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private GetPickListResponse? _pickList;
    private bool _loading = true;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadPickListAsync();
    }

    /// <summary>
    /// Loads the pick list details from the API.
    /// </summary>
    private async Task LoadPickListAsync()
    {
        try
        {
            _loading = true;
            _pickList = await Client.GetPickListEndpointAsync("1", PickListId).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load pick list details: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Gets the appropriate color for displaying pick list status.
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
    /// Gets the appropriate color for displaying pick list item status.
    /// </summary>
    private Color GetItemStatusColor(string status) => status switch
    {
        "Pending" => Color.Default,
        "Picked" => Color.Success,
        "Short" => Color.Warning,
        "Substituted" => Color.Info,
        _ => Color.Default
    };

    /// <summary>
    /// Calculates the completion percentage as a formatted string.
    /// </summary>
    private string GetCompletionPercentage()
    {
        if (_pickList == null || _pickList.TotalLines == 0) return "0%";
        var percentage = (_pickList.PickedLines * 100.0 / _pickList.TotalLines);
        return $"{percentage:F1}%";
    }

    /// <summary>
    /// Calculates the completion percentage as a numeric value for progress bar.
    /// </summary>
    private double GetCompletionValue()
    {
        if (_pickList == null || _pickList.TotalLines == 0) return 0;
        return (_pickList.PickedLines * 100.0 / _pickList.TotalLines);
    }

    /// <summary>
    /// Opens dialog to add an item to the pick list.
    /// </summary>
    private async Task AddItem()
    {
        var parameters = new DialogParameters<AddPickListItemDialog>
        {
            { x => x.PickListId, PickListId },
            { x => x.PickListNumber, _pickList?.PickListNumber ?? string.Empty }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
        };

        var dialog = await DialogService.ShowAsync<AddPickListItemDialog>("Add Item to Pick List", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadPickListAsync();
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

