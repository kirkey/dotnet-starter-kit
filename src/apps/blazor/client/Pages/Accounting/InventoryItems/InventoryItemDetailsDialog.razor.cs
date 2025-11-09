namespace FSH.Starter.Blazor.Client.Pages.Accounting.InventoryItems;

public partial class InventoryItemDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public InventoryItemResponse Item { get; set; } = default!;

    private void Cancel() => MudDialog?.Close();
}

