namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

/// <summary>
/// Help dialog providing comprehensive guidance on write-offs.
/// </summary>
public partial class WriteOffsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

