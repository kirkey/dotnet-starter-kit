namespace FSH.Starter.Blazor.Client.Pages.Accounting.Checks;

/// <summary>
/// Help dialog providing comprehensive guidance on check management.
/// </summary>
public partial class ChecksHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

