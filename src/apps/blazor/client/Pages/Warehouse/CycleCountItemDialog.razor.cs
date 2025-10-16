namespace FSH.Starter.Blazor.Client.Pages.Warehouse;

/// <summary>
/// Dialog component for adding or recording cycle count items.
/// Provides form validation and submission logic for item management.
/// </summary>
public partial class CycleCountItemDialog
{

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public DefaultIdType CycleCountId { get; set; }
    [Parameter] public CycleCountItemModel Model { get; set; } = new();
    [Parameter] public bool IsRecording { get; set; }

    private MudForm _form = default!;

    /// <summary>
    /// Saves the cycle count item (add or record count).
    /// </summary>
    private async Task SaveAsync()
    {
        await _form.Validate();
        if (!_form.IsValid) return;

        try
        {
            if (IsRecording)
            {
                // Record the counted quantity for existing item
                var command = new RecordCycleCountItemCommand
                {
                    CycleCountId = CycleCountId,
                    CycleCountItemId = Model.Id,
                    CountedQuantity = Model.CountedQuantity ?? 0,
                    CountedBy = Model.CountedBy,
                    Notes = Model.Notes
                };

                await Client.RecordCycleCountItemEndpointAsync("1", CycleCountId, Model.Id, command).ConfigureAwait(false);
                Snackbar.Add("Count recorded successfully", Severity.Success);
            }
            else
            {
                // Add new item to cycle count
                var command = new AddCycleCountItemCommand
                {
                    CycleCountId = CycleCountId,
                    ItemId = Model.ItemId,
                    SystemQuantity = Model.SystemQuantity,
                    CountedQuantity = Model.CountedQuantity
                };

                await Client.AddCycleCountItemEndpointAsync("1", CycleCountId, command).ConfigureAwait(false);
                Snackbar.Add("Item added successfully", Severity.Success);
            }

            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to save: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Cancels the dialog and closes it without saving.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
