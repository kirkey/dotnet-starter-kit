namespace FSH.Starter.Blazor.Client.Pages.Accounting.FiscalPeriodClose;

/// <summary>
/// Checklist dialog for managing fiscal period close tasks and validation status.
/// </summary>
public partial class FiscalPeriodCloseChecklistDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    
    /// <summary>
    /// Event callback triggered when a task is completed.
    /// </summary>
    [Parameter] public EventCallback OnTaskCompleted { get; set; }

    private FiscalPeriodCloseDetailsDto? _periodClose;
    private DefaultIdType _periodCloseId;
    private bool _loading;

    /// <summary>
    /// Shows the checklist dialog for the specified period close.
    /// </summary>
    public async Task ShowAsync(DefaultIdType periodCloseId)
    {
        _periodCloseId = periodCloseId;
        _loading = true;
        _periodClose = null;

        var dialogOptions = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<FiscalPeriodCloseChecklistDialog>(
            "Period Close Checklist", 
            dialogOptions);

        await LoadData();
        StateHasChanged();

        await dialog.Result;
    }

    private async Task LoadData()
    {
        try
        {
            _periodClose = await Client.FiscalPeriodCloseGetEndpointAsync("1", _periodCloseId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading period close: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Marks a task as complete.
    /// </summary>
    private async Task CompleteTask(string taskName)
    {
        try
        {
            var command = new CompleteFiscalPeriodTaskCommand
            {
                FiscalPeriodCloseId = _periodCloseId,
                TaskName = taskName
            };

            await Client.CompleteFiscalPeriodCloseTaskEndpointAsync("1", _periodCloseId, command);
            Snackbar.Add($"Task '{taskName}' completed successfully", Severity.Success);

            await LoadData();
            await OnTaskCompleted.InvokeAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error completing task: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Reloads the period close data.
    /// </summary>
    private async Task Reload()
    {
        _loading = true;
        await LoadData();
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Close() => MudDialog.Close();

    /// <summary>
    /// Gets the color for the status chip.
    /// </summary>
    private Color GetStatusColor(string status) => status switch
    {
        "Completed" => Color.Success,
        "InProgress" => Color.Info,
        "Reopened" => Color.Warning,
        _ => Color.Default
    };

    /// <summary>
    /// Calculates the completion percentage.
    /// </summary>
    private double GetProgressPercentage()
    {
        if (_periodClose == null) return 0;
        
        var totalTasks = _periodClose.TasksCompleted + _periodClose.TasksRemaining;
        if (totalTasks == 0) return 0;

        return (_periodClose.TasksCompleted / (double)totalTasks) * 100;
    }
}
