namespace FSH.Starter.Blazor.Client.Pages.Hr.DesignationAssignments;

/// <summary>
/// Help dialog providing comprehensive guidance on designation assignments management.
/// </summary>
public partial class DesignationAssignmentsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

