namespace FSH.Starter.Blazor.Client.Pages.Store.Warehouses;

/// <summary>
/// Help dialog providing comprehensive guidance on warehouse management.
/// </summary>
public partial class WarehousesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

