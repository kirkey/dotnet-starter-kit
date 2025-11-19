namespace FSH.Starter.Blazor.Client.Pages.Store.PickLists;

/// <summary>
/// Help dialog providing comprehensive guidance on pick list management.
/// </summary>
public partial class PickListsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

