namespace FSH.Starter.Blazor.Client.Pages.Store.CycleCounts;

/// <summary>
/// Help dialog providing comprehensive guidance on cycle count management.
/// </summary>
public partial class CycleCountsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

