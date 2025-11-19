namespace FSH.Starter.Blazor.Client.Pages.Store.Suppliers;

/// <summary>
/// Help dialog providing comprehensive guidance on supplier management.
/// </summary>
public partial class SuppliersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

