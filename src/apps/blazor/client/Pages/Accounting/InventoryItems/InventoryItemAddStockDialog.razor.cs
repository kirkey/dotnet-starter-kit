namespace FSH.Starter.Blazor.Client.Pages.Accounting.InventoryItems;

public partial class InventoryItemAddStockDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType ItemId { get; set; }
    [Parameter] public string ItemName { get; set; } = string.Empty;
    
    private decimal _quantity;

    private async Task AddStock()
    {
        if (_quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new AddStockCommand { Id = ItemId, Quantity = _quantity };
            await Client.InventoryItemAddStockEndpointAsync("1", ItemId, cmd);
            Snackbar.Add($"Added {_quantity:N2} units successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error adding stock: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

