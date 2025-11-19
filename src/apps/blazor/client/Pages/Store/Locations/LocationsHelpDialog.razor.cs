namespace FSH.Starter.Blazor.Client.Pages.Store.Locations;

/// <summary>
/// Help dialog providing comprehensive guidance on warehouse location management.
/// </summary>
public partial class LocationsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

