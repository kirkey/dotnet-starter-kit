namespace FSH.Starter.Blazor.Client.Pages.Store.Bins;

/// <summary>
/// Help dialog providing comprehensive guidance on storage bin management.
/// </summary>
public partial class BinsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

