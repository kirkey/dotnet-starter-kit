namespace FSH.Starter.Blazor.Client.Pages.Accounting.Vendors;

/// <summary>
/// Help dialog providing comprehensive guidance on vendor management.
/// </summary>
public partial class VendorsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

