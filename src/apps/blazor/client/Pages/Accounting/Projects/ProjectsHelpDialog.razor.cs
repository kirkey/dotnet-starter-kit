namespace FSH.Starter.Blazor.Client.Pages.Accounting.Projects;

/// <summary>
/// Help dialog providing comprehensive guidance on project accounting.
/// </summary>
public partial class ProjectsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

