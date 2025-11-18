namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

/// <summary>
/// Help dialog providing comprehensive guidance on fixed asset management.
/// </summary>
public partial class FixedAssetsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

