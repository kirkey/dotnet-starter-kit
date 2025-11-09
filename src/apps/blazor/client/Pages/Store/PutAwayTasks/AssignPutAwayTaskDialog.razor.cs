namespace FSH.Starter.Blazor.Client.Pages.Store.PutAwayTasks;

/// <summary>
/// Dialog component for assigning a put-away task to a warehouse worker.
/// Allows the user to specify who should be assigned the put-away task for warehouse operations.
/// </summary>
public partial class AssignPutAwayTaskDialog
{
    [Parameter] public DefaultIdType PutAwayTaskId { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private GetPutAwayTaskResponse? _putAwayTask;
    private string _assignedTo = string.Empty;
    private bool _loading = true;
    private bool _assigning;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
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
            _errorMessage = $"Failed to load put-away task: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Assigns the put-away task by calling the API with the provided assignee.
    /// </summary>
    private async Task Assign()
    {
        if (string.IsNullOrWhiteSpace(_assignedTo))
        {
            Snackbar.Add("Please specify who this put-away task should be assigned to", Severity.Warning);
            return;
        }

        try
        {
            _assigning = true;
            _errorMessage = null;

            var command = new AssignPutAwayTaskCommand
            {
                PutAwayTaskId = PutAwayTaskId,
                AssignedTo = _assignedTo
            };

            await Client.AssignPutAwayTaskEndpointAsync("1", PutAwayTaskId, command).ConfigureAwait(false);
            
            Snackbar.Add("Put-away task assigned successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to assign put-away task: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _assigning = false;
        }
    }

    /// <summary>
    /// Closes the dialog without assigning the put-away task.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

