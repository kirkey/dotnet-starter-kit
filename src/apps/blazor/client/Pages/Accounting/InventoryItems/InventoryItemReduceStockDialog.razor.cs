namespace FSH.Starter.Blazor.Client.Pages.Accounting.InventoryItems;

public partial class InventoryItemReduceStockDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType ItemId { get; set; }
    [Parameter] public string ItemName { get; set; } = string.Empty;
    
    private decimal _quantity;

    private async Task ReduceStock()
    {
        if (_quantity <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new ReduceStockCommand { Id = ItemId, Quantity = _quantity, };
            await Client.InventoryItemReduceStockEndpointAsync("1", ItemId, cmd);
            Snackbar.Add($"Reduced {_quantity:N2} units successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error reducing stock: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

