using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Dialog component for adding or editing purchase order items.
/// Provides form validation and submission logic for item management.
/// </summary>
public partial class PurchaseOrderItemDialog
{
    [Inject] private IClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public DefaultIdType PurchaseOrderId { get; set; }
    [Parameter] public PurchaseOrderItemModel Model { get; set; } = new();

    private MudForm _form = default!;

    /// <summary>
    /// Saves the purchase order item (add or update).
    /// </summary>
    private async Task SaveAsync()
    {
        await _form.Validate();
        if (!_form.IsValid) return;

        try
        {
            if (Model.Id == default)
            {
                // Add new item
                var command = new AddPurchaseOrderItemCommand(
                    PurchaseOrderId,
                    Model.ItemId,
                    Model.Quantity,
                    (double)Model.UnitPrice,
                    (double)Model.DiscountAmount,
                    Model.Notes);

                await Client.AddPurchaseOrderItemEndpointAsync("1", command).ConfigureAwait(false);
                Snackbar.Add("Item added successfully", Severity.Success);
            }
            else
            {
                // Update existing item
                var command = new UpdatePurchaseOrderItemCommand(
                    Model.ItemId,
                    Model.Quantity,
                    (double)Model.UnitPrice,
                    (double)Model.DiscountAmount,
                    Model.ReceivedQuantity,
                    Model.Notes);

                await Client.UpdatePurchaseOrderItemEndpointAsync("1", PurchaseOrderId, Model.Id, command).ConfigureAwait(false);
                Snackbar.Add("Item updated successfully", Severity.Success);
            }

            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to save item: {ex.Message}", Severity.Error);
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

