namespace FSH.Starter.Blazor.Client.Pages.Store.PutAwayTasks;

/// <summary>
/// Help dialog providing comprehensive guidance on put away task management.
/// </summary>
public partial class PutAwayTasksHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

