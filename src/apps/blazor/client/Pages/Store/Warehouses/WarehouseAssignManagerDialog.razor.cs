namespace FSH.Starter.Blazor.Client.Pages.Store.Warehouses;

/// <summary>
/// Dialog component for assigning a manager to a warehouse.
/// </summary>
public partial class WarehouseAssignManagerDialog
{
    /// <summary>
    /// Warehouse identifier to assign manager to.
    /// </summary>
    [Parameter]
    public DefaultIdType WarehouseId { get; set; }

    /// <summary>
    /// Dialog instance reference.
    /// </summary>
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? _form;
    private bool _isProcessing;

    /// <summary>
    /// Command state for assigning manager.
    /// </summary>
    private AssignManagerState _command = new();

    /// <summary>
    /// State class for assign manager command.
    /// </summary>
    private class AssignManagerState
    {
        public string ManagerName { get; set; } = string.Empty;
        public string ManagerEmail { get; set; } = string.Empty;
        public string ManagerPhone { get; set; } = string.Empty;
    }

    /// <summary>
    /// Initializes the dialog.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        _command = new AssignManagerState();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Submits the assign manager request.
    /// </summary>
    private async Task Submit()
    {
        if (_form is null) return;

        await _form.Validate();
        if (!_form.IsValid) return;

        try
        {
            _isProcessing = true;

            var command = new AssignWarehouseManagerCommand
            {
                Id = WarehouseId,
                ManagerName = _command.ManagerName,
                ManagerEmail = _command.ManagerEmail,
                ManagerPhone = _command.ManagerPhone
            };

            await Client.AssignWarehouseManagerEndpointAsync("1", WarehouseId, command);
            Snackbar.Add("Manager assigned successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error assigning manager: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    /// <summary>
    /// Closes the dialog without saving.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

