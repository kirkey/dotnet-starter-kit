namespace FSH.Starter.Blazor.Client.Pages.Store.SalesImports;

/// <summary>
/// Help dialog providing comprehensive guidance on sales import management.
/// </summary>
public partial class SalesImportsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

