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
            if (Model.Id == Guid.Empty)
            {
                // Add new item
                var command = new AddPurchaseOrderItemCommand
                {
                    PurchaseOrderId = PurchaseOrderId,
                    ItemId = Model.ItemId,
                    Quantity = Model.Quantity,
                    UnitPrice = (double)Model.UnitPrice,
                    Discount = Model.DiscountAmount > 0 ? (double)Model.DiscountAmount : null
                };

                await Client.AddPurchaseOrderItemEndpointAsync("1", PurchaseOrderId, command).ConfigureAwait(false);
                Snackbar.Add("Item added successfully", Severity.Success);
            }
            else
            {
                var quantityCommand = new UpdatePurchaseOrderItemQuantityCommand
                {
                    PurchaseOrderItemId = Model.Id,
                    Quantity = Model.Quantity
                };

                await Client.UpdatePurchaseOrderItemQuantityEndpointAsync("1", PurchaseOrderId, Model.ItemId, quantityCommand).ConfigureAwait(false);
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

