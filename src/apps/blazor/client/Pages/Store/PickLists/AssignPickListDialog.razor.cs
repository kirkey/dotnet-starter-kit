namespace FSH.Starter.Blazor.Client.Pages.Store.PickLists;

/// <summary>
/// Dialog component for assigning a pick list to a picker.
/// Allows the user to specify who should be assigned the pick list for picking operations.
/// </summary>
public partial class AssignPickListDialog
{
    [Parameter] public DefaultIdType PickListId { get; set; }
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private GetPickListResponse? _pickList;
    private string _assignedTo = string.Empty;
    private bool _loading = true;
    private bool _assigning;
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
            _errorMessage = $"Failed to load pick list: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    /// <summary>
    /// Assigns the pick list by calling the API with the provided assignee.
    /// </summary>
    private async Task Assign()
    {
        if (string.IsNullOrWhiteSpace(_assignedTo))
        {
            Snackbar.Add("Please specify who this pick list should be assigned to", Severity.Warning);
            return;
        }

        try
        {
            _assigning = true;
            _errorMessage = null;

            var command = new AssignPickListCommand
            {
                PickListId = PickListId,
                AssignedTo = _assignedTo
            };

            await Client.AssignPickListEndpointAsync("1", PickListId, command).ConfigureAwait(false);
            
            Snackbar.Add("Pick list assigned successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to assign pick list: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _assigning = false;
        }
    }

    /// <summary>
    /// Closes the dialog without assigning the pick list.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

